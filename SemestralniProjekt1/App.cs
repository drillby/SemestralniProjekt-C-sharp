using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SemestralniProjekt
{

    internal class App
    {


        static void Main(string[] args)
        {
            Console.Clear();
            string opakovat;
            // možnost opakování programu
            do
            {
                //int inputType = GetIOType(true);
                int inputType = IOHandler.GetIOType(true);
                // pokud inputType == 1 -> zadání z terminálu
                if (inputType == 1)
                {
                    int outputType = IOHandler.GetIOType(false);

                    int number = IOHandler.GetNumberFromTerminal();

                    IOHandler.OutputPrimeNumbers<int>(outputType, number, ErathosenovoSito.ESito);
                }
                // jinak zadání ze souboru
                else
                {

                    // kontrola zda soubor existuje
                    string input_numbers = "\\input.txt";
                    if (!File.Exists(Directory.GetCurrentDirectory() + input_numbers))
                    {
                        Console.WriteLine(Directory.GetCurrentDirectory() + input_numbers);
                        Console.WriteLine("Soubor input.txt neexistuje v aktivním adresáři {0}, konec programu", Directory.GetCurrentDirectory());
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                    int outputType = IOHandler.GetIOType(false);

                    bool succes;
                    // pro každý řádek zvalidujeme zda je na řádku číslo, pokud validace neprojde řádek se přeskočí
                    // obdobně jako u samotné funkce ErathosenovoSito je zde zvolena metoda čtení dat pomocí Generátoru,
                    // tento důvod byl zvolen, protože nevíme jak velký soubor nám uživatel předá ke zpracování
                    foreach (string line in File.ReadLines(Directory.GetCurrentDirectory() + input_numbers))
                    {
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

                // vybrání možnosti opakování chodu programu
                Console.WriteLine("Chcete program ukončit, nebo spustit znovu?");
                Console.WriteLine("'yes' pro ukončení");
                Console.WriteLine("Vše ostatní pro pokračování");
                opakovat = Console.ReadLine();
                // tento regex umnožňuje zapsat všechny kombinace slova "yes" bez ohledu na malá a velká písmena
            } while (!Regex.IsMatch(opakovat, @"[\W * ((? i)yes(?-i))\W *]"));
        }
    }
}
