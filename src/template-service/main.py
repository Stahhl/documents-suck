from imports import *
from fastapi import FastAPI, Request
from fastapi.responses import HTMLResponse
from fastapi.templating import Jinja2Templates
from fastapi.middleware.cors import CORSMiddleware
from fastapi.exceptions import RequestValidationError
from fastapi.responses import PlainTextResponse, HTMLResponse, FileResponse, StreamingResponse
import document_service
# from document_service import get_response

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

templates = Jinja2Templates(directory="templates")


# @app.exception_handler(RequestValidationError)
# async def validation_exception_handler(request: Request, exc):
#     print("validation_exception_handler")
#     print(exc)
#     return PlainTextResponse(str(exc), status_code=400)

@app.post("/template/{id}/{ext}")
async def template_latex(id: str, ext: FExtension,  request: Request, data: Item):
    print("template_latex")
    return document_service.get_response(templates, id, ext.value, request, data)
    # return PlainTextResponse("lol")

# @app.post("/template/confirmation", response_class=HTMLResponse)
# async def template_confirmation(request: Request, data: Confirmation):
#     return templates.TemplateResponse("confirmation/confirmation.html", {"request": request, "data": data})

# @app.post("/template/playground", response_class=HTMLResponse)
# async def template_playground(request: Request, data: Playground):
#     return templates.TemplateResponse("playground/playground.html", {"request": request, "data": data})
