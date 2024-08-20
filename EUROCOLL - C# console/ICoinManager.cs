namespace EUROCOLL
{
    internal interface ICoinManager
    {
        //STATISTICHE
        int NumeroTotaleDivisionali();
        double ValoreTotaleDivisionali();
        int NumeroTotaleCommemorative();
        double ValoreTotaleCommemorative();
        int NumeroMoneteTotale();
        double ValoreMoneteTotale();
        
        
        //GESTIONE INSERIMENTO NUOVE MONETE ADMIN
        bool AnnoDiInserimentoCommemorativaCoerente(int anno);
        void AddDivisionaliAnnataSuccessiva(int anno);
        int AnnataDivisionaleDaInserire();
        bool AddNuovaCommemorativa(string nome, int anno);
        List<Coin> MoneteDivisionaliPerTaglio(Taglio taglio);
        List<Coin> MonetaCommemorativeOrdinate();
        bool AnnataInseritaPresente(Taglio taglio, int anno);
        bool CommemorativaPerNomePresente(string nomeCommemorativa);
        Coin CercaMonetaDivisionale(Taglio taglio, double valore, int anno);
        Coin CercaMonetaCommemorativa(string nome);
    }
}
