namespace Api.Shared.Exceptions;

public class EntityNotCreatedException : Exception
{
    public EntityNotCreatedException(string message) : base(message) { }
}