namespace Espoir.Entities
{
    internal enum CardType
    {
        Rock,
        Paper,
        Scissors,
        Unknown,
    }

    internal static class CardHelper
    {
        public static CardType GenerateRandomCard()
        {
            return (CardType)(Random.Shared.Next() % 3);
        }

        public static IList<CardType> GenerateStartingHand()
        {
            var cards = new List<CardType>();
            for (int i = 0; i < 3; i++)
            {
                var card = CardHelper.GenerateRandomCard();
                cards.Add(card);
            }
            return cards;
        }

        public static CardType Parse(string card)
        {
            var cardLower = card.ToLower();
            return cardLower switch
            {
                "rock" => CardType.Rock,
                "paper" => CardType.Paper,
                "scissors" => CardType.Scissors,
                _ => CardType.Unknown,
            };
        }
    }
}