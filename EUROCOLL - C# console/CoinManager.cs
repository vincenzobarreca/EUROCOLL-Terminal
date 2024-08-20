namespace EUROCOLL
{
    internal class CoinManager : ICoinManager
    {
        private IDataBase _db;

        public CoinManager(IDataBase db)
        {
            _db = db;
        }

        //MODIFICHE CATALOGO MONETE
        public void AddDivisionaliAnnataSuccessiva(int anno)
        {
            string[] nomiDivisionali = { "Un centesimo", "Due centesimi", "Cinque centesimi", "Dieci centesimi", "Venti centesimi", "Cinquanta centesimi", "Un euro", "Due euro" };
            Taglio[] tagliDivisionali = (Taglio[])Enum.GetValues(typeof(Taglio));
            double[] valoriDivisionali = { 0.01, 0.02, 0.05, 0.10, 0.20, 0.50, 1.00, 2.00 };

            for (int i = 0; i < 8; i++)
            {
                _db.Coins.Add(
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
        public int AnnataDivisionaleDaInserire()
        {
            return _db.Coins.FindAll(coin => coin.Taglio == Taglio.UnCentesimo).ConvertAll(coin => coin.Anno).Max() + 1;
        }

        public bool AnnoDiInserimentoCommemorativaCoerente(int anno)
        {
            return anno >= _db.Coins.FindAll(coin => coin.Commemorativa == true).ConvertAll(coin => coin.Anno).Max();
        }
        public bool AddNuovaCommemorativa(string nome, int anno)
        {
            if (!_db.Coins.FindAll(coin => coin.Commemorativa == true).ConvertAll(coin => coin.Nome).Contains(nome))
            {
                _db.Coins.Add(
                    new Coin()
                    {
                        Nome = nome,
                        Anno = anno,
                        Id = Guid.NewGuid(),
                        Taglio = Taglio.DueEuro,
                        Commemorativa = true,
                        Valore = 2.00
                    }
                );
                return true;
            }
            else
            {
                return false;
            }
        }

        //STATISTICHE
        public int NumeroTotaleDivisionali()
        {
            return _db.Coins.FindAll(coin => coin.Commemorativa == false).Count();
        }
        public double ValoreTotaleDivisionali()
        {
            return _db.Coins.FindAll(coin => coin.Commemorativa == false).ConvertAll(coin => coin.Valore).Sum();
        }
        public int NumeroTotaleCommemorative()
        {
            return _db.Coins.FindAll(coin => coin.Commemorativa == true).Count();
        }
        public double ValoreTotaleCommemorative()
        {
            return _db.Coins.FindAll(coin => coin.Commemorativa == true).ConvertAll(coin => coin.Valore).Sum();
        }
        public int NumeroMoneteTotale()
        {
            return _db.Coins.Count;
        }
        public double ValoreMoneteTotale()
        {
            return _db.Coins.ConvertAll(coin => coin.Valore).Sum();
        }

        //
        public List<Coin> MoneteDivisionaliPerTaglio(Taglio taglio)
        {
            var monete = _db.Coins.FindAll(coin => coin.Taglio == taglio && coin.Commemorativa == false);
            monete.Sort((Coin c1, Coin c2) => c1.Anno.CompareTo(c2.Anno));
            return monete;
        }
        public List<Coin> MonetaCommemorativeOrdinate()
        {
            var commemorativeOrdinatePerAnno = _db.Coins.FindAll(coin => coin.Commemorativa == true);
            commemorativeOrdinatePerAnno.Sort((c1, c2) => c1.Anno.CompareTo(c2.Anno));
            return commemorativeOrdinatePerAnno;
        }
        public bool AnnataInseritaPresente(Taglio taglio, int anno)
        {
            List<int> annate = _db.Coins.FindAll(coin => (coin.Taglio == taglio) && (coin.Commemorativa == false)).ConvertAll(coin => coin.Anno);
            return annate.Contains(anno);
        }
        public bool CommemorativaPerNomePresente(string nomeCommemorativa)
        {
            return _db.Coins.FindAll(coin => coin.Commemorativa == true).ConvertAll(coin => coin.Nome).Contains(nomeCommemorativa);
        }
        public Coin CercaMonetaDivisionale(Taglio taglio, double valore, int anno)
        {
            return _db.Coins.Find(coin => (coin.Taglio == taglio) && (coin.Valore == valore) && (coin.Commemorativa == false) && (coin.Anno == anno));
        }
        public Coin CercaMonetaCommemorativa(string nome)
        {
            return _db.Coins.Find(coin => (coin.Commemorativa == true) && (coin.Nome.Equals(nome)));
        }
    }
}
