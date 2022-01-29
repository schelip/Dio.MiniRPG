using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.View.BaseView;

namespace Dio.MiniRPG.View
{
    /// <summary>
    /// The base functions to print characters in the console
    /// </summary>
    public static partial class CharacterView
    {
        /// <summary>
        /// Wrapper to call the appropriate printing method depending on the declaring type of the character
        /// </summary>
        /// <param name="character">The character to print</param>
        /// <exception cref="ArgumentException">When the character is not of a printable class</exception>
        public static void PrintCharacter(this ICharacter character)
        {
            if ((character as IHero) != null)
                ((IHero)character).PrintCharacter();

            else if ((character as IEnemy) != null)
                ((IEnemy)character).PrintCharacter();

            else throw new ArgumentException("The character could not be casted to a printable object");
        }

        /// <summary>
        /// Plays an animation overlaying a character
        /// </summary>
        /// <param name="character">The character where to play the animation</param>
        /// <param name="frames">The animation's frames</param>
        /// <exception cref="ArgumentException">When the character is not of a printable class</exception>
        public static void PlayAnimation(this ICharacter character, IEnumerable<string[]> frames)
        {
            if ((character as IHero) != null)
                BaseView.PlayAnimation(frames, _heroes.GetCoords((IHero)character));

            else if ((character as IEnemy) != null)
                BaseView.PlayAnimation(frames, _enemies.GetCoords((IEnemy)character));

            else throw new ArgumentException("The character could not be casted to a printable object");
        }

        /// <summary>
        /// Prints a character's sprite and hp on the given coordinates
        /// </summary>
        /// <param name="character">The character to be printed</param>
        /// <param name="coords">The coordinates where to print</param>
        private static void PrintCharacter(this ICharacter character, (int left, int top) coords, bool isHighlited = false)
        {
            var hp = Math.Ceiling(character.HP);
            var bg = isHighlited ? ConsoleColor.White : ConsoleColor.Black;
            var fg =
                hp == 0 ? ConsoleColor.Red :
                isHighlited ? ConsoleColor.Black : ConsoleColor.White;

            PrintSprite(character.Sprite, coords, bg, fg);

            Console.SetCursorPosition(
                coords.left,
                coords.top + character.Sprite.GetSpriteDimensions().height
            );

            Write($"HP {hp}{new string(' ', 3 - hp.ToString().Length)}");

            ResetCursor();
        }

        /// <summary>
        /// Wrapper to print a character with a highlithed background
        /// </summary>
        /// <param name="character">The character to be printed</param>
        /// <param name="coords">The coordinates where to print</param>
        private static void HighlightCharacter(this ICharacter character, (int left, int top) coords)
        {
            character.PrintCharacter(coords, true);
            ResetCursor();
        }

        /// <summary>
        /// Wrapper to print a character with the default colors
        /// </summary>
        /// <param name="character">The character to be printed</param>
        /// <param name="coords">The coordinates where to print</param>
        private static void UnhighlightCharacter(this ICharacter character, (int left, int top) coords)
        {
            character.PrintCharacter(coords, false);
            ResetCursor();
        }

        /// <summary>
        /// Parses a dictionary that maps coordinates to printed entities until the given entities is found
        /// </summary>
        /// <typeparam name="T">The printed entity type</typeparam>
        /// <param name="dict">The dictionary that maps coordinates to printed entities</param>
        /// <param name="entity">The entity to look for</param>
        /// <returns>The coordinates of the printed entity</returns>
        /// <exception cref="InvalidOperationException">If the entity was not in the dictionary</exception>
        private static (int left, int top) GetCoords<T>(this IDictionary<(int left, int top), T> dict, T entity)
        {
            var currCoords = dict.FirstOrDefault((h) => h.Value != null && h.Value.Equals(entity)).Key;

            if (!currCoords.Equals(default((int left, int top))))
                return currCoords;

            var freeCords = dict.FirstOrDefault((h) => h.Value == null).Key;

            if (!freeCords.Equals(default((int left, int top))))
                return freeCords;

            throw new InvalidOperationException("The entity is currently not in the view and the max number for this kind of entity was already reached");
        }
    }
}