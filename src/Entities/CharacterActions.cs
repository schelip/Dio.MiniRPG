using Dio.MiniRPG.Infrastructure;
using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Exceptions;

namespace Dio.MiniRPG.Entities
{
    public delegate void CharacterActionDelegate(ICharacter actor, ICharacter[] targets);

    public class CharacterAction : BaseEntity, ICharacterAction
    {
        public string Description { get; }
        public ActionType ActionType { get; }
        public CharacterActionDelegate ActionMethod { get; }

        public CharacterAction (
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

    public static class CharacterActions
    {
        public static CharacterAction WeaponStrike
        {
            get => new CharacterAction(
                name: "Weapon Strike",
                description: "Hit one enemy with a sharp strike from your weapon, breaking its defense",
                actionType: ActionType.OFFENSIVE,
                actionMethod: (ICharacter actor, ICharacter[] targets) =>
                {
                    if (targets.Count() != 1)
                        throw new InvalidTargetsException("Weapon Strike can only hit one target at once");

                    var target = targets[0];
                    double damagePoints = actor.ATK - (target.IsDefending ? target.DEF : target.END);
                    target.ReceiveDamage(damagePoints);
                    target.StopDefending();

                    Console.WriteLine(actor.Name + " hit " + targets[0].Name + " with Weapon Strike!");
                }
            );
        }

        public static CharacterAction WideSlash
        {
            get => new CharacterAction(
                name: "Wide Slash",
                description: "Hit all the enemies with a slash from your weapon",
                actionType: ActionType.OFFENSIVE,
                actionMethod: (ICharacter actor, ICharacter[] targets) =>
                {
                    if (targets.Count() < 1)
                        throw new InvalidTargetsException("Wide Slash needs at least one target");

                    foreach (var target in targets)
                    {
                        double damagePoints = actor.ATK - (target.IsDefending ? target.DEF : target.END);
                        target.ReceiveDamage(damagePoints);
                    }

                    Console.WriteLine(actor.Name + $" hit {targets.Count()} enemies with Wide Slash!");
                }
            );
        }

        public static CharacterAction ReadyShield
        {
            get => new CharacterAction(
                name: "Ready Shield",
                description: "Ready your shield, entering the defensive stance to reduce damage",
                actionType: ActionType.DEFENSIVE,
                actionMethod: (ICharacter actor, ICharacter[] targets) =>
                {
                    if (targets.Count() != 0)
                        throw new InvalidTargetsException("ReadyShield doesn't have any targets");

                    actor.StartDefending();
                    Console.WriteLine(actor.Name + " prepared his shield!");
                }
            );
        }
    }
}