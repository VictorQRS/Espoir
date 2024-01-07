namespace Espoir.Entities
{
    internal class HallCommands
    {
        public static List<string> CommandHelpList = new()
        {
            "To show the next page of players, say: next",
            //"To talk to a player, say: talk <player name>",
            "To battle a player, say: battle <player name> <number of stars>",
            "To leave the game room, say: leave",
        };

        public static bool Validate(string command, EspoirContext context)
        {
            var isValid = false;
            var commandParts = command.Split(' ');
            isValid = commandParts[0] switch
            {
                "prev" => commandParts.Length == 1,
                "next" => commandParts.Length == 1,
                "leave" => commandParts.Length == 1,
                //"talk" =>   commandParts.Length == 2
                //                && this.Context.Players.FirstOrDefault(p => p.Name == commandParts[1]) is NonPlayablePlayer player,
                "battle" => commandParts.Length == 3
                                && context.Players.FirstOrDefault(p => p.Name == commandParts[1]) is NonPlayablePlayer player
                                && int.TryParse(commandParts[2], out _),
                _ => false,
            };
            return isValid;
        }
    }
}
