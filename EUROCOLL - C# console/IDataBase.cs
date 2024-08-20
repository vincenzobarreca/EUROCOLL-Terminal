namespace EUROCOLL
{
    internal interface IDataBase
    {
        public List<User> Users { get; }
        public List<Coin> Coins { get; }
        public List<Permesso> ListaPermessi { get; }
        void Salva();
        void Init();

    }
}
