using Dio.MiniRPG.Enum;

namespace Dio.MiniRPG.Infrastructure
{
    public interface IHero : ICharacter
    {
        /// <summary>
        /// The hero's type
        /// </summary>
        HeroType HeroType { get; }
    }
}