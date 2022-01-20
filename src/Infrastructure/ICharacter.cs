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
        /// Decrements the health of the character
        /// </summary>
        /// <param name="damagePoints">Points to be decremented of the health</param>
        void ReceiveDamage(double damagePoints);
        /// <summary>
        /// Increments the health of the character
        /// </summary>
        /// <param name="healPoints">Points to be incremented on the helth</param>
        void HealDamage(double healPoints);
        /// <summary>
        /// Changes the character to defensive stance
        /// </summary>
        void StartDefending();
        /// <summary>
        /// Changes the character to default stance
        /// </summary>
        void StopDefending();
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