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

        public override string ToString()
        {
            string result = $"{this.Name} // {this.HeroType.ToString().ToUpper()}\n" +
                $"///// Stats /////\n" +
                $" LVL: \t{this.LVL}\t" +
                $" EXP: \t{this.EXP} / {this.RequiredEXP}\n" +
                $" HP: \t{Math.Round(this.HP)} / {Math.Round(this.MaxHP)}\t" +
                $" ATK: \t{Math.Round(this.ATK)}\n" +
                $" DEF: \t{Math.Round(this.DEF)}\t" +
                $" END: \t{Math.Round(this.END)}\n" +
                $"///// Actions /////\n";
            
            int i = 1;
            this.CharacterActionsList.ForEach((a) => { result +=
                $"{i++} - {a.Name} - {a.ActionType}" +
                $"\n{a.Description}\n\n";
            });

            return result;
        }
}
}