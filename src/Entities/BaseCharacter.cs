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
        public double END { get; protected set; }
        public bool IsDefending { get; protected set; } = false;
        public bool IsDead { get; protected set; }

        public uint RequiredEXP { get; protected set; } = 100;

        public double MaxHPFactor { get; protected set; }
        public double ATKFactor { get; protected set; }
        public double DEFFactor { get; protected set; }
        public double ENDFactor { get; protected set; }

        public abstract List<ICharacterAction> CharacterActionsList { get; protected set; }

        public BaseCharacter(string name)
        : base(name)
        { }

        public bool Act(int index, params ICharacter[] targets)
        {
            if (this.CheckIsDead()) return false;
            CharacterActionsList[index].Execute(this, targets);
            return true;
        }


        public virtual bool ReceiveDamage(double damagePoints)
        {
            if (this.CheckIsDead()) return false;

            this.HP -=
                damagePoints < 0 ? 0 : // Damage nothing if negative
                this.HP < damagePoints ? this.HP : // Damage only the maximum possible
                damagePoints;

            if (this.HP <= 0)
            {
                this.IsDead = true;
                Console.WriteLine($"{this.Name} has died!");
            }

            return true;
        }

        public virtual bool HealDamage(double healPoints)
        {
            if (this.CheckIsDead()) return false;
            
            this.HP +=
                healPoints < 0 ? 0 : // Heal nothing if negative
                this.MaxHP - this.HP > healPoints ? this.MaxHP - this.HP : // Heal only the maximum possible
                healPoints;

            return true;
        }

        public virtual bool StartDefending()
        {
            if (this.CheckIsDead()) return false;
            this.IsDefending = true;
            return true;
        }

        public virtual bool StopDefending()
        {
            if (this.CheckIsDead()) return false;
            this.IsDefending = false;
            return true;
        }

        public virtual bool ReceiveExperience(uint expPoints)
        {
            if (this.CheckIsDead()) return false;

            this.EXP += expPoints;

            if (this.RequiredEXP < this.EXP)
                this.LevelUp();

            return true;
        }

        protected virtual void LevelUp()
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

        private bool CheckIsDead()
        {
            if (this.IsDead)
            {
                Console.WriteLine($"{this.Name} is dead!");
                return true;
            }
            else return false;
        }
    }
}