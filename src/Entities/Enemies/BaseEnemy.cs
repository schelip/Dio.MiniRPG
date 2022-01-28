using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.CharacterHelpers;

namespace Dio.MiniRPG.Entities.Enemies
{
    public abstract class BaseEnemy : BaseCharacter, IEnemy
    {
        public override List<ICharacterAction> CharacterActionsList { get; protected set; }
        = new List<ICharacterAction>()
        {
            CharacterActions.WeaponStrike,
            CharacterActions.ReadyShield,
        };

        public BaseEnemy(string name)
        : base(name)
        {

        }

        public void Act(IHero[] heroes, IEnemy[] enemies)
        {

            var heroesHPRatio = heroes.GetHPRatio();
            var enemiesHPRatio = enemies.GetHPRatio();

            // If enemy is close to dying or the enemies are losing, try healing or defending
            if (this.GetHPRatio() < 0.15 || (enemiesHPRatio < 0.15 && enemiesHPRatio < heroesHPRatio))
            {
                if (
                    ActWeakestOrParty(enemies, ActionType.Healing) ||
                    ActWeakestOrParty(enemies, ActionType.Defensive)
                ) return;
            }
            // The heroes are losing or the other actions werent succesful, therefore attack
            ActStrongestOrParty(heroes, ActionType.Offensive);
        }

        public override string ToString() => $"{this.Name} LVL{this.LVL} {this.GetType().Name.ToUpper()}";

        /// <summary>
        /// Searches the character actions for the ActionType and either applies it to the weakest member of the party or the whole party
        /// </summary>
        /// <param name="party">The target party for the action</param>
        /// <param name="actionType">The action type to serach for</param>
        /// <returns>True if an action was found and applied</returns>
        private bool ActWeakestOrParty(ICharacter[] party, ActionType actionType)
        {
            var weakest = party.GetWeakestCharacter();
            var partyHPRatio = party.GetHPRatio();

            var singleActionIndex = this.GetCharacterActionIndex(actionType, ActionTargetType.SingleTarget);
            var multiActionIndex = this.GetCharacterActionIndex(actionType, ActionTargetType.MultiTarget);
            var reflectiveActionIndex = this.GetCharacterActionIndex(actionType, ActionTargetType.Reflective);

            try
            {
                if (
                    singleActionIndex > -1
                    && (weakest.GetHPRatio() < partyHPRatio || multiActionIndex < 0)
                )
                    this.Act(singleActionIndex, weakest);

                else if (reflectiveActionIndex > -1 && this.GetHPRatio() < partyHPRatio)
                    this.Act(reflectiveActionIndex);

                else if (multiActionIndex > -1)
                    this.Act(multiActionIndex, party);

                else
                    return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Searches the character actions for the ActionType and either applies it to the strongest member of the party or the whole party
        /// </summary>
        /// <param name="party">The target party for the action</param>
        /// <param name="actionType">The action type to search for</param>
        /// <returns>True if an action was found and applied</returns>
        private bool ActStrongestOrParty(ICharacter[] party, ActionType actionType)
        {
            var strongest = party.GetStrongestCharacter();
            var partyHPRatio = party.GetHPRatio();

            var singleActionIndex = this.GetCharacterActionIndex(actionType, ActionTargetType.SingleTarget);
            var multiActionIndex = this.GetCharacterActionIndex(actionType, ActionTargetType.MultiTarget);
            var reflectiveActionIndex = this.GetCharacterActionIndex(actionType, ActionTargetType.Reflective);

            try
            {
                if (
                    singleActionIndex > -1
                    && (strongest.GetHPRatio() > partyHPRatio || multiActionIndex < 0)
                )
                    this.Act(singleActionIndex, strongest);

                else if (reflectiveActionIndex > -1 || this.GetHPRatio() > partyHPRatio)
                    this.Act(reflectiveActionIndex);

                else if (multiActionIndex > -1)
                    this.Act(multiActionIndex, party);

                else
                    return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}