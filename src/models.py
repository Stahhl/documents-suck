from pydantic import BaseModel

class Data(BaseModel):
    name: str
    foo: bool