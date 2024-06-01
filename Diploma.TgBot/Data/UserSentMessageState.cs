namespace Diploma.TgBot.Data;

public class UserSentMessageState
{
    public string Title { get; set; }
    public string Text { get; set; }
    public bool IsAnonymous { get; set; } = false;
    public string? ProfessorName { get; set; }
    public List<string> FileIds { get; set; } = new List<string>();
}