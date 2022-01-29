using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.View.BaseView;
using static Dio.MiniRPG.View.CharacterView;

namespace Dio.MiniRPG.Entities
{
    public static class GameLogic
    {
        public static bool IsPlayerTurn { get; private set; } = true;
        public static IList<IHero> Party { get; } = new List<IHero>();

        public static void ResetTurns() => IsPlayerTurn = true;
        public static void NextTurn() => IsPlayerTurn = !IsPlayerTurn;

        public static void ReceivePlayerAction(IList<IEnemy> enemies)
        {
            foreach (var hero in Party.Where((h) => !h.IsDead))
            {
                hero.SelectCharacter();

                var action = GetAction();

                if (action.TargetType == ActionTargetType.SingleTarget)
                    action.Execute(hero, GetTarget());

                else if (action.TargetType == ActionTargetType.MultiTarget)
                    action.Execute(hero, enemies.ToArray());

                else if (action.TargetType == ActionTargetType.Reflective)
                    action.Execute(hero);

                hero.DeselectCharacter();
                if (enemies.All((e) => e.IsDead))
                    break;
            }
        }

        public static void RecruitHero(IHero hero)
        {
            int count = Party.Count();
            // if (count == 4)
            // {
            //     ReadValidAnswer<char>(
            //         out char answer,
            //         "Your party is full! Do you wish to remove someone from the party? (Y/N)",
            //         'Y', 'N', 'y', 'n'
            //     );

            //     if (char.ToUpper(answer) == 'N')
            //         return;

            //     var i = 1;
            //     var question = "";
            //     foreach (var partyHero in Party)
            //         question += $"\n{i++} - {hero.Name}";
            //     var validAnswers = Enumerable.Range(1, Party.Count()).ToArray();

            //     ReadValidAnswer<int>(out int heroIndex, question, validAnswers);
            //     Party.Remove(hero);
            // }

            if (Party.Count() < 4)
                Party.Add(hero);
            
            PrintMessage($"{hero.Name} is now on the party!");
        }
    }
};