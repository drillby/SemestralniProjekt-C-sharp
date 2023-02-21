using System;
using System.Collections.Generic;

namespace SemestralniProjekt
{
    internal class ErathosenovoSito
    {
        /// <summary>
        /// Erathosenovo síto hledá prvočísla do zadané horní hranice 
        /// </summary>
        /// <param name="max">Horní hranice</param>
        /// <returns>Prvočísla</returns>
        /// <exception cref="ArgumentOutOfRangeException">Pokud <param>max</param>max < 2</exception>
        public static IEnumerable<uint> ESito(uint max)
        {
            // neexistuje nižší prvočíslo než 2
            if (max < 2)
            {
                throw new ArgumentOutOfRangeException("Číslo musí bý větší než 2.");
            }

            // Nastavení pole, přes které se prvočísla budou "přesívat"
            // toto pole je místo které může vést k problémům s operační pamětí,
            // i přes to, že v poli jsou bool hodnoty při velkém poli může být zabraná paměť znatelná
            bool[] sito = new bool[max + 1];
            for (int i = 0; i < max; i++)
                sito[i] = true;

            for (uint p = 2; p * p <= max; p++)
            {
                // pokud nedošlo ke změně, pak sito[p] je prvočíslo
                if (!sito[p])
                {
                    continue;
                }
                // násobky p změnit na false
                for (uint i = p * p; i <= max; i += p)
                    sito[i] = false;
            }

            for (uint i = 2; i <= max; i++)
            {
                if (!sito[i])
                {
                    continue;
                }
                // Generátor z důvodu paměťové nenáročnosti, nevíme jak velkou hranici uživatel zadá
                // i při velké hranici pro zjištění prvočísel, budeme v jednom cyklu zabírat paměť pouze velikost jednoho int
                yield return i;
            }
        }
    }
}
