using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.Helpers;
using static Dio.MiniRPG.View.BaseView;

namespace Dio.MiniRPG.View
{
    /// <summary>
    /// Functions specific to printing enemies
    /// </summary>
    public static partial class CharacterView
    {
        /// <summary>
        /// The possible coordinates for printing enemies
        /// </summary>
        private static IDictionary<(int left, int top), IEnemy?> _enemies =
            new Dictionary<(int left, int top), IEnemy?>
            {
                { (90, 18), null },
                { (80, 20), null },
                { (90, 25), null },
                { (80, 27), null }
            };

        /// <summary>
        /// Prints an enemy in the appropriate coordinates
        /// </summary>
        /// <param name="enemy">The enemy to be printed</param>
        public static void PrintCharacter(this IEnemy enemy)
        {
            var coords = _enemies.GetCoords(enemy);
            enemy.PrintCharacter(coords);
            _enemies[coords] = enemy;
        }

        /// <summary>
        /// Highlights a printed enemy
        /// </summary>
        /// <param name="enemy">The enemy to be selected</param>
        public static void SelectCharacter(this IEnemy enemy) =>
            enemy.HighlightCharacter(_enemies.GetCoords(enemy));

        /// <summary>
        /// Unhighlights a printed enemy
        /// </summary>
        /// <param name="enemy">The enemy to be deselected</param>
        public static void DeselectCharacter(this IEnemy enemy) =>
            enemy.UnhighlightCharacter(_enemies.GetCoords(enemy));

        /// <summary>
        /// Receives user input to select a target between the currently alive printed enemies
        /// </summary>
        /// <returns>The selected target</returns>
        /// <exception cref="InvalidOperationException">If there are no available targets</exception>
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