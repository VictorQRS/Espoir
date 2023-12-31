using Espoir.Entities;

namespace Espoir.Scenarios
{
    internal class Start : Scenario
    {
        public Start(EspoirContext context) : base(context) { }

        public override Scenario? Run()
        {
            Console.WriteLine("Espoir is a text-based strategy-based game! We welcome you to try it.");
            Console.WriteLine("Follow the screens accordingly and pay attention to the instructions or you may not have a great game experience.");
            this.GetPlayerName();
            this.Instructions();

            return new Tutorial(this.Context);
        }

        private void GetPlayerName()
        {
            var name = Scene.GetInput(
                "Before we begin, may I have your name, please: ",
                (input) => true,
                "Are you sure you want to play this game? Please give me your name!");

            this.Context.MainCharacter.Name = name;
        }

        private void Instructions()
        {
            Scene.Dialogue("To pass the dialogue, please press ENTER. Now, let's start!");
        }
    }
}
