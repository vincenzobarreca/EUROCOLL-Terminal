namespace EUROCOLL
{
    internal interface IUserManager
    {
        void AddNewUser(User user);
        bool VerificaRegistrazione(string nickname);
        bool VerificaAdmin(string nickname, string password);
        bool VerificaAccesso(string nickname, string password);
        User GetUser(string nickname);
    }
}


