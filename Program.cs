using Dio.MiniRPG.Infrastructure;
using Dio.MiniRPG.Entities;
using Dio.MiniRPG.Entities.Heroes;
using Dio.MiniRPG.Entities.Enemies;
using System.Runtime.Versioning;

using static Dio.MiniRPG.Helpers.InterfaceHelpers;

[assembly:SupportedOSPlatform("windows")]
namespace Dio.MiniRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!OperatingSystem.IsWindows() || !PrepareConsole())
                return;
            
            GameLogic.RecruitHero(new Warrior("Aras", 5));
            GameLogic.RecruitHero(new Warrior("Arus", 5));
            var enemies = new IEnemy[] { new Skeleton("Skeleton"), new Skeleton("Skelly"), new Skeleton("Skeletor") };
            var level = new DungeonLevel(1, enemies);
            level.Start();
        }
    }

}