using System.Text;
using static Dio.MiniRPG.Helpers.Helpers;

namespace Dio.MiniRPG.View
{
    /// <summary>
    /// Static functions for generic i/o
    /// </summary>
    public static class BaseView
    {
        private const ConsoleColor DEFAULT_BG = ConsoleColor.Black;
        private const ConsoleColor DEFAULT_FG = ConsoleColor.White;

        private static (int width, int height) _dimensions = (120, 50);
        private static string _spritesFolder;
        private static string[] _lastMessages = new string[3].Populate(string.Empty);
        private static Pixel[,] _view = new Pixel[_dimensions.height, _dimensions.width];

        static BaseView()
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

        public static IEnumerable<string[]> GetAnimationFrames(string filename)
        {
            var lines = File.ReadAllLines($"{_spritesFolder}\\animations\\{filename}.txt");
            List<string> frame = new List<string>();
            foreach (string line in lines)
            {
                if (line == "")
                {
                    yield return frame.ToArray();
                    frame = new List<string>();
                }
                else frame.Add(line);
            }
        }

        public static (int width, int height) GetSpriteDimensions(this string[] sprite) =>
            (
                sprite.Longest().Length,
                sprite.Count()
            );

        
        public static void TempWrite(Pixel pixel, (int left, int top) coords)
        {
            if (coords.left >= _dimensions.width || coords.top >= _dimensions.height)
                return;

            Console.SetCursorPosition(coords.left, coords.top);

            Console.BackgroundColor = pixel.Background;
            Console.ForegroundColor = pixel.Foreground;

            Console.Write(pixel.Value);
        }

        public static void Write(Pixel pixel, (int left, int top) coords)
        {
            TempWrite(pixel, coords);
            pixel.Value = pixel.Value == '\n' ? ' ' : pixel.Value;
            _view[coords.top, coords.left] = pixel;
        }

        public static void Write(Pixel pixel) =>
            Write(pixel, (Console.GetCursorPosition()));

        public static void Write(
            string value,
            (int left, int top) coords,
            ConsoleColor bg = ConsoleColor.Black,
            ConsoleColor fg = ConsoleColor.White
        )
        {
            foreach (char c in value)
            {
                Write(new Pixel(c, bg, fg), coords);
                ++coords.left;
                if (coords.left >= _dimensions.width)
                {
                    coords.left = 0;
                    coords.top++;
                }
            }
        }

        public static void Write(
            string value,
            ConsoleColor bg = ConsoleColor.Black,
            ConsoleColor fg = ConsoleColor.White
        ) =>
            Write(value, (Console.GetCursorPosition()), bg, fg);

        public static Pixel Read((int left, int top) coords) =>
            _view[coords.top, coords.left];

        public static void PrintSprite(
            string[] sprite,
            (int left, int top) coords = default((int left, int top)),
            ConsoleColor bg = ConsoleColor.Black,
            ConsoleColor fg = ConsoleColor.White
        )
        {
            int i;
            for (i = 0; i < sprite.Count() - 1; i++)
            {
                Console.SetCursorPosition(coords.left, coords.top + i);
                Write(sprite[i] + "\n", bg, fg);
            }
            Console.SetCursorPosition(coords.left, coords.top + i);
            Write(sprite[i], bg, fg);
            ResetCursor();
        }

        public static bool PrepareConsole()
        {
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

            Console.SetWindowSize(_dimensions.width, _dimensions.height);

            Console.BackgroundColor = DEFAULT_BG;
            Console.ForegroundColor = DEFAULT_FG;

            Console.Clear();

            ResetCursor();

            PrintSprite(GetSprite("Frame"));

            return true;
        }

        public static void PrintMessage(string message)
        {
            var lines = message.SplitWordsBy(57);
            _lastMessages.Unshift(lines.ToArray());
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(3, _dimensions.height - 13 - i);
                Write(_lastMessages[i] + new string(' ', 57 - _lastMessages[i].Length) + "\n");
            }
            ResetCursor();
        }

        public static void PrintDescription(string description)
        {
            var lines = description.SplitWordsBy(56).ToArray();
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(62, _dimensions.height - 15 + i);
                Write(
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
                Write(
                    i < lines.Count()
                    ? lines[i].Truncate(52) + new string(' ', 58 - lines[i].Length)
                    : new string(' ', 58)
                );
            }
        }

        public static void ResetCursor() =>
            Console.SetCursorPosition(_dimensions.width - 1, _dimensions.height - 2);

        public static void PlayAnimation(
            IEnumerable<string[]> frames,
            (int left, int top) coords,
            ConsoleColor bg = ConsoleColor.Black,
            ConsoleColor fg = ConsoleColor.White,
            int duration = 200
        )
        {
            (int width, int height) = frames.First().GetSpriteDimensions();

            Pixel[,] overlayedArea = new Pixel[width, height];
            var emptyFrame = new string[height].Populate(new string(' ', width));
            foreach (string[] frame in frames.Append(emptyFrame))
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        TempWrite(
                            frame[i][j] != ' '
                                ? new Pixel(frame[i][j], bg, fg)
                                : _view[coords.top + i, coords.left + j],
                            (coords.left + j, coords.top + i)
                        );
                Thread.Sleep(duration / frame.Count());
            }
        }
    }
}