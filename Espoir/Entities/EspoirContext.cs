using System.Numerics;

namespace Espoir.Entities
{
    internal class EspoirContext
    {
        const int MAX_PLAYERS = 100;

        public Player MainCharacter { get; set; }

        public IList<Player> Players { get; set; }
        
        public EspoirContext()
        {
            this.MainCharacter = new Player();
            this.Players = new List<Player> { this.MainCharacter };

            for (int i=1; i<MAX_PLAYERS; i++)
            {
                this.Players.Add(NonPlayablePlayer.Generate(i));
            }
        }

        public IDictionary<CardType, int> GetCardFrequency()
        {
            return this.Players
                .SelectMany(p => p.Cards)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
        }
    }
}
