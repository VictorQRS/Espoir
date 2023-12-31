namespace Espoir.Entities
{
    internal class Player
    {
        public string Name { get; set; }

        public decimal WarFunds { get; set; }

        public decimal Debt { get; set; }

        public int Stars { get; set; }

        public IList<CardType> Cards { get; set; }

        public Player()
        {
            Name = string.Empty;
            Cards = new List<CardType>();
            Stars = 3;
        }

        public string LookAtCards()
        {
            var groupedCards = Cards
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count())
                .Select(x => $"{x.Value}x{x.Key}");

            return $"You have {string.Join(", ", groupedCards)}.";
        }

        public virtual void RemoveCard(CardType card)
        {
            this.Cards.Remove(card);
        }
    }
}
