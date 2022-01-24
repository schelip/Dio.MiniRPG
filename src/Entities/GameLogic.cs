using Dio.MiniRPG.Enum;
using Dio.MiniRPG.Infrastructure;

using static Dio.MiniRPG.Helpers.Helpers;

namespace Dio.MiniRPG.Entities
{
    public static class GameLogic
    {
        public static bool IsPlayerTurn { get; private set; } = true;
        public static List<IHero> Party { get; } = new List<IHero>();

        public static void ResetTurns() => IsPlayerTurn = true;
        public static void NextTurn() => IsPlayerTurn = !IsPlayerTurn;

        public static void ReceivePlayerAction(IList<IEnemy> enemies)
        {
            foreach (var hero in Party.Where((h) => !h.IsDead))
            {
                Console.Write(hero);

                var validAnswers = Enumerable.Range(1, hero.CharacterActionsList.Count()).ToArray();
                ReadValidAnswer<int>(out int actionIndex, "Select an action -> ", validAnswers);

                var action = hero.CharacterActionsList[actionIndex - 1];

                if (action.TargetType == ActionTargetType.SingleTarget)
                    action.Execute(hero, SelectTarget(enemies));
                
                else if (action.TargetType == ActionTargetType.MultiTarget)
                    action.Execute(hero, enemies.ToArray());
                
                else if (action.TargetType == ActionTargetType.Reflective)
                action.Execute(hero);
            }
        }

        public static void RecruitHero(IHero hero)
        {
            if (Party.Count() == 4)
            {
                ReadValidAnswer<char>(
                    out char answer,
                    "Your party is full! Do you wish to remove someone from the party? (Y/N)",
                    'Y', 'N', 'y', 'n'
                );

                if (char.ToUpper(answer) == 'N')
                    return;

                    var i = 1;
                    var question = "";
                    foreach (var partyHero in Party)
                        question += $"\n{i++} - {hero.Name}";
                    var validAnswers = Enumerable.Range(1, Party.Count()).ToArray();

                    ReadValidAnswer<int>(out int heroIndex, question, validAnswers);
                    Party.RemoveAt(heroIndex - 1);
            }
            Party.Add(hero);
            Console.WriteLine($"{hero.Name} is now on the party!");
        }

        private static IEnemy SelectTarget(IList<IEnemy> enemies)
        {
            var i = 1;
            var question = "";
            foreach (var enemy in enemies)
                question += !enemy.IsDead ? $"\n{i++} - {enemy.Name}" : "";
            question += $"\nSelect a target -> ";

            var validAnswers = Enumerable.Range(1, enemies.Count()).ToArray();
            ReadValidAnswer<int>(out int enemyIndex, question, validAnswers);

            return enemies[enemyIndex - 1];
        }

    }
};