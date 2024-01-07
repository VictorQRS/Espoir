using Espoir.Helpers;

namespace Espoir.Entities
{
    internal class EspoirContext
    {
        const int MAX_PLAYERS = 100;

        public Player MainCharacter { get; set; }

        public IList<Player> Players { get; set; }
        
        public Repeater TenMinuteTask { get; private set; }

        public EspoirContext()
        {
            this.MainCharacter = new Player();
            this.Players = new List<Player> { this.MainCharacter };
            for (int i=1; i<MAX_PLAYERS; i++)
            {
                this.Players.Add(NonPlayablePlayer.Generate(i));
            }

            this.TenMinuteTask = new Repeater(this.TenMinuteTick);
        }

        public void StartGame()
        {
            this.TenMinuteTask.Start();
        }

        public void FinishGame()
        {
            this.TenMinuteTask.Stop();
        }

        public IDictionary<CardType, int> GetCardFrequency()
        {
            return this.Players
                .SelectMany(p => p.Cards)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
        }

        public async Task TenMinuteTick(CancellationToken token)
        {
            while (true)
            {
                var tenMinutes = 60 * 10 * 1000;
                await Task.Delay(tenMinutes, token);
                if (!token.IsCancellationRequested)
                {
                    this.MainCharacter.Debt *= new decimal(1.0150);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
