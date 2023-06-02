using System;
using System.Collections.Generic;

namespace DomeinLaag {
    public class Reservering {
        public int ReserveringsNummer { get; private set; }
        public Klant Klant { get; private set; }
        public Auto Auto { get; private set; }
        public DateTime StartDatum { get; private set; }
        public TimeSpan Duur { get; private set; }
        public decimal Eenheidsprijs { get; private set; }
        public List<Auto> Wagens { get; private set; }
        public DateTime StartUur { get; set; }

        public Reservering(int reserveringsNummer, Klant klant, Auto auto, DateTime startDatum, TimeSpan duur, decimal eenheidsprijs) {
            if (klant == null)
                throw new ArgumentNullException("klant", "Klant mag niet null zijn.");

            if (auto == null)
                throw new ArgumentNullException("auto", "Auto mag niet null zijn.");

            if (startDatum < DateTime.Now)
                throw new ArgumentException("Startdatum kan niet in het verleden liggen.", "startDatum");

            if (duur.TotalHours <= 0 || duur.TotalHours > 11)
                throw new ArgumentException("Ongeldige duur voor reservering.", "duur");

            if (eenheidsprijs <= 0)
                throw new ArgumentException("Eenheidsprijs moet groter zijn dan 0.", "eenheidsprijs");

            ReserveringsNummer = reserveringsNummer;
            Klant = klant;
            Auto = auto;
            StartDatum = startDatum;
            Duur = duur;
            Eenheidsprijs = eenheidsprijs;
            Wagens = new List<Auto>();
        }

        public decimal BerekenTotaalPrijs() {
            decimal totaalPrijs = 0;

            // Eerste uur
            totaalPrijs += Eenheidsprijs;

            // Overige uren
            if (Duur.TotalHours > 1) {
                decimal uurPrijs = Eenheidsprijs * 0.6m; // 60% van de eenheidsprijs
                decimal nachtUurPrijs = Eenheidsprijs * 1.2m; // 120% van de eenheidsprijs

                TimeSpan overigeUren = Duur - TimeSpan.FromHours(1);

                for (int i = 0; i < overigeUren.TotalHours; i++) {
                    DateTime huidigUur = StartDatum.AddHours(i + 1);
                    if (IsNachtUur(huidigUur))
                        totaalPrijs += Math.Round(nachtUurPrijs, 0, MidpointRounding.AwayFromZero);
                    else
                        totaalPrijs += Math.Round(uurPrijs, 0, MidpointRounding.AwayFromZero);
                }
            }

            return totaalPrijs;
        }

        private bool IsNachtUur(DateTime uur) {
            return uur.Hour >= 22 || uur.Hour < 7;
        }
    }

    public class Klant {
        public int Klantnummer { get; private set; }
        public string Voornaam { get; private set; }
        public string Achternaam { get; private set; }
        public string Adres { get; private set; }
        public string BTWNummer { get; private set; }

        public Klant(int klantnummer, string voornaam, string achternaam, string adres, string btwNummer) {
            if (string.IsNullOrEmpty(voornaam))
                throw new ArgumentException("Voornaam mag niet leeg zijn.", "voornaam");

            if (string.IsNullOrEmpty(achternaam))
                throw new ArgumentException("Achternaam mag niet leeg zijn.", "achternaam");

            if (string.IsNullOrEmpty(adres))
                throw new ArgumentException("Adres mag niet leeg zijn.", "adres");

            Klantnummer = klantnummer;
            Voornaam = voornaam;
            Achternaam = achternaam;
            Adres = adres;
            BTWNummer = btwNummer;
        }
    }

    public class Auto {
        public string Naam { get; private set; }
        public decimal EersteUurPrijs { get; private set; }
        public decimal NightlifePrijs { get; private set; }
        public decimal WeddingPrijs { get; private set; }
        public int Bouwjaar { get; private set; }
        public int Id { get; private set; }

        public Auto(string naam, decimal eersteUurPrijs, decimal nightlifePrijs, decimal weddingPrijs, int bouwjaar) {
            if (string.IsNullOrEmpty(naam))
                throw new ArgumentException("Naam van de auto mag niet leeg zijn.", "naam");

            if (eersteUurPrijs <= 0)
                throw new ArgumentException("Eerste uur prijs moet groter zijn dan 0.", "eersteUurPrijs");

            if (nightlifePrijs <= 0)
                throw new ArgumentException("Nightlife prijs moet groter zijn dan 0.", "nightlifePrijs");

            if (weddingPrijs <= 0)
                throw new ArgumentException("Wedding prijs moet groter zijn dan 0.", "weddingPrijs");

            if (bouwjaar <= 0)
                throw new ArgumentException("Ongeldig bouwjaar van de auto.", "bouwjaar");

            Naam = naam;
            EersteUurPrijs = eersteUurPrijs;
            NightlifePrijs = nightlifePrijs;
            WeddingPrijs = weddingPrijs;
            Bouwjaar = bouwjaar;
        }
    }
}
