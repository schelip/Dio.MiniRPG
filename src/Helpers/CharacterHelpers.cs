using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Infrastructure;

namespace Dio.MiniRPG.Helpers
{
    /// <summary>
    /// Static methods for interacting with characters
    /// </summary>
    public static class CharacterHelpers
    {
        public static ICharacter GetWeakestCharacter(this IList<ICharacter> characters) =>
            characters.Aggregate(characters[0], (curr, n) => curr.HP <= n.HP ? curr : n);

        public static ICharacter GetStrongestCharacter(this IList<ICharacter> characters) =>
            characters.Aggregate(characters[0], (curr, n) => curr.HP >= n.HP ? curr : n);

        public static double GetHPRatio(this ICharacter character) =>
            character.HP / character.MaxHP;

        public static double GetHPRatio(this IList<ICharacter> characters) =>
            characters.Sum((c) => c.HP) / characters.Sum((c) => c.MaxHP);

        public static ICharacterAction? GetCharacterAction(this ICharacter character, ActionType? actionType = null, ActionTargetType? targetType = null) =>
            character.CharacterActionsList.FirstOrDefault((a) => (
                (actionType != null ? a.ActionType == actionType : true) &&
                (targetType != null ? a.TargetType == targetType : true)
            ));
        
        public static int GetCharacterActionIndex(this ICharacter character, ActionType? actionType = null, ActionTargetType? targetType = null) =>
            character.CharacterActionsList.FindIndex((a) => (
                (actionType != null ? a.ActionType == actionType : true) &&
                (targetType != null ? a.TargetType == targetType : true)
            ));
    }
}