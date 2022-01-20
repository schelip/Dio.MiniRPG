using Dio.MiniRPG.Infrastructure;
using Dio.MiniRPG.Entities.Heroes;

namespace Dio.MiniRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            Warrior heroi = new Warrior("arus", 6);
            Warrior vilao = new Warrior("vilao", 5);

            Console.Write(heroi);
            Console.Write(vilao);
            heroi.Act(0, new ICharacter[]{ vilao });
            Console.Write(vilao);

            vilao.Act(1, new ICharacter[] { });
            heroi.Act(0, new ICharacter[]{ vilao });
            Console.Write(vilao);
        }
    }

}