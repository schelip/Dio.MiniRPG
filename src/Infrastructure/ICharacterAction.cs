using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Entities;

namespace Dio.MiniRPG.Infrastructure
{
    /// <summary>
    /// Delegates the method that is called when a CharacterAction is executed
    /// </summary>
    /// <param name="actor">The character that executed the action</param>
    /// <param name="targets">The targets of the action</param>
    public delegate void CharacterActionDelegate(ICharacter actor, params ICharacter[] targets);

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
        /// The action type, e.g offensive, defensive healing
        /// </summary>
        ActionType ActionType { get; }
        /// <summary>
        /// The action target type, e.g single target, multi target, reflective
        /// </summary>
        ActionTargetType TargetType { get; }

        /// <summary>
        /// Executes the action
        /// </summary>
        /// <param name="actor">The character that executed the action</param>
        /// <param name="targets">The targets of the action</param>
        void Execute(ICharacter actor, params ICharacter[] targets);
    }
}