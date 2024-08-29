using EUROCOLL.Data;
using EUROCOLL.Save.Interface;
using System.Text.Json;

namespace EUROCOLL.Save.Class
{
    public class DataBase : IDataBase
    {
        public List<Permesso> ListaPermessi { get; set; }
        public List<Coin> Coins { get; set; }
        public List<User> Users { get; set; }

        public void Init()
        {
            //inizializzazione dei permessi
            ListaPermessi = new List<Permesso>()
            {
                 new Permesso() { Id = Guid.NewGuid(), Nome = "User" },
                 new Permesso() { Id = Guid.NewGuid(), Nome = "Admin" }
            };

            //inizializzazione delle monete
            Coins = new List<Coin>();

            //inserimento monete divisionali
            string[] nomiDivisionali = { "Un centesimo", "Due centesimi", "Cinque centesimi", "Dieci centesimi", "Venti centesimi", "Cinquanta centesimi", "Un euro", "Due euro" };
            Taglio[] tagliDivisionali = (Taglio[])Enum.GetValues(typeof(Taglio));
            double[] valoriDivisionali = { 0.01, 0.02, 0.05, 0.10, 0.20, 0.50, 1.00, 2.00 };

            for (int anno = 2002; anno < 2025; anno++)
            {
                for (int i = 0; i < 8; i++)
                {
                    Coins.Add(
                        new Coin()
                        {
                            Nome = nomiDivisionali[i],
                            Anno = anno,
                            Id = Guid.NewGuid(),
                            Taglio = tagliDivisionali[i],
                            Commemorativa = false,
                            Valore = valoriDivisionali[i]
                        }
                    );
                }
            }

            //inserimento monete commemorative
            string[] nomiCommemorative = { "World Food Programme", "Primo anniversario Costituzione Europea", "XX Olimpiadi Invernali Torino 2006",
                                            "50° anniversario Trattati di Roma", "60° anniversario Dichiarazione Universale dei diritti dell uomo",
                                            "200° anniversario nascita Louis Braille", "10° anniversario Unione Economica e Monetaria",
                                            "200° anniviversario nascita Camillo Benso di Cavour", "150° anniversario Unità d Italia",
                                            "100° anniversario morte Giovanni Pascoli", "10 anni di banconote e monete in euro",
                                            "200° anniversario nascita di Giuseppe Verdi", "700° anniversario nascita Giovanni Boccaccio",
                                            "200° anniversario fondazione Arma dei Carabinieri", "450° anniversario nascita Galileo Galilei",
                                            "World Expo Milano 2015", "750° anniversario nascita Dante Alighieri", "30° anniversario bandiera europea",
                                            "550° anniversario morte Donatello", "2200° anniversario morte Tito Maccio Plauto",
                                            "400° anniv. completamento basilica di San Marco a Venezia", "2000° anniversario morte Tito Livio",
                                            "70° anniversario Costituzione Repubblica Italiana", "60° anniversario Ministero della Salute",
                                            "500° anniversario morte Leonardo da Vinci", "80° anniversario Fondazione Corpo Nazionale dei Vigili del Fuoco",
                                            "150º anniversario nascita Maria Montessori", "150° anniversario Roma Capitale d'Italia", "Professioni sanitarie",
                                            "170º anniversario Polizia di Stato", "30º anniversario morte di Giovanni Falcone e Paolo Borsellino",
                                            "35º anniversario Programma Erasmus", "100° anniversario Aeronautica militare", "150º anniversario morte Alessandro Manzoni",
                                            "250° anniversario Fondazione Guardia di Finanza", "Rita Levi Montalcini"};
            int[] anniCommemorative = { 2004, 2005, 2006, 2007, 2008, 2009, 2009, 2010, 2011, 2012, 2012, 2013 , 2013, 2014, 2014, 2015, 2015, 2015,
                                        2016, 2016, 2017, 2017, 2018, 2018, 2019, 2020, 2020, 2021, 2021, 2022, 2022, 2022, 2023, 2023, 2024, 2024 };

            for (int i = 0; i < nomiCommemorative.Length; i++)
            {
                Coins.Add(
                    new Coin()
                    {
                        Nome = nomiCommemorative[i],
                        Anno = anniCommemorative[i],
                        Id = Guid.NewGuid(),
                        Taglio = Taglio.DueEuro,
                        Commemorativa = true,
                        Valore = 2.00
                    }
                );
            }

            //inizializzazione della lista di User e inserimento dell'ADMIN
            Users = new List<User>()
            {
                 new User()
                    {
                        NickName = "vincenzo",
                        Password = "anteria2024",
                        Id = Guid.NewGuid(),
                        Coins = new List<Coin>(),
                        Permessi = new List<Permesso>()
                        {
                            ListaPermessi[0],
                            ListaPermessi[1]
                        }
                    }
            };
        }
        public void Salva()
        {
            //si serializza l'oggetto su cui è chiamato il metodo in una stionga in formato JSON
            string jsonString = JsonSerializer.Serialize(this);

            //si salva sul file database.json il contenuto appena ricavato
            File.WriteAllText("database.json", jsonString);
        }
    }
}
