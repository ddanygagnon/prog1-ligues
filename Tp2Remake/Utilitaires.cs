#region

using System;
using System.Linq;

#endregion

namespace Tp2Remake
{
    static class Utilitaires
    {
        public static void WriteLineError(string texte)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(texte);
            ResetConsole();
        }
        
        public static void WriteLineSucess(string texte)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(texte);
            ResetConsole();
        }
        public static void WriteLineInfo(string texte)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(texte);
            ResetConsole();
        }

        static void ResetConsole()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static string LireChiffreString(string p_question, int nombreChiffres)
        {
            string texte;
            for (;;)
            {
                Console.Write(p_question);
                texte = Console.ReadLine()?.Trim();
                if(texte.All(char.IsDigit) && texte.Length == nombreChiffres) break;
                WriteLineError($"*** Veuillez entrer {nombreChiffres} chiffres.");
            }

            return texte;
        }
        public static string LireString(string p_question)
        {
            string texte;
            for (;;)
            {
                Console.Write(p_question);
                texte = Console.ReadLine()?.Trim();
                if (texte?.Length >= 1) break;

                WriteLineError("*** Veuillez entrer un texte.");
            }

            return texte;
        }

        public static string LireStringTailleControlee(string p_question, int p_lgMin, int p_lgMax)
        {
            string texte;

            for (; ; )
            {
                Console.Write(p_question);
                texte = Console.ReadLine()?.Trim();

                if (p_lgMin <= texte.Length && texte.Length <= p_lgMax) break;

                WriteLineError($"*** Veuillez entrer un texte contenant entre {p_lgMin} et {p_lgMax} caractères.");
            }

            return texte;
        }
        
        public static int LireStringPointage(string p_question, out int deuxiemeNb)
        {
            int premierNb;

            for (;;)
            {
                Console.Write(p_question);

                string input = Console.ReadLine();
                string[] hello = input?.Split("-");
                if (hello != null && hello.Length == 2)
                    if (int.TryParse(hello[0], out premierNb) && int.TryParse(hello[1], out deuxiemeNb) && premierNb >= 0 && deuxiemeNb >= 0) break;
                WriteLineError("*** Veuillez entrer un score sous le format nombre-nombre, (ex. 0-0).");
            }

            return premierNb;
        }

        public static int LireInt32Write(string p_question)
        {
            int nb;

            for (;;)
            {
                Console.Write(p_question);
                if (int.TryParse(Console.ReadLine(), out nb)) break;
                WriteLineError("*** Veuillez entrer un simple nombre entier.");
            }

            return nb;
        }
        
        public static DateTime LireDate(string p_question)
        {
            DateTime date;

            for (; ; )
            {
                Console.Write(p_question);
                if (DateTime.TryParse(Console.ReadLine(), out date)) break;
                WriteLineError("*** Veuillez entrer une date valide. par exemple: (aaaa-mm-jj).");
            }

            return date;
        }

        public static int LireInt32DansIntervalleWrite(string p_question, int p_minimum, int p_maximum)
        {
            int nombre;

            for (;;)
            {
                nombre = LireInt32Write(p_question);
                if (p_minimum <= nombre && nombre <= p_maximum) break;
                WriteLineError($"*** Veuillez entrer un nombre entier entre {p_minimum} et {p_maximum}.");
            }

            return nombre;
        }
    }
}