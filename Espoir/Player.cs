namespace Espoir
{
    public class Player
    {
        public string Name { get; set; }

        public decimal WarFunds { get; set; }

        public decimal Debt { get; set; }

        public int Stars { get; set; }

        public IEnumerable<CardType> Cards { get; set; }
    }
}
