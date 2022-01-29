using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.View.CharacterView;
using static Dio.MiniRPG.View.BaseView;

namespace Dio.MiniRPG.Entities
{
    public class CharacterAction : BaseEntity, ICharacterAction
    {
        public string Description { get; }
        public ActionType ActionType { get; }
        public ActionTargetType TargetType { get; }
        private CharacterActionDelegate ActionMethod { get; }
        public IEnumerable<string[]>? AnimationFrames { get; }

        public CharacterAction(
            string name,
            string description,
            ActionType actionType,
            ActionTargetType targetType,
            CharacterActionDelegate actionMethod,
            string animationPath = ""
        )
        : base(name)
        {
            this.Description = description;
            this.ActionType = actionType;
            this.TargetType = targetType;
            this.ActionMethod = actionMethod;
            this.AnimationFrames = animationPath == ""
                ? null : GetAnimationFrames($"actions\\{animationPath}");
        }

        public void Execute(ICharacter actor, params ICharacter[] targets)
        {
            switch (TargetType)
            {
                case ActionTargetType.SingleTarget:
                    if (targets.Count() != 1)
                        throw new ArgumentException($"{Name} can only hit one target");
                    if (AnimationFrames != null)
                        targets[0].PlayAnimation(AnimationFrames);
                    break;
                
                case ActionTargetType.MultiTarget:
                    if (targets.Count() < 1)
                        throw new ArgumentException($"{this.Name} needs at least one target");
                    if (AnimationFrames != null)
                        foreach (ICharacter target in targets)
                            target.PlayAnimation(AnimationFrames);
                    break;

                case ActionTargetType.Friendly:
                    if (actor.GetType() != targets.GetType())
                        throw new ArgumentException($"{this.Name} can only affect the actor's party");
                    if (AnimationFrames != null)
                        foreach (ICharacter target in targets)
                            target.PlayAnimation(AnimationFrames);
                    break;

                case ActionTargetType.Reflective:
                    if (targets.Count() != 0)
                        throw new ArgumentException($"{this.Name} can only affect its actor");
                    if (AnimationFrames != null)
                    {
                        actor.PrintCharacter();
                        actor.PlayAnimation(AnimationFrames);
                    }
                    break;
            }

            this.ActionMethod(actor, targets);

            foreach (ICharacter target in targets)
                target.PrintCharacter();
        }
    }
}
