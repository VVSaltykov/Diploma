using Telegram.Bot.Types.ReplyMarkups;

namespace Diploma.TgBot.UI;

public class Buttons
{
    public static IReplyMarkup StudentButtons()
    {
        // Создаем кнопки
        var buttons = new[]
        {
            new[]
            {
                new KeyboardButton("Отправить сообщение"),
                new KeyboardButton("Отправить сообщение преподавателю")
            },
            new[]
            {
                new KeyboardButton("Мои сообщения"),
                new KeyboardButton("Полученные ответы")
            },
            new[]
            {
                new KeyboardButton("Мои достижения"),
                new KeyboardButton("Достижения учеников")
            },
            new[]
            {
                new KeyboardButton("Полезные ресурсы")
            },
        };

        // Создаем клавиатуру с кнопками
        return new ReplyKeyboardMarkup(buttons)
        {
            ResizeKeyboard = true // Опционально: уменьшает размер кнопок для удобства
        };
    }
    public static IReplyMarkup GraduateButtons()
    {
        // Создаем кнопки
        var buttons = new[]
        {
            new[]
            {
                new KeyboardButton("Отправить сообщение")
            },
            new[]
            {
                new KeyboardButton("Мои сообщения"),
                new KeyboardButton("Полученные ответы")
            },
            new[]
            {
                new KeyboardButton("Мои достижения"),
                new KeyboardButton("Достижения учеников")
            },
            new[]
            {
                new KeyboardButton("Полезные ресурсы")
            },
        };

        // Создаем клавиатуру с кнопками
        return new ReplyKeyboardMarkup(buttons)
        {
            ResizeKeyboard = true // Опционально: уменьшает размер кнопок для удобства
        };
    }
    public static IReplyMarkup ApplicantButtons()
    {
        // Создаем кнопки
        var buttons = new[]
        {
            new[]
            {
                new KeyboardButton("Отправить сообщение")
            },
            new[]
            {
                new KeyboardButton("Мои сообщения"),
                new KeyboardButton("Полученные ответы")
            },
            new[]
            {
                new KeyboardButton("Достижения учеников")
            },
            new[]
            {
                new KeyboardButton("Полезные ресурсы")
            },
        };

        // Создаем клавиатуру с кнопками
        return new ReplyKeyboardMarkup(buttons)
        {
            ResizeKeyboard = true // Опционально: уменьшает размер кнопок для удобства
        };
    }
    public static IReplyMarkup FlippingButtons()
    {
        // Создаем кнопки
        var buttons = new[]
        {
            new[]
            {
                new KeyboardButton("Назад"),
                new KeyboardButton("Вперед")
            },
            new[]
            {
                new KeyboardButton("Домой")
            },
        };

        // Создаем клавиатуру с кнопками
        return new ReplyKeyboardMarkup(buttons)
        {
            ResizeKeyboard = true // Опционально: уменьшает размер кнопок для удобства
        };
    }
}