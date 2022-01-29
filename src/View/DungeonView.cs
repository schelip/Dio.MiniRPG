using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.View.BaseView;

namespace Dio.MiniRPG.View
{
    public static class DungeonView
    {
        /// <summary>
        /// Prints the dungeon title, the heroes on the party and the enemies from the level
        /// </summary>
        /// <param name="level">The number of the dungeon level</param>
        /// <param name="heroes">The heroes on the party</param>
        /// <param name="enemies">The enemies of the level</param>
        public static void PrintDungeonLevel(int level, IList<IHero> heroes, IList<IEnemy> enemies)
        {
            PrintSprite(GetSprite("DungeonLevel"), (24, 10));

            var numberSprites = GetSprite("Numbers");
            var tensSprite = numberSprites.Skip(level / 10 * 3).Take(3).ToArray();
            var unitsSprite = numberSprites.Skip(level % 10 * 3).Take(3).ToArray();
            PrintSprite(unitsSprite, (89, 10));
            PrintSprite(tensSprite, (85, 10));

            foreach (var hero in heroes)
                hero.PrintCharacter();

            foreach (var enemy in enemies)
                enemy.PrintCharacter();

            ResetCursor();
        }
    }
}