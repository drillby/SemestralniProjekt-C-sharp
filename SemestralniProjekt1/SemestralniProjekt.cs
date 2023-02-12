using CsvHelper;
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




        static void Main(string[] args)
        {
            // původní nastavení
            string setUp = "";
            bool podminka = setUp != "1" || setUp != "2";
            
            // dokud uživatel nezadá správný vstup "1", nebo "2"
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

            // pokud vybral 
            if (setUp == "1")
            {
                bool succes;
                do { 
                    Console.WriteLine("Napište číslo, které chcete rozložit na prvočísla:");
                    setUp= Console.ReadLine();
                    succes = int.TryParse(setUp, out int ignore);
                    if (!succes)
                    {
                        Console.WriteLine("Vstup se nepovedlo převést na celé číslo, zkuste to znovu...");
                        Console.WriteLine("Stiskněte Enter pro nový pokus");
                        Console.Clear();
                    }
                } while(!succes);

                int id = 0;
                foreach (int i in ErathosenovoSito(int.Parse(setUp)))
                {
                    Prvocislo cislo = new Prvocislo { Id = id, Value= i};
                    id++;
                }
                Console.ReadLine();
            }
        }
    }
}
