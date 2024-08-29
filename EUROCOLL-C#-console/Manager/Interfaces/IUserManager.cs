using EUROCOLL.Data;

namespace EUROCOLL.Manager.Interfaces
{
    public interface IUserManager
    {
        #region VERIFICHE
        bool VerificaRegistrazione(string nickname);
        bool VerificaAdmin(string nickname, string password);
        bool VerificaAccesso(string nickname, string password);
        #endregion

        #region GESTIONE_UTENTI
        User GetUser(string nickname);
        void AddNewUser(User user);
        #endregion
    }
}
