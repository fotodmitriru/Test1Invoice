using System;

namespace Test1Invoice.Band
{
    [Serializable]
    class BandCheques
    {
        public int IdBand { get; set; }
        public int CountCheques { get; set; }
        public decimal SumAllCheques { get; set; }
        public Cheque[] Cheques { get; set; }

        public BandCheques(int countCheques)
        {
            Cheques = new Cheque[countCheques];
        }
    }
}
