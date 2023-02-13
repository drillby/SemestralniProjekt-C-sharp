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
        static int getReadType() {
            string setUp = "";
            // chceme aby uživatel zadal jenom "1", nebo "2"
            bool podminka = setUp != "1" || setUp != "2";
            
            // dokud uživatel nezadá správný vstup
            do
            {
                Console.WriteLine("Chcete zadat číslo ručně, nebo načíst ze souboru?");
                Console.WriteLine("1. Načíst z terminálu");
                Console.WriteLine("2. Načíst ze souboru");
                setUp = Console.ReadLine();
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

        static void Main(string[] args)
        {
            int inputType = getReadType();

            if (inputType == 1)
            {
                int number = getNumberFromTerminal();
                
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
                        sw.Write(i.ToString(), ", ")
                    }
                    // příklad zápisu: 10 -> 2, 3, 5, 7
                }

                Console.ReadLine();
            }
        }
    }
}
