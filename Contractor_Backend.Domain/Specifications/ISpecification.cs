using System.Linq.Expressions;

namespace Contractor_Backend.Domain.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
