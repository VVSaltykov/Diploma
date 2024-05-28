using System;

namespace Diploma.Common.Models;

public class Session
{
    public User User { get; set; }
    public string Token { get; set; }
    public DateTime DateTime { get; set; }
}