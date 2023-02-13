using System;
using System.Collections.Generic;

namespace SemestralniProjekt
{
    internal class ErathosenovoSito
    {
        // Erathosenovo síto hledá prvočísla do zadané horní hranice
        public static IEnumerable<int> ESito(int max)
        {
            // neexistuje nižší prvočíslo než 2
            if (max < 2)
            {
                throw new ArgumentOutOfRangeException("Číslo musí bý větší než 2.");
            }

            // Nastavení pole, přes které se prvočísla budou "přesívat"
            bool[] sito = new bool[max + 1];
            for (int i = 0; i < max; i++)
                sito[i] = true;

            for (int p = 2; p * p <= max; p++)
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
    }
}
