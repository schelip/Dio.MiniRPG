namespace Dio.MiniRPG.View
{
    public struct Pixel
    {
        public char Value { get; set; }
        public ConsoleColor Background { get; set; }
        public ConsoleColor Foreground { get; set; }

        public Pixel(
            char value,
            ConsoleColor bg = ConsoleColor.Black,
            ConsoleColor fg = ConsoleColor.White)
        {
            this.Value = value;
            this.Background = bg;
            this.Foreground = fg;
        }
    }
}