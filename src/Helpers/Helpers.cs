namespace Dio.MiniRPG.Helpers
{
    public static class Helpers
    {
        public static void Unshift(this object[] array, params object[] elements)
        {
            for (int i = array.Length - 1; i >= elements.Length; i--)
                array[i] = array[i - elements.Length];
            for (int i = 0; i < elements.Length; i++)
                array[i] = elements[i];
        }

        public static T[] Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
                arr[i] = value;
            return arr;
        }

        public static IEnumerable<string> SplitWordsBy(this string str, int chunkLength)
        {
            string chunk = string.Empty;
            foreach (var word in str.Split(" "))
            {
                if (chunk.Length + word.Length <= chunkLength)
                    chunk += word + " ";
                else
                {
                    yield return chunk;
                    chunk = word + " ";
                }
            }
            yield return chunk;
        }

        public static string Truncate(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Substring(0, Math.Min(str.Length, maxLength));
        }

        public static void Flush()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }
    }
}