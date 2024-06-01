using Radzen;

namespace Diploma.WebClient.Utils;

public static class NotificationExtensions
{
    public static void ShowExceptionNotification(this NotificationService service, Exception e)
    {
        service.Notify(GetNotification("Ошибка", e.Message, NotificationSeverity.Error));
    }

    public static void ShowNotification(this NotificationService service, string title, string description,
        NotificationSeverity type)
    {
        service.Notify(GetNotification(title, description, type));
    }

    private static NotificationMessage GetNotification(string title, string description, NotificationSeverity type)
    {
        return new NotificationMessage
        {
            Severity = type,
            Duration = type is NotificationSeverity.Error ? 15000 : 8000,
            Summary = title,
            Detail = description
        };
    }
}