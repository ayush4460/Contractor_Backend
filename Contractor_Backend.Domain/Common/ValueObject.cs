namespace Contractor_Backend.Domain.Common
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;

            var other = (ValueObject)obj;
            using var thisValues = GetEqualityComponents().GetEnumerator();
            using var otherValues = other.GetEqualityComponents().GetEnumerator();

            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current is null ^ otherValues.Current is null) return false;
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current)) return false;
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
