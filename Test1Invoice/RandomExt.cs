using System;

namespace Test1Invoice
{
    class RandomExt : Random
    {
        public int NextNotZero(int maxValue)
        {
            int value = 0;
            while (value == 0)
            {
                value = Next(maxValue);
            }

            return value;
        }
    }
}
