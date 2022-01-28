using Dio.MiniRPG.Infrastructure;

namespace Dio.MiniRPG.Helpers
{
    /// <summary>
    /// Static methods for displaying data and receiving user input
    /// </summary>
    public static class ViewHelpers
    {
        private static (int width, int height) _dimensions = (120, 50);
        private static string _spritesFolder;
        private static string[] _lastMessages = new string[3].Populate(string.Empty);

        static ViewHelpers()
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

            if (!OperatingSystem.IsWindows())
            {
                Console.WriteLine(
                    "This program needs to a console with exactly 120 columns and at least 50 rows" +
                    "to render properly. Please resize your window and run the program again =D" +
                    "Tip: $ resize -s 50 120"
                );
                return false;
            }
            else Console.SetWindowSize(_dimensions.width, _dimensions.height);


            Console.Clear();
            PrintSprite(GetSprite("Frame"));
            Console.SetCursorPosition(0, 9);

            ResetCursor();
            return true;
        }

        public static void PrintMessage(string message)
        {
            var lines = message.SplitWordsBy(57);
            _lastMessages.Unshift(lines.ToArray());
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(3, _dimensions.height - 13 - i);
                Console.WriteLine(_lastMessages[i] + new string(' ', 57 - _lastMessages[i].Length));
            }
            ResetCursor();
        }

        public static void PrintDescription(string description)
        {
            var lines = description.SplitWordsBy(56).ToArray();
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(62, _dimensions.height - 15 + i);
                Console.Write(
                    lines.Length > i
                    ? lines[i].Trim() + new string(' ', 56 - lines[i].Length)
                    : new string(' ', 56)
                );
            }
            ResetCursor();
        }

        public static void PrintInfo(string info)
        {
            var lines = info.Split("\n");
            for (int i = 0; i < 7; i++)
            {
                Console.SetCursorPosition(3, _dimensions.height - 10 + i);
                Console.Write(
                    i < lines.Count()
                    ? lines[i].Truncate(52) + new string(' ', 58 - lines[i].Length)
                    : new string(' ', 58)
                );
            }
        }

        public static void ResetCursor()
        {
            Console.SetCursorPosition(_dimensions.width - 1, _dimensions.height - 2);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}