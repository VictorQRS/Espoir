using Espoir.Entities;

namespace Espoir.Scenarios
{
    internal class Tutorial : Scenario
    {
        public Tutorial(EspoirContext context) : base(context) { }

        public override Scenario? Run()
        {
            this.Welcome();
            this.WarFundDistribution();
            this.GameExplanation();
            this.CardDistribution();

            this.Context.StartGame();

            return new Hall(this.Context);
        }

        private void Welcome()
        {
            Scene.Dialogue(new List<string>
            {
                "Welcome do Espoir, the best and most expensive cruiser in the world.",
                "You are more than welcome to enjoy its luxury for free.",
                "Well, free as long you can buy yourselves a ticket.",
                "For you to afford it, you'll need to play a game.",
            });
        }
        
        private void WarFundDistribution()
        {
            Scene.Dialogue(new List<string>
            {
                "Before we can start the game, we need you to choose your war fund amount.",
                "There are two rules, though.",
                "First, the minimum amount is $1 Million and the maximum amount is $10 Million.",
                "Secondly, this is not given, but a loan. We require you to return it to us with interest of 1.5% every ten minutes.",
            });

            string input = Scene.GetInput(
                "Choose your war funds: ",
                (quantity) =>
                {
                    decimal parsed = Decimal.Parse(quantity);
                    return parsed >= 1000000 && parsed <= 10000000;
                },
                "Are you dumb? Are you deaf? Haven't I just explained to you that it's supposed to be between one million and ten million? I don't foresee you going great in this game...");

            var warFunds = Decimal.Parse(input);
            this.Context.MainCharacter.WarFunds = warFunds;
            this.Context.MainCharacter.Debt = warFunds;

            Scene.Dialogue($"Player {this.Context.MainCharacter.Name} has chosen ${this.Context.MainCharacter.WarFunds} as the war fund.");
        }

        private void GameExplanation()
        {
            Scene.Dialogue(new List<string>
            {
                "Now that everyone chose their war funds, let me explain the rules of the game.",
                "To earn the ticket, you'll need three of these plastic stars. You start the game with 3 stars already.",
                "But here's the catch: you can only pay the ticket after you end the game.",
            });

            Scene.Dialogue(new List<string>
            {
                "The game is pretty simple: Restricted Rock-Paper-Scissors. Each player shall receive three cards in the beginning, those of which can be of any type Rock, Paper or Scissor.",
                "Then, 2 players will decide to play against each other, bet at least one star, and if both parties agree, they will play a card.",
                "And just like Rock-Paper-Scissor, rock beats scissor, which beats paper, which beats rock.",
                "The winner wins the bet and it's their possession by right.",
                "The cards used for this bet are then thrown away.",
            });

            // TODO: this is supposed to be equal for everyone, but for now we will keep it random.
            Scene.Dialogue(new List<string>
            {
                "As a nice gesture for you from us, once the game begins, we will keep count of the number of cards remaining in this game, separated by type.",
                "But remember, you are not allowed to throw away your cards. And you'll be heavily penalized for that!",
            });

            Scene.Dialogue(new List<string>
            {
                "To leave this room, one needs to pay 3 stars and have NO cards left.",
                "Remember that you still need to pay your debt, and if you don't have the amount with you, you'll need to pay that even after the Espoir returns to land.",
                "Ah! About those who could not leave this room by the end of this 1 hour journey, well... let's just say it will not be pretty.",
            });
        }

        private void CardDistribution()
        {
            Scene.Dialogue("Now, let's start card distribution...");

            this.Context.MainCharacter.Cards = CardHelper.GenerateStartingHand();
            Scene.Dialogue(this.Context.MainCharacter.LookAtCards());

            Scene.Dialogue("Now, let the game begin!");
        }
    }
}
