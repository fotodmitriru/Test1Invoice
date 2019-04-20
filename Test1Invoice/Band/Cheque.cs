using System;

namespace Test1Invoice.Band
{
    [Serializable]
    public class Cheque
    {
        public int IdCheque { get; set; }
        public DateTime DateTimeCheque { get; set; }
        public decimal SumCheque { get; set; }
        public StringPosition[] StringPositions { get; set; }
        public int IdBand { get; set; }
    }
}
