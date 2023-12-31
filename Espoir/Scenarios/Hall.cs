using Espoir.Entities;

namespace Espoir.Scenarios
{
    internal class Hall : Scenario
    {
        public Hall(EspoirContext context) : base(context) {}

        public override Scenario? Run()
        {
            var cardFrequency = this.Context.GetCardFrequency().Select(kv => $"{kv.Value}x{kv.Key}");
            Scene.Dialogue($"There are {string.Join(", ", cardFrequency)} remaining.");

            // Show paginated player list
            var pageNum = 0;

            while (true)
            {
                Scene.PaginatedList(this.Context.Players, pageNum);

                var commandHelp = new List<string>()
                {
                    "To show the next page of players, say: next",
                    //"To talk to a player, say: talk <player name>",
                    "To battle a player, say: battle <player name> <number of stars>",
                    "To leave the game room, say: leave",
                };
                Scene.Dialogue(string.Join("\n", commandHelp));

                var command = Scene.GetInput(
                    "Command: ",
                    (input) => this.TryParseCommand(input),
                    "Are you drunk? You're speaking nonsense.");

                if (command.StartsWith("next"))
                {
                    pageNum++;
                }
                else if (command.StartsWith("prev"))
                {
                    pageNum--;
                }
                else if (command.StartsWith("battle"))
                {
                    var commandParts = command.Split(' ');
                    var stars = int.Parse(commandParts[2]);
                    var opponent = this.Context.Players.First(p => p.Name == commandParts[1]);
                    var opponentStars = opponent.Stars;
                    if (opponentStars < stars)
                    {
                        Scene.Dialogue($"{opponent.Name} does not have that many stars, it only has {opponentStars}. Try with someone else or try with less stars.");
                    }
                    else if (this.Context.MainCharacter.Stars < stars)
                    {
                        Scene.Dialogue($"You don't not have that many stars, only {this.Context.MainCharacter.Stars}. Try with less stars.");
                    }
                    else
                    {
                        return new Battle(playerName: commandParts[1], stars, this.Context);
                    }
                }
                else if (command.StartsWith("leave"))
                {
                    var mc = this.Context.MainCharacter;
                    if (mc.Cards.Any())
                    {
                        Scene.Dialogue("You still have cards in your possession, you must use them up before leaving.");
                    }
                    else if(mc.Stars < 3)
                    {
                        Scene.Dialogue("You need at least 3 stars to leave this room. Go get some more before leaving.");
                    }
                    else
                    {
                        return new GameOver(success: true, this.Context);
                    }
                }
            }
        }

        private bool TryParseCommand(string input)
        {
            var isValid = false;
            var commandParts = input.Split(' ');
            isValid = commandParts[0] switch
            {
                "prev" =>   commandParts.Length == 1,
                "next" => commandParts.Length == 1,
                "leave" => commandParts.Length == 1,
                //"talk" =>   commandParts.Length == 2
                //                && this.Context.Players.FirstOrDefault(p => p.Name == commandParts[1]) is NonPlayablePlayer player,
                "battle" => commandParts.Length == 3
                                && this.Context.Players.FirstOrDefault(p => p.Name == commandParts[1]) is NonPlayablePlayer player
                                && int.TryParse(commandParts[2], out _),
                _ => false,
            };
            return isValid;
        }
    }
}
