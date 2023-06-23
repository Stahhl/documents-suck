from imports import *

app = FastAPI()

templates = Jinja2Templates(directory="templates")


@app.post("/template/confirmation")
async def createMall(request: Request, data: Confirmation):
    return templates.TemplateResponse("confirmation/confirmation.html", {"request": request, "data": data})