using System;

namespace ChallengeApp
{
    public class Statistics
    {
        private double sum;
        public int Total;
        public double Low;
        public double High;

        public Statistics()
        {
            sum = 0;
            Total = 0;
            Low = 100;
            High = 0;
        }

        public double Average
        {
            get
            {
                return Total switch
                {
                    0 => 0,
                    _ => sum / Total,
                };
            }
        }

        public char Letter
        {
            get
            {
                return Average switch
                {
                    > 80 => 'A',
                    > 60 => 'B',
                    > 40 => 'C',
                    > 20 => 'D',
                    > 0 => 'E',
                    _ => '_',
                };
            }
        }

        public void Add(double number)
        {
            sum += number;
            Total++;
            Low = Math.Min(number, Low);
            High = Math.Max(number, High);
        }
    }
}