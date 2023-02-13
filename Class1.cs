using System;
using System.Text.RegularExpressions;


namespace IOWrapper
{

    public class IOWrapper
    {
        public IOWrapper()
        {
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
        }
    }
}