using Contractor_Backend.Domain.Common;
using Contractor_Backend.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Contractor_Backend.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; }

        public Email(string value)
        {
            if (!IsValid(value)) throw new BusinessRuleViolationException("Invalid email format.");
            Value = value;
        }

        private bool IsValid(string email) => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.ToLowerInvariant();
        }
    }
}
