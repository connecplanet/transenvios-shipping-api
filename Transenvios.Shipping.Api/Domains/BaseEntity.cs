namespace Transenvios.Shipping.Api.Domains
{
    public abstract class BaseEntity<T>
    {
        public virtual T Id { get; set; }
    }
}
