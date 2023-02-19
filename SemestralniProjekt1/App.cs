using System;
using System.IO;

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
            bool outputType = IOHandler.GetIOType(false);

            // pozor na int overflow
            uint number = IOHandler.GetNumberFromTerminal();

            IOHandler.OutputPrimeNumbers<uint>(outputType, number, ErathosenovoSito.ESito);
        }
        /// <summary>
        /// Spustí část programu pro zpracování čísel ze souboru
        /// </summary>
        /// <param name="input_numbers">Název souboru ze které ho se čísla budou číst. Název souboru se připojí k cestě adresáře ve kterém je program spuštěm</param>
        private static void HandleFileInput(string input_numbers)
        {
            // kontrola zda soubor existuje
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), input_numbers)))
            {
                Console.WriteLine(Path.Combine(Directory.GetCurrentDirectory(), input_numbers));
                Console.WriteLine("Soubor input.txt neexistuje v aktivním adresáři {0}, konec programu", Directory.GetCurrentDirectory());
                Console.ReadLine();
                Environment.Exit(0);
            }
            bool outputType = IOHandler.GetIOType(false);

            bool succes;
            // pro každý řádek zvalidujeme zda je na řádku číslo, pokud validace neprojde řádek se přeskočí
            // obdobně jako u samotné funkce ErathosenovoSito.ESito je zde zvolena metoda čtení dat pomocí Generátoru,
            // tento důvod byl zvolen, protože nevíme jak velký soubor nám uživatel předá ke zpracování
            foreach (string line in File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), input_numbers)))
            {
                // pozor na int overflow
                succes = int.TryParse(line, out _);
                // pokud string na řádku nelze přetypovat na int, překočíme iteraci
                if (!int.TryParse(line, out _))
                {
                    Console.WriteLine("Text {0} nelze předělat na celé číslo...", line);
                    continue;
                }
                Console.WriteLine("Nalezeno číslo " + line);
                IOHandler.OutputPrimeNumbers<int>(outputType, uint.Parse(line), ErathosenovoSito.ESito);
            }
        }

        /// <summary>
        /// Vyhodnocuje ukončení programu
        /// </summary>
        /// <returns>
        /// True -> chod programu se opakuje,
        /// False -> konec programu
        /// </returns>
        private static bool EndApp()
        {
            // vybrání možnosti opakování chodu programu
            Console.WriteLine("Chcete program ukončit, nebo spustit znovu?");
            Console.WriteLine("'pokracovat' pro pokračování");
            Console.WriteLine("Vše ostatní pro pokračování");
            string opakovat = Console.ReadLine();
            // jednoduché porovnání hodnoty stringu, spíš by se sem hodil regex aby input nebyl case sensitive
            // True -> program pokračuje
            // False -> konec programu
            return opakovat == "pokracovat";
        }

        /// <summary>
        /// Hlavní smyčka programu
        /// </summary>
        /// <param name="args">Argumennty předané pří spušnění programu, využito</param>
        static void Main(string[] args)
        {
            Console.Clear();
            string input_numbers = "input.txt";
            // do-while pro možnost opakování programu
            do
            {
                bool inputType = IOHandler.GetIOType(true);
                // pokud inputType == 1 -> zadání z terminálu
                if (inputType)
                {
                    HandleTerminalInput();
                }
                // jinak zadání ze souboru
                else
                {
                    HandleFileInput(input_numbers);
                }
            } while (EndApp());
        }
    }
}
