from imports import *
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

templates = Jinja2Templates(directory="templates")


@app.post("/template/confirmation")
async def createMall(request: Request, data: Confirmation):
    return templates.TemplateResponse("confirmation/confirmation.html", {"request": request, "data": data})