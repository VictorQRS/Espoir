using Espoir.Entities;
using static Espoir.Entities.GameLogic;

namespace Espoir.Scenarios
{
    internal class Hall : Scenario
    {
        public Hall(EspoirContext context) : base(context) {}

        public override Scenario? Run()
        {
            var cardFrequency = this.Context.GetCardFrequency().Select(kv => $"{kv.Value}x{kv.Key}");
            Scene.Dialogue($"There are {string.Join(", ", cardFrequency)} remaining.");

            var pageNum = 0;

            while (true)
            {
                Scene.PaginatedList(this.Context.Players, pageNum);

                Scene.Dialogue(string.Join("\n", HallCommands.CommandHelpList));

                var command = Scene.GetInput(
                    "Command: ",
                    (input) => HallCommands.Validate(input, this.Context),
                    "Are you drunk? You're speaking nonsense.");

                Scenario? nextScenario = null;
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
                    nextScenario = this.TryStartBattle(commandParts[1], int.Parse(commandParts[2]));
                }
                else if (command.StartsWith("leave"))
                {
                    nextScenario = this.TryLeave();
                }

                if (nextScenario != null)
                {
                    return nextScenario;
                }
            }
        }

        private Scenario? TryStartBattle(string opponentName, int stars)
        {
            var opponent = this.Context.Players.First(p => p.Name == opponentName);
            if (GameLogic.CanBattleOccur(
            this.Context.MainCharacter,
                        opponent,
                        stars,
                        out GameLogic.BattleDenialReason? reason))
            {
                return new Battle(opponentName, stars, this.Context);
            }
            
            if (reason == GameLogic.BattleDenialReason.Player2LackStars)
            {
                Scene.Dialogue($"{opponent.Name} does not have that many stars, it only has {opponent.Stars}. Try with someone else or try with less stars.");
            }
            else if (reason == GameLogic.BattleDenialReason.Player1LackStars)
            {
                Scene.Dialogue($"You don't not have that many stars, only {this.Context.MainCharacter.Stars}. Try with less stars.");
            }
            else if (reason == GameLogic.BattleDenialReason.PlayersCanNotFightThemselves)
            {
                Scene.Dialogue("Players cannot fight themselves.");
            }

            return null;
        }

        private Scenario? TryLeave()
        {
            if (GameLogic.CanLeave(this.Context.MainCharacter, out LeaveDenialReason? reason))
            {
                this.Context.FinishGame();
                return new GameOver(success: true, this.Context);
            }

            if (reason == LeaveDenialReason.PlayerHasCards)
            {
                Scene.Dialogue("You still have cards in your possession, you must use them up before leaving.");
            }
            else if (reason == LeaveDenialReason.NotEnoughStars)
            {
                Scene.Dialogue("You need at least 3 stars to leave this room. Go get some more before leaving.");
            }

            return null;
        }
    }
}
