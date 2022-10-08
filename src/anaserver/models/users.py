from anaserver.database import Base
import sqlalchemy


class User(Base):
    __tablename__ = "users"

    id = sqlalchemy.Column(sqlalchemy.Integer, primary_key=True, index=True)
    role = sqlalchemy.Column(sqlalchemy.ForeignKey("roles.id"), nullable=False)
