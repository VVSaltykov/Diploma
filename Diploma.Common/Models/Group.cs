using System.Collections.Generic;

namespace Diploma.Common.Models;

public class Group : BaseEntity<int>
{
    public string Name { get; set; }
    
    public List<User>? Users { get; set; } = new List<User>();
}