using Diploma.Common.Models;

namespace Diploma.API.Data;

public class Sessions
{
    public static Dictionary<string, Session> sessions { get; set; } = new Dictionary<string, Session>();
}