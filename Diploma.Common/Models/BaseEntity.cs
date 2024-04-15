using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Common.Models;

public abstract class BaseEntity<T>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public T? Id { get; set; }
}