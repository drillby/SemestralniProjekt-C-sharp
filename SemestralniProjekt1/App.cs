using System;
using System.IO;

namespace SemestralniProjekt
{
    // hlavní aplikace
    internal class App
    {
        // funkce pro zpracování čísla z terminálu
        static void HandleInputType1()
        {
            int outputType = IOHandler.GetIOType(false);

            // pozor na int overflow
            int number = IOHandler.GetNumberFromTerminal();

            IOHandler.OutputPrimeNumbers<int>(outputType, number, ErathosenovoSito.ESito);
        }

        // funkce pro zpracování čísel ze souboru
        static void HandleInputType2(string input_numbers)
        {
            // kontrola zda soubor existuje
            // musíme si dát pozor na spoštění tohoto programu na UNIXových platformách, adresáře se oddělují / a ne \ (možná vyřešil Path.Combine ?)
            if (!File.Exists(Directory.GetCurrentDirectory() + input_numbers))
            {
                // debug
                Console.WriteLine(Path.Combine(Directory.GetCurrentDirectory(), input_numbers));
                Console.WriteLine("Soubor input.txt neexistuje v aktivním adresáři {0}, konec programu", Directory.GetCurrentDirectory());
                Console.ReadLine();
                Environment.Exit(0);
            }
            int outputType = IOHandler.GetIOType(false);

            bool succes;
            // pro každý řádek zvalidujeme zda je na řádku číslo, pokud validace neprojde řádek se přeskočí
            // obdobně jako u samotné funkce ErathosenovoSito.ESito je zde zvolena metoda čtení dat pomocí Generátoru,
            // tento důvod byl zvolen, protože nevíme jak velký soubor nám uživatel předá ke zpracování
            foreach (string line in File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), input_numbers)))
            {
                // pozor na int overflow
                succes = int.TryParse(line, out _);
                // pokud string na řádku nelze přetypovat na int, překočíme iteraci
                if (!succes)
                {
                    Console.WriteLine("Text {0} nelze předělat na celé číslo...", line);
                    continue;
                }
                Console.WriteLine("Nalezeno číslo " + line);
                IOHandler.OutputPrimeNumbers<int>(outputType, int.Parse(line), ErathosenovoSito.ESito);
            }
        }

        // funkce pro vyhodnocení zda se má aplikace ukončit
        static bool EndApp()
        {
            // vybrání možnosti opakování chodu programu
            Console.WriteLine("Chcete program ukončit, nebo spustit znovu?");
            Console.WriteLine("'pokracovat' pro pokračování");
            Console.WriteLine("Vše ostatní pro pokračování");
            opakovat = Console.ReadLine();
            // jednoduché porovnání hodnoty stringu, spíš by se sem hodil regex aby input nebyl case sensitive
            // True -> program pokračuje
            // False -> konec programu
            return opakovat == "pokracovat";
        }

        static void Main(string[] args)
        {
            Console.Clear();
            string input_numbers = "input.txt";
            bool pokracovat;
            // do-while pro možnost opakování programu
            do
            {
                int inputType = IOHandler.GetIOType(true);
                // pokud inputType == 1 -> zadání z terminálu
                if (inputType == 1)
                {
                    HandleInputType1();
                }
                // jinak zadání ze souboru
                else
                {
                    HandleInputType2();
                }
            
            pokracovat = EndApp();
            } while (pokracovat);
        }
    }
}
