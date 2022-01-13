using Dio.MiniRPG.src.Infrastructure;

namespace Dio.MiniRPG.src.Entities
{
    public abstract class BaseCharacter : BaseNamedEntity, ICharacter
    {
        public uint BaseAttackPoints { get; } = 3;
        public uint BaseDefensePoints { get; }  = 3;
        public uint BaseEndurancePoints { get; }  = 1;
        public uint BaseMaxHealthPoints { get; }  = 10;
        public uint BaseBonusPoints { get; }  = 0;

        public uint Level { get; private set; } = 1;
        public uint Experience { get; private set; } = 0;

        public uint AttackPoints { get; private set; }
        public uint DefensePoints { get; private set; }
        public uint EndurancePoints { get; private set; }
        public uint MaxHealthPoints { get; private set; }
        public uint BonusPoints { get; private set; }
        public uint RequiredExperiencePoints { get; private set; }

        public uint HealthPoints { get; private set; }
        public bool IsDefending { get; private set; } = false;

        public BaseCharacter(string name)
        : base(name)
        {
            SetAllPointss();
            this.HealthPoints = this.MaxHealthPoints;
        }

        public BaseCharacter(string name, uint level)
        : base(name)
        {
            this.Level = level;
            SetAllPointss();
            this.HealthPoints = this.MaxHealthPoints;
        }

        public void Attack(ICharacter attackReceiver)
        {
            attackReceiver.ReceiveAttack(this.AttackPoints);
            Console.WriteLine(this.Name + " has attacked " + attackReceiver.Name + "!");

            if (attackReceiver.HealthPoints == 0)
            {
                this.ReceiveExperience((uint)(attackReceiver.Level * 1.5 * attackReceiver.BonusPoints));
                Console.WriteLine(attackReceiver.Name + "has died!");
            }
        }

        public void ReceiveAttack(uint attackPoints)
        {
            uint damagePoints = attackPoints;

            if (this.IsDefending)
            {
                damagePoints -= this.DefensePoints;
                this.StopDefending();
            }
            else
                damagePoints -= this.EndurancePoints;

            this.ReceiveDamage(damagePoints);
        }

        public void StartDefending()
        {
            this.IsDefending = true;
        }

        public void StopDefending()
        {
            this.IsDefending = false;
        }

        public void ReceiveDamage(uint damagePoints)
        {
            this.HealthPoints -= Math.Min(damagePoints, this.HealthPoints);
        }

        public void HealDamage(uint healPoints)
        {
            this.HealthPoints += Math.Min(healPoints, this.MaxHealthPoints - this.HealthPoints);
        }

        public void ReceiveExperience(uint expPoints)
        {
            this.Experience += expPoints;

            if (this.RequiredExperiencePoints < this.Experience)
                this.LevelUp();
        }

        public void LevelUp()
        {
            this.Experience = 0;
            this.Level++;
            SetAllPointss();
            this.HealthPoints = this.MaxHealthPoints;
        }

        // Points Calculators
        protected virtual uint GetAttackPoints() => this.BaseAttackPoints +  (uint)(this.BonusPoints * (0.66));
        protected virtual uint GetEndurancePoints() => this.BaseEndurancePoints + this.BonusPoints / 3;
        protected virtual uint GetDefensePoints() => this.BaseDefensePoints + this.BonusPoints / 2;
        protected virtual uint GetMaxHealthPoints() => this.BaseMaxHealthPoints + (uint)(this.BonusPoints * (1.3));
        protected virtual uint GetRequiredExperiencePoints() => this.BonusPoints * 50;
        protected virtual uint GetBonusPoints()
        {
            uint bonus = this.BaseBonusPoints;
            for (uint i = 1; i <= this.Level; i++)
                bonus += i;
            return bonus;
        }

        protected void SetAllPointss()
        {
            this.BonusPoints = GetBonusPoints();

            this.AttackPoints = GetAttackPoints();
            this.EndurancePoints = GetEndurancePoints();
            this.DefensePoints = GetDefensePoints();
            this.RequiredExperiencePoints = GetRequiredExperiencePoints();
            this.MaxHealthPoints = GetMaxHealthPoints();
        }

    }
}