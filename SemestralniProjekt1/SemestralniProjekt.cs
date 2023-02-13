using System;
using System.Globalization;
using System.IO;

namespace SemestralniProjekt1
{

    public class Prvocislo
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }

    internal class SemestralniProjekt
    {
        static System.Collections.Generic.IEnumerable<int> ErathosenovoSito(int max)
        {
            // neexistuje nižší prvočíslo než 2
            if(max < 2)
            {
                throw new ArgumentOutOfRangeException("Číslo musí bý větší než 2.");
            }

            // Nastavení pole, přes které se prvočísla budou "přesívat"
            bool[] sito = new bool[max + 1];
            for (int i = 0; i <= max; i++)
                sito[i] = true;

            for (int p = 2; p * p <= Math.Sqrt(max); p++)
            {
                // pokud nedošlo ke změně, pak sito[p] je prvočíslo
                if (sito[p])
                {
                    // násobky p změnit na false
                    for (int i = p * p; i <= max; i += p)
                        sito[i] = false;
                }
            }

            for (int i = 2; i <= max; i++)
            {
                if (sito[i])
                    // Iterátor z důvodu paměťové nenáročnosti, nevíme jak velkou hranici uživatel zadá
                    yield return i;
            }
        }

        // funkce pro zjištění zda budeme načítat čísla z terminálu, nebo ze souboru
        // input == true -> výběr způsobu zadání čísla
        // input == false -> výběr způsobu vypsání prvočísel
        static int getIOType(bool input) {
            // předinicializace, abychom se vyhnuli inicializaci proměnné pokaždé když uživatel zadá chybný vstup
            string setUp;
            // chceme aby uživatel zadal jenom "1", nebo "2"
            bool podminka = setUp != "1" || setUp != "2";
            
            // dokud uživatel nezadá správný vstup
            do
            {
                if (input) {
                    Console.WriteLine("Chcete zadat číslo ručně, nebo načíst ze souboru?");
                    Console.WriteLine("1. Načíst z terminálu");
                    Console.WriteLine("2. Načíst ze souboru (načte se ze souboru input.txt)");
                    Console.WriteLine("pozn.: Při načtení z tefrminálu lze zadat pouze jedno číslo, při načtení ze souboru je počet čísel neomezený (každé číslo musí být na samostatném řádku)");
                    setUp = Console.ReadLine();
                }
                else {
                    Console.WriteLine("Chcete vypsat čísla do konzole, nebo zapsat do souboru?");
                    Console.WriteLine("1. Vypsat do konzole");
                    Console.WriteLine("2. Zapsat do souboru (uloží se do souboru prvocisla.txt)");
                    setUp = Console.ReadLine();
                }
                if (!podminka)
                {
                    Console.WriteLine("Španý vstup, zkuste to znovu...");
                    Console.WriteLine("Stiskněte Enter pro pokračování");
                    Console.ReadLine();
                    Console.Clear();
                }
            } while (!podminka);

            return int.Parse(setUp);
        }

        // funkce pro přečtení čísla z konzole
        static int getNumberFromTerminal() {
            bool succes;

            // dokud uživatel nazadá číslo
            do { 
                Console.WriteLine("Napište číslo, které chcete rozložit na prvočísla:");
                number= Console.ReadLine();

                // validátor, zda uživatel zadal číslo
                succes = int.TryParse(number, out int ignore);
                if (!succes)
                {
                    Console.WriteLine("Vstup se nepovedlo převést na celé číslo, zkuste to znovu...");
                    Console.WriteLine("Stiskněte Enter pro nový pokus");
                    Console.Clear();
                }
            } while(!succes);

            return int.Parse(number);
        }

        static void outputPrimeNumbers(int outputType) {
            // pokud outputType == 1 vypíšeme prvočísla do konzole
                if(outputType) {
                    Console.WriteLine("Prvočísla do hranice " + number.ToString() + " jsou:");
                    foreach(int i in ErathosenovoSito(number)) {
                        Console.Write(i.ToString() + ", ");
                    }
                }

                // v opačném případě zapíšeme do souboru
                else {
                    // do souboru prvocisla.txt se budou zapisovat prvnočísla
                    string path = "./prvocisla.txt";

                    // nemusíme kontrolovat zda soubor existuje, StreamWriter automaticky vytvoří soubor pokud neexistuje
                    // používám using, abych se nemusel starat o zavírání souboru, using ho zavře automaticky
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        // každá hranice se vypíše na jeden řádek
                        sw.WriteLine(number.ToString() + " -> ");
                        foreach (int i in ErathosenovoSito(number))
                        {
                            // každé prvočíslo se vypíše za hranici, na ten samý řádek
                            sw.Write(i.ToString(), ", ");
                        }
                        // příklad zápisu: 10 -> 2, 3, 5, 7
                    }
                }
                return;
        }

        static void Main(string[] args)
        {
            int inputType = getIOType(true);

            if (inputType)
            {
                int outputType = getIOType(false);
                
                int number = getNumberFromTerminal();

                outputPrimeNumbers(outputType);
                Console.ReadLine();
            }
            else {
                // kontrola zda soubor existuje
                string input_numbers = "./input.txt";
                if(!File.Exists(input_numbers)) {
                    Console.WriteLine("Soubor input neexistuje, konec programu");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                int outputType = getIOType(false);
                
                bool succes;
                // pro každý řádek zvalidujeme zda je na řádku číslo, pokud validace neprojde řádek se přeskočí
                foreach (string line in File.ReadLines(input_numbers))
                {  
                    succes = int.TryParse(number, out int ignore);
                    // pokud řádek nelze předělat na int, překočíme iteraci
                    if(!succes) {
                        Console.WriteLine("Číslo nenalezeno...");
                        continue;
                    }
                    Console.WriteLine("Nalezeno číslo " + line);
                    outputPrimeNumbers(outputType);
                }
            }
        }
    }
}
