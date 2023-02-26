using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SemestralniProjekt
{
    /// <summary>
    /// Wrapper class vytořená pro zastřešení IO operací
    /// </summary>
    public class IOHandler
    {
        /// <summary>
        /// Zjistí zda uživatel chce načíst/vypsat data z/do terminálu, nebo ze/do souboru
        /// </summary>
        /// <param name="input">True -> funkce zjišťuje input dat, False -> funkce zjišťuje output dat</param>
        /// <returns>
        /// Rozhodnutí uživatele
        /// true -> Uživatel vybral terminál
        /// false -> uživatel vybral soubor
        /// </returns>
        public static bool GetIOType(bool input)
        {
            string setUp;

            // dokud uživatel nezadá správný vstup
            do
            {
                if (input)
                {
                    Console.WriteLine("Chcete zadat číslo ručně, nebo načíst ze souboru?");
                    Console.WriteLine("1. Načíst z terminálu");
                    Console.WriteLine("2. Načíst ze souboru (input.txt)");
                    Console.WriteLine("pozn.: Při načtení z terminálu lze zadat pouze jedno číslo, při načtení ze souboru je počet čísel neomezený (každé číslo musí být na samostatném řádku)");
                    setUp = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Chcete vypsat čísla do konzole, nebo zapsat do souboru?");
                    Console.WriteLine("1. Vypsat do konzole");
                    Console.WriteLine("2. Zapsat do souboru (uloží se do prvocisla.txt)");
                    setUp = Console.ReadLine();
                }

                // pokud uživatelký vstup neodpovídá podmínce vypíše se chybová hláška
                if (!Regex.IsMatch(setUp, @"^(1|2)$"))
                {
                    Console.WriteLine("Španý vstup, zkuste to znovu...");
                    Console.WriteLine("Stiskněte Enter pro pokračování");
                    Console.ReadLine();
                    Console.Clear();

                }
                // kontrola vstupu pomocí regex, vrací true pokud setUp == 1 || setUp == 2
            } while (!Regex.IsMatch(setUp, "^(1|2)$"));
            return setUp == "1";
        }

        /// <summary>
        /// Funkce pro čtení input dat z terminálu
        /// </summary>
        /// <returns>
        /// Číslo zadané uživatelem
        /// </returns>
        public static uint GetNumberFromTerminal()
        {
            bool isParsable;

            // dokud uživatel nazadá číslo
            do
            {
                Console.WriteLine("Napište hranici do které chcete vypsat prvočísla: ");
                string number = Console.ReadLine();

                // validátor, zda uživatel zadal číslo
                isParsable = uint.TryParse(number, out _);
                if (!isParsable)
                {
                    Console.WriteLine("Vstup se nepovedlo převést na celé číslo, zkuste to znovu...");
                    Console.WriteLine("Stiskněte Enter pro nový pokus");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    return uint.Parse(number);
                }
            } while (!isParsable);
            return 0;
        }

        /// <summary>
        /// Funkce pro vypsání prvočisel do terminálu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="number">Horní hranice do které se mají zjistit prvočisla</param>
        /// <param name="Function">Funkce ve formě generátoru pro zjištění prvočísel do horní hranice</param>
        private static void OutputPrimeNumbersToTerminal<T>(uint number, Func<uint, IEnumerable<uint>> Function)
        {
            Console.WriteLine("Prvočísla do hranice {0} jsou:", number.ToString());
            // pro každé prvočíslo
            foreach (uint i in Function(number))
            {
                Console.Write(i.ToString() + ", ");
            }
            Console.WriteLine(Environment.NewLine);
            return;
        }

        /// <summary>
        /// Funkce pro vypsání prvočísel do souboru
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Název souboru kam vypsat prvočísla, soubor se připojí k cestě složky ve které byl program spuštěn</param>
        /// <param name="number">Horní hranice do které se mají zjistit prvočisla</param>
        /// <param name="Function">Funkce ve formě generátoru pro zjištění prvočísel do horní hranice</param>
        private static void OutputPrimeNumbersToFile<T>(string fileName, uint number, Func<uint, IEnumerable<uint>> Function)
        {
            Console.WriteLine("Začínám zápis");

            // nemusíme kontrolovat zda soubor existuje, StreamWriter automaticky vytvoří soubor pokud neexistuje
            // používám using, abych se nemusel starat o zavírání souboru, using ho zavře automaticky
            using (StreamWriter sw = File.AppendText(Path.Combine(Directory.GetCurrentDirectory(), fileName)))
            {
                // každá hranice se vypíše na jeden řádek
                sw.Write("{0} -> ", number.ToString());
                foreach (uint i in Function(number))
                {
                    // každé prvočíslo se vypíše za hranici, na ten samý řádek
                    sw.Write(i.ToString() + ", ");
                }
                // příklad zápisu: 10 -> 2, 3, 5, 7
                sw.Write(Environment.NewLine);
            }
            Console.WriteLine("Prvočísla jsou zapsaná zde: {0}", Path.Combine(Directory.GetCurrentDirectory(), fileName));
            return;
        }

        /// <summary>
        /// Wrapper funkce pro zajištění outputu čísel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="outputType">
        /// true -> output do terminálu
        /// false ->output do souboru
        /// </param>
        /// <param name="number">Horní hranice do které se mají zjistit prvočisla</param>
        /// <param name="Function">Funkce ve formě generátoru pro zjištění prvočísel do horní hranice</param>
        public static void OutputPrimeNumbers<T>(bool outputType, uint number, Func<uint, IEnumerable<uint>> Function)
        {
            // předcházíme vyhození chyby ArgumentOutOfRangeException z Generátoru ErathosenovoSito
            const uint minValue = 2;
            if (number < minValue)
            {
                Console.WriteLine("Pro číslo {0} neexistují prvočísla", number);
                return;
            }
            if (outputType)
            {
                OutputPrimeNumbersToTerminal<T>(number, Function);
            }
            else
            {
                // fileName je relativní cesta z aktivního adresáře tj. adresář ze kterého byl program spuštěn
                string fileName = "prvocisla.txt";
                OutputPrimeNumbersToFile<T>(fileName, number, Function);
            }
            return;
        }
    }

}
