namespace Dio.MiniRPG.View
{
    /// <summary>
    /// A structure that saves a char and console background and foreground colors.
    /// Seeing how information printed on the console is write only, this structure is used
    /// to save the printed information to be accessed in the future, most useful
    /// for printing "overlays", such as as animations.
    /// </summary>
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