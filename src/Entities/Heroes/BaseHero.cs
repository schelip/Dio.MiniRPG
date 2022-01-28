using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Infrastructure;

namespace Dio.MiniRPG.Entities.Heroes
{
    public abstract class BaseHero : BaseCharacter, IHero
    {
        public abstract HeroType HeroType { get; }

        public BaseHero(string name)
        : base(name)
        { }

        
        public override string ToString() => $"{this.Name} LVL{this.LVL} {this.HeroType.ToString().ToUpper()}";
    }
}