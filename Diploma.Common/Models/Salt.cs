using System;

namespace Diploma.Common.Models;

public class Salt : BaseEntity<Guid>
{
    public byte[] HashSalt { get; set; }
}