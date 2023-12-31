namespace Espoir.Entities
{
    internal class NonPlayablePlayer : Player
    {
        public static NonPlayablePlayer Generate(int id)
        {
            var warFunds = new Decimal((Random.Shared.Next() % 9000001) + 1000000);
            return new NonPlayablePlayer()
            {
                Name = $"NPC#{id}",
                Cards = CardHelper.GenerateStartingHand(),
                WarFunds = warFunds,
                Debt = warFunds,
            };
        }

        public CardType GetRandomCard()
        {
            return this.Cards[Random.Shared.Next() % this.Cards.Count];
        }
    }
}
