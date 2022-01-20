using Dio.MiniRPG.Infrastructure;
using Dio.MiniRPG.Exceptions;

namespace Dio.MiniRPG.Entities
{
    public delegate void CharacterAction(ICharacter actor, ICharacter[] targets);

    public static class CharacterActions
    {
        public static void WeaponStrike(this ICharacter actor, ICharacter[] targets)
        {
            if (targets.Count() != 1)
                throw new InvalidTargetsException("Weapon Strike can only hit one target at once");
            
            var target = targets[0];
            double damagePoints = actor.ATK - (target.IsDefending ? target.DEF : target.END);
            target.ReceiveDamage(damagePoints);
            target.StopDefending();

            Console.WriteLine(actor.Name + " hit " + targets[0].Name + " with Weapon Strike!");
        }

        public static void ReadyShield(this ICharacter actor, ICharacter[] targets)
        {
            if (targets.Count() != 0)
                throw new InvalidTargetsException("ReadyShield doesn't have any targets");
            
            actor.StartDefending();
            Console.WriteLine( actor.Name + " prepared his shield!");
        }

        public static void WideSlash(this ICharacter actor, ICharacter[] targets)
        {
            if (targets.Count() < 1)
                throw new InvalidTargetsException("Wide Slash needs at least one target");
            
            foreach (var target in targets)
            {
                double damagePoints = actor.ATK - (target.IsDefending ? target.DEF : target.END);
                target.ReceiveDamage(damagePoints);
            }

            Console.WriteLine( actor.Name + $" hit {targets.Count()} enemies with Wide Slash!");
        }
    }
}