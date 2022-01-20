namespace Dio.MiniRPG.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; }
        public string Name { get; }

        public BaseEntity(string name)
        {
            this.Id = new Guid();
            this.Name = name;
        }
    }
}