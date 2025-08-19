namespace Contractor_Backend.Domain.Exceptions
{
    public class EntityNotFoundException(string entity, object key) : DomainException($"{entity} with key '{key}' was not found.")
    {
    }
}
