using Diploma.Common.Interfaces;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.TgBot.Data;
using Diploma.TgBot.Services;
using Diploma.TgBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.TgBot.Controllers;

public class UserAchievementsController : BotController
{ 
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IKeyboardButtonsGenerationService _keyboardButtonsGenerationService;
    
    public UserAchievementsController(IInlineButtonsGenerationService buttonsGenerationService, IKeyboardButtonsGenerationService keyboardButtonsGenerationService)
    {
        _buttonsGenerationService = buttonsGenerationService;
        _keyboardButtonsGenerationService = keyboardButtonsGenerationService;
    }
    
    [Message("Мои достижения", ignoreCase: true)]
    public async Task SendMessages()
    {
        IAchievementsService achievementsService = SingletonService.GetAchievementsService();
        IAccountService accountService = SingletonService.GetAccountService();

        var user = await accountService.Read(Update.GetChatId());
        var achievements = await achievementsService.GetUserAchievements(user);

        if (achievements.Count > 0)
        {
            var userAchievementsState = new UserAchievementsState()
            {
                Achievements = achievements,
                CurrentAchievementIndex = 0
            };

            UserAchievementsStateService.SetUserAchievementState(Update.GetChatId(), userAchievementsState);

            await ShowCurrentAchievement(userAchievementsState);
        }
        else
        {
            await Client.SendTextMessageAsync(Update.GetChatId(), "У вас нет достижений.");
        }
    }

    [Callback("previousMyAchievement", ignoreCase: true)]
    public async Task ShowPreviousMessage()
    {
        var userAchievementState = UserAchievementsStateService.GetUserAchievementState(Update.GetChatId());

        if (userAchievementState != null && userAchievementState.Achievements.Count > 0)
        {
            if (userAchievementState.CurrentAchievementIndex < userAchievementState.Achievements.Count - 1)
            {
                userAchievementState.CurrentAchievementIndex++;
                await ShowCurrentAchievement(userAchievementState);
            }
        }
    }

    [Callback("nextMyAchievement", ignoreCase: true)]
    public async Task ShowNextMessage()
    {
        var userAchievementState = UserAchievementsStateService.GetUserAchievementState(Update.GetChatId());

        if (userAchievementState != null && userAchievementState.Achievements.Count > 0)
        {
            if (userAchievementState.CurrentAchievementIndex > 0)
            {
                userAchievementState.CurrentAchievementIndex--;
                await ShowCurrentAchievement(userAchievementState);
            }
        }
    }
    
    [Callback("tohomeMyAchievement", ignoreCase: true)]
    public async Task ToHome()
    {
        AccountService accountService = SingletonService.GetAccountService();
        var User = await accountService.Read(BotContext.Update.GetChatId());

        if (User.Role == Role.Graduate)
        {
            await Client.DeleteMessageAsync(Update.GetChatId(), BotContext.Update.CallbackQuery.Message.MessageId);
            await Client.SendTextMessageAsync(Update.GetChatId(), "Выберите действие", replyMarkup: Buttons.GraduateButtons());
        }
        if (User.Role == Role.Applicant)
        {
            await Client.DeleteMessageAsync(Update.GetChatId(), BotContext.Update.CallbackQuery.Message.MessageId);
            await Client.SendTextMessageAsync(Update.GetChatId(), "Выберите действие", replyMarkup: Buttons.ApplicantButtons());
        }
        if (User.Role == Role.Student)
        {
            await Client.DeleteMessageAsync(Update.GetChatId(), BotContext.Update.CallbackQuery.Message.MessageId);
            await Client.SendTextMessageAsync(Update.GetChatId(), "Выберите действие", replyMarkup: Buttons.StudentButtons());
        }
    }

    private async Task ShowCurrentAchievement(UserAchievementsState state)
    {
        var currentAchievement = state.Achievements[state.CurrentAchievementIndex];


        var formattedMessage = $"**{currentAchievement.Tittle}:**\n{currentAchievement.Text}\n\n**Список участников:**\n\n";

        foreach (var user in currentAchievement.Users)
        {
            formattedMessage += $"{user.Name}\n";
        }
        
        var inlineKeyboardRows = new List<InlineKeyboardButton[]>();

        if (state.Achievements.Count > 1)
        {
            if (state.CurrentAchievementIndex <= state.Achievements.Count - 1 && state.CurrentAchievementIndex != 0)
            {
                inlineKeyboardRows.Insert(0, new []
                {
                    InlineKeyboardButton.WithCallbackData("Следующее", "nextAchievements")
                });
            }

            if (state.CurrentAchievementIndex >= 0)
            {
                inlineKeyboardRows.Insert(0, new[]
                {
                    InlineKeyboardButton.WithCallbackData("Предыдущее", "previousAchievements")
                });
            }
        }

        var inlineKeyboard = new InlineKeyboardMarkup(inlineKeyboardRows);
        
        if (state.LastAchievementId == 0)
        {
            var removeKeyboardMessage = await Client.SendTextMessageAsync(Update.GetChatId(), ".", replyMarkup: new ReplyKeyboardRemove());
            await Client.DeleteMessageAsync(Update.GetChatId(), removeKeyboardMessage.MessageId);
            var sentMessage = await Client.SendTextMessageAsync(Update.GetChatId(), formattedMessage, replyMarkup: inlineKeyboard, parseMode: ParseMode.Markdown);
            state.LastAchievementId = sentMessage.MessageId;
        }
        else
        {
            await Client.EditMessageTextAsync(Update.GetChatId(), state.LastAchievementId, formattedMessage, replyMarkup: inlineKeyboard, parseMode: ParseMode.Markdown);
        }
    }
}