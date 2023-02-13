using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SemestralniProjekt1
{

    internal class SemestralniProjekt
    {
        static IEnumerable<int> ErathosenovoSito(int max)
        // Erathosenovo síto hledá prvočísla do zadané horní hranice
        {
            // neexistuje nižší prvočíslo než 2
            if (max < 2)
            {
                throw new ArgumentOutOfRangeException("Číslo musí bý větší než 2.");
            }

            // Nastavení pole, přes které se prvočísla budou "přesívat"
            bool[] sito = new bool[max + 1];
            for (int i = 0; i <= max; i++)
                sito[i] = true;

            for (int p = 2; p * p < max; p++)
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
                    // Generátor z důvodu paměťové nenáročnosti, nevíme jak velkou hranici uživatel zadá
                    // i při velké hranici pro zjištění prvočísel, budeme v jednom cyklu zabírat paměť pouze velikost jednoho int
                    yield return i;
            }
        }

        // funkce pro zjištění zda budeme načítat čísla z terminálu, nebo ze souboru
        // input == true -> výběr způsobu zadání čísla
        // input == false -> výběr způsobu vypsání prvočísel
        static int GetIOType(bool input)
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
                    Console.WriteLine("pozn.: Při načtení z tefrminálu lze zadat pouze jedno číslo, při načtení ze souboru je počet čísel neomezený (každé číslo musí být na samostatném řádku)");
                    setUp = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Chcete vypsat čísla do konzole, nebo zapsat do souboru?");
                    Console.WriteLine("1. Vypsat do konzole");
                    Console.WriteLine("2. Zapsat do souboru (uloží se do souboru prvocisla.txt)");
                    setUp = Console.ReadLine();
                }
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
        static int GetNumberFromTerminal()
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
        static void OutputPrimeNumbersToTerminal(int number)
        {
            Console.WriteLine("Prvočísla do hranice " + number.ToString() + " jsou:");
            foreach (int i in ErathosenovoSito(number))
            {
                Console.Write(i.ToString() + ", ");
            }
            Console.WriteLine(Environment.NewLine);
            return;
        }

        static void OutputPrimeNumbersToFile(string fileName, int number)
        {
            // nemusíme kontrolovat zda soubor existuje, StreamWriter automaticky vytvoří soubor pokud neexistuje
            // používám using, abych se nemusel starat o zavírání souboru, using ho zavře automaticky
            Console.WriteLine("Začínám zápis");
            using (StreamWriter sw = File.AppendText(Directory.GetCurrentDirectory() + fileName))
            {
                // každá hranice se vypíše na jeden řádek
                sw.Write(number.ToString() + " -> ");
                foreach (int i in ErathosenovoSito(number))
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
        static void OutputPrimeNumbers(int outputType, int number)
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
                OutputPrimeNumbersToTerminal(number);
            }

            // v opačném případě zapíšeme do souboru
            else
            {
                // fileName je relativní cesta z aktivního adresáře tj. adresář ze kterého byl program spuštěn
                // -> soubor prvocisla.txt se vytvoří "vedle" tohoto programu
                string fileName = "\\prvocisla.txt";
                OutputPrimeNumbersToFile(fileName, number);
            }
            return;
        }

        static void Main(string[] args)
        {
            Console.Clear();
            string opakovat;
            // možnost opakování programu
            do
            {
                int inputType = GetIOType(true);
                // pokud inputType == 1 -> zadání z terminálu
                if (inputType == 1)
                {
                    int outputType = GetIOType(false);

                    int number = GetNumberFromTerminal();

                    OutputPrimeNumbers(outputType, number);
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
                    int outputType = GetIOType(false);

                    bool succes;
                    // pro každý řádek zvalidujeme zda je na řádku číslo, pokud validace neprojde řádek se přeskočí
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
                        OutputPrimeNumbers(outputType, int.Parse(line));
                    }
                }

                // vybrání možnosti opakování
                Console.WriteLine("Chcete program spustit znovu, nebo ukočit?");
                Console.WriteLine("'yes' pro ukončení");
                Console.WriteLine("Vše ostatní pro pokračování");
                opakovat = Console.ReadLine();
                // tento regex umnožňuje zapsat všechny kombinace slova "yes" bez ohledu na malá a velká písmena
            } while (Regex.IsMatch(opakovat, @"[\W * ((? i)yes(?-i))\W *]"));
        }
    }
}
