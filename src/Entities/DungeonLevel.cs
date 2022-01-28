using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.View.DungeonView;
using static Dio.MiniRPG.Helpers.ViewHelpers;

namespace Dio.MiniRPG.Entities
{
    public class DungeonLevel : IDungeonLevel
    {
        public int Level { get; }
        public IEnemy[] Enemies { get; }

        public DungeonLevel(int level)
        {
            this.Level = level;
            this.Enemies = new IEnemy[] { };
        }

        public DungeonLevel(int level, IEnemy[] enemies)
        {
            this.Level = level;
            this.Enemies = enemies;
        }

        public void Start()
        {
            PrintMessage($"You have entered Dungeon Level {this.Level}!");
            PrintDungeonLevel(this.Level, GameLogic.Party.ToArray(), this.Enemies);

            var aliveEnemies = this.Enemies.Where((e) => !e.IsDead).ToList();
            GameLogic.ResetTurns();
            while (this.Enemies.Any((e) => !e.IsDead))
            {
                GameLogic.ReceivePlayerAction(aliveEnemies);
                GameLogic.NextTurn();
                aliveEnemies = this.Enemies.Where((e) => !e.IsDead).ToList();
                foreach (var enemy in aliveEnemies)
                {
                    Thread.Sleep(500);
                    enemy.Act(GameLogic.Party.ToArray(), aliveEnemies.ToArray());
                }
                GameLogic.NextTurn();
            }

            Clear();
        }

        public void Clear()
        {
            PrintMessage($"Dungeon Level {this.Level} complete!");
        }
    }
}