using System;
using System.Collections.Generic;
using System.Linq;

namespace diff2
{
    internal class Program
    {
        // y''+p(x)*y'+q(x)*y=f(x)   
        //a<=x<=b   
        //Граничные условия 1 рода:   
        //y(a) = ya   
        //y(b) = yb   

        // p(x)
        public static double p(double x)
        {
            return 5*x;
        }

        // q(x)
        public static double q(double x)
        {
            return -3*x*x;
        }


        // f(x)
        public static double f(double x)
        {
            return 0;
        }

        //  Явная разностная схема
        // http://ikt.muctr.ru/html2/6/lek6_6.html
        // Решается с помощью рекуррентного соотношения

        // v(x)
        public static double v(double x)
        {
            return p(x)/q(x);
        }

        // s(x)
        public static double s(double x)
        {
            return -1/q(x);
        }

        // k()
        public static double k(double x)
        {
            return 1;
        }

        public static void Do(double a, double b, int N, double epsilon, double dt)
        {
            var prev = new List<double>();
            prev.Add(a);
            for (int i = 1; i < N - 1; i++)
            {
                prev.Add(0); // Нулевые значения в качестве начального прмиближения
            }
            prev.Add(b);
            foreach (double x in prev) Console.Write("{0},", x);
            Console.WriteLine();
            for (int step = 1;; step++)
            {
                List<double> next = Step(prev, a, b, dt);
                double delta =
                    Enumerable.Range(0, N)
                        .Select(i => Math.Abs(next.ElementAt(i) - prev.ElementAt(i)))
                        .Max()/dt;
                Console.WriteLine("Step={0}, Delta={1}", step, delta);
                prev = next;
                foreach (double x in prev) Console.Write("{0},", x);
                Console.WriteLine();
                if (delta < epsilon) break;
            }
            foreach (double x in prev) Console.WriteLine(x);
        }

        public static List<double> Step(List<double> prev, double a, double b, double dt)
        {
            var next = new List<double>();
            next.Add(a);
            for (int j = 1; j < prev.Count() - 1; j++)
            {
                double xj = a + (j*(b - a)/(prev.Count() - 1));
                double h = (b - a)/(prev.Count() - 1);

                // http://ikt.muctr.ru/html2/6/lek6_2_2.html#6_4

                next.Add(prev.ElementAt(j)
                         + (s(xj)*dt*(prev.ElementAt(j + 1) + prev.ElementAt(j - 1) - 2*prev.ElementAt(j))/(h*h))
                         - (v(xj)*dt*(prev.ElementAt(j + 1) - prev.ElementAt(j - 1))/(2*h))
                         - (k(xj)*dt*prev.ElementAt(j))
                         + (f(xj)/q(xj)*dt));
            }
            next.Add(b);
            return next;
        }

        private static void Main(string[] args)
        {
            // Должна быть проверена устойчивость 
            // http://ikt.muctr.ru/html2/6/lek6_6.html

            Do(1, 2, 10, 0.01, 0.0001);
        }
    }
}