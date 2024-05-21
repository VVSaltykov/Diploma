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
            }
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
            }
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
            }
        };

        // Создаем клавиатуру с кнопками
        return new ReplyKeyboardMarkup(buttons)
        {
            ResizeKeyboard = true // Опционально: уменьшает размер кнопок для удобства
        };
    }
}