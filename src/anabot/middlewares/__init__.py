from loader import dp

from anabot.middlewares.throttling import ThrottlingMiddleware

if __name__ == "middlewares":
    dp.middleware.setup(ThrottlingMiddleware())
