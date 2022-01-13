namespace Dio.MiniRPG.src.Infrastructure
{
    public interface ICharacter
    {
        /// <summary>
        /// Name of the character
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Base stat for damage dealt
        /// </summary>
        uint BaseAttackPoints { get; }
        /// <summary>
        /// Base stat for damage reduction when defending
        /// </summary>
        uint BaseDefensePoints { get; }
        /// <summary>
        /// Base stat for damage reduction when not defending
        /// </summary>
        uint BaseEndurancePoints { get; }
        /// <summary>
        /// Base stat for max health points
        /// </summary>
        uint BaseMaxHealthPoints { get; }
        /// <summary>
        /// Base stat for increments on other stats when leveling up
        /// </summary>
        uint BaseBonusPoints { get; }

        /// <summary>
        /// Current level of the character
        /// </summary>
        uint Level { get; }
        /// <summary>
        /// Current amount of experience points
        /// </summary>
        uint Experience { get; }

        /// <summary>
        /// Current attack stat
        /// </summary>
        uint AttackPoints { get; }
        /// <summary>
        /// Current defense stat
        /// </summary>
        uint DefensePoints { get; }
        /// <summary>
        /// Current endurance stat
        /// </summary>
        uint EndurancePoints { get; }
        /// <summary>
        /// Current max health stat
        /// </summary>
        uint MaxHealthPoints { get; }
        /// <summary>
        /// Current bonus stat
        /// </summary>
        uint BonusPoints { get; }
        /// <summary>
        /// Current amount of experience points required to level up
        /// </summary>
        uint RequiredExperiencePoints { get; }

        /// <summary>
        /// Current health stat
        /// </summary>
        uint HealthPoints { get; }
        /// <summary>
        /// True if the character is currently on the defensive stance
        /// </summary>
        bool IsDefending { get; }
        
        /// <summary>
        /// Attacks another character
        /// </summary>
        /// <param name="receiver">The character that will receive the attack</param>
        void Attack(ICharacter receiver);
        /// <summary>
        /// Calculates damage Points, decreases Health and stops defending
        /// </summary>
        /// <param name="attackPoints">Points of damage dealt by the attacker</param>
        void ReceiveAttack(uint attackPoints);
        /// <summary>
        /// Changes the character to defensive stance
        /// </summary>
        void StartDefending();
        /// <summary>
        /// Changes the character to default stance
        /// </summary>
        void StopDefending();
        /// <summary>
        /// Decrements the health of the character
        /// </summary>
        /// <param name="damagePoints">Points to be decremented of the health</param>
        void ReceiveDamage(uint damagePoints);
        /// <summary>
        /// Increments the health of the character
        /// </summary>
        /// <param name="healPoints">Points to be incremented on the helth</param>
        void HealDamage(uint healPoints);
        /// <summary>
        /// Increments the character experience points and levels the character up if possible
        /// </summary>
        /// <param name="expPoints">The amount of experience points to be incremented</param>
        void ReceiveExperience(uint expPoints);
        /// <summary>
        /// Levels the character up by one level
        /// </summary>
        void LevelUp();
    }
}