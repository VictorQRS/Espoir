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
            if (success)
            {
                this.RunSuccess();
            }
            else
            {
                this.RunFailure();
            }
            
            return null;
        }

        private void RunSuccess()
        {
            Scene.Dialogue("Congratulations for clearing out the Restricted Rock-Paper-Scissors game!");
            Scene.Dialogue($"You have {this.Context.MainCharacter.Stars} stars and we'll collect three for your ticket.");
            this.Context.MainCharacter.Stars -= 3;

            Scene.Dialogue("Additionally, we'd like to tell that you can sell the excess of stars, each costing $10 million.");

            if (this.Context.MainCharacter.Stars > 0)
            {
                Scene.Dialogue($"It seems you have an additional of {this.Context.MainCharacter.Stars} stars.");
                var reward = this.Context.MainCharacter.Stars * 10000000;
                Scene.Dialogue($"So here's your ${reward}.");
                this.Context.MainCharacter.WarFunds += reward;
            }
            else
            {
                Scene.Dialogue("It's unfortunate that you don't have any remaining stars.");
            }

            this.Context.MainCharacter.Debt *= new decimal(1.4);
            Scene.Dialogue($"Now, for the debt collection, according to our calculations you owe us ${this.Context.MainCharacter.Debt}.");
            Scene.Dialogue("We will now collect that from your war funds.");

            var debt = this.Context.MainCharacter.Debt - this.Context.MainCharacter.WarFunds;
            if (debt > 0)
            {
                Scene.Dialogue($"It seems you now owe us ${debt}.");
            }
            else
            {
                Scene.Dialogue($"Congratulations, you survived the game and is now leaving with ${-1 * debt} in your account. That was truly a great night, wasn't it?");
            }
        }

        private void RunFailure()
        {
            Scene.Dialogue("You now have NO cards! It's time for you to pay...");
            Scene.Dialogue("Say good bye to your life. Die? Oh no, far worse...");

            Scene.Dialogue("Game Over");
        }
    }
}