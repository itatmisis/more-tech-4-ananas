from aiogram import types
from aiogram.dispatcher.filters import Command, Text
from anabot.keyboards.default import digest_menu, registration_menu
from anabot.loader import dp
from anabot.states.registration_states import RegistrationState


@dp.message_handler(Command("registration"), state=None)
async def start_registration(message: types.Message):
    await message.answer("1. Выбери свою роль:\n", reply_markup=registration_menu.role_selection_menu)
    await RegistrationState.role.set()


@dp.message_handler(Text(equals=["Генеральный директор", "Бухгалтер"]), state=RegistrationState.role)
async def source_selection(message: types.Message):
    # save role info
    answer = (
        "2. По умолчанию вы будете получать новости из всех доступных источников\nЧтобы изменить источники "
        'новостей напишите команду:\n/sources\nНапишите "Продолжить", чтобы перейти на следующий шаг регистрации'
    )
    await message.answer(answer, reply_markup=registration_menu.source_selection_menu)
    await RegistrationState.next()
    # save source info


@dp.message_handler(Command("sources"), state=RegistrationState.sources)
async def source_changer(message: types.Message):
    # save source info
    pass


@dp.message_handler(Text(equals=["Продолжить"]), state=RegistrationState.sources)
async def time_selection(message: types.Message):
    answer = (
        "3. По умолчанию вы будете получать новости в 12:00 по Московскому времени\nЧтобы изменить время, в "
        'которое вы хотите получать сообщения напишите команду:\n/time\nНапишите "Завершить", чтобы завершить '
        "ргеистрацию"
    )
    await message.answer(answer, reply_markup=registration_menu.time_selection_menu)
    await RegistrationState.next()
    # save time info


@dp.message_handler(Command("time"), state=RegistrationState.send_time)
async def time_changer(message: types.Message):
    # save time info
    pass


@dp.message_handler(Text(equals=["Завершить"]), state=RegistrationState.send_time)
async def finish_registration(message: types.Message):
    answer = "Процесс регистрации окончен\nТеперь вы можете ознакомиться с самыми горячими новостями"
    await message.answer(answer, reply_markup=digest_menu.digest_menu)
    await RegistrationState.next()
