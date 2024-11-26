using System;

namespace math
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выбор метода:\n1 - Метод Северо-Западного угла\n2 - Метод Минимальных элементов\n\nВвод:");
            string method = Console.ReadLine();

            if (method != "1" && method != "2")
            {
                Console.WriteLine("\nНекорректный ввод данных. По умолчанию будет выбран 1.\n");
                method = "1";
            }

            int[] supply = EnterSupply();
            int[] demand = EnterDemand();
            int[,] costs = EnterCosts(demand.Length, supply.Length);

            if (method == "1")
            {
                NorthwestAngleMethod(costs, supply, demand);
            }
            else if (method == "2")
            {
                MinimalElementsMethod(costs, supply, demand);
            }
            Console.ReadLine();
        }

        static int[] EnterSupply()
        {
            Console.WriteLine("\nВведите возможности продавцов (через пробел):\n");
            return Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
        }

        static int[] EnterDemand()
        {
            Console.WriteLine("\nВведите потребности потребителей (через пробел):\n");
            return Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
        }

        static int[,] EnterCosts(int numConsumers, int numSuppliers)
        {
            int[,] costs = new int[numConsumers, numSuppliers];
            Console.WriteLine("\nВведите матрицу затрат (по строкам, между значениями должны стоять пробелы):\n");
            for (int i = 0; i < numConsumers; i++)
            {
                string[] input = Console.ReadLine().Split(' ');
                for (int j = 0; j < numSuppliers; j++)
                {
                    costs[i, j] = int.Parse(input[j]);
                }
            }
            return costs;
        }

        static void NorthwestAngleMethod(int[,] costs, int[] supply, int[] demand)
        {
            Console.WriteLine("\nМЕТОД СЕВЕРО-ЗАПАДНОГО УГЛА\n");
            int numConsumers = costs.GetLength(0);
            int numSuppliers = costs.GetLength(1);
            int[,] allocation = new int[numConsumers, numSuppliers];

            int totalCost = 0;
            int i = 0, j = 0;

            while (i < numConsumers && j < numSuppliers)
            {
                // Минимум между спросом и предложением
                int amount = Math.Min(supply[j], demand[i]);
                allocation[i, j] = amount;
                totalCost += amount * costs[i, j]; // Подсчет стоимости
                supply[j] -= amount;
                demand[i] -= amount;

                // Если предложение исчерпано, переходим к следующему продавцу
                if (supply[j] == 0) j++;
                // Если спрос исчерпан, переходим к следующему потребителю
                if (demand[i] == 0) i++;
            }

            PrintAllocation(allocation);
            Console.WriteLine($"Общая стоимость: {totalCost}\n");
        }

        static void MinimalElementsMethod(int[,] costs, int[] supply, int[] demand)
        {
            Console.WriteLine("\nМЕТОД МИНИМАЛЬНЫХ ЭЛЕМЕНТОВ\n");
            int numConsumers = costs.GetLength(0);
            int numSuppliers = costs.GetLength(1);
            int[,] allocation = new int[numConsumers, numSuppliers];

            int totalCost = 0;

            while (true)
            {
                // Находим минимальную стоимость
                int minCost = int.MaxValue;
                int minRow = -1, minCol = -1;

                for (int i = 0; i < numConsumers; i++)
                {
                    for (int j = 0; j < numSuppliers; j++)
                    {
                        if (allocation[i, j] == 0 && costs[i, j] < minCost && supply[j] > 0 && demand[i] > 0)
                        {
                            minCost = costs[i, j];
                            minRow = i;
                            minCol = j;
                        }
                    }
                }

                // Если не найдено минимальных элементов, выходим из цикла
                if (minRow == -1 || minCol == -1) break;

                // Минимум между спросом и предложением
                int amount = Math.Min(supply[minCol], demand[minRow]);
                allocation[minRow, minCol] = amount;
                totalCost += amount * costs[minRow, minCol]; // Подсчет стоимости
                supply[minCol] -= amount;
                demand[minRow] -= amount;

                // Проверяем оставшееся предложение и спрос
                if (supply[minCol] == 0)
                {
                    // Убираем столбец из рассмотрения
                    for (int i = 0; i < numConsumers; i++)
                    {
                        costs[i, minCol] = int.MaxValue; // Убираем столбец из рассмотрения
                    }
                }

                if (demand[minRow] == 0)
                {
                    // Убираем строку из рассмотрения
                    for (int j = 0; j < numSuppliers; j++)
                    {
                        costs[minRow, j] = int.MaxValue; // Убираем строку из рассмотрения
                    }
                }
            }

            PrintAllocation(allocation);
            Console.WriteLine($"Общая стоимость: {totalCost}\n");
        }


        static void PrintAllocation(int[,] allocation)
        {
            Console.WriteLine("Распределение ресурсов:");
            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    Console.Write(allocation[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
