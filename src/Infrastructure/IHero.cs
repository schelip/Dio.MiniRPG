using Dio.MiniRPG.Enum;

namespace Dio.MiniRPG.Infrastructure
{
    public interface IHero : ICharacter
    {
        /// <summary>
        /// The hero's type
        /// </summary>
        HeroType HeroType { get; }

        /// <summary>
        /// Executes an action of the hero's list
        /// </summary>
        /// <param name="index">The index of the action meant to be performed</param>
        /// <param name="targets">The targets on which the action will be performed</param>
        void Act(int index, ICharacter[] targets);
    }
}