using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SemestralniProjekt
{
    // hlavní aplikace
    internal class App
    {
        /// <summary>
        /// Spustí část programu pro zpracování čísla z terminálu
        /// </summary>
        private static void HandleTerminalInput()
        {
            const bool isInput = false;
            bool isTerminal = IOHandler.GetIOType(isInput);

            // pozor na int overflow
            uint number = IOHandler.GetNumberFromTerminal();

            IOHandler.OutputPrimeNumbers<uint>(isTerminal, number, ErathosenovoSito.ESito);
        }

        /// <summary>
        /// Spustí část programu pro zpracování čísel ze souboru
        /// </summary>
        /// <param name="inputNumbers">Název souboru ze které ho se čísla budou číst. Název souboru se připojí k cestě adresáře ve kterém je program spuštěm</param>
        private static void HandleFileInput(string inputNumbers)
        {
            // kontrola zda soubor existuje
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), inputNumbers)))
            {
                Console.WriteLine(Path.Combine(Directory.GetCurrentDirectory(), inputNumbers));
                Console.WriteLine("Soubor input.txt neexistuje v aktivním adresáři {0}, konec programu", Directory.GetCurrentDirectory());
                Console.ReadLine();
                Environment.Exit(0);
            }

            const bool isInput = false;
            bool isTerminal = IOHandler.GetIOType(isInput);

            // pro každý řádek zvalidujeme zda je na řádku číslo, pokud validace neprojde řádek se přeskočí
            // načítáme řádek po řádku, ne vše najednou, tento důvod byl zvolen, protože nevíme jak velký soubor nám uživatel předá ke zpracování
            foreach (string line in File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), inputNumbers)))
            {
                // pokud string na řádku nelze přetypovat na int, překočíme iteraci
                bool isParsable = uint.TryParse(line, out _);
                if (!isParsable)
                {
                    Console.WriteLine("Text {0} nelze předělat na celé číslo...", line);
                    continue;
                }
                Console.WriteLine("Nalezeno číslo " + line);
                IOHandler.OutputPrimeNumbers<uint>(isTerminal, uint.Parse(line), ErathosenovoSito.ESito);
            }
        }

        /// <summary>
        /// Vyhodnocuje ukončení programu
        /// </summary>
        /// <returns>
        /// True -> chod programu se opakuje,
        /// False -> konec programu
        /// </returns>
        private static bool ContinueApp()
        {
            // vybrání možnosti opakování chodu programu
            Console.WriteLine("Chcete program ukončit, nebo spustit znovu?");
            Console.WriteLine("'pokracovat' pro pokračování");
            Console.WriteLine("Vše ostatní pro konec programu");
            string opakovat = Console.ReadLine();
            // jednoduché porovnání hodnoty stringu, spíš by se sem hodil regex aby input nebyl case sensitive
            return Regex.IsMatch(opakovat, "^((?i)pokračovat|(?i)pokracovat)$");
        }

        /// <summary>
        /// Hlavní smyčka programu
        /// </summary>
        /// <param name="args">Argumennty předané pří spušnění programu, nevyužito</param>
        static void Main(string[] args)
        {
            Console.Clear();
            string inputNumbers = "input.txt";
            // do-while pro možnost opakování programu
            do
            {
                const bool isInput = true;
                bool isTerminal = IOHandler.GetIOType(isInput);
                if (isTerminal)
                {
                    HandleTerminalInput();
                }
                else
                {
                    HandleFileInput(inputNumbers);
                }
            } while (ContinueApp());
        }
    }
}
