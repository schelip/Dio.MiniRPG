namespace Dio.MiniRPG.src.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }

        public BaseEntity()
        {
            this.Id = new Guid();
        }
    }
}