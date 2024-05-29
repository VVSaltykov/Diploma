using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Diploma.Common.Utils;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName() ?? enumValue.ToString();
    }
    
    public static List<EnumDisplayItem<T>> GetFilteredEnumDisplayItems<T>(Func<T, bool> filter) where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Where(filter)
            .Select(e => new EnumDisplayItem<T>
            {
                Value = e,
                DisplayName = e.GetDisplayName()
            })
            .ToList();
    }
}
public class EnumDisplayItem<T>
{
    public T Value { get; set; }
    public string DisplayName { get; set; }
}