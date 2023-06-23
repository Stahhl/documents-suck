from fastapi import FastAPI, Request
from fastapi.responses import HTMLResponse
from fastapi.staticfiles import StaticFiles
from fastapi.templating import Jinja2Templates
from models import Data

app = FastAPI()

templates = Jinja2Templates(directory="templates")


@app.post("/template/confirmation")
async def createMall(request: Request, data: Data):
    return templates.TemplateResponse("confirmation/confirmation.html", {"request": request, "data": data})
