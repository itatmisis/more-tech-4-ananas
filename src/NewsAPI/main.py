from fastapi import FastAPI
import uvicorn
import databases
from Models import models
import sqlalchemy
from typing import List
from dotenv import load_dotenv, find_dotenv
import  os

load_dotenv(find_dotenv())

user = os.getenv("USER")
password = os.getenv("PASSWORD")
host = os.getenv("HOST")
db = os.getenv("DB")


DATABASE_URL = f"postgresql://{user}:{password}@{host}/{db}"
# DATABASE_URL = "postgresql://user:password@postgresserver/db"

database = databases.Database(DATABASE_URL)

engine = sqlalchemy.create_engine(
    DATABASE_URL
)
models.metadata.create_all(engine)


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
async def news():
    query = models.news.select()
    return await database.fetch_all(query)

@app.post("/user", response_model=models.User)
async  def addUser(user: models.User):
    query = models.users.insert().values(userid=user.userid, role = user.role)
    last_record_id = await database.execute(query)
    return {**user.dict(), "id": last_record_id}

@app.get("/roles", response_model=List[models.RoleDb])
async def roles():
    query = models.roles.select()
    return await database.fetch_all(query)


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)

