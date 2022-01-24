using System.Linq;
using Dio.MiniRPG.Infrastructure;

namespace Dio.MiniRPG.Entities
{
    public class DungeonLevel : IDungeonLevel
    {
        public uint Level { get; }
        public IEnemy[] Enemies { get; }

        public DungeonLevel(uint level)
        {
            this.Level = level;
            this.Enemies = new IEnemy[] { };
        }

        public DungeonLevel(uint level, IEnemy[] enemies)
        {
            this.Level = level;
            this.Enemies = enemies;
        }

        public void Start()
        {
            Console.WriteLine($"You have entered Dungeon Level {this.Level}");
            var aliveEnemies = this.Enemies.Where((e) => !e.IsDead).ToList();
            GameLogic.ResetTurns();
            while (this.Enemies.Any((e) => !e.IsDead))
            {
                GameLogic.ReceivePlayerAction(aliveEnemies);
                GameLogic.NextTurn();
                aliveEnemies = this.Enemies.Where((e) => !e.IsDead).ToList();
                foreach (var enemy in aliveEnemies)
                    enemy.Act(GameLogic.Party.ToArray(), aliveEnemies.ToArray());
                GameLogic.NextTurn();
            }

            Clear();
        }

        public void Clear()
        {
            Console.WriteLine($"Dungeon Level {this.Level} complete!");
        }
    }
}