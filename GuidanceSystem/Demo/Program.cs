using System;
using System.Collections.Generic;
using System.Text;
using GuidanceSystem;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            double tax = 1000;
            double tay = -2000;
            double taz = 0;
            double tvx = -3000;
            double tvy = 8000;
            double tvz = 0;
            double tpx = 3000;
            double tpy = -9000;
            double tpz = 0;
            double svx = 0;
            double svy = 0;
            double svz = 0;
            double spx = 0;
            double spy = 0;
            double spz = 0;
			double pam = 75;
			double pvm = 1000;
			double ppm = 50;
            int rbt = 6000;
			double rot = 1;
			double[,] vectors;
			Targeting.Intercept(tax, tay, taz, tvx, tvy, tvz, tpx, tpy, tpz, svx, svy, svz, spx, spy, spz, pam, pvm, ppm, rbt, rot, out vectors);
            Console.WriteLine("All posible launch vectors (sorted from fastest to slowest):");
            Console.WriteLine("----------------------");
            for (int i = 0; i != vectors.GetLength(0); i++)
            {
                Console.WriteLine("X" + (i + 1) + ": " + vectors[i, 0]);
                Console.WriteLine("Y" + (i + 1) + ": " + vectors[i, 1]);
                Console.WriteLine("Z" + (i + 1) + ": " + vectors[i, 2]);
                Console.WriteLine("----------------------");
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);
        }
    }
}
