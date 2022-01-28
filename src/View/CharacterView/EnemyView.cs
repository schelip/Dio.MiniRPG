using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.Helpers;
using static Dio.MiniRPG.Helpers.ViewHelpers;

namespace Dio.MiniRPG.View
{
    public static partial class CharacterView
    {
        private static IDictionary<Coords, IEnemy?> _enemies =
            new Dictionary<Coords, IEnemy?>
            {
                { new Coords(90, 18), null },
                { new Coords(80, 20), null },
                { new Coords(90, 25), null },
                { new Coords(80, 27), null }
            };

        public static void PrintCharacter(this IEnemy enemy)
        {
            var coords = _enemies.GetCoords(enemy);
            enemy.PrintCharacter(coords);
            _enemies[coords] = enemy;
        }

        public static void SelectCharacter(this IEnemy enemy) =>
            enemy.HighlightCharacter(_enemies.GetCoords(enemy));

        public static void DeselectCharacter(this IEnemy enemy) =>
            enemy.UnhighlightCharacter(_enemies.GetCoords(enemy));

        public static IEnemy GetTarget()
        {
            var aliveTargets = (
                from e in _enemies.Values
                where e != null && !e.IsDead
                select e
            ).ToList();

            if (aliveTargets.Count() == 0)
                throw new InvalidOperationException("There are no available targets");

            int selected = 0, newSelected = 0;
            aliveTargets[selected].SelectCharacter();

            Flush();
            ConsoleKey input = Console.ReadKey(true).Key;

            while (input != ConsoleKey.Enter)
            {
                if (input == ConsoleKey.UpArrow)
                    newSelected = selected - 1;

                else if (input == ConsoleKey.DownArrow)
                    newSelected = selected + 1;

                if (newSelected >= 0 && newSelected < aliveTargets.Count())
                {
                    aliveTargets[selected].DeselectCharacter();
                    SelectCharacter(aliveTargets[newSelected]);
                    selected = newSelected;
                }

                Flush();
                input = Console.ReadKey(true).Key;
            }
            aliveTargets[selected].DeselectCharacter();
            ResetCursor();
            return aliveTargets[selected];
        }
    }
}