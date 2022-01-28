using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.ViewHelpers;

namespace Dio.MiniRPG.View
{
    public static partial class CharacterView
    {
        public static void PrintCharacter(this ICharacter character)
        {
            if ((character as IHero) != null)
                ((IHero)character).PrintCharacter();

            else if ((character as IEnemy) != null)
                ((IEnemy)character).PrintCharacter();
            
            else throw new ArgumentException("The character could not be casted to a printable object");
        }

        private static void PrintCharacter(this ICharacter character, Coords coords)
        {
            var hp = Math.Ceiling(character.HP);
            if (hp == 0) Console.ForegroundColor = ConsoleColor.Red;

            PrintSprite(character.Sprite, coords.Left, coords.Top);

            Console.SetCursorPosition(
                coords.Left,
                coords.Top + GetSpriteDimensions(character.Sprite).height
            );

            Console.Write($"HP {hp}{new string(' ', 3 - hp.ToString().Length)}");

            ResetCursor();
        }

        private static void HighlightCharacter(this ICharacter character, Coords coords)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            character.PrintCharacter(coords);
            ResetCursor();
        }

        private static void UnhighlightCharacter(this ICharacter character, Coords coords)
        {
            ResetCursor();
            character.PrintCharacter(coords);
        }
            
        private static Coords GetCoords<T>(this IDictionary<Coords, T> dict, T character)
        {
            var currCoords = dict.FirstOrDefault((h) => h.Value != null && h.Value.Equals(character)).Key;

            if (!currCoords.Equals(default(Coords)))
                return currCoords;

            var freeCords = dict.FirstOrDefault((h) => h.Value == null).Key;

            if (!freeCords.Equals(default(Coords)))
                return freeCords;

            throw new InvalidOperationException("The entity is currently not in the view and the max number for this kind of entity was already reached");
        }

    }
}