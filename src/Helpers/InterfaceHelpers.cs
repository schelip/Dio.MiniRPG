namespace Dio.MiniRPG.Helpers
{
    /// <summary>
    /// Static methods for displaying data and receiving user input
    /// </summary>
    public static class InterfaceHelpers
    {
        public static void ReadValidAnswer<T>(out T? answer, string question, params T[] validAnswers)
        where T : IConvertible
        {
            bool invalid = true;
            do
            {
                Console.Write(question);
                answer = (T?)Convert.ChangeType(Console.ReadLine(), typeof(T));
                if (answer == null || !validAnswers.Contains(answer))
                    Console.WriteLine("Invalid answer");
                else invalid = false;
            } while (invalid);
        }
    }
}