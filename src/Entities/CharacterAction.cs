using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Exceptions;
using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.InterfaceHelpers;

namespace Dio.MiniRPG.Entities
{
    public class CharacterAction : BaseEntity, ICharacterAction
    {
        public string Description { get; }
        public ActionType ActionType { get; }
        public ActionTargetType TargetType { get; }
        private CharacterActionDelegate ActionMethod { get; }

        public CharacterAction(
            string name,
            string description,
            ActionType actionType,
            ActionTargetType targetType,
            CharacterActionDelegate actionMethod
        )
        : base(name)
        {
            this.Description = description;
            this.ActionType = actionType;
            this.TargetType = targetType;
            this.ActionMethod = actionMethod;
        }

        public void Execute(ICharacter actor, params ICharacter[] targets)
        {
            if (TargetType == ActionTargetType.SingleTarget && targets.Count() != 1)
                throw new InvalidTargetsException($"{this.Name} can only hit one target");

            if (TargetType == ActionTargetType.MultiTarget && targets.Count() < 1)
                throw new InvalidTargetsException($"{this.Name} needs at least one target");
            
            if (TargetType == ActionTargetType.Friendly && actor.GetType() != targets.GetType())
                throw new InvalidTargetsException($"{this.Name} can only affect the actor's party");
            
            if (TargetType == ActionTargetType.Reflective && targets.Count() != 0)
                throw new InvalidTargetsException($"{this.Name} can only affect its actor");

            this.ActionMethod(actor, targets);

            foreach (var target in targets)
                target.PrintCharacter();
        }
    }
}
