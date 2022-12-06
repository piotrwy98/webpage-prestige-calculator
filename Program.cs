namespace WebpagePrestigeCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            uint n = 0;

            if(args.Length != 2 || !uint.TryParse(args[0], out n))
            {
                Console.WriteLine("Uruchom program z argumentami <liczba iteracji> <ścieżka do pliku tekstowego z macierzą>");
            }
            else
            {
                ReadFile(n, args[1]);
            }

            Console.ReadKey();
        }

        static void ReadFile(uint n, string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Podany plik nie istnieje");
            }
            else
            {
                string[] fileContent;

                try
                {
                    fileContent = File.ReadAllLines(filePath);
                }
                catch
                {
                    Console.WriteLine("Podany plik nie jest tekstowy");
                    return;
                }

                double[][] matrix = new double[fileContent.Length][];

                for (int i = 0; i < fileContent.Length; i++)
                {
                    var row = fileContent[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if(row.Length != fileContent.Length)
                    {
                        Console.WriteLine("Macierz nie jest symetryczna");
                        return;
                    }

                    matrix[i] = new double[row.Length];

                    for (int j = 0; j < row.Length; j++)
                    {
                        try
                        {
                            matrix[i][j] = Convert.ToDouble(row[j]);
                        }
                        catch
                        {
                            Console.WriteLine($"Wartość komórki ({i}, {j}) nie jest liczbą całkowitą");
                            return;
                        }
                    }
                }

                CalculatePrestige(n, matrix);
            }
        }

        static void CalculatePrestige(uint n, double[][] matrix)
        {
            var prestige = new double[matrix.Length];
            var savedPrestige = new double[matrix.Length];

            for (int x = 0; x <= n; x++)
            {
                Console.WriteLine($"Iteracja #{x}:\t");

                for (int i = 0; i < matrix.Length; i++)
                {
                    if (x > 0)
                    {
                        prestige[i] = 0;

                        for (int j = 0; j < matrix[i].Length; j++)
                        {
                            prestige[i] += matrix[j][i] * prestige[j];
                        }
                    }
                    else
                    {
                        prestige[i] = 1;
                    }

                    Console.WriteLine($"N = {i+1}\t  P = {Math.Round(prestige[i], 3)}");
                }

                if (x == 2)
                {
                    prestige.CopyTo(savedPrestige, 0);
                }

                Console.WriteLine();
            }

            SortDescendingByPrestige(2, savedPrestige);
        }

        static void SortDescendingByPrestige(int iteration, double[] prestige)
        {
            Console.WriteLine($"Ranking stron dla iteracji #{iteration}:");

            var list = new List<Tuple<int, double>>();

            for (int i = 0; i < prestige.Length; i++)
            {
                list.Add(new Tuple<int, double>(i + 1, prestige[i]));
            }

            list = list.OrderByDescending(x => x.Item2).ToList();

            for (int i = 0; i < list.Count(); i++)
            {
                Console.WriteLine($"#{i+1}:\tN = {list[i].Item1}\tP = {Math.Round(list[i].Item2, 3)}");
            }
        }
    }
}