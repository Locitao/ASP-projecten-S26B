namespace ReservationSystem
{
    /// <summary>
    /// Used to keep track of the person who is creating a reservation, and used to insert a user into AD.
    /// </summary>
    public class Person
    {
        public Person(string voornaam, string tussenvoegsel, string achternaam, string straat, int huisnr,
            string woonplaats, string banknr)
        {
            Voornaam = voornaam;
            Tussenvoegsel = tussenvoegsel;
            Achternaam = achternaam;
            Straat = straat;
            Huisnr = huisnr;
            Woonplaats = woonplaats;
            Banknr = banknr;
        }

        public string Voornaam { get; set; }
        public string Tussenvoegsel { get; set; }
        public string Achternaam { get; set; }
        public string Straat { get; set; }
        public int Huisnr { get; set; }
        public string Woonplaats { get; set; }
        public string Banknr { get; set; }
    }
}