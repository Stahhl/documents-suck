from imports import *

class TemplateData(BaseModel):
    contents: str = "<h1>hello world</h2>"

class Playground(RequestModel):
    template: TemplateData