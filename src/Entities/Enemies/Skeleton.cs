namespace Dio.MiniRPG.Entities.Enemies
{
    public class Skeleton : BaseEnemy
    {
        public Skeleton() : base("Skeleton")
        {
            this.MaxHP = this.HP = 7;
            this.ATK = 3;
        }

        public Skeleton(string name) : base(name)
        {
            this.MaxHP = this.HP = 15;
            this.ATK = 5;
        }
    }
}