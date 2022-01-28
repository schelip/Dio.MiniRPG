using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.Helpers;
using static Dio.MiniRPG.Helpers.ViewHelpers;

namespace Dio.MiniRPG.View
{
    public static partial class CharacterView
    {
        private static IDictionary<Coords, IHero?> _heroes =
            new Dictionary<Coords, IHero?>
            {
                { new Coords(25, 18), null },
                { new Coords(35, 20), null },
                { new Coords(25, 25), null },
                { new Coords(35, 27), null },
            };
        private static IDictionary<Coords, ICharacterAction?> _actions =
            new Dictionary<Coords, ICharacterAction?>
            {
                { new Coords(64, 40), null },
                { new Coords(90, 40), null },
                { new Coords(64, 44), null },
                { new Coords(90, 44), null },
            };

        public static void PrintCharacter(this IHero hero)
        {
            var coords = _heroes.GetCoords(hero);
            _heroes[coords] = hero;
            hero.PrintCharacter(coords);
        }

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

        public static void DeselectCharacter(this IHero hero)
        {
            var coords = _heroes.FirstOrDefault((e) => e.Value == hero).Key;
            hero.UnhighlightCharacter(coords);
            ResetActions();
        }

        public static ICharacterAction GetAction()
        {
            var count = _actions.Count((e) => e.Value != null);
            if (count == 0)
                throw new InvalidOperationException("The character has no actions");

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

        private static void PrintAction(this ICharacterAction action, bool isSelected = false)
        {
            var coords = _actions.GetCoords(action);

            if (isSelected)
                PrintDescription(action.Description);

            Console.BackgroundColor = isSelected ? ConsoleColor.DarkGray : ConsoleColor.White;
            Console.ForegroundColor = isSelected ? ConsoleColor.White : ConsoleColor.Black;
            for (int j = 0; j < 3; j++)
            {
                Console.SetCursorPosition(coords.Left, coords.Top + j);
                Console.Write(new string(' ', 20));
            }
            Console.SetCursorPosition(coords.Left + (20 - action.Name.Length) / 2, coords.Top + 1);
            Console.Write(new string(action.Name));

            _actions[coords] = action;
            ResetCursor();
        }

        private static void ResetActions()
        {
            foreach (var key in _actions.Keys)
                _actions[key] = null;
        }

        private static ICharacterAction SelectAction(int index)
        {
            var action = _actions.ElementAt(index).Value
                ?? throw new IndexOutOfRangeException();
            
            action.PrintAction(true);
            PrintDescription(action.Description);
            
            return action;
        }

        private static void UnselectAction(int index)
        {
            var action = _actions.ElementAt(index).Value
                ?? throw new IndexOutOfRangeException();
            
            action.PrintAction(false);
            PrintDescription("");
        }
    }
}