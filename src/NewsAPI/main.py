from fastapi import FastAPI, HTTPException
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
    try:
        query = models.news.select()
        return await database.fetch_all(query)
    except Exception:
        raise HTTPException(status_code=500, detail="Error while getting news")

@app.post("/user", response_model=models.User)
async  def add_user(user: models.User):
    try:
        query = models.users.insert().values(userid=user.userid, role=user.role)
        last_record_id = await database.execute(query)
        return {**user.dict(), "id": last_record_id}
    except Exception:
        raise HTTPException(status_code=500, detail="Error while adding user")


@app.get("/roles", response_model=List[models.RoleDb])
async def roles():
    try:
        query = models.roles.select()
        return await database.fetch_all(query)
    except:
        raise HTTPException(status_code=500, detail="Error while getting news")


@app.post('/user_reaction', response_model=models.UserReaction)
async def add_user_reaction(user_reaction: models.UserReaction):
    query = models.user_reaction.insert().values(userid=user_reaction.userid, newsid=user_reaction.newsid, reaction=user_reaction.reaction)
    try:
        await database.execute(query)
        return {**user_reaction.dict()}
    except Exception as ex:
        raise HTTPException(status_code=500, detail="Error while adding reaction")

@app.post('/digest')
async def digest(digest: models.Digest):
    return что-то




if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)

