namespace Contractor_Backend.Domain.Exceptions
{
    public class BusinessRuleViolationException(string message) : DomainException(message)
    {
    }
}
