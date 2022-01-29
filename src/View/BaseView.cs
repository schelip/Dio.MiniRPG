using System.Text;
using static Dio.MiniRPG.Helpers.Helpers;

namespace Dio.MiniRPG.View
{
    /// <summary>
    /// Static functions for IO
    /// </summary>
    public static class BaseView
    {
        /// <summary>
        /// The console dimensions where the interface will be drawn
        /// </summary>
        private static (int width, int height) _dimensions = (120, 50);
        /// <summary>
        /// The folder where the sprites are being stored
        /// </summary>
        private static string _spritesFolder;
        /// <summary>
        /// The messages that were last printed on the "Messages" section
        /// </summary>
        private static string[] _lastMessages = new string[3].Populate(string.Empty);
        /// <summary>
        /// A matrix storing a copy of the information printed to the console, in order to be able to print overlays.
        /// </summary>
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

        /// <summary>
        /// Reads all the line of a file containing a sprite
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>The lines of the sprite</returns>
        public static string[] GetSprite(string fileName) =>
            File.ReadAllLines($"{_spritesFolder}\\{fileName}.txt");

        /// <summary>
        /// Returns an Enumrable of animation frames separated by empty lines.
        /// </summary>
        /// <param name="filename">The file containing</param>
        /// <returns>An iterator for the animation frames</returns>
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

        /// <summary>
        /// Returns the dimensions for a sprite
        /// </summary>
        /// <param name="sprite">The sprite of which dimensions wll be returned</param>
        /// <returns>The dimensions of the sprite</returns>
        public static (int width, int height) GetSpriteDimensions(this string[] sprite) =>
            (
                sprite.Longest().Length,
                sprite.Count()
            );

        /// <summary>
        /// Prints a Pixel on the console
        /// </summary>
        /// <param name="pixel">The Pixel to be printed</param>
        /// <param name="coords">The coordinates where to print</param>
        public static void TempWrite(Pixel pixel, (int left, int top) coords)
        {
            if (coords.left >= _dimensions.width || coords.top >= _dimensions.height)
                return;

            Console.SetCursorPosition(coords.left, coords.top);

            Console.BackgroundColor = pixel.Background;
            Console.ForegroundColor = pixel.Foreground;

            Console.Write(pixel.Value);
        }

        /// <summary>
        /// Prints a Pixel on the console and saves it to the view matrix
        /// </summary>
        /// <param name="pixel">The pixel to be printed</param>
        /// <param name="coords">The coordinates where to print</param>
        public static void Write(Pixel pixel, (int left, int top) coords)
        {
            TempWrite(pixel, coords);
            pixel.Value = pixel.Value == '\n' ? ' ' : pixel.Value;
            _view[coords.top, coords.left] = pixel;
        }

        /// <summary>
        /// Prints a pixel on the current cursor position and saves it to the view matrix
        /// </summary>
        /// <param name="pixel">The pixel to be printed</param>
        public static void Write(Pixel pixel) =>
            Write(pixel, (Console.GetCursorPosition()));

        /// <summary>
        /// Prints a string to the console and saves the pixels on the view matrix
        /// </summary>
        /// <param name="value">The string to be printed</param>
        /// <param name="coords">The coordinates where to print the string</param>
        /// <param name="bg">The background color while printing</param>
        /// <param name="fg">The foreground color while printing</param>
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

        /// <summary>
        /// Prints a string to the console on the current cursor position and saves the
        /// pixels on the view matrix
        /// </summary>
        /// <param name="value">The string to be printed</param>
        /// <param name="bg">The background color while printing</param>
        /// <param name="fg">The foreground color while printing</param>
        public static void Write(
            string value,
            ConsoleColor bg = ConsoleColor.Black,
            ConsoleColor fg = ConsoleColor.White
        ) =>
            Write(value, (Console.GetCursorPosition()), bg, fg);

        /// <summary>
        /// Retrieves a pixel from the view matrix
        /// </summary>
        /// <param name="coords">The coordinates where to retrieve the pixel from</param>
        /// <returns></returns>
        public static Pixel Read((int left, int top) coords) =>
            _view[coords.top, coords.left];

        /// <summary>
        /// Prints a sprite on the console and saves it on the view matrix
        /// </summary>
        /// <param name="sprite">The sprite to be printed</param>
        /// <param name="coords">The coordinates where to print the sprite</param>
        /// <param name="bg">The background color while printing</param>
        /// <param name="fg">The foreground color while printing</param>
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

        /// <summary>
        /// Checks wheter the console is adequate for running the application, resizes it if possible
        /// and prints the mains frame
        /// </summary>
        /// <returns>True if the console is adequate for running the application</returns>
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

            if (
                (Console.WindowWidth != _dimensions.width || Console.WindowHeight < _dimensions.height)
                && !OperatingSystem.IsWindows())
            {
                Console.WriteLine(
                    "This program needs to a console with exactly 120 columns and at least 50 rows" +
                    "to render properly. Please resize your window and run the program again =D" +
                    "Tip: $ resize -s 50 120"
                );
                return false;
            }

            Console.SetWindowSize(_dimensions.width, _dimensions.height);

            Console.Clear();

            ResetCursor();

            PrintSprite(GetSprite("Frame"));

            return true;
        }

        /// <summary>
        /// Prints on the "Messages" console section.
        /// Should be used to print messages about what is happening in-game.
        /// </summary>
        /// <param name="message">The message to be printed</param>
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

        /// <summary>
        /// Prints on the "Description" console section.
        /// Should be used to print descriptions when the user is selecting an option
        /// </summary>
        /// <param name="description">The description to be printed</param>
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

        /// <summary>
        /// Prints on the "Info" console section.
        /// Should be used to print detailed information about characters
        /// </summary>
        /// <param name="info"></param>
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

        /// <summary>
        /// Sets the cursor on the lower, rightmost point of the screen
        /// </summary>
        public static void ResetCursor() =>
            Console.SetCursorPosition(_dimensions.width - 1, _dimensions.height - 2);

        /// <summary>
        /// Plays an animation the given coordinates.
        /// </summary>
        /// <param name="frames">An iterator for the animation's frames</param>
        /// <param name="coords">The coordinates where to play the animation</param>
        /// <param name="bg">The background color while playing the animation</param>
        /// <param name="fg">The foreground color while playing the animation</param>
        /// <param name="duration"></param>
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