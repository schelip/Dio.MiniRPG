namespace Dio.MiniRPG.Infrastructure
{
    public interface IDungeonLevel
    {
        /// <summary>
        /// The cardinal value of the level
        /// </summary>
        uint Level { get; }
        /// <summary>
        /// An array of the enemies in the level
        /// </summary>
        IEnemy[] Enemies { get; }

        /// <summary>
        /// Starts the level
        /// </summary>
        void Start();
        /// <summary>
        /// Ends the level
        /// </summary>
        void Clear();
    }
}