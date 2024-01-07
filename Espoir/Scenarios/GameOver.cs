using Espoir.Entities;

namespace Espoir.Scenarios
{
    internal class GameOver : Scenario
    {
        private readonly bool success;

        public GameOver(bool success, EspoirContext context) : base(context)
        {
            this.success = success;
        }

        public override Scenario? Run()
        {
            return success
                ? this.RunSuccess()
                : this.RunFailure();
        }

        private Scenario? RunSuccess()
        {
            Scene.Dialogue("Congratulations for clearing out the Restricted Rock-Paper-Scissors game!");
            Scene.Dialogue($"You have {this.Context.MainCharacter.Stars} stars and we'll collect three for your ticket.");
            this.Context.MainCharacter.Stars -= 3;

            Scene.Dialogue("Additionally, we'd like to tell that you can sell the excess of stars, each costing $10 million.");

            decimal reward = GameLogic.RewardCerimony(this.Context);
            int remainingStars = this.Context.MainCharacter.Stars;
            if (remainingStars > 0)
            {
                Scene.Dialogue($"It seems you have an additional of {remainingStars} stars.");
                Scene.Dialogue($"So here's your ${reward}.");
            }
            else
            {
                Scene.Dialogue("It's unfortunate that you don't have any remaining stars.");
            }

            var debt = GameLogic.DebtCerimony(this.Context, out decimal originalDebt);
            Scene.Dialogue($"Now, for the debt collection, according to our calculations you owe us ${originalDebt}.");
            Scene.Dialogue("We will now collect that from your war funds.");

            if (debt > 0)
            {
                Scene.Dialogue($"It seems you now owe us ${debt}. Don't worry, you can enjoy the cruise, but we will be seeing each other soon...");
            }
            else
            {
                Scene.Dialogue($"Congratulations, you survived the game and is now leaving with ${-1 * debt} in your account. That was truly a great night, wasn't it?");
            }

            return null;
        }

        private Scenario? RunFailure()
        {
            Scene.Dialogue(new List<string>{
                "You now have NO cards! It's time for you to pay...",
                "Say good bye to your life. Die? Oh no, far worse..."
            });

            return null;
        }
    }
}