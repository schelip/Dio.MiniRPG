using Dio.MiniRPG.Infrastructure;
using Dio.MiniRPG.Enum;

namespace Dio.MiniRPG.Entities.Heroes
{
    public class Warrior : BaseHero
    {
        public override HeroType HeroType { get => HeroType.WARRIOR; }

        public override List<ICharacterAction> CharacterActionsList
        { get; protected set; } = new List<ICharacterAction>()
        {
            CharacterActions.WeaponStrike,
            CharacterActions.ReadyShield,
        };

        public Warrior(string name, uint level = 1)
        : base(name)
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
                this.CharacterActionsList.Add(CharacterActions.WideSlash);
        }
    }
}