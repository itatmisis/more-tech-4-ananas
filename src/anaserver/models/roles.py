from anaserver.database import Base
import sqlalchemy


class Role(Base):
    __tablename__ = "roles"
    id = sqlalchemy.Column(sqlalchemy.Integer, primary_key=True, index=True)
    name = sqlalchemy.Column(sqlalchemy.String)
