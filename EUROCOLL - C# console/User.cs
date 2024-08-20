namespace EUROCOLL
{
    internal class User
    {
        public string NickName { get; set; }
        public string Password { get; set; }
        public Guid Id { get; set; }
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

        //GESTIONE MONETE
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

        //STATISTICHE
        public int NumeroDivisionaliCollezionate()
        {
            return Coins.FindAll(coin => coin.Commemorativa == false).Count;
        }
        public double ValoreDivisionaliCollezionate()
        {
            return Coins.FindAll(coin => coin.Commemorativa == false).ConvertAll(coin => coin.Valore).Sum();
        }
        public int NumeroCommemorativeCollezionate()
        {
            return Coins.FindAll(coin => coin.Commemorativa == true).Count;
        }
        public double ValoreCommemorativeCollezionate()
        {
            return Coins.FindAll(coin => coin.Commemorativa == true).ConvertAll(coin => coin.Valore).Sum();
        }
        public int NumeroMoneteCollezionate()
        {
            return Coins.Count;
        }
        public double ValoreMoneteCollezionate()
        {
            return Coins.ConvertAll(coin => coin.Valore).Sum();
        }

    }
}
