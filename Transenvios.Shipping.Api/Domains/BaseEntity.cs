namespace Transenvios.Shipping.Api.Domains
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
