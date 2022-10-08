from aiogram import types
from aiogram.dispatcher.filters import Text
from anabot.keyboards.inline.callback_datas import continue_callback, reaction_callback
from anabot.keyboards.inline.reaction_buttons import create_reaction_keyboard
from anabot.loader import dp, bot


@dp.message_handler(Text(equals=["Дневной дайджест"]), state=None)
async def send_news(message: types.Message):
    url_ = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSr7ISTuF5LYO47wWFzXxyRQaEGRtO58ttf1tnfYns&s"
    await message.answer_photo(photo=url_, caption="<b>Article Title 1</b>",
                               reply_markup=create_reaction_keyboard("www.google.com", 1, 929751))
    await message.answer_photo(photo=url_, caption="<b>Article Title 2</b>",
                               reply_markup=create_reaction_keyboard("www.google.com", 2, 929752))
    await message.answer_photo(photo=url_, caption="<b>Article Title 3</b>",
                               reply_markup=create_reaction_keyboard("www.google.com", 3, 929753, continue_button=True))
    print("Запрос дневного дайджеста")


@dp.callback_query_handler(reaction_callback.filter(reaction=["like", "dislike"]))
async def save_reaction(call: types.CallbackQuery, callback_data: dict):
    await call.answer(cache_time=15)
    reaction_bool = True if callback_data["reaction"] == "like" else False
    await call.message.edit_reply_markup(
        create_reaction_keyboard(
            callback_data["link"],
            int(callback_data["article_number"]),
            int(callback_data["news_id"]),
            reaction=reaction_bool,
            continue_button=bool(int(callback_data["continue_button"])),
        )
    )
    print(callback_data)
    # save like/dislike


@dp.callback_query_handler(continue_callback.filter(article_number=["1", "2", "3"]))
async def continue_news(call: types.CallbackQuery, callback_data: dict):
    await call.answer(cache_time=15)
    if callback_data["reaction"] == "None":
        reaction = None
    elif callback_data["reaction"] == "True":
        reaction = True
    else:
        reaction = False
    await call.message.edit_reply_markup(
        create_reaction_keyboard(
            callback_data["link"],
            int(callback_data["article_number"]),
            int(callback_data["news_id"]),
            reaction=reaction,
        )
    )
    url_ = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSr7ISTuF5LYO47wWFzXxyRQaEGRtO58ttf1tnfYns&s"
    await bot.send_photo(call.from_user.id, photo=url_, caption="<b>Article Title 4</b>",
                         reply_markup=create_reaction_keyboard("www.google.com", 1, 1111))
    await bot.send_photo(call.from_user.id, photo=url_, caption="<b>Article Title 5</b>",
                         reply_markup=create_reaction_keyboard("www.google.com", 2, 1111))
    await bot.send_photo(call.from_user.id, photo=url_, caption="<b>Article Title 6</b>",
                         reply_markup=create_reaction_keyboard("www.google.com", 1, 1111, continue_button=True))
    print(callback_data)
