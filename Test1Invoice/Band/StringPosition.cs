using System;

namespace Test1Invoice.Band
{
    [Serializable]
    public class StringPosition
    {
        public int Npp { get; set; }
        public double CountPositions { get; set; }
        public decimal PricePosition { get; set; }
        public decimal SumPricePositions { get; set; }
        public int IdCheque { get; set; }
    }
}
