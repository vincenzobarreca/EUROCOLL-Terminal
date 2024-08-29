using EUROCOLL.Data;
using EUROCOLL.Manager.Interfaces;
using EUROCOLL.Save.Interface;

namespace EUROCOLL.Manager.Classes
{
    public class UserManager : IUserManager
    {
        private IDataBase _db;

        public UserManager(IDataBase db)
        {
            _db = db;
        }


        #region VERIFICHE
        public bool VerificaRegistrazione(string nickname)
        {
            /*Verifica che nessun altro utende già loggato
             * abbia lo stesso nickname di chi tenta di registrarsi
             */

            return !_db.Users.Any(us => us.NickName == nickname);
        }

        public bool VerificaAdmin(string nickname, string password)
        {
            /*Verifica che chi tenta l'accesso come
             * admin abbia il permesso per farlo
             */

            return _db.Users.Exists(us => us.NickName.Equals(nickname) && us.Password.Equals(password) && us.Permessi.Contains(_db.ListaPermessi[1]));
        }

        public bool VerificaAccesso(string nickname, string password)
        {
            /*Verifica che chi tenta l'accesso come user
             * sia già loggato
             */

            return _db.Users.Exists(us => us.NickName == nickname && us.Password == password && us.Permessi.Contains(_db.ListaPermessi[0]));
        }
        #endregion

        #region GESTIONE_UTENTI
        public User GetUser(string nickname)
        {
            return _db.Users.Find(us => us.NickName == nickname);
        }

        public void AddNewUser(User user)
        {
            /*Viene aggiunto l'user alla lista di users esistenti e 
             * si salva l'informazione sul Database
             */

            _db.Users.Add(user);
            _db.Salva();
        }
        #endregion
    }
}
