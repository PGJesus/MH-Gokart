using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH_Gokart
{
    class Helyszin
    {
        public string Nev;
        public string Cim;
        public string Telefon;
        public string Domain;

        public Helyszin(string nev, string cim, string telefon, string domain)
        {
            Nev = nev;
            Cim = cim;
            Telefon = telefon;
            Domain = domain;
        }
        public void Kiir()
        {
            Console.WriteLine($"\tNév:\t\t{Nev}");
            Console.WriteLine($"\tCím:\t\t{Cim}");
            Console.WriteLine($"\tTelefon:\t{Telefon}");
            Console.WriteLine($"\tWeboldal:\thttp://{Domain}");
        }
    }

    class Versenyzo
    {
        public string Vezeteknevev;
        public string Keresztnev;
        public DateTime Szulido;
        public bool Vane18;
        public string Azonosito;
        public string Email;
        public Versenyzo(string vezeteknevev, string keresztnev, DateTime szulido)
        {
            Vezeteknevev = vezeteknevev;
            Keresztnev = keresztnev;
            Szulido = szulido;
            Vane18 = DateTime.Now.Year - Szulido.Year >= 18;
            Azonosito = "GO-" + Vezeteknevev + Keresztnev + "-" + Szulido.ToString("yyyyMMdd");
            Email = (Vezeteknevev + "." + Keresztnev + "@gmail.com").ToLower();
        }
        public static void KiirFejlec()
        {
            Console.WriteLine($"\t{"Azonosító",-32}{"Név",-22}{"Szül. idő",-14}{"18 éves-e",-12}{"Email",-32}");
            Console.WriteLine("\t" + new string('-', 112));
        }

        public void Kiir()
        {
            Console.WriteLine($"\t{Azonosito,-32}{(Vezeteknevev + " " + Keresztnev),-22}{Szulido,-14:yyyy.MM.dd}{Vane18,-12}{Email,-32}");
        }
    }

    class Naptar
    {
        public DateTime Datum;
        public List<string>[] Foglalasok;

        public Naptar(DateTime datum)
        {
            Datum = datum;
            Foglalasok = new List<string>[11];
            for (int i = 0; i < 11; i++)
            {
                Foglalasok[i] = new List<string>();
            }
        }

        public bool Szabade(int oraIndex)
        {
            return Foglalasok[oraIndex].Count < 4;
        }

        public void Foglal(int oraIndex, string azonosito)
        {
            Foglalasok[oraIndex].Add(azonosito);
        }

        public void Kiir()
        {
            Console.Write($"\t{Datum:yyyy.MM.dd}\t");
            ConsoleColor original = Console.ForegroundColor;

            for (int i = 0; i < 11; i++)
            {
                Console.ForegroundColor = Foglalasok[i].Count == 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write($"{i + 8,2}-{i + 9,-3}");
            }

            Console.ForegroundColor = original;
            Console.WriteLine();
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            #region fejlec
            Console.WriteLine("MH-Gokart");
            Console.WriteLine("2026.09.07.");
            Console.WriteLine();
            /*MH-Gokart
             2026.09.07.
             */
            #endregion

            Random rnd = new Random();

            #region 1.
            Console.WriteLine("1.Feladat");

            Helyszin helyszin = new Helyszin("MH-Gokart Kft.", "1011 Budapest, Fő utca 1.", "06-1-234-5678", "MH-gokart.hu");
            helyszin.Kiir();

            #endregion

            #region 2.
            Console.WriteLine();
            Console.WriteLine("2.Feladat");

            string[] vezeteknevek = File.ReadAllLines("vezeteknevek.txt");
            string[] keresztnevek = File.ReadAllLines("keresztnevek.txt");

            int versenyzoSzam = rnd.Next(1, 151);
            List<Versenyzo> versenyzok = new List<Versenyzo>();

            DateTime legkorabbi = new DateTime(1962, 1, 1);
            int napokTartomanya = (DateTime.Now - legkorabbi).Days;

            for (int i = 0; i < versenyzoSzam; i++)
            {
                string vezeteknev = vezeteknevek[rnd.Next(vezeteknevek.Length)];
                string keresztnev = keresztnevek[rnd.Next(keresztnevek.Length)];
                DateTime szulido = legkorabbi.AddDays(rnd.Next(napokTartomanya));

                versenyzok.Add(new Versenyzo(vezeteknev, keresztnev, szulido));
            }

            Console.WriteLine($"Versenyzők száma: {versenyzoSzam}");
            #endregion

            #region 3.
            Console.WriteLine();
            Console.WriteLine("3.Feladat");

            Console.WriteLine("A pálya 8:00 és 19:00 között van nyitva.");
            Console.WriteLine("A pályát minimum 1 óra időtartamra kell bérelni.");
            Console.WriteLine("A pályán minimum 8 - maximum 20 versenyző lehet egyszerre.");
            Console.WriteLine("Nem kell, hogy egy csoport tagjai legyenek a pályán, külsősként is be lehet csatlakozni a menetbe.");
            Console.WriteLine("Az adott személy min. 1 , max. 2 órára foglaljon. A 2 órás foglalás összefüggő kell, hogy legyen!");
            #endregion

            #region 4.
            Console.WriteLine();
            Console.WriteLine("4.feladat:");

            DateTime ma = DateTime.Now.Date;
            DateTime honapVege = new DateTime(ma.Year, ma.Month, DateTime.DaysInMonth(ma.Year, ma.Month));

            List<Naptar> naptarak = new List<Naptar>();
            for (DateTime nap = ma; nap <= honapVege; nap = nap.AddDays(1))
            {
                naptarak.Add(new Naptar(nap));
            }

            Console.WriteLine("\t\t\t8-9  9-10 10-11 11-12 12-13 13-14 14-15 15-16 16-17 17-18 18-19");
            foreach (Naptar n in naptarak)
            {
                n.Kiir();
            }
            #endregion

            #region 5.
            Console.WriteLine();
            Console.WriteLine("5.Feladat:");

            bool kilepes = false;
            while (!kilepes)
            {
                Console.WriteLine();
                Console.WriteLine("\t1 - Versenyzők listázása");
                Console.WriteLine("\t2 - Időpont foglalása/módosítása azonosító alapján");
                Console.WriteLine("\t3 - Naptár megjelenítése");
                Console.WriteLine("\t0 - Kilépés");
                Console.Write("\tVálasztás: ");
                string valasztas = Console.ReadLine();

                if (valasztas == "1") 
                {
                    foreach (Versenyzo v in versenyzok)
                    {
                        Versenyzo.KiirFejlec();
                        foreach (Versenyzo versenyzo in versenyzok)
                        {
                            versenyzo.Kiir();
                        }
                    }
                }
                else if (valasztas == "2") 
                {

                }
                else if (valasztas == "3") 
                {

                }
                else if (valasztas == "0") 
                {
                    kilepes = true;
                }
                else
                {
                    Console.WriteLine("Válasszon egy érvényes opciót!");
                }
            }
            #endregion
        }
    }
}
