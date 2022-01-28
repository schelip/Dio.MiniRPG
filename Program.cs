using Dio.MiniRPG.Infrastructure;
using Dio.MiniRPG.Entities;
using Dio.MiniRPG.Entities.Heroes;
using Dio.MiniRPG.Entities.Enemies;

using static Dio.MiniRPG.Helpers.ViewHelpers;

namespace Dio.MiniRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!PrepareConsole())
                return;
            
            GameLogic.RecruitHero(new Warrior("Aras", 5));
            GameLogic.RecruitHero(new Warrior("Arus", 5));
            var enemies = new IEnemy[] { new Skeleton("Skeleton"), new Skeleton("Skelly"), new Skeleton("Skeletor") };
            var level = new DungeonLevel(1, enemies);
            level.Start();
        }
    }

}