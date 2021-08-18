using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GasInformServiceTestWork
{
    class Program
    {
        private static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            //1 задание
            (new Thread(() =>
            {
                Singleton singleton = Singleton.GetInstance;
                singleton.GetSingletonThreadId();
            })).Start();

            Console.WriteLine($"Thread main - {Thread.CurrentThread.GetHashCode()}");
            Thread.Sleep(50);

            //2 задание
            //Console.WriteLine("Введите стартовое значение для последовательности ряда Фибоначчи: ");
            //int fibStartValue = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine("Введите кол-во чисел Фиббоначчи: ");
            //int fibIterations = Convert.ToInt32(Console.ReadLine());


            //if (fibIterations >= fibStartValue)
            //{
            //    Console.WriteLine($"Ряд Фибоначчи от {fibStartValue} до {fibIterations}:");
            //    for (int i = 0; i <= fibIterations + fibStartValue; i++)
            //    {
            //        //скипаем расчёт для чисел, меньше чем стартовая точка
            //        if (i > fibStartValue)
            //            Console.Write($" {FibonacciRecursion(i)}");
            //    }
            //}
            //else
            //    Console.WriteLine("Конечная точка последовательности не может быть меньше начальной.");


            /*3 задание. Eсть 3 возможных способа решения проблемы SocketException. Singleton, статический класс и интерфейс IHttpClientFactory.
            Поскольку singleton уже реализован, то сделал решение в нём. Для более крупного проекта я бы лучше использовал IHttpClientFactory.*/
            Singleton singleton = Singleton.GetInstance;
            await singleton.GetDoggyPickUrl();

        }

        //Формула: F(n) = F(n-1) + F(n-2) 
        static int FibonacciRecursion(int iterationsNumber) => (iterationsNumber > 1) ? (FibonacciRecursion(iterationsNumber - 2) + FibonacciRecursion(--iterationsNumber)) : iterationsNumber;
    }
}
