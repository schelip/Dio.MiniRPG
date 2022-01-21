using Dio.MiniRPG.Infrastructure;

namespace Dio.MiniRPG.Entities
{
    public abstract class BaseCharacter : BaseEntity, ICharacter
    {

        public uint LVL { get; protected set; } = 1;
        public uint EXP { get; protected set; }
        public double HP { get; protected set; }
        public double MaxHP { get; protected set; }
        public double ATK { get; protected set; }
        public double DEF { get; protected set; }
        public bool IsDefending { get; protected set; } = false;
        public double END { get; protected set; }

        public uint RequiredEXP { get; protected set; } = 100;

        public double MaxHPFactor { get; protected set; }
        public double ATKFactor { get; protected set; }
        public double DEFFactor { get; protected set; }
        public double ENDFactor { get; protected set; }

        public abstract List<ICharacterAction> CharacterActionsList { get; protected set; }

        public BaseCharacter(string name)
        : base(name)
        { }

        public void Act(int index, ICharacter[] targets) => CharacterActionsList[index].ActionMethod(this, targets);

        public virtual void ReceiveDamage(double damagePoints) => this.HP -= Math.Min(damagePoints, this.HP);

        public virtual void HealDamage(double healPoints) => this.HP += Math.Min(healPoints, this.MaxHP - this.HP);

        public virtual void StartDefending() => this.IsDefending = true;

        public virtual void StopDefending() => this.IsDefending = false;

        public virtual void ReceiveExperience(uint expPoints)
        {
            this.EXP += expPoints;

            if (this.RequiredEXP < this.EXP)
                this.LevelUp();
        }

        public virtual void LevelUp()
        {
            this.RequiredEXP += this.LVL * 50;
            this.LVL++;
            this.EXP = 0;

            this.MaxHP += this.MaxHPFactor;
            this.ATK += this.ATKFactor;
            this.DEF += this.DEFFactor;
            this.END += this.ENDFactor;

            this.HP = this.MaxHP;
        }
    }
}