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
            Scene.Dialogue($"You challenged {opponent.Name} to a fight worth {stars} stars and it was accepted!");
            Scene.Dialogue(this.Context.MainCharacter.LookAtCards());

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
            Scene.Dialogue($"You played {mcCard}!");
            Scene.Dialogue($"{opponent.Name} played {opponentCard}!");
            this.Fight(this.Context.MainCharacter, mcCard, this.opponent, opponentCard, this.stars);

            var result = CardHelper.Battle(mcCard, opponentCard);
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
            this.SimulateBattles();

            return new Hall(this.Context);
        }

        private void SimulateBattles()
        {
            NonPlayablePlayer getRandomPlayer() => this.Context.Players[Random.Shared.Next() % (this.Context.Players.Count - 1) + 1] as NonPlayablePlayer;
            int maxSimultaneousBattles = Math.Min(20, this.Context.Players.Count);
            var numSimultaneousBattles = Random.Shared.Next() % maxSimultaneousBattles;

            for (int i=0; i<numSimultaneousBattles; i++)
            {
                NonPlayablePlayer player1, player2;
                do
                {
                    player1 = getRandomPlayer();
                    player2 = getRandomPlayer();
                }
                while (player1.Name == player2.Name);

                var card1 = player1.GetRandomCard();
                var card2 = player2.GetRandomCard();
                var stars = (Random.Shared.Next() % Math.Min(player1.Stars, player2.Stars)) + 1;
                this.Fight(player1, card1, player2, card2, stars);

                // kick players that has 0 stars or 0 cards
                // TODO: this logic needs to change as people can buy stuff
                this.Context.Players = this.Context.Players.Where(p => p.Stars > 0 && p.Cards.Any()).ToList();
            }
        }

        private BattleResult Fight(Player player1, CardType card1, Player player2, CardType card2, int stars)
        {
            player1.RemoveCard(card1);
            player2.RemoveCard(card2);
            
            var result = CardHelper.Battle(card1, card2);
            if (result == BattleResult.Victory)
            {
                player1.Stars += stars;
                player2.Stars -= stars;
            }
            else if (result == BattleResult.Loss)
            {
                player1.Stars -= stars;
                player2.Stars += stars;
            }

            return result;
        }
    }
}
