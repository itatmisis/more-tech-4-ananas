from fastapi import FastAPI
import uvicorn
import databases
from Models import models
DATABASE_URL = "postgresql://ananasik:ananasik_parol@194.58.118.87/news"
# DATABASE_URL = "postgresql://user:password@postgresserver/db"

database = databases.Database(DATABASE_URL)


app = FastAPI(title="News API")


@app.on_event("startup")
async def startup():
    await database.connect()


@app.on_event("shutdown")
async def shutdown():
    await database.disconnect()

@app.get("/")
async def start():
    return "hello"

@app.get("/news")
async def News():
    query = models.news.select()
    return await database.fetch_all(query)


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)

