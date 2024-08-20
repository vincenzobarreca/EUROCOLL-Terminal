namespace EUROCOLL
{
    internal class UserManager : IUserManager
    {
        private IDataBase _db;

        public UserManager(IDataBase db)
        {
            _db = db;
        }

        public void AddNewUser(User user)
        {
            _db.Users.Add(user);
            _db.Salva();
        }

        public bool VerificaRegistrazione(string nickname)
        {
            return !_db.Users.Any(us => us.NickName == nickname);
        }

        public bool VerificaAdmin(string nickname, string password)
        {
            return _db.Users.Exists(us => us.NickName.Equals(nickname) && us.Password.Equals(password) && us.Permessi.Contains(_db.ListaPermessi[1]));
        }

        public bool VerificaAccesso(string nickname, string password)
        {
            return _db.Users.Exists(us => (us.NickName == nickname) && (us.Password == password) && (us.Permessi.Contains(_db.ListaPermessi[0])));
        }

        public User GetUser(string nickname)
        {
            return _db.Users.Find(us => us.NickName == nickname);
        }
    }
}
