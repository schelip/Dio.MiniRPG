using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Infrastructure;

namespace Dio.MiniRPG.Entities.Heroes
{
    public abstract class BaseHero : BaseCharacter, IHero
    {
        public HeroType HeroType { get; }
        public abstract List<CharacterAction> HeroActions { get; protected set; }

        public BaseHero(string name, HeroType heroType)
        : base(name)
        {
            this.HeroType = heroType;
        }

        public void Act(int index, ICharacter[] targets) => HeroActions[index](this, targets);

        public override string ToString()
        {
            string result = $"{this.Name} // {this.HeroType.ToString().ToUpper()}\n" +
                $"///// Stats /////\n" +
                $" LVL: \t{this.LVL}\n" +
                $" EXP: \t{this.EXP} / {this.RequiredEXP}\n" +
                $" HP: \t{Math.Round(this.HP)} / {Math.Round(this.MaxHP)}\n" +
                $" ATK: \t{Math.Round(this.ATK)}\n" +
                $" DEF: \t{Math.Round(this.DEF)}\n" +
                $" END: \t{Math.Round(this.END)}\n" +
                $"///// Actions /////\n";
            
            int i = 1;
            this.HeroActions.ForEach((a) => { result += ($" {i++} - \t{a.Method.Name}\n"); });

            return result;
        }
}
}