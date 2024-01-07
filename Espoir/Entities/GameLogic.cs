namespace Espoir.Entities
{
    internal class GameLogic
    {
        public static bool CanBattleOccur(Player player1, Player player2, int stars, out BattleDenialReason? reason)
        {
            reason = null;

            if (player1.Name == player2.Name)
            {
                reason = BattleDenialReason.PlayersCanNotFightThemselves;
                return false;
            }

            if (player1.Stars < stars)
            {
                reason = BattleDenialReason.Player1LackStars;
                return false;
            }

            if (player2.Stars < stars)
            {
                reason = BattleDenialReason.Player2LackStars;
                return false;
            }

            return true;
        }

        public static BattleResult Fight(Player player1, CardType card1, Player player2, CardType card2, int stars)
        {
            player1.RemoveCard(card1);
            player2.RemoveCard(card2);

            var result = GameLogic.Fight(card1, card2);
            if (result == BattleResult.Victory)
            {
                player1.Stars += stars;
                player2.Stars -= stars;
            }
            else if (result == BattleResult.Loss)
            {
                player1.Stars -= stars;
                player2.Stars += stars;
            }

            return result;
        }

        public static BattleResult Fight(CardType card1, CardType card2)
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

        public static void SimulateBattles(EspoirContext context)
        {
            NonPlayablePlayer getRandomPlayer() => context.Players[Random.Shared.Next() % (context.Players.Count - 1) + 1] as NonPlayablePlayer;
            int maxSimultaneousBattles = Math.Min(20, context.Players.Count);
            var numSimultaneousBattles = Random.Shared.Next() % maxSimultaneousBattles;

            for (int i = 0; i < numSimultaneousBattles; i++)
            {
                NonPlayablePlayer player1, player2;
                do
                {
                    player1 = getRandomPlayer();
                    player2 = getRandomPlayer();
                }
                while (player1.Name == player2.Name);

                var card1 = player1.GetRandomCard();
                var card2 = player2.GetRandomCard();
                var stars = (Random.Shared.Next() % Math.Min(player1.Stars, player2.Stars)) + 1;
                GameLogic.Fight(player1, card1, player2, card2, stars);

                // kick players that has 0 stars or 0 cards
                // TODO: this logic needs to change as people can buy stuff
                context.Players = context.Players.Where(p => p.Stars > 0 && p.Cards.Any()).ToList();
            }
        }

        public static bool CanLeave(Player player, out LeaveDenialReason? reason)
        {
            if (player.Cards.Any())
            {
                reason = LeaveDenialReason.PlayerHasCards;
                return false;
            }

            if (player.Stars < 3)
            {
                reason = LeaveDenialReason.NotEnoughStars;
                return false;
            }

            reason = null;
            return true;
        }

        public static decimal RewardCerimony(EspoirContext context)
        {
            context.MainCharacter.Stars -= 3;
            var reward = context.MainCharacter.Stars * 1000000;
            context.MainCharacter.WarFunds += reward;
            return reward;
        }

        public static decimal DebtCerimony(EspoirContext context, out decimal originalDebt)
        {
            // TODO: when add time to the game, then we need to change this
            originalDebt = context.MainCharacter.Debt * new decimal(1.4);
            return context.MainCharacter.Debt - context.MainCharacter.WarFunds;
        }

        #region helpers
        public enum BattleDenialReason
        {
            Player1LackStars,
            Player2LackStars,
            PlayersCanNotFightThemselves,
        }

        public enum LeaveDenialReason
        {
            PlayerHasCards,
            NotEnoughStars
        }
        #endregion
    }
}
