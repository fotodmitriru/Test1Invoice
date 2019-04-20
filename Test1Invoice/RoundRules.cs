using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Test1Invoice
{
    public static class RoundRules
    {
        private static decimal Get05K(decimal d)
        {
            Regex r = new Regex(@"\d*\.[0-9]5$");
            if (r.IsMatch(d.ToString(CultureInfo.InvariantCulture)))
                return d;
            return 0;
        }
        public static decimal RuleMathTwoDigits(decimal d)
        {
            return Math.Round(d, 2, MidpointRounding.AwayFromZero);
        }
        public static decimal RuleMathMiddle(decimal d)
        {
            if (Get05K(d) > 0)
                return d;
            return Math.Round(d, 1, MidpointRounding.AwayFromZero);
        }
        public static decimal RuleMathOneDigits(decimal d)
        {
            return Math.Round(d, 1, MidpointRounding.AwayFromZero);
        }
    }
}
