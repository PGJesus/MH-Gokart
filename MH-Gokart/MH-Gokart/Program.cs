using System;
using System.Collections.Generic;
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
            Console.WriteLine($"Név: {Nev}");
            Console.WriteLine($"Cím: {Cim}");
            Console.WriteLine($"Telefon: {Telefon}");
            Console.WriteLine($"Domain: {Domain}");
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
        public Versenyzo(string vezeteknevev, string keresztnev, DateTime szulido, bool vane18, string azonosito, string email)
        {
            Vezeteknevev = vezeteknevev;
            Keresztnev = keresztnev;
            Szulido = szulido;
            Vane18 = DateTime.Now.Year - Szulido.Year >= 18;
            Azonosito = "GO-" + Vezeteknevev + Keresztnev + "-" + Szulido.ToString("yyyyMMdd");
            Email = (Vezeteknevev + "." + Keresztnev + "@gmail.com").ToLower();
        }
        public void Kiir()
        {
            Console.WriteLine($"Vezetéknév: {Vezeteknevev}");
            Console.WriteLine($"Keresztnév: {Keresztnev}");
            Console.WriteLine($"Születési idő: {Szulido}");
            Console.WriteLine($"18 éves: {Vane18}");
            Console.WriteLine($"Azonosító: {Azonosito}");
            Console.WriteLine($"Email: {Email}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MH-Gokart");
            Console.WriteLine("2026.09.07.");
            /*MH-Gokart
             2026.09.07.
             */

            Helyszin helyszin = new Helyszin("MH-Gokart Kft.", "1011 Budapest, Fő utca 1.", "06-1-234-5678", "MH-gokart.hu");

        }
    }
}
