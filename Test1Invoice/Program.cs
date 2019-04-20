using System;
using System.Collections.Generic;
using System.IO;
using Test1Invoice.Band;

namespace Test1Invoice
{
    class Program
    {
        static void Main()
        {
            string fileName = "band.bin";
            if (File.Exists(fileName))
            {
                Console.WriteLine("1. Загрузить данные предыдущей ленты");
                Console.WriteLine("2. Рассчитать сумму за каждую дату разными правилами округления");
                Console.WriteLine("3. Сгенерировать новую ленту");
                int selectKey = Convert.ToInt32(Console.ReadLine());

                switch (selectKey)
                {
                    case 1:
                        LoadBand(fileName);
                        break;
                    case 2:
                        LoadBandRecalcSum(fileName);
                        break;
                    case 3:
                        GenerateNewBand(fileName);
                        break;
                }
            }
            else
            {
                GenerateNewBand(fileName);
            }

            Console.ReadKey();
        }
        public static bool LoadBand(string fileNameLoad)
        {
            var fileManager = new FileManager();
            fileManager.LoadFromFile(fileNameLoad, out var x);
            if (x[0] is BandCheques bandCheques)
                Console.WriteLine("Сумма ленты итого {0}", bandCheques.SumAllCheques);

            return true;
        }

        public static bool LoadBandRecalcSum(string fileNameLoad)
        {
            var fileManager = new FileManager();
            fileManager.LoadFromFile(fileNameLoad, out var x);

            if (x[0] is BandCheques bandCheques)
            {
                var sums = new Dictionary<DateTime, decimal[]>();
                decimal sumAllCheque = 0;

                foreach (var cheque in bandCheques.Cheques)
                {
                    decimal rule1 = 0;
                    decimal rule2 = 0;
                    decimal rule3 = 0;
                    foreach (var stringPosition in cheque.StringPositions)
                    {
                        rule1 += RoundRules.RuleMathTwoDigits(stringPosition.SumPricePositions);
                        rule2 += RoundRules.RuleMathMiddle(stringPosition.SumPricePositions);
                        rule3 += RoundRules.RuleMathOneDigits(stringPosition.SumPricePositions);
                    }

                    if (!sums.ContainsKey(cheque.DateTimeCheque))
                        sums.Add(cheque.DateTimeCheque, new[] {rule1, rule2, rule3});
                    else
                    {
                        var rules = sums[cheque.DateTimeCheque];
                        rules[0] += rule1;
                        rules[1] += rule2;
                        rules[2] += rule3;
                        sums[cheque.DateTimeCheque] = rules;
                    }
                }

                foreach (var sum in sums)
                {
                    Console.WriteLine("Рассчёт суммы за дату: {0}", sum.Key);
                    Console.WriteLine("Правило №1: {0}", sum.Value[0]);
                    Console.WriteLine("Правило №2: {0}", sum.Value[1]);
                    Console.WriteLine("Правило №3: {0}", sum.Value[2]);
                    sumAllCheque += sum.Value[0];
                }
                Console.WriteLine("Сумма ленты итого {0}", sumAllCheque);
            }

            return true;
        }

        public static bool GenerateNewBand(string fileNameSave)
        {
            int startWeek;
            int endWeek;
            GetDaysOfMonthCurrentWeek(out startWeek, out endWeek);

            int countCheques = 100;
            Console.WriteLine("Генерируем случайные чеки.");
            BandCheques bandCheques = new BandCheques(countCheques);
            bandCheques.IdBand = 1;

            RandomExt countRandom = new RandomExt();
            RandomExt countPositionsRandom = new RandomExt();
            Random pricePositionRandom = new Random(1);
            Random randomDayWeek = new Random();

            for (int i = 0; i < countCheques; i++)
            {
                bandCheques.Cheques[i] = new Cheque();
                var cheque = bandCheques.Cheques[i];
                cheque.IdCheque = i + 1;
                cheque.IdBand = bandCheques.IdBand;
                cheque.DateTimeCheque = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    randomDayWeek.Next(startWeek, endWeek));

                int countStringsPosition = countRandom.NextNotZero(10);
                cheque.StringPositions = new StringPosition[countStringsPosition];
                for (int curStringPosition = 0; curStringPosition < countStringsPosition; curStringPosition++)
                {
                    cheque.StringPositions[curStringPosition] = new StringPosition()
                    {
                        Npp = curStringPosition + 1,
                        IdCheque = cheque.IdCheque,
                        CountPositions = countPositionsRandom.NextNotZero(20),
                        PricePosition = RoundRules.RuleMathTwoDigits((decimal)pricePositionRandom.NextDouble() * 100)
                    };
                    cheque.StringPositions[curStringPosition].SumPricePositions =
                        RoundRules.RuleMathTwoDigits(
                            (decimal)cheque.StringPositions[curStringPosition].CountPositions *
                            cheque.StringPositions[curStringPosition].PricePosition);
                    cheque.SumCheque += cheque.StringPositions[curStringPosition].SumPricePositions;
                    var stringPosition = cheque.StringPositions[curStringPosition];
                    Console.WriteLine("IdCh: {0} Npp: {1} Кол-во: {2} Цена: {3} Сумма: {4}",
                        stringPosition.IdCheque, stringPosition.Npp, stringPosition.CountPositions,
                        stringPosition.PricePosition, stringPosition.SumPricePositions);
                }

                bandCheques.Cheques[i] = cheque;
                bandCheques.SumAllCheques += cheque.SumCheque;
                Console.WriteLine("Дата чека {0}", cheque.DateTimeCheque);
                Console.WriteLine("Сумма чека {0}", cheque.SumCheque);
            }

            var fileManager = new FileManager();
            var x = new BandCheques[1];
            x[0] = bandCheques;
            Console.WriteLine(StatusFileMsg.Msg[(int)fileManager.SaveToFile(fileNameSave, x)]);

            return true;
        }

        public static void GetDaysOfMonthCurrentWeek(out int startWeek, out int endWeek)
        {
            startWeek = DateTime.Now.Day - (int)DateTime.Now.DayOfWeek + 1;
            endWeek = DateTime.Now.Day + (7 - (int)DateTime.Now.DayOfWeek);
        }
    }
}
