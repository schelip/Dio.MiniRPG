using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.InterfaceHelpers;

namespace Dio.MiniRPG.Entities
{
    public static class CharacterActions
    {
        public static CharacterAction WeaponStrike
        {
            get => new CharacterAction(
                name: "Weapon Strike",
                description: "Hit one enemy with a sharp strike from your weapon, breaking its defense",
                actionType: ActionType.Offensive,
                targetType: ActionTargetType.SingleTarget,
                actionMethod: (ICharacter actor, ICharacter[] targets) =>
                {
                    PrintMessage($"{actor.Name} hit {targets[0].Name} with Weapon Strike!");

                    var target = targets[0];
                    double damagePoints = actor.ATK - (target.IsDefending ? target.DEF : target.END);
                    target.ReceiveDamage(damagePoints);
                    if (!target.IsDead) target.StopDefending();

                }
            );
        }

        public static CharacterAction WideSlash
        {
            get => new CharacterAction(
                name: "Wide Slash",
                description: "Hit all the enemies with a slash from your weapon",
                actionType: ActionType.Offensive,
                targetType: ActionTargetType.MultiTarget,
                actionMethod: (ICharacter actor, ICharacter[] targets) =>
                {
                    PrintMessage(actor.Name + $" hit {targets.Count()} enemies with Wide Slash!");

                    foreach (var target in targets)
                    {
                        double damagePoints = actor.ATK - (target.IsDefending ? target.DEF : target.END);
                        target.ReceiveDamage(damagePoints);
                    }
                }
            );
        }

        public static CharacterAction ReadyShield
        {
            get => new CharacterAction(
                name: "Ready Shield",
                description: "Ready your shield, entering the defensive stance to reduce damage",
                actionType: ActionType.Defensive,
                targetType: ActionTargetType.Reflective,
                actionMethod: (ICharacter actor, ICharacter[] targets) =>
                {
                    PrintMessage(actor.Name + " prepared his shield!");
                    actor.StartDefending();
                }
            );
        }
    }
}