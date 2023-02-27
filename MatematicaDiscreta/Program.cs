using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static MatematicaDiscreta.Program;

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
                              "\n 3. Calcolo Soluzioni Congruenza Lineare" +
                              "\n 4. Calcolo Soluzioni Sistema di Congruenze Lineari\n", Console.ForegroundColor = ConsoleColor.White);

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

                    case 4:
                        Console.WriteLine("\nInserisci in una singola linea, separati da uno spazio, il fattore 'a', poi il risultato 'b' e infine il modulo 'n' desiderato" +
                                          "\nPremi invio per aggiungere la congruenza al sistema e ripeti le precedenti operazioni per il numero di congruenze nel sistema" +
                                          "\nUna volta finito di immettere nuove congruenze scrivi 'fine' per ottenere le soluzioni\n", Console.ForegroundColor = ConsoleColor.White);

                        string input = Console.ReadLine();
                        List<int[]> linearCongruences = new List<int[]>();

                        while (!string.Equals(input, "fine", StringComparison.CurrentCultureIgnoreCase))
                        {
                            int[] linearCongruenceFactors = Array.ConvertAll(input.Split(" "), int.Parse);
                            linearCongruences.Add(linearCongruenceFactors);
                            input = Console.ReadLine();
                        }

                        LinearCongruenceSystem(linearCongruences);
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

        static int MCD(int x, int y, BezoutCalculator? bezoutCalculator = null, bool writeLogs = true)
        {
            int divisionResult = 0;
            int divisionRemainder = 0;

            divisionResult = x / y;
            divisionRemainder = x % y;

            if (writeLogs) Console.WriteLine($"\n{x} = {y} * {divisionResult} + {divisionRemainder}", Console.ForegroundColor = ConsoleColor.Gray);

            if (divisionRemainder == 0) return y;

            if (bezoutCalculator != null) bezoutCalculator.setBezoutNumbers(x, y, divisionResult, divisionRemainder);

            return MCD(y, divisionRemainder, bezoutCalculator, writeLogs);
        }

        static int? Diofantea(int x, int y, int result, bool isLinearCongruence, bool writeLogs = true)
        {
            BezoutCalculator bezoutCalculator = new BezoutCalculator();

            if (isLinearCongruence)
            {
                bool wasSimplified = false;
                if (writeLogs) Console.WriteLine($"\nTrasformo la congruenza lineare in equazione diofantea e risolvo", Console.ForegroundColor = ConsoleColor.White);

                if (x % y != x)
                {
                    if (writeLogs) Console.WriteLine($"\nSemplifico eseguendo il modulo su 'a': {x} % {y} =  {x % y}", Console.ForegroundColor = ConsoleColor.White);
                    x = x % y;
                    wasSimplified = true;
                }
                if (result % y != result)
                {
                    if (writeLogs) Console.WriteLine($"\nSemplifico eseguendo il modulo su 'n': {result} % {y} =  {result % y}", Console.ForegroundColor = ConsoleColor.White);
                    result = result % y;
                    wasSimplified = true;
                }
                if (wasSimplified)
                {
                    if (writeLogs) Console.WriteLine($"\nOttengo l'equazione diofantea semplificata: x * {x} + y * {y} = {result}", Console.ForegroundColor = ConsoleColor.White);
                    wasSimplified = false;
                }
            }

            if (writeLogs) Console.WriteLine($"\nCerco MCD({x}, {y}):", Console.ForegroundColor = ConsoleColor.White);
            int mcd = MCD(x, y, bezoutCalculator, writeLogs);

            if ((result % mcd) != 0)
            {
                if (writeLogs) Console.WriteLine($"\nL'equazione inserita non ha soluzione: l'MCD ({mcd}) non divide il risultato", Console.ForegroundColor = ConsoleColor.Red);
                return null;
            }
            else
            {
                if (writeLogs) Console.WriteLine($"\nL'equazione inserita ha soluzione: l'MCD ({mcd}) divide il risultato", Console.ForegroundColor = ConsoleColor.Green);
                if (writeLogs) Console.WriteLine("\nEseguo Algoritmo di Euclide:", Console.ForegroundColor = ConsoleColor.White);

                int[] bezoutNumbers = bezoutCalculator.calculateBezout(writeLogs);
                int resultMultiplier = result / mcd;
                int multipliedResult = bezoutNumbers[0] * resultMultiplier;

                if (writeLogs) Console.WriteLine("\nIl risultato ricercato è:", Console.ForegroundColor = ConsoleColor.White);
                if (writeLogs) Console.WriteLine($"\n {bezoutNumbers[0]} * {bezoutNumbers[1]} + {bezoutNumbers[2]} * {bezoutNumbers[3]} = {mcd}", Console.ForegroundColor = ConsoleColor.Gray);
                if (writeLogs) Console.WriteLine($"\n {bezoutNumbers[0]}({resultMultiplier}) * {bezoutNumbers[1]} + {bezoutNumbers[2]}({resultMultiplier}) * {bezoutNumbers[3]} = {mcd}({resultMultiplier})", Console.ForegroundColor = ConsoleColor.Gray);
                if (writeLogs) Console.WriteLine($"\n {multipliedResult} * {bezoutNumbers[1]} + {bezoutNumbers[2] * resultMultiplier} * {bezoutNumbers[3]} = {result}", Console.ForegroundColor = ConsoleColor.Green);

                if (isLinearCongruence)
                {
                    if (mcd > 1)
                    {
                        if (writeLogs) Console.WriteLine($"\nLa congruenza lineare presenta {mcd} soluzioni equivalenti nella forma {multipliedResult} + k * ({y} / {mcd}):", Console.ForegroundColor = ConsoleColor.White);

                        int n = 1;
                        int solutionNumber = 1;
                        int solution = 1;
                        bool isFirst = true;

                        while (solutionNumber - 1 < mcd)
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
                                if (writeLogs) Console.WriteLine($"\nSoluzione 1: {multipliedResult} ovvero {x} * {multipliedResult} = {result} (mod{y})", Console.ForegroundColor = ConsoleColor.Gray);
                                solutionNumber++;
                                isFirst = false;
                            }

                            if (writeLogs) Console.WriteLine($"\nSoluzione {solutionNumber}: {multipliedResult} + {n} * ({y} / {mcd}) = {solution} ovvero {x} * {solution} = {result} (mod{y})", Console.ForegroundColor = ConsoleColor.Gray);
                            solutionNumber++;
                            n++;
                        }

                        return solution;
                    }
                    else
                    {
                        //Aggiungeri commenti per chiarire comportamento in caso di risultato negativo e trasferire il check fuori dall'if in modo da non dover ripetere il codice per risultati multipli e risultato singolo
                        if (multipliedResult < 0)
                        {
                            int n = 1;
                            int solution = multipliedResult + n * (y / mcd);

                            if (writeLogs) Console.WriteLine("\nPoichè MCD = 1 la congruenza lineare presenta una sola soluzione." +
                                                             "\nIl risultato di una congruenza lineare tuttavia non può essere negativo, cerco dunque il primo risultato positivo:", Console.ForegroundColor = ConsoleColor.White);

                            while (solution < 0)
                            {
                                n++;
                                solution = multipliedResult + n * (y / mcd);
                            }

                            if (writeLogs) Console.WriteLine($"\n{multipliedResult} + {n} * ({y} / {mcd}) = {solution} ovvero {x} * {solution} = {result} (mod{y})", Console.ForegroundColor = ConsoleColor.Gray);
                            return solution;
                        }
                        else
                        {
                            if (writeLogs) Console.WriteLine("\nPoichè MCD = 1 la congruenza lineare presenta una sola soluzione:", Console.ForegroundColor = ConsoleColor.White);
                            if (writeLogs) Console.WriteLine($"\n{multipliedResult} + 0 * ({y} / {mcd}) = {multipliedResult} ovvero {x} * {multipliedResult} = {result} (mod{y})", Console.ForegroundColor = ConsoleColor.Gray);
                        }
                    }
                }

                return multipliedResult;
            }
        }

        static void LinearCongruenceSystem(List<int[]> linearCongruences)
        {
            int numberOfLinearCongruences = 0;
            int N = 1;
            int[] specificN = new int[linearCongruences.Count];
            string Ncalculation = "";
            bool isFirst = true;

            Console.WriteLine("\nSistema:\n", Console.ForegroundColor = ConsoleColor.White);

            foreach (int[] congruence in linearCongruences)
            {
                numberOfLinearCongruences++;

                Console.WriteLine($"{congruence[0]}x = {congruence[1]} (mod{congruence[2]})", Console.ForegroundColor = ConsoleColor.Gray);
                if (isFirst)
                {
                    Ncalculation = $"N = {congruence[2]}";
                    isFirst = false;
                }
                else Ncalculation = Ncalculation + $" * {congruence[2]}";

                N = N * congruence[2];
            }

            Console.WriteLine("\nCalcolo gli N:\n", Console.ForegroundColor = ConsoleColor.White);
            Console.WriteLine(Ncalculation + " = " + N, Console.ForegroundColor = ConsoleColor.Gray);

            for (int i = 0; i < numberOfLinearCongruences; i++)
            {
                specificN[i] = N / linearCongruences[i][2];
                Console.WriteLine($"N{i} = {N} / {linearCongruences[i][2]} = {specificN[i]}", Console.ForegroundColor = ConsoleColor.Gray);
            }

            Console.WriteLine("\nRiscrivo il sistema e risolvo le singole congruenze lineari:\n", Console.ForegroundColor = ConsoleColor.White);
            int[] specificNmultiplied = new int[numberOfLinearCongruences];
            int?[] congruencesResults = new int?[numberOfLinearCongruences];

            for (int i = 0; i < numberOfLinearCongruences; i++)
            {
                specificNmultiplied[i] = specificN[i] * linearCongruences[i][0];
                string toPrint = $"({specificN[i]} * {linearCongruences[i][0]})x = {linearCongruences[i][1]} (mod{linearCongruences[i][2]})";
                congruencesResults[i] = Diofantea(specificNmultiplied[i], linearCongruences[i][2], linearCongruences[i][1], true, false);

                if (congruencesResults[i] == null)
                {
                    Console.WriteLine(toPrint + $" ---> IMPOSSIBILE", Console.ForegroundColor = ConsoleColor.Gray);
                    Console.WriteLine("\nIl sistema non ammette soluzioni", Console.ForegroundColor = ConsoleColor.Red);
                    return;
                }

                Console.WriteLine(toPrint + $" ---> {specificNmultiplied[i]} * {congruencesResults[i]} = {linearCongruences[i][1]} (mod{linearCongruences[i][2]})", Console.ForegroundColor = ConsoleColor.Gray);
            }

            Console.WriteLine("\nCalcolo il risultato:\n", Console.ForegroundColor = ConsoleColor.White);
            isFirst = true;
            int finalResult = 0;
            string finalResultCalculation = "";

            for (int i = 0; i < numberOfLinearCongruences; i++)
            {
                if (isFirst)
                {
                    finalResultCalculation = $"X = {specificN[i]} * {congruencesResults[i]}";
                    isFirst = false;
                }
                else finalResultCalculation = finalResultCalculation + $" + {specificN[i]} * {congruencesResults[i]}";

                finalResult = (int)(finalResult + specificN[i] * congruencesResults[i]);
            }

            Console.WriteLine(finalResultCalculation + " = " + finalResult, Console.ForegroundColor = ConsoleColor.Green);
        }

        public class BezoutCalculator
        {
            List<List<int>> bezoutTable = new List<List<int>>();
            List<int> bezoutRow = new List<int>();

            public void setBezoutNumbers (int x, int y, int result, int module)
            {
                bezoutTable.Add(new List<int> { x, y, result, module });
            }

            public int[] calculateBezout(bool writeLogs = true)
            {
                bool isFirst = true;
                int result = bezoutTable[bezoutTable.Count - 1][0];
                int resultMultiplicand = 1;
                int multiplier = bezoutTable[bezoutTable.Count - 1][1];
                int multiplicand = -bezoutTable[bezoutTable.Count - 1][2];
                int remainder = bezoutTable[bezoutTable.Count - 1][3];
                return calculateBezout2(resultMultiplicand, result, multiplicand, multiplier, bezoutTable.Count - 1, writeLogs);


                int[] calculateBezout2(int multiplier1, int multiplicand1, int multiplier2, int multiplicand2, int counter, bool writeLogs = true)
                {
                    if (isFirst)
                    {
                        if (writeLogs) Console.WriteLine($"\n{remainder} = {multiplier1} * {multiplicand1} + {multiplier2} * {multiplicand2}", Console.ForegroundColor = ConsoleColor.Gray);
                        isFirst = false;
                        return calculateBezout2(multiplier1, multiplicand1, multiplier2, multiplicand2, counter, writeLogs);
                    }
                    else
                    {
                        if (counter == 0) return new int[] { multiplier1, multiplicand1, multiplier2, multiplicand2 };
                        int newMultiplicand2 = bezoutTable[counter - 1][0];
                        int newMultiplier3 = bezoutTable[counter - 1][2];
                        if (writeLogs) Console.WriteLine($"\n  = {multiplier1} * {multiplicand1} + {multiplier2} * ({newMultiplicand2} - {multiplicand1} * {newMultiplier3})", Console.ForegroundColor = ConsoleColor.Gray);
                        int newMultiplier2 = multiplier2 * -newMultiplier3 + multiplier1;
                        if (writeLogs) Console.WriteLine($"\n  = {multiplier2} * {newMultiplicand2} + {newMultiplier2} * {multiplicand1}", Console.ForegroundColor = ConsoleColor.Gray);
                        return calculateBezout2(multiplier2, newMultiplicand2, newMultiplier2, multiplicand1, --counter, writeLogs);
                    }
                }
            }
        }
    }
}

