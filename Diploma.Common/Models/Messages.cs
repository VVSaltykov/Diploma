using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Common.Models;

public class Messages : BaseEntity<int>
{
    public string Tittle { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsAnonymous { get; set; }
    public List<long?> RecepientInTelegramIds { get; set; } = new List<long?>();
    public List<Guid?> RecepientInWebIds { get; set; } = new List<Guid?>();
    public List<string>? FilesIds { get; set; } = new List<string>();
    public List<Files>? FilesList { get; set; } = new List<Files>();
    
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
}