namespace Api.Shared.Exceptions;

public class EntityNotUpdatedException: Exception
{
    public EntityNotUpdatedException(string message) : base(message) {}
}