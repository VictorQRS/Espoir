using Espoir.Entities;

namespace Espoir.Scenarios
{
    internal class Scene
    {
        public static void Dialogue(IEnumerable<string> messages)
        {
            foreach(var msg in messages)
            {
                Scene.Dialogue(msg);
            }
        }

        public static void Dialogue(string message)
        {
            Console.WriteLine(message);
            while (ConsoleKey.Enter != Console.ReadKey().Key) { }
        }

        public static string GetInput(string query, Func<string, bool> inputIsValid, string failureMessage)
        {
            while (true)
            {
                Console.Write(query);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && inputIsValid(input))
                {
                    return input;
                }

                Scene.Dialogue(failureMessage);
            }
        }

        internal static void PaginatedList(IList<Player> players, int pageNum)
        {
            const int pageSize = 10;
            var skipCount = pageNum * pageSize;
            var playerList = players.Skip(skipCount).Take(pageSize).ToList();
            var playerCount = playerList.Count;
            for (int i=0; i<playerCount; i++)
            {
                Console.WriteLine($"{skipCount + i + 1}. {playerList[i].Name}");
            }
            Console.WriteLine();
        }
    }
}
