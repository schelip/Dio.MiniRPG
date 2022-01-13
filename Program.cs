using Dio.MiniRPG.src.Entities;

namespace Dio.MiniRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            Hero heroi = new Hero("arus", "warrior", 4);
            Console.WriteLine(heroi);

            Hero vilao = new Hero("apir", "deathclaw", 3);
            Console.WriteLine(vilao);

            heroi.Attack(vilao);
            Console.WriteLine(heroi);
            Console.WriteLine(vilao);

            heroi.Attack(vilao);
            Console.WriteLine(heroi);
            Console.WriteLine(vilao);

            heroi.Attack(vilao);
            Console.WriteLine(heroi);
            Console.WriteLine(vilao);
        }
    }

}