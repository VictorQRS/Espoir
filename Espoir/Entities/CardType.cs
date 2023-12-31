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

        public static BattleResult Battle(CardType card1, CardType card2)
        {
            if (card1 == card2) return BattleResult.Draw;
            
            if (card1 == CardType.Rock)
            {
                if (card2 == CardType.Paper) return BattleResult.Loss;
                if (card2 == CardType.Scissors) return BattleResult.Victory;
            }

            if (card1 == CardType.Paper)
            {
                if (card2 == CardType.Rock) return BattleResult.Victory;
                if (card2 == CardType.Scissors) return BattleResult.Loss;
            }

            if (card1 == CardType.Scissors)
            {
                if (card2 == CardType.Paper) return BattleResult.Victory;
                if (card2 == CardType.Rock) return BattleResult.Loss;
            }

            throw new ArgumentException("CardType not allowed.");
        }
    }
}