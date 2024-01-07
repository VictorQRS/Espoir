using Espoir.Entities;

namespace Espoir.Scenarios
{
    internal class Battle : Scenario
    {
        private NonPlayablePlayer opponent;

        private int stars;
        
        public Battle(string playerName, int stars, EspoirContext context) : base(context)
        {
            this.stars = stars;
#pragma warning disable CS8601 // Possible null reference assignment.
            opponent = this.Context.Players.First(p => p.Name == playerName) as NonPlayablePlayer;
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public override Scenario? Run()
        {
            Scene.Dialogue(new List<string>
            {
                $"You challenged {opponent.Name} to a fight worth {stars} stars and it was accepted!",
                this.Context.MainCharacter.LookAtCards(),
            });

            var mcCard = CardType.Unknown;
            Scene.GetInput(
                "Choose the card you wish to use: ",
                (input) =>
                {
                    mcCard = CardHelper.Parse(input);
                    return mcCard != CardType.Unknown && this.Context.MainCharacter.Cards.Contains(mcCard);
                },
                "Do you want me to kick you off this ship?");
            var opponentCard = this.opponent.GetRandomCard();

            // BATTLE!
            Scene.Dialogue(new List<string>
            {
                $"You played {mcCard}!",
                $"{opponent.Name} played {opponentCard}!",
            });
            
            var result = GameLogic.Fight(this.Context.MainCharacter, mcCard, this.opponent, opponentCard, this.stars);
            if (result == BattleResult.Victory)
            {
                Scene.Dialogue($"Congratulations! You won {this.stars} stars and now have {this.Context.MainCharacter.Stars} in total.");
            }
            else if (result == BattleResult.Loss)
            {
                Scene.Dialogue($"Too bad! You lost {this.stars} stars and now have {this.Context.MainCharacter.Stars} in total.");
            }
            else
            {
                Scene.Dialogue($"It's a draw! No stars changed hands and now have {this.Context.MainCharacter.Stars} stars in total.");
            }

            // MC stars should never be smaller than 0, but just in case.
            if (this.Context.MainCharacter.Stars <= 0)
            {
                return new GameOver(success: false, this.Context);
            }

            // TODO: while we don't have timing and personalities, simulate battles after every battle
            GameLogic.SimulateBattles(this.Context);

            return new Hall(this.Context);
        }
    }
}
