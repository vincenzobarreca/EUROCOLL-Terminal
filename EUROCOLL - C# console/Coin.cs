namespace EUROCOLL
{
    internal class Coin
    {
        public int Anno { get; set; }
        public bool Commemorativa { get; set; }
        public string? Nome { get; set; }
        public Guid Id { get; set; }
        public Taglio Taglio { get; set; }
        public double Valore { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (!(obj is Coin)) return false;
            Coin other = (Coin)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Anno;
        }

        public override string ToString()
        {
            return $"Id: {Id}\nTaglio: {Taglio}\nValore: {string.Format("{0:F2}", Valore)}\nAnno: {Anno}\nNome: {Nome}\n" +
                $"Commemorativa: {Commemorativa}";
        }
    }

    enum Taglio
    {
        UnCentesimo,
        DueCentesimi,
        CinqueCentesimi,
        DieciCentesimi,
        VentiCentesimi,
        CinquantaCentesimi,
        UnEuro,
        DueEuro
    }
}
