using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Entities;

namespace Dio.MiniRPG.Infrastructure
{
    public interface ICharacterAction
    {
        string Name { get; }
        string Description { get; }
        ActionType ActionType { get; }
        CharacterActionDelegate ActionMethod { get; }
    }
}