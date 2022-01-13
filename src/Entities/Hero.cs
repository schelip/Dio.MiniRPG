namespace Dio.MiniRPG.src.Entities
{
    public class Hero : BaseCharacter
    {
        public string HeroType;

        public Hero(string name, string heroType, uint level)
        : base(name, level)
        {
            this.HeroType = heroType;
        }

        public override string ToString() =>
            $"{this.Name.ToUpper()} - {this.HeroType}\n" +
            $"LVL: \t{this.Level}\n" +
            $"EXP: \t{this.Experience} / {this.RequiredExperiencePoints}\n" +
            $"HP: \t{this.HealthPoints} / {this.MaxHealthPoints}\n" +
            $"ATK: \t{this.AttackPoints}\n" +
            $"DEF: \t{this.DefensePoints}\n" +
            $"END: \t{this.EndurancePoints}\n";
    }
}