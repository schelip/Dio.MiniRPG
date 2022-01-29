using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.Helpers;
using static Dio.MiniRPG.View.BaseView;

namespace Dio.MiniRPG.View
{
    /// <summary>
    /// The functions specific to printing heroes
    /// </summary>
    public static partial class CharacterView
    {
        /// <summary>
        /// The available coordinates for heroes to be printed
        /// </summary>
        private static IDictionary<(int left, int top), IHero?> _heroes =
            new Dictionary<(int left, int top), IHero?>
            {
                { (25, 18), null },
                { (35, 20), null },
                { (25, 25), null },
                { (35, 27), null },
            };
        /// <summary>
        /// The available coordinates for a hero's actions to be printed
        /// </summary>
        private static IDictionary<(int left, int top), ICharacterAction?> _heroActions =
            new Dictionary<(int left, int top), ICharacterAction?>
            {
                { (64, 40), null },
                { (90, 40), null },
                { (64, 44), null },
                { (90, 44), null },
            };

        /// <summary>
        /// Prints a hero in the appropriate coordinates
        /// </summary>
        /// <param name="hero">The hero to be printed</param>
        public static void PrintCharacter(this IHero hero)
        {
            var coords = _heroes.GetCoords(hero);
            _heroes[coords] = hero;
            hero.PrintCharacter(coords);
        }

        /// <summary>
        /// Highlights a hero and prints its info and actions
        /// </summary>
        /// <param name="hero">The hero to be selected</param>
        public static void SelectCharacter(this IHero hero)
        {
            var coords = _heroes.GetCoords(hero);

            hero.HighlightCharacter(coords);
            PrintInfo(
                $"\t{hero.Name} {hero.HeroType.ToString().ToUpper()}\n\n" +
                $"\tLVL: \t{hero.LVL}" +
                $"\tEXP: \t{hero.EXP} / {hero.RequiredEXP}\n" +
                $"\tHP: \t{Math.Ceiling(hero.HP)} / {Math.Ceiling(hero.MaxHP)}" +
                $"\tATK: \t{Math.Ceiling(hero.ATK)}\n" +
                $"\tDEF: \t{Math.Ceiling(hero.DEF)}" +
                $"\tEND: \t{Math.Ceiling(hero.END)}\n"
            );

            foreach (var action in hero.CharacterActionsList)
                action.PrintAction();
            if (hero.CharacterActionsList.Count() > 0) SelectAction(0);
        }

        /// <summary>
        /// Unhighlights a hero and clears its info and actions from the screen
        /// </summary>
        /// <param name="hero"></param>
        public static void DeselectCharacter(this IHero hero)
        {
            var coords = _heroes.FirstOrDefault((e) => e.Value == hero).Key;
            hero.UnhighlightCharacter(coords);
            ResetActions();
        }

        /// <summary>
        /// Receives user input to select an action between a hero's actions list
        /// </summary>
        /// <returns>The selected action</returns>
        /// <exception cref="InvalidOperationException">If the hero has no actions</exception>
        public static ICharacterAction GetAction()
        {
            var count = _heroActions.Count((e) => e.Value != null);
            if (count == 0)
                throw new InvalidOperationException("The hero has no actions");

            var selected = 0;
            var action = SelectAction(selected);

            Flush();
            var input = Console.ReadKey(true).Key;

            while (input != ConsoleKey.Enter)
            {
                int newSelected = selected;

                switch (input)
                {
                    case ConsoleKey.RightArrow:
                        newSelected++;
                        break;

                    case ConsoleKey.LeftArrow:
                        newSelected--;
                        break;

                    case ConsoleKey.DownArrow:
                        newSelected += 2;
                        break;

                    case ConsoleKey.UpArrow:
                        newSelected -= 2;
                        break;
                }

                if (
                    newSelected != selected &&
                    newSelected >= 0 &&
                    newSelected < count
                )
                {
                    UnselectAction(selected);
                    action = SelectAction(newSelected);
                    selected = newSelected;
                }

                input = Console.ReadKey(true).Key;
                ResetCursor();
            }

            UnselectAction(selected);
            return action;
        }

        /// <summary>
        /// Prints a hero's action in the appropriate coordinates
        /// </summary>
        /// <param name="action">The action to be printed</param>
        /// <param name="isSelected">True if the action should be highlighted</param>
        private static void PrintAction(this ICharacterAction action, bool isSelected = false)
        {
            var coords = _heroActions.GetCoords(action);

            if (isSelected)
                PrintDescription(action.Description);

            var bg = isSelected ? ConsoleColor.DarkGray : ConsoleColor.White;
            var fg = isSelected ? ConsoleColor.White : ConsoleColor.Black;
            for (int j = 0; j < 3; j++)
            {
                Console.SetCursorPosition(coords.left, coords.top + j);
                Write(new string(' ', 20), bg, fg);
            }
            Console.SetCursorPosition(coords.left + (20 - action.Name.Length) / 2, coords.top + 1);
            Write(new string(action.Name), bg, fg);

            _heroActions[coords] = action;
            ResetCursor();
        }

        /// <summary>
        /// Clears the actions from the coordinates map
        /// </summary>
        private static void ResetActions()
        {
            foreach (var key in _heroActions.Keys)
                _heroActions[key] = null;
        }

        /// <summary>
        /// Highlights a printed action and prints its description
        /// </summary>
        /// <param name="index">Which of the actions should be selected</param>
        /// <returns>The action that was selected</returns>
        /// <exception cref="ArgumentException">If the index was invalid</exception>
        private static ICharacterAction SelectAction(int index)
        {
            var action = _heroActions.ElementAt(index).Value
                ?? throw new ArgumentException();

            action.PrintAction(true);
            PrintDescription(action.Description);

            return action;
        }

        /// <summary>
        /// Unhighlits a printed action and clears its description
        /// </summary>
        /// <param name="index">Which of the actions should be unselected</param>
        /// <exception cref="ArgumentException">If the index was invalid</exception>
        private static void UnselectAction(int index)
        {
            var action = _heroActions.ElementAt(index).Value
                ?? throw new ArgumentException();

            action.PrintAction(false);
            PrintDescription("");
        }
    }
}