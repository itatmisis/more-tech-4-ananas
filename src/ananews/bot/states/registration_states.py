from aiogram.dispatcher.filters.state import StatesGroup, State


class RegistrationState(StatesGroup):
    role = State()
    sources = State()
    send_time = State()
