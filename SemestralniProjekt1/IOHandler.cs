using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SemestralniProjekt
{

    public class IOHandler
    {
        // funkce pro zjištění zda budeme načítat čísla z terminálu, nebo ze souboru
        // input == true -> výběr způsobu zadání čísla
        // input == false -> výběr způsobu vypsání prvočísel
        public static int GetIOType(bool input)
        {
            // předinicializace, abychom se vyhnuli inicializaci proměnné pokaždé když uživatel zadá chybný vstup
            string setUp;

            // dokud uživatel nezadá správný vstup
            do
            {
                if (input)
                {
                    Console.WriteLine("Chcete zadat číslo ručně, nebo načíst ze souboru?");
                    Console.WriteLine("1. Načíst z terminálu");
                    Console.WriteLine("2. Načíst ze souboru (načte se ze souboru input.txt)");
                    Console.WriteLine("pozn.: Při načtení z terminálu lze zadat pouze jedno číslo, při načtení ze souboru je počet čísel neomezený (každé číslo musí být na samostatném řádku)");
                    setUp = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Chcete vypsat čísla do konzole, nebo zapsat do souboru?");
                    Console.WriteLine("1. Vypsat do konzole");
                    Console.WriteLine("2. Zapsat do souboru (uloží se do souboru prvocisla.txt)");
                    setUp = Console.ReadLine();
                }

                // pokud uživatelký vstup neodpovídá podmínce vypíše se chybová hláška
                if (!Regex.IsMatch(setUp, @"[1,2]"))
                {
                    Console.WriteLine("Španý vstup, zkuste to znovu...");
                    Console.WriteLine("Stiskněte Enter pro pokračování");
                    Console.ReadLine();
                    Console.Clear();

                }
                // kontrola vstupu pomocí regex, vrací true pokud setUp == 1 || setUp == 2
            } while (!Regex.IsMatch(setUp, @"[1,2]"));
            return int.Parse(setUp);
        }
        // funkce pro přečtení čísla z konzole
        public static int GetNumberFromTerminal()
        {
            bool succes;

            // dokud uživatel nazadá číslo
            do
            {
                Console.WriteLine("Napište hranici do které chcete vypsat prvočísla: ");
                string number = Console.ReadLine();

                // validátor, zda uživatel zadal číslo
                succes = int.TryParse(number, out _);
                if (!succes)
                {
                    Console.WriteLine("Vstup se nepovedlo převést na celé číslo, zkuste to znovu...");
                    Console.WriteLine("Stiskněte Enter pro nový pokus");
                    Console.Clear();
                }
                else
                {
                    return int.Parse(number);
                }
            } while (!succes);
            return -1;
        }

        // funkce pro vypsání prvočísel do terminálu
        public static void OutputPrimeNumbersToTerminal<T>(int number, Func<int, IEnumerable<int>> Function)
        {
            Console.WriteLine("Prvočísla do hranice " + number.ToString() + " jsou:");
            // pro každé prvočíslo
            foreach (int i in Function(number))
            {
                Console.Write(i.ToString() + ", ");
            }
            Console.WriteLine(Environment.NewLine);
            return;
        }

        // funkce pro vypisování prvočísel do souboru
        public static void OutputPrimeNumbersToFile<T>(string fileName, int number, Func<int, IEnumerable<int>> Function)
        {
            // nemusíme kontrolovat zda soubor existuje, StreamWriter automaticky vytvoří soubor pokud neexistuje
            // používám using, abych se nemusel starat o zavírání souboru, using ho zavře automaticky
            Console.WriteLine("Začínám zápis");
            using (StreamWriter sw = File.AppendText(Directory.GetCurrentDirectory() + fileName))
            {
                // každá hranice se vypíše na jeden řádek
                sw.Write(number.ToString() + " -> ");
                foreach (int i in Function(number))
                {
                    // každé prvočíslo se vypíše za hranici, na ten samý řádek
                    sw.Write(i.ToString() + ", ");
                }
                // příklad zápisu: 10 -> 2, 3, 5, 7
                sw.Write(Environment.NewLine);
            }
            Console.WriteLine("Prvočísla jsou zapsaná zde: {0}/{1}", Directory.GetCurrentDirectory(), fileName);
            return;
        }
        public static void OutputPrimeNumbers<T>(int outputType, int number, Func<int, IEnumerable<int>> Function)
        {
            // wrapper funkce pro výpis prvočísel

            // předcházíme vyhození chyby ArgumentOutOfRangeException z Generátoru ErathosenovoSito
            if (number < 2)
            {
                Console.WriteLine("Pro číslo {0} neexistují prvočísla", number);
                return;
            }
            // pokud outputType == 1 vypíšeme prvočísla do konzole
            if (outputType == 1)
            {
                OutputPrimeNumbersToTerminal<T>(number, Function);
            }

            // v opačném případě zapíšeme do souboru
            else
            {
                // fileName je relativní cesta z aktivního adresáře tj. adresář ze kterého byl program spuštěn
                // -> soubor prvocisla.txt se vytvoří "vedle" tohoto programu
                string fileName = "\\prvocisla.txt";
                OutputPrimeNumbersToFile<T>(fileName, number, Function);
            }
            return;
        }
    }

}
