using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Infrastructure;

namespace Dio.MiniRPG.Entities
{
    public class CharacterAction : BaseEntity, ICharacterAction
    {
        public string Description { get; }
        public ActionType ActionType { get; }
        public CharacterActionDelegate ActionMethod { get; }

        public CharacterAction(
            string name,
            string description,
            ActionType actionType,
            CharacterActionDelegate actionMethod
        )
        : base(name)
        {
            this.Description = description;
            this.ActionType = actionType;
            this.ActionMethod = actionMethod;
        }
    }
}
