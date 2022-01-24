namespace Dio.MiniRPG.Infrastructure
{
    public interface IEnemy : ICharacter
    {
        /// <summary>
        /// Decides which is the best action at the time base on the heroes and enemies and executes it
        /// </summary>
        /// <param name="heroes">The heroes the enemy is fighting against</param>
        /// <param name="enemies">The other enemies on this enemy's party</param>
        void Act(IHero[] heroes, IEnemy[] enemies);
    }
}