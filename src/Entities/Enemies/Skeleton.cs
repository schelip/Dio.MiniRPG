namespace Dio.MiniRPG.Entities.Enemies
{
    public class Skeleton : BaseEnemy
    {
        public Skeleton(string name) : base(name)
        {
            this.MaxHP = this.HP = 15;
            this.ATK = 5;
        }

        public Skeleton()
        : this("Skeleton")
        { }
    }
}