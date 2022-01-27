using Dio.MiniRPG.Infrastructure;

namespace Dio.MiniRPG.Helpers
{
    /// <summary>
    /// Static methods for displaying data and receiving user input
    /// </summary>
    public static class InterfaceHelpers
    {
        public struct Coords
        {
            public int Left { get; set; }
            public int Top { get; set; }

            public Coords(int left, int top)
            {
                Left = left;
                Top = top;
            }
        }

        private static (int width, int height) _dimensions = (120, 50);
        private static string _spritesFolder;
        private static Coords[] _characterCoords =
            {
                // Hero coords
                new Coords(25, 18),
                new Coords(35, 20),
                new Coords(25, 25),
                new Coords(35, 27),
                // Enemy coords
                new Coords(90, 18),
                new Coords(80, 20),
                new Coords(90, 25),
                new Coords(80, 27),
            };
        private static ICharacter[] _characters = new ICharacter[8];
        private static Coords[] _actionsCoords = new Coords[]
            {
                new Coords(68, 40),
                new Coords(94, 40),
                new Coords(68, 44),
                new Coords(94, 44),
            };
        private static ICharacterAction[] _actions = new ICharacterAction[4];
        private static string[] _lastMessages = new string[3].Populate(string.Empty);

        static InterfaceHelpers()
        {
            var i = AppContext.BaseDirectory.ToString().IndexOf("Dio.MiniRPG\\") + 12;
            var dir = AppContext.BaseDirectory.ToString().Substring(0, i);
            _spritesFolder = Path.Combine(
                AppContext.BaseDirectory.ToString()
                    .Substring(0, AppContext.BaseDirectory.ToString().IndexOf("Dio.MiniRPG\\") + 12),
                $"assets\\sprites"
            );
        }

        public static string[] GetSprite(string fileName) =>
            File.ReadAllLines($"{_spritesFolder}\\{fileName}.txt");

        public static (int width, int height) GetSpriteDimensions(string[] sprite) =>
            (
                sprite.Aggregate(string.Empty, (curr, n) => n.Length > curr.Length ? n : curr).Length,
                sprite.Count()
            );

        public static void PrintSprite(string[] sprite, int left = 0, int top = 0)
        {
            int i;
            for (i = 0; i < sprite.Count() - 1; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.WriteLine(sprite[i]);
            }
            Console.SetCursorPosition(left, top + i);
            Console.Write(sprite[i]);
            ResetCursor();
        }

        public static void PrintSprite(this ICharacter character, int left = 0, int top = 0) =>
            PrintSprite(character.Sprite, left, top);

        public static bool PrepareConsole()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            if (Console.LargestWindowWidth < _dimensions.width || Console.LargestWindowHeight < _dimensions.height)
            {
                Console.WriteLine(
                    "The program is unable to draw its GUI with your current " +
                    "font settings. Please change your configurations to allow " +
                    $"at least {_dimensions.width} columns and {_dimensions.height} rows and try again =D"
                );
                return false;
            }

            Console.SetWindowSize(_dimensions.width, _dimensions.height);
            Console.Clear();
            PrintSprite(GetSprite("Frame"));
            Console.SetCursorPosition(0, 9);

            ResetCursor();
            return true;
        }

        public static void PrintCharacter(this ICharacter character, int index, bool isSelected = false)
        {
            _characters[index] = character;

            if (isSelected)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            var hp = Math.Ceiling(character.HP);
            if (hp == 0) Console.ForegroundColor = ConsoleColor.Red;

            PrintSprite(character, _characterCoords[index].Left, _characterCoords[index].Top);

            Console.SetCursorPosition(
                _characterCoords[index].Left,
                _characterCoords[index].Top + GetSpriteDimensions(character.Sprite).height
            );

            Console.Write($"HP {hp}{new string(' ', 3 - hp.ToString().Length)}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            ResetCursor();
        }

        public static void PrintCharacter(this ICharacter character, bool isSelected = false) =>
            character.PrintCharacter(Array.FindIndex(_characters, (c) => c == character), isSelected);

        public static void SelectCharacter(int index) =>
            _characters[index].PrintCharacter(index, true);

        public static void SelectCharacter(ICharacter character) =>
            SelectCharacter(Array.FindIndex(_characters, (c) => c == character));

        public static void UnselectCharacter(int index) =>
            _characters[index].PrintCharacter(index, false);

        public static void UnselectCharacter(ICharacter character) =>
            UnselectCharacter(Array.FindIndex(_characters, (c) => c == character));

        public static ICharacter GetTarget()
        {
            var aliveTargets = _characters.Skip(4).Where((c) => c != null && !c.IsDead).ToArray();
            int selected = 0, newSelected = 0;
            SelectCharacter(aliveTargets[selected]);
            
            Helpers.Flush();
            ConsoleKey input = Console.ReadKey(true).Key;

            while (input != ConsoleKey.Enter)
            {
                if (input == ConsoleKey.UpArrow)
                    newSelected = selected - 1;

                else if (input == ConsoleKey.DownArrow)
                    newSelected = selected + 1;

                if (newSelected >= 0 && newSelected < aliveTargets.Count())
                {
                    UnselectCharacter(aliveTargets[selected]);
                    SelectCharacter(aliveTargets[newSelected]);
                    selected = newSelected;
                }

                Helpers.Flush();
                input = Console.ReadKey(true).Key;
            }
            UnselectCharacter(selected);
            ResetCursor();
            return aliveTargets[selected];
        }

        public static void PrintDungeonLevel(int level, IHero[] heroes, IEnemy[] enemies)
        {
            PrintSprite(GetSprite("DungeonLevel"), 24, 10);

            var numberSprites = GetSprite("Numbers");
            var tensSprite = numberSprites.Skip(level / 10 * 3).Take(3).ToArray();
            var unitsSprite = numberSprites.Skip(level % 10 * 3).Take(3).ToArray();
            PrintSprite(unitsSprite, 89, 10);
            PrintSprite(tensSprite, 85, 10);

            for (int i = 0; i < 4; i++)
            {
                if (heroes.Count((h) => h != null) > i)
                    PrintCharacter(heroes[i], i);

                if (enemies.Count((e) => e != null) > i)
                    PrintCharacter(enemies[i], i + 4);
            }

            ResetCursor();

        }

        public static void PrintMessage(string message)
        {
            var lines = message.SplitWordsBy(67);
            _lastMessages.Unshift(lines.ToArray());
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(3, _dimensions.height - 13 - i);
                Console.WriteLine(_lastMessages[i] + new string(' ', 66 - _lastMessages[i].Length));
            }
            ResetCursor();
        }

        public static void PrintDescription(string description)
        {
            var lines = description.SplitWordsBy(50).ToArray();
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(68, _dimensions.height - 15 + i);
                Console.Write(
                    lines.Length > i
                    ? lines[i].Trim() + new string(' ', 50 - lines[i].Length)
                    : new string(' ', 50)
                );
            }
            ResetCursor();
        }

        public static void PrintHeroInfo(int position) => PrintHeroInfo((IHero)_characters[position]);

        public static void PrintHeroInfo(IHero character)
        {
            Console.SetCursorPosition(3, _dimensions.height - 9);
            Console.Write(
                $"\t{character.Name} {character.HeroType.ToString().ToUpper()}\n\n" +
                $"█\tLVL: \t{character.LVL}\t" +
                $" EXP: \t{character.EXP} / {character.RequiredEXP}\n" +
                $"█\tHP: \t{Math.Ceiling(character.HP)} / {Math.Ceiling(character.MaxHP)}\t" +
                $" ATK: \t{Math.Ceiling(character.ATK)}\n" +
                $"█\tDEF: \t{Math.Ceiling(character.DEF)}\t" +
                $" END: \t{Math.Ceiling(character.END)}\n"
            );

            _actions = character.CharacterActionsList.ToArray();
            for (int i = 0; i < _actions.Count((a) => a != null); i++)
                PrintAction(i);

            ResetCursor();
        }

        public static ICharacterAction GetAction()
        {
            var selected = 0;
            SelectAction(selected);

            Helpers.Flush();
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
                    newSelected < _actions.Count((a) => a != null)
                )
                {
                    UnselectAction(selected);
                    SelectAction(newSelected);
                    selected = newSelected;
                }

                input = Console.ReadKey(true).Key;
                ResetCursor();
            }

            UnselectAction(selected);
            return _actions[selected];
        }

        private static void PrintAction(int index, bool isSelected = false)
        {
            var action = _actions[index];

            if (isSelected)
                PrintDescription(action.Description);

            Console.BackgroundColor = isSelected ? ConsoleColor.DarkGray : ConsoleColor.White;
            Console.ForegroundColor = isSelected ? ConsoleColor.White : ConsoleColor.Black;
            for (int j = 0; j < 3; j++)
            {
                Console.SetCursorPosition(_actionsCoords[index].Left, _actionsCoords[index].Top + j);
                Console.Write(new string(' ', 20));
            }
            Console.SetCursorPosition(_actionsCoords[index].Left + (20 - action.Name.Length) / 2, _actionsCoords[index].Top + 1);
            Console.Write(new string(action.Name));

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            ResetCursor();
        }

        private static void SelectAction(int index)
        {
            PrintAction(index, true);
            PrintDescription(_actions[index].Description);
        }
        
        private static void UnselectAction(int index)
        {
            PrintAction(index, false);
            PrintDescription("");
        }

        private static void ResetCursor() => Console.SetCursorPosition(_dimensions.width - 1, _dimensions.height - 2);
    }
}