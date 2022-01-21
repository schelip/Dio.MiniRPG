using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Entities;

namespace Dio.MiniRPG.Infrastructure
{
    /// <summary>
    /// Delegates the method that is called when a CharacterAction is executed
    /// </summary>
    /// <param name="actor">The character that executed the action</param>
    /// <param name="targets">The targets of the action</param>
    public delegate void CharacterActionDelegate(ICharacter actor, ICharacter[] targets);

    public interface ICharacterAction
    {
        /// <summary>
        /// The name of the action
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The description of the action and its effects
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The action type e.g offensive, defensive, healing
        /// </summary>
        ActionType ActionType { get; }
        /// <summary>
        /// The method to be called when the action is executed
        /// </summary>
        CharacterActionDelegate ActionMethod { get; }
    }
}