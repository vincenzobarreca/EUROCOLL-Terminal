namespace EUROCOLL.Data
{
    public class User
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public List<Coin> Coins { get; set; }
        public List<Permesso> Permessi { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (!(obj is User)) return false;
            User other = (User)obj;
            return Id == other.Id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"NickName: {NickName}\nPassword: {Password}\nPermessi: {Permessi.ToString()}\nMonete collezionete:\n{Coins.ToString()}\n";
        }


        #region AGGIORNAMENTO_COLLEZIONE
        public bool AggiungiMonetaAllaCollezione(Coin coin)
        {
            if (Coins.Contains(coin))
            {
                return false;
            }
            else
            {
                Coins.Add(coin);
                return true;
            }
        }
        public bool RimuoviMonetaDallaCollezione(Coin coin)
        {
            return Coins.Remove(coin);
        }
        public bool MonetaPresente(Coin coin)
        {
            return Coins.Contains(coin);
        }
        #endregion


        #region STATISTICHE
        public int NumeroDivisionaliCollezionate()
        {
            return Coins.Count(coin => coin.Commemorativa == false);
        }
        public double ValoreDivisionaliCollezionate()
        {
            return Coins.Where(coin => coin.Commemorativa == false)
                .Sum(coin => coin.Valore);
        }
        public int NumeroCommemorativeCollezionate()
        {
            return Coins.Count(coin => coin.Commemorativa == true);
        }
        public double ValoreCommemorativeCollezionate()
        {
            return Coins.Where(coin => coin.Commemorativa == true)
                .Sum(coin => coin.Valore);
        }
        public int NumeroMoneteCollezionate()
        {
            return Coins.Count;
        }
        public double ValoreMoneteCollezionate()
        {
            return Coins.Sum(coin => coin.Valore);
        }
        #endregion
    }
}
