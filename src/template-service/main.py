from imports import *
from fastapi.middleware.cors import CORSMiddleware
from fastapi.exceptions import RequestValidationError
from fastapi.responses import PlainTextResponse

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

templates = Jinja2Templates(directory="templates")


@app.exception_handler(RequestValidationError)
async def validation_exception_handler(request: Request, exc):
    print(exc)
    return PlainTextResponse(str(exc), status_code=400)

@app.post("/template/confirmation")
async def createMall(request: Request, data: Confirmation):
    return templates.TemplateResponse("confirmation/confirmation.html", {"request": request, "data": data})

@app.post("/template/playground")
async def foo(request: Request, data: Playground):
    print(data.contents)
    return templates.TemplateResponse("playground/playground.html", {"request": request, "data": data})