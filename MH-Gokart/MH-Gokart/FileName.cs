using System;
using System.Collections.Generic;
using System.IO;

namespace MH_gokart
{
    class Helyszin
    {
        public string Nev;
        public string Cim;
        public string Telefonszam;
        public string Domain;

        public Helyszin(string nev, string cim, string telefonszam, string domain)
        {
            Nev = nev;
            Cim = cim;
            Telefonszam = telefonszam;
            Domain = domain;
        }

        public void Kiiratas()
        {
            Console.WriteLine($"\tNév:\t\t{Nev}");
            Console.WriteLine($"\tCím:\t\t{Cim}");
            Console.WriteLine($"\tTelefon:\t{Telefonszam}");
            Console.WriteLine($"\tWeboldal:\thttp://{Domain}");
        }
    }

    class Versenyzo
    {
        public string Vezeteknev;
        public string Keresztnev;
        public DateTime SzuletesiIdo;
        public bool ElmultE18;
        public string Azonosito;
        public string Email;

        public Versenyzo(string vezeteknev, string keresztnev, DateTime szuletesiIdo)
        {
            Vezeteknev = vezeteknev;
            Keresztnev = keresztnev;
            SzuletesiIdo = szuletesiIdo;

            ElmultE18 = SzuletesiIdo.AddYears(18) <= DateTime.Now;

            Azonosito = "GO-" + Vezeteknev + Keresztnev + "-" + SzuletesiIdo.ToString("yyyyMMdd");
            Email = (Vezeteknev + "." + Keresztnev + "@gmail.com").ToLower();
        }

        public void Kiiratas()
        {
            Console.WriteLine($"\t{Azonosito,-30}{(Vezeteknev + " " + Keresztnev),-25}{SzuletesiIdo:yyyy.MM.dd}\t18 éves: {ElmultE18,-6}\t{Email}");
        }
    }

    class Naptar
    {
        public DateTime Datum;
        public List<string>[] Foglalasok; // 11 sáv: 8-9, 9-10, ..., 18-19

        public Naptar(DateTime datum)
        {
            Datum = datum;
            Foglalasok = new List<string>[11];
            for (int i = 0; i < 11; i++)
                Foglalasok[i] = new List<string>();
        }

        public bool VanSzabadHely(int oraIndex)
        {
            return Foglalasok[oraIndex].Count < 20;
        }

        public void Foglal(int oraIndex, string azonosito)
        {
            Foglalasok[oraIndex].Add(azonosito);
        }

        public void Kiiratas()
        {
            Console.Write($"\t{Datum:yyyy.MM.dd}\t");
            ConsoleColor eredeti = Console.ForegroundColor;

            for (int i = 0; i < 11; i++)
            {
                Console.ForegroundColor = Foglalasok[i].Count == 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write($"{i + 8,2}-{i + 9,-3}");
            }

            Console.ForegroundColor = eredeti;
            Console.WriteLine();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            #region fejlec
            /*MH-gokart
             Gokart időpontfoglaló - Egyéni kisprojekt
             2026.09.14
             */

            Console.WriteLine("MH-gokart");
            Console.WriteLine("Gokart időpontfoglaló - Egyéni kisprojekt");
            Console.WriteLine("2026.09.14");
            #endregion

            Random veletlen = new Random();

            #region 1. Helyszín
            Helyszin helyszin = new Helyszin(
                "MH Gokartpálya",
                "9700 Szombathely, Fő tér 12.",
                "+36-30-123-4567",
                "MH-gokart.hu");

            Console.WriteLine();
            Console.WriteLine("1.feladat:");
            helyszin.Kiiratas();
            #endregion

            #region 2. Versenyzők generálása
            Console.WriteLine();
            Console.WriteLine("2.feladat:");

            string[] vezeteknevek = File.ReadAllLines("vezeteknevek.txt");
            string[] keresztnevek = File.ReadAllLines("keresztnevek.txt");

            int versenyzoSzam = veletlen.Next(1, 151); // 1-150 közötti darabszám
            List<Versenyzo> versenyzok = new List<Versenyzo>();

            DateTime legkorabbi = new DateTime(1950, 1, 1);
            int napokTartomanya = (DateTime.Now - legkorabbi).Days;

            for (int i = 0; i < versenyzoSzam; i++)
            {
                string vezeteknev = vezeteknevek[veletlen.Next(vezeteknevek.Length)];
                string keresztnev = keresztnevek[veletlen.Next(keresztnevek.Length)];
                DateTime szuletesiIdo = legkorabbi.AddDays(veletlen.Next(napokTartomanya));

                versenyzok.Add(new Versenyzo(vezeteknev, keresztnev, szuletesiIdo));
            }

            Console.WriteLine($"\tGenerált versenyzők száma: {versenyzoSzam}");
            #endregion

            #region 3. Pályabérlés szabályai
            Console.WriteLine();
            Console.WriteLine("3.feladat:");
            Console.WriteLine("\tNyitvatartás: 8:00 - 19:00");
            Console.WriteLine("\tEgyszerre a pályán: minimum 8, maximum 20 versenyző");
            Console.WriteLine("\tEgy fő: minimum 1, maximum 2 órára foglalhat (2 óra esetén összefüggően)");
            #endregion

            #region 4. Időpontok megjelenítése
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
                n.Kiiratas();
            }
            #endregion

            #region 5. Manuális időpont-módosítás
            Console.WriteLine();
            Console.WriteLine("5.feladat:");

            bool kilepes = false;
            while (!kilepes)
            {
                Console.WriteLine();
                Console.WriteLine("\t1 - Versenyzők listázása");
                Console.WriteLine("\t2 - Időpont foglalása/módosítása azonosító alapján");
                Console.WriteLine("\t3 - Naptár újbóli megjelenítése");
                Console.WriteLine("\t0 - Kilépés");
                Console.Write("\tVálasztás: ");
                string valasztas = Console.ReadLine();

                if (valasztas == "1")
                {
                    foreach (Versenyzo v in versenyzok)
                    {
                        v.Kiiratas();
                    }
                }
                else if (valasztas == "2")
                {
                    Console.Write("\tVersenyző azonosítója: ");
                    string azonosito = Console.ReadLine();

                    Versenyzo talalt = null;
                    foreach (Versenyzo v in versenyzok)
                    {
                        if (v.Azonosito == azonosito)
                        {
                            talalt = v;
                            break;
                        }
                    }

                    if (talalt == null)
                    {
                        Console.WriteLine("\tNincs ilyen azonosítójú versenyző!");
                        continue;
                    }

                    Console.Write("\tDátum (éééé.hh.nn): ");
                    DateTime valasztottNap;
                    if (!DateTime.TryParse(Console.ReadLine(), out valasztottNap))
                    {
                        Console.WriteLine("\tHibás dátum!");
                        continue;
                    }

                    Naptar valasztottNaptar = null;
                    foreach (Naptar n in naptarak)
                    {
                        if (n.Datum.Date == valasztottNap.Date)
                        {
                            valasztottNaptar = n;
                            break;
                        }
                    }

                    if (valasztottNaptar == null)
                    {
                        Console.WriteLine("\tEz a dátum a hónapon kívül esik!");
                        continue;
                    }

                    Console.Write("\tKezdő időpont (8-18): ");
                    int oraKezdet;
                    if (!int.TryParse(Console.ReadLine(), out oraKezdet) || oraKezdet < 8 || oraKezdet > 18)
                    {
                        Console.WriteLine("\tHibás időpont!");
                        continue;
                    }

                    Console.Write("\tIdőtartam órában (1 vagy 2): ");
                    int oraTartam;
                    if (!int.TryParse(Console.ReadLine(), out oraTartam) || (oraTartam != 1 && oraTartam != 2))
                    {
                        Console.WriteLine("\tHibás időtartam!");
                        continue;
                    }

                    if (oraKezdet + oraTartam > 19)
                    {
                        Console.WriteLine("\tA foglalás túlnyúlik a nyitvatartáson!");
                        continue;
                    }

                    bool vanHely = true;
                    for (int i = 0; i < oraTartam; i++)
                    {
                        int index = oraKezdet - 8 + i;
                        if (!valasztottNaptar.VanSzabadHely(index))
                        {
                            vanHely = false;
                        }
                    }

                    if (!vanHely)
                    {
                        Console.WriteLine("\tA pálya betelt ebben az időszakban (max. 20 fő)!");
                        continue;
                    }

                    for (int i = 0; i < oraTartam; i++)
                    {
                        int index = oraKezdet - 8 + i;
                        valasztottNaptar.Foglal(index, talalt.Azonosito);
                    }

                    Console.WriteLine("\tSikeres foglalás!");
                }
                else if (valasztas == "3")
                {
                    foreach (Naptar n in naptarak)
                    {
                        n.Kiiratas();
                    }
                }
                else if (valasztas == "0")
                {
                    kilepes = true;
                }
                else
                {
                    Console.WriteLine("\tÉrvénytelen választás!");
                }
            }
            #endregion

            Console.WriteLine();
            Console.WriteLine("Program vége.");
            Console.ReadLine();
        }
    }
}
