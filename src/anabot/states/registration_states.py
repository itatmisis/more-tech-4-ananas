from aiogram.dispatcher.filters.state import State, StatesGroup


class RegistrationState(StatesGroup):
    role = State()
    sources = State()
    send_time = State()
