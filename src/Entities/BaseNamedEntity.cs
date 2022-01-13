namespace Dio.MiniRPG.src.Entities
{
    public abstract class BaseNamedEntity : BaseEntity
    {
        public string Name { get; private set; }

        public BaseNamedEntity(string name)
        : base()
        {
            this.Name = name;
        }
    }
}