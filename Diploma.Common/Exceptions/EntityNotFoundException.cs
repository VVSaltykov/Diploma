using System.Runtime.Serialization;

namespace Diploma.Common.Exceptions;

[Serializable]
public class EntityNotFound : Exception
{
    private Type type;

    public EntityNotFound(Type type)
    {
        this.type = type;
    }

    public EntityNotFound(string? message) : base(message)
    {
    }

    public EntityNotFound(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntityNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}