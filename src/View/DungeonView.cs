using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.ViewHelpers;

namespace Dio.MiniRPG.View
{
    public static class DungeonView
    {
        public static void PrintDungeonLevel(int level, IList<IHero> heroes, IList<IEnemy> enemies)
        {
            PrintSprite(GetSprite("DungeonLevel"), 24, 10);

            var numberSprites = GetSprite("Numbers");
            var tensSprite = numberSprites.Skip(level / 10 * 3).Take(3).ToArray();
            var unitsSprite = numberSprites.Skip(level % 10 * 3).Take(3).ToArray();
            PrintSprite(unitsSprite, 89, 10);
            PrintSprite(tensSprite, 85, 10);

            foreach (var hero in heroes)
                hero.PrintCharacter();

            foreach (var enemy in enemies)
                enemy.PrintCharacter();

            ResetCursor();
        }
    }
}