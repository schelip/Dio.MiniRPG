using System.Linq;
using Dio.MiniRPG.Enum;

namespace Dio.MiniRPG.Entities.Heroes
{
    public class Warrior : BaseHero
    {
        public override List<CharacterAction> HeroActions
        { get; protected set; } = new List<CharacterAction>()
        {
            CharacterActions.WeaponStrike,
            CharacterActions.ReadyShield
        };

        public Warrior(string name, uint level = 1)
        : base(name, HeroType.WARRIOR)
        {
            this.HP = this.MaxHP = 10;
            this.ATK = 3;
            this.DEF = 3;
            this.END = 1;
            this.ATKFactor = 1;
            this.DEFFactor = 0.8;
            this.ENDFactor = 0.33;
            this.MaxHPFactor = 3;

            while (this.LVL < level)
                this.LevelUp();
        }

        public override void LevelUp()
        {
            base.LevelUp();

            if (this.LVL == 10)
                this.HeroActions.Add(CharacterActions.WideSlash);
        }
    }
}