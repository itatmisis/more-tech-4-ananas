from anabot.middlewares.throttling import ThrottlingMiddleware


def setup_middleware(dp):
    dp.middleware.setup(ThrottlingMiddleware())
