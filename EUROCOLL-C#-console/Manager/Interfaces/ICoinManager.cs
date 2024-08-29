using EUROCOLL.Data;

namespace EUROCOLL.Manager.Interfaces
{
    public interface ICoinManager
    {
        #region STATISTICHE_CATALOGO
        int NumeroTotaleDivisionali();
        double ValoreTotaleDivisionali();
        int NumeroTotaleCommemorative();
        double ValoreTotaleCommemorative();
        int NumeroMoneteTotale();
        double ValoreMoneteTotale();
        #endregion

        #region FILTRAGGIO_MONETE_CATALOGO
        List<Coin> MoneteDivisionaliPerTaglio(Taglio taglio);
        List<Coin> MonetaCommemorativeOrdinate();
        #endregion

        #region VERIFICA_RESTITUZIONE_MONETE_CATALOGO

        /*
         * metodi usati in fase di inserimento di nuove monete da parte degli USER per verificare 
         * che le monete che intendono inserire alla propria collezione esistano realmente
         */

        bool AnnataInseritaPresente(Taglio taglio, int anno);
        Coin CercaMonetaDivisionale(Taglio taglio, double valore, int anno);
        bool CommemorativaPerNomePresente(string nomeCommemorativa);
        Coin CercaMonetaCommemorativa(string nome);
        #endregion

        #region GESTIONE_INSERIMENTO_NUOVE_MONETE_CATALOGO
        //Metodi usati in fase di inserimento di nuove monete da parte dell'ADMIN
        
        /*
         * Restituisce l'annata successiva all'annata più recente di cui 
         * sono già presenti sul catalogo le divisionali dei vari tagli
         */
        int AnnataDivisionaleDaInserire();

        /*
         * Crea tutte le monete dei relativi 8 tagli 
         * della specifica annata passata a parametro
         */
        void AddDivisionaliAnnataSuccessiva(int anno);

        /*
         * Verifica che l'annata della commemorativa da inserire sia 
         * più recente o dello stesso anno della commemorativa più recente del catalogo
         */
        bool AnnoDiInserimentoCommemorativaCoerente(int anno);

        /*
         * verifica che la nuova commemorativa che si sta tentando di inserire nel catalogo 
         * abbia un nome diverso da tutte le altri già presenti. 
         * In caso affermativo, inserisce la nuova moneta al catalogo
         */
        bool AddNuovaCommemorativa(string nome, int anno);
        #endregion
    }
}
