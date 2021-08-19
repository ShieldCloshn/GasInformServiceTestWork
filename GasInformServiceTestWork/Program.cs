using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GasInformServiceTestWork.Managers;
using Microsoft.Extensions.Configuration;

namespace GasInformServiceTestWork
{
    //реализовать меню
    public class Program
    {
        static async Task Main(string[] args)
        {
           

            var closeApp = false;
            Console.WriteLine("Газинформсервис тестовое задание.");

            while (!closeApp)
            {
                Console.WriteLine("Выберите номер задания:");
                Console.WriteLine("1. Thread-safe Singleton");
                Console.WriteLine("2. Числа Фиббоначчи");
                Console.WriteLine("3. Загрузка изображений на рабочий стол с Dog Api");
                var choice = Console.ReadLine();

                await ShowTestResult(choice);

                Console.WriteLine("Введите /esc и нажмите Enter для закрытия приложения, либо нажмите любую кнопку чтобы продолжить");

                if (Console.ReadLine() == "/esc")
                    closeApp = true;

                Console.WriteLine("\n");
            }
        }

        public static async Task ShowTestResult(string choice)
        {
            switch (choice)
            {
                case "1":
                    SafeSingleton();
                    break;

                case "2":
                    GetFibonacciValues();
                    break;

                /*3 задание. Eсть 3 возможных способа решения проблемы SocketException. Singleton, статический класс и интерфейс IHttpClientFactory.
                 Поскольку singleton уже реализован, то сделал решение в нём. Для более крупного проекта предпочтительнее использовать IHttpClientFactory.*/
                case "3":
                    var singleton = Singleton.GetInstance;
                    await singleton.GetDoggyPickUrl();
                    break;

                default:
                    break;
            }     
        }

        public static void SafeSingleton()
        {
            new Thread(() =>
            {
                var singleton = Singleton.GetInstance;
                Console.WriteLine($"Хэшкод singleton - {singleton.GetHashCode()} в {singleton.GetSingletonThreadId()} потоке");
            }).Start();

            new Thread(() =>
            {
                var singleton = Singleton.GetInstance;
                Console.WriteLine($"Хэшкод singleton - {singleton.GetHashCode()} в {singleton.GetSingletonThreadId()} потоке");
            }).Start();
            
            //Пауза, для корректного вывода сообщений в консоль
            Thread.Sleep(60);
        }

        public static void GetFibonacciValues()
        {
            int fibStartValue, fibIterations;
            Console.WriteLine("Введите стартовое значение для последовательности ряда Фибоначчи: ");

            while (true)
            {
                var fibStartString = Console.ReadLine();

                if (int.TryParse(fibStartString, out fibStartValue))
                    break;
                else
                    Console.WriteLine("Введите целое число"); 
            }

            Console.WriteLine("Введите кол-во чисел Фиббоначчи: ");

            while (true)
            {
                var fibIterationsString = Console.ReadLine();

                if (int.TryParse(fibIterationsString, out fibIterations))
                    break;
                else
                    Console.WriteLine("Введите целое число");
            }
            
            if (fibIterations >= fibStartValue)
            {
                Console.WriteLine($"Ряд Фибоначчи от {fibStartValue} до {fibIterations}:");
                int totalValue;

                //Если последовательность начинается от 0 или 1, то суммировать кол-во итераций нет необхоимости
                if (fibStartValue <= 1) 
                    totalValue = fibIterations;
                else
                    totalValue = fibIterations + fibStartValue;

                for (int i = 0; i <= totalValue; i++)
                {
                    //скипаем расчёт для чисел, меньше чем стартовая точка
                    if (i > fibStartValue)
                        Console.Write($" {FibonacciRecursion(i)}");
                }

                Console.WriteLine("\n");
            }
            else
                Console.WriteLine("Конечная точка последовательности не может быть меньше начальной.");
        }

        //Формула: F(n) = F(n-1) + F(n-2) 
        public static int FibonacciRecursion(int iterationsNumber) => (iterationsNumber > 1) ? (FibonacciRecursion(iterationsNumber - 2) + FibonacciRecursion(--iterationsNumber)) : iterationsNumber;
    }
}
