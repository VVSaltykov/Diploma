using Diploma.Common.Models;

namespace Diploma.TgBot.Data;

public class UserMessagesState
{
    public List<Messages> Messages { get; set; } = new List<Messages>();
    public int CurrentMessageIndex { get; set; } = 0;
    public int LastMessageId { get; set; } = 0;
}