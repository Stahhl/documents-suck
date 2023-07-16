import os, tempfile, shutil, subprocess, zipfile
from imports import *
from fastapi import Request
from fastapi.responses import (
    Response,
)
from fastapi.templating import Jinja2Templates

input_html = "input.html"
input_tex = "input.tex"
output_tex = "output.tex"
output_pdf = "output.pdf"
payload = "payload.json"
template_tex = "template.tex"
logo_image = "logo.jpg"


def zip_directory(directory_path):
    output_directory_name = os.path.basename(directory_path)
    output_zip_path = os.path.join(
        tempfile.gettempdir(), f"{output_directory_name}.zip"
    )

    with zipfile.ZipFile(output_zip_path, "w", zipfile.ZIP_DEFLATED) as zipf:
        for root, _, files in os.walk(directory_path):
            for file in files:
                file_path = os.path.join(root, file)
                zipf.write(file_path, os.path.relpath(file_path, directory_path))

    with open(output_zip_path, "rb") as file:
        zip_data = file.read()

    return output_zip_path, zip_data


def latex_to_pdf(work_dir, input_latex_file_path):
    static_file_path = "./templates/_latex/"
    logo_image = "logo.jpg"
    output_pdf = "output.pdf"

    shutil.copyfile(
        os.path.join(static_file_path, logo_image), os.path.join(work_dir, logo_image)
    )

    command = ["pdflatex", input_latex_file_path]

    subprocess.run(command, cwd=work_dir)

    output_pdf_path = os.path.join(work_dir, output_pdf)
    with open(output_pdf_path, "rb") as file:
        pdf_data = file.read()

    return pdf_data


def insert_latex_into_template(
    templates: Jinja2Templates, work_dir: str, data: AnyRequestModel, latex_contents: str
):
    output_file_path = os.path.join(work_dir, "output.tex")
    template = templates.get_template("_latex/template.tex")
    output = template.render({"data": data, "body": latex_contents})

    with open(output_file_path, "w") as file:
        file.write(output)

    return output_file_path, output


def get_html(templates: Jinja2Templates, id: str, request: Request, data: AnyRequestModel):
    template = templates.get_template(f"{id}/{id}.html")
    contents = template.render({"request": request, "data": data})
    return contents


def convert_html_to_latex(work_dir, html):
    input_file_path = os.path.join(work_dir, "input.html")
    output_file_path = os.path.join(work_dir, "input.tex")

    with open(input_file_path, "w") as file:
        file.write(html)

    command = [
        "docker",
        "run",
        "--rm",
        "--volume",
        f"{work_dir}:/data",
        "pandoc/latex",
        "input.html",
        "-f",
        "html",
        "-t",
        "latex",
        "--output",
        "input.tex",
    ]

    subprocess.run(command)

    with open(output_file_path, "r") as file:
        output = file.read()

    return output_file_path, output


def get_response(
    templates: Jinja2Templates, id: str, ext: str, request: Request, data: AnyRequestModel
):
    work_dir = ""
    zip_dir = ""

    try:
        html = get_html(templates, id, request, data)

        work_dir = tempfile.mkdtemp(dir=tempfile.gettempdir())
        latex_path, latex_contents = convert_html_to_latex(work_dir, html)

        print(latex_path)

        latex_output_file_path, latex_output = insert_latex_into_template(
            templates, work_dir, data, latex_contents
        )

        print(latex_output)

        pdf_bytes = latex_to_pdf(work_dir, latex_output_file_path)

        if ext == "pdf":
            return Response(pdf_bytes, media_type="application/pdf")

        zip_dir, zip_bytes = zip_directory(work_dir)

        return Response(zip_bytes, media_type="application/zip")
    except Exception:
        raise
    finally:
        if os.path.exists(work_dir):
            shutil.rmtree(work_dir)
        if os.path.exists(zip_dir):
            shutil.rmtree(zip_dir)
        print("finally")
