from fastapi import FastAPI
import databases
DATABASE_URL = "Host=194.58.118.87; port=5432; user id=ananasik; password=ananasik_parol; database=news"
# DATABASE_URL = "postgresql://user:password@postgresserver/db"

database = databases.Database(DATABASE_URL)


app = FastAPI(title="News API")


@app.on_event("startup")
async def startup():
    await database.connect()
    #await db.connect()
#
#
# @app.on_event("shutdown")
# async def shutdown():
#     #await db.disconnect()

@app.get("/")
async def start():
    return "hello"

