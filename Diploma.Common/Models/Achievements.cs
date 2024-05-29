using System.Collections.Generic;

namespace Diploma.Common.Models;

public class Achievements : BaseEntity<int>
{
    public string Tittle { get; set; }
    public string Text { get; set; }
    public List<User>? Users { get; set; } = new List<User>();
}