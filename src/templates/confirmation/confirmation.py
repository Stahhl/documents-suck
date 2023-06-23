from pydantic import BaseModel

class Confirmation(BaseModel):
    name: str
    condition: bool