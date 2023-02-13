using System.Security.Cryptography.X509Certificates;

namespace MatematicaDiscreta
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "MatematicaDiscreta";

            Console.WriteLine("\nQuale operazione desideri eseguire?\n" +
                              "\n 1. Calcolo Massimo Comun Divisore" +
                              "\n 2. Calcolo Soluzione Equazione Diofantea" +
                              "\n 3. calcolo Soluzioni Congruenza Lineare\n", Console.ForegroundColor = ConsoleColor.White);

            try
            {
                int requestedOperation = Convert.ToInt32(Console.ReadLine());

                switch (requestedOperation)
                {
                    case 1:
                        Console.WriteLine("\nInserisci i due numeri di cui desideri trovare il MCD:\n", Console.ForegroundColor = ConsoleColor.White);
                        int mcd = MCD(Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()));
                        Console.WriteLine("\nMCD: " + mcd, Console.ForegroundColor = ConsoleColor.Green);
                        Console.ReadKey(true);
                        break;

                    case 2:
                        Console.WriteLine("\nInserisci i due fattori dell'equazione diofantea e poi il risultato desiderato:\n", Console.ForegroundColor = ConsoleColor.White);
                        Diofantea(Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()), false);
                        Console.ReadKey(true);
                        break;

                    case 3:
                        Console.WriteLine("\nInserisci prima 'a', poi il modulo 'n' e infine il risultato 'b' desiderato:\n", Console.ForegroundColor = ConsoleColor.White);
                        Diofantea(Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()), true);
                        Console.ReadKey(true);
                        break;

                    default:
                        Console.WriteLine("\nRichiesta non valida, riprova\n", Console.ForegroundColor = ConsoleColor.Red);
                        Main(args);
                        break;
                }

                Console.WriteLine("\nDesideri eseguire ulteriori operazioni?\n" +
                                  "\n 1. Sì" +
                                  "\n 2. No\n", Console.ForegroundColor = ConsoleColor.White);

                if (Convert.ToInt32(Console.ReadLine()) == 1) Main(args);
                else Environment.Exit(0);
            }
            catch
            {
                Console.WriteLine("\nInput non riconosciuto, riprova\n", Console.ForegroundColor = ConsoleColor.Red);
                Main(args);
            }
        }

        static int MCD (int x, int y, BezoutCalculator? bezoutCalculator = null)
        { 
            int divisionResult = 0;
            int divisionRemainder = 0;

            divisionResult = x / y;
            divisionRemainder = x % y;

            Console.WriteLine($"\n{x} = {y} * {divisionResult} + {divisionRemainder}", Console.ForegroundColor = ConsoleColor.Gray);

            if (divisionRemainder == 0) return y;

            if (bezoutCalculator != null)
            {
                bezoutCalculator.bezoutTable.Add(new List<int> { x, y, divisionResult, divisionRemainder });
            }

            return MCD(y, divisionRemainder, bezoutCalculator);
        }

        static void Diofantea (int x, int y, int result, bool isCongruenzaLineare) 
        {
            BezoutCalculator bezoutCalculator = new BezoutCalculator();

            if (isCongruenzaLineare)
            {
                bool wasSimplified = false;
                Console.WriteLine($"\nTrasformo la congruenza lineare in equazione diofantea e risolvo", Console.ForegroundColor = ConsoleColor.White);

                if (x % y != x)
                {
                    Console.WriteLine($"\nSemplifico eseguendo il modulo su 'a': {x} % {y} =  {x % y}", Console.ForegroundColor = ConsoleColor.White);
                    x = x % y;
                    wasSimplified = true;
                }
                if (result % y != result)
                {
                    Console.WriteLine($"\nSemplifico eseguendo il modulo su 'n': {result} % {y} =  {result % y}", Console.ForegroundColor = ConsoleColor.White);
                    result = result % y;
                    wasSimplified = true;
                }
                if (wasSimplified)
                {
                    Console.WriteLine($"\nOttengo l'equazione diofantea semplificata: x * {x} + y * {y} = {result}", Console.ForegroundColor = ConsoleColor.White);
                    wasSimplified = false;
                }
            }

            Console.WriteLine($"\nCerco MCD({x}, {y}):", Console.ForegroundColor = ConsoleColor.White);
            int mcd = MCD(x, y, bezoutCalculator);

            if ((result % mcd) != 0)
            {
                Console.WriteLine($"\nL'equazione inserita non ha soluzione: l'MCD ({mcd}) non divide il risultato", Console.ForegroundColor = ConsoleColor.Red);
            }
            else
            {
                Console.WriteLine($"\nL'equazione inserita ha soluzione: l'MCD ({mcd}) divide il risultato", Console.ForegroundColor = ConsoleColor.Green);
                Console.WriteLine("\nEseguo Algoritmo di Euclide:", Console.ForegroundColor = ConsoleColor.White);

                int[] bezoutNumbers = bezoutCalculator.calculateBezout(bezoutCalculator.bezoutTable);
                int resultMultiplier = result / mcd;
                int multipliedResult = bezoutNumbers[0] * resultMultiplier;

                Console.WriteLine("\nIl risultato ricercato è:", Console.ForegroundColor = ConsoleColor.White);
                Console.WriteLine($"\n {bezoutNumbers[0]} * {bezoutNumbers[1]} + {bezoutNumbers[2]} * {bezoutNumbers[3]} = {mcd}", Console.ForegroundColor = ConsoleColor.Gray);
                Console.WriteLine($"\n {bezoutNumbers[0]}({resultMultiplier}) * {bezoutNumbers[1]} + {bezoutNumbers[2]}({resultMultiplier}) * {bezoutNumbers[3]} = {mcd}({resultMultiplier})", Console.ForegroundColor = ConsoleColor.Gray);
                Console.WriteLine($"\n {multipliedResult} * {bezoutNumbers[1]} + {bezoutNumbers[2] * resultMultiplier} * {bezoutNumbers[3]} = {result}", Console.ForegroundColor = ConsoleColor.Green);

                if (isCongruenzaLineare)
                {
                    if (mcd > 1)  
                    {
                        Console.WriteLine($"\nLa congruenza lineare presenta {mcd} soluzioni equivalenti nella forma {multipliedResult} + k * ({y} / {mcd}):", Console.ForegroundColor = ConsoleColor.White);

                        int n = 1;
                        int solutionNumber = 1;
                        int solution = 1;
                        bool isFirst = true;

                        while (solutionNumber-1 < mcd)
                        {
                            solution = multipliedResult + n * (y / mcd);

                            if (multipliedResult < 0)
                            {
                                while (solution < 0)
                                {
                                    n++;
                                    solution = multipliedResult + n * (y / mcd);
                                }
                            }
                            else if (isFirst)
                            {
                                Console.WriteLine($"\nSoluzione 1: {multipliedResult} ovvero {x} * {multipliedResult} = {result} (mod{y})", Console.ForegroundColor = ConsoleColor.Gray);
                                solutionNumber++;
                                isFirst = false;
                            }

                            Console.WriteLine($"\nSoluzione {solutionNumber}: {multipliedResult} + {n} * ({y} / {mcd}) = {solution} ovvero {x} * {solution} = {result} (mod{y})", Console.ForegroundColor = ConsoleColor.Gray);
                            solutionNumber++;
                            n++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nLa congruenza lineare presenta una sola soluzione:", Console.ForegroundColor = ConsoleColor.White);
                        Console.WriteLine($"\n{multipliedResult} + 0 * ({y} / {mcd}) = {multipliedResult} ovvero {x} * {multipliedResult} = {result} (mod{y})", Console.ForegroundColor = ConsoleColor.Gray);
                    }
                }
            }
        }
    }

    public class BezoutCalculator
    {
        public List<List<int>> bezoutTable = new List<List<int>>(); 
        public List<int> bezoutRow = new List<int>();

        public int[] calculateBezout(List<List<int>> bezoutTable)
        {
            bool isFirst = true;
            int result = bezoutTable[bezoutTable.Count-1][0];
            int resultMultiplicand = 1;
            int multiplier = bezoutTable[bezoutTable.Count - 1][1];
            int multiplicand = -bezoutTable[bezoutTable.Count - 1][2];
            int remainder = bezoutTable[bezoutTable.Count - 1][3];
            return calculateBezout2(resultMultiplicand, result, multiplicand, multiplier, bezoutTable.Count - 1);


            int[] calculateBezout2(int multiplier1, int multiplicand1, int multiplier2, int multiplicand2, int counter)
            {
                if (isFirst)
                {
                    Console.WriteLine($"\n{remainder} = {multiplier1} * {multiplicand1} + {multiplier2} * {multiplicand2}", Console.ForegroundColor = ConsoleColor.Gray);
                    isFirst = false;
                    return calculateBezout2(multiplier1, multiplicand1, multiplier2, multiplicand2, counter);
                }
                else
                {
                    if (counter == 0) return new int[] { multiplier1, multiplicand1, multiplier2, multiplicand2 };
                    int newMultiplicand2 = bezoutTable[counter-1][0];
                    int newMultiplier3 = bezoutTable[counter-1][2];
                    Console.WriteLine($"\n  = {multiplier1} * {multiplicand1} + {multiplier2} * ({newMultiplicand2} - {multiplicand1} * {newMultiplier3})", Console.ForegroundColor =ConsoleColor.Gray);
                    int newMultiplier2 = multiplier2 * -newMultiplier3 + multiplier1;
                    Console.WriteLine($"\n  = {multiplier2} * {newMultiplicand2} + {newMultiplier2} * {multiplicand1}", Console.ForegroundColor = ConsoleColor.Gray);
                    return calculateBezout2(multiplier2, newMultiplicand2, newMultiplier2, multiplicand1, --counter);
                }
            }
        }
    }
}

