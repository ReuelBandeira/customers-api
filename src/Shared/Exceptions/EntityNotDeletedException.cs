namespace Api.Shared.Exceptions;

public class EntityNotDeletedException : Exception
{
    public EntityNotDeletedException(string message) : base(message) { }
}