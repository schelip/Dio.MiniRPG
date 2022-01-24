namespace Dio.MiniRPG.Infrastructure
{
    public interface ICharacter
    {
        /// <summary>
        /// Name of the character
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Current level of the character
        /// </summary>
        uint LVL { get; }
        /// <summary>
        /// Current amount of experience points
        /// </summary>
        uint EXP { get; }
        /// <summary>
        /// Current health points
        /// </summary>
        double HP { get; }
        /// <summary>
        /// Current max health stat
        /// </summary>
        double MaxHP { get; }
        /// <summary>
        /// Current attack stat
        /// </summary>
        double ATK { get; }
        /// <summary>
        /// Current defense stat
        /// </summary>
        double DEF { get; }
        /// <summary>
        /// Current endurance stat
        /// </summary>
        double END { get; }
        /// <summary>
        /// True if the character is currently in the defense stance
        /// </summary>
        bool IsDefending { get; }
        /// <summary>
        /// True if the character has 0 HP
        /// </summary>
        bool IsDead { get; }
        /// <summary>
        /// A List of all actions that the character is currently able to perform
        /// </summary>
        List<ICharacterAction> CharacterActionsList { get; }

        
        /// <summary>
        /// Executes an action of the hero's IList
        /// </summary>
        /// <param name="index">The index of the action meant to be performed</param>
        /// <param name="targets">The targets on which the action will be performed</param>
        bool Act(int index, params ICharacter[] targets);
        /// <summary>
        /// Decrements the health of the character
        /// </summary>
        /// <param name="damagePoints">Points to be decremented of the health</param>
        bool ReceiveDamage(double damagePoints);
        /// <summary>
        /// Increments the health of the character
        /// </summary>
        /// <param name="healPoints">Points to be incremented on the health</param>
        bool HealDamage(double healPoints);
        /// <summary>
        /// Changes the character to defensive stance
        /// </summary>
        bool StartDefending();
        /// <summary>
        /// Changes the character to default stance
        /// </summary>
        bool StopDefending();
        /// <summary>
        /// Increments the character experience points and levels the character up if possible
        /// </summary>
        /// <param name="expPoints">The amount of experience points to be incremented</param>
        bool ReceiveExperience(uint expPoints);
    }
}