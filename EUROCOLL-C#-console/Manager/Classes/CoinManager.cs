using EUROCOLL.Data;
using EUROCOLL.Manager.Interfaces;
using EUROCOLL.Save.Interface;

namespace EUROCOLL.Manager.Classes
{
    public class CoinManager : ICoinManager
    {
        private IDataBase _db;

        public CoinManager(IDataBase db)
        {
            _db = db;
        }

        #region STATISTICHE_CATALOGO
        public int NumeroTotaleDivisionali()
        {
            return _db.Coins.Count(coin => coin.Commemorativa == false);
        }
        public double ValoreTotaleDivisionali()
        {
            return _db.Coins.Where(coin => coin.Commemorativa == false)
                .Sum(coin => coin.Valore);
        }
        public int NumeroTotaleCommemorative()
        {
            return _db.Coins.Count(coin => coin.Commemorativa == true);
        }
        public double ValoreTotaleCommemorative()
        {
            return _db.Coins.Where(coin => coin.Commemorativa == true)
                .Sum(coin => coin.Valore);
        }
        public int NumeroMoneteTotale()
        {
            return _db.Coins.Count;
        }
        public double ValoreMoneteTotale()
        {
            return _db.Coins.Sum(coin => coin.Valore);
        }
        #endregion

        #region FILTRAGGIO_MONETE_CATALOGO
        public List<Coin> MoneteDivisionaliPerTaglio(Taglio taglio)
        {
            var monete = _db.Coins.Where(coin => coin.Taglio == taglio && coin.Commemorativa == false).ToList();
            monete.Sort((c1, c2) => c1.Anno.CompareTo(c2.Anno));
            return monete;
        }
        public List<Coin> MonetaCommemorativeOrdinate()
        {
            var commemorativeOrdinatePerAnno = _db.Coins.Where(coin => coin.Commemorativa == true).ToList();
            commemorativeOrdinatePerAnno.Sort((c1, c2) => c1.Anno.CompareTo(c2.Anno));
            return commemorativeOrdinatePerAnno;
        }
        #endregion

        #region VERIFICA_RESTITUZIONE_MONETE_CATALOGO
        public bool AnnataInseritaPresente(Taglio taglio, int anno)
        {
            return _db.Coins.Any(coin => coin.Taglio == taglio && coin.Commemorativa == false && coin.Anno == anno);
        }
        public Coin CercaMonetaDivisionale(Taglio taglio, double valore, int anno)
        {
            return _db.Coins.FirstOrDefault(coin => coin.Taglio == taglio && coin.Valore == valore && coin.Commemorativa == false && coin.Anno == anno);
        }
        public bool CommemorativaPerNomePresente(string nomeCommemorativa)
        {
            return _db.Coins.Any(coin => coin.Commemorativa == true && coin.Nome == nomeCommemorativa);
        }
        public Coin CercaMonetaCommemorativa(string nome)
        {
            return _db.Coins.FirstOrDefault(coin => coin.Commemorativa == true && coin.Nome == nome);
        }
        #endregion

        #region GESTIONE_INSERIMENTO_NUOVE_MONETE_CATALOGO
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
            return _db.Coins
                .Where(coin => coin.Taglio == Taglio.UnCentesimo)
                .Select(coin => coin.Anno)
                .Max() + 1;  
        }
        public bool AnnoDiInserimentoCommemorativaCoerente(int anno)
        {
            return anno >= _db.Coins
                                .Where(coin => coin.Commemorativa == true)
                                .Select(coin => coin.Anno)
                                .Max();                
        }
        public bool AddNuovaCommemorativa(string nome, int anno)
        {
            //Se tutte le monete hanno un nome diverso dal nome della moneta che si vuole inserire allora crea la moneta
            if (_db.Coins.All(coin => coin.Commemorativa == true && coin.Nome != nome))
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
        #endregion
    }
}
