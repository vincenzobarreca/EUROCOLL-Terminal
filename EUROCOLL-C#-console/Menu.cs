using EUROCOLL.Data;
using EUROCOLL.Manager.Classes;
using EUROCOLL.Manager.Interfaces;
using EUROCOLL.Save.Class;
using EUROCOLL.Save.Interface;
using System.Text.Json;

namespace EUROCOLL
{
    internal class Menu
    {
        private static IDataBase? _dB;
        private static IUserManager? _userManager;
        private static ICoinManager? _coinManager;
        private static User? _user;

        #region START

        public static void Start()
        {
            /*
             * Se il file database.json non esiste allora stiamo runnando per la prima volta
             * l'applicazione, dunque si crea un nuovo oggetto di tipo Database., e si invocano 
             * i metodi Init per inizializzare tutto e Salva per salvare tutto sul file stesso.
             * 
             * Altrimenti, tutte le informazioni si trovano già nel file database.json, dunque 
             * si preleva il contenuto di tale file e si deserializza l'oggetto di tipo Database
             */
            if (!File.Exists("database.json"))
            {
                _dB = new DataBase();
                _dB.Init();
                _dB.Salva();
            }
            else
            {
                string jsonString = File.ReadAllText("database.json");
                _dB = JsonSerializer.Deserialize<DataBase>(jsonString);
            }

            _userManager = new UserManager(_dB);
            _coinManager = new CoinManager(_dB);

            SchermataIniziale();
        }

        private static void SchermataIniziale()
        {
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nBenvenuto su EUROCOLL\n");


            string? scelta;

            do
            {
                Console.WriteLine("Digita:\n1 - per ACCEDERE\n2 - per REGISTRARTI\n3 - per ACCEDERE COME ADMIN\n");
                scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        AccessoUser();
                        return;
                    case "2":
                        Registrazione();
                        return;
                    case "3":
                        AccessoAdmin();
                        return;
                    default:
                        Console.Write("\nScelta non valida. ");
                        break;
                }
            }
            while (true);
        }
        #endregion

        #region AREA_USER

        private static void AccessoUser()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nACCEDI");
            Console.WriteLine("\nInserisci il tuo NickName:\n");
            string? nickname = Console.ReadLine();
            Console.WriteLine("\nInserisci la tua password:\n");
            string? password = Console.ReadLine();

            //verifica se le credenziali immesse sono valide
            if (_userManager.VerificaAccesso(nickname, password))
            {
                //recupera l'oggetto User associato all'utente che effettua correttamente il Login 
                _user = _userManager.GetUser(nickname);
                AreaPersonale();
            }
            else

            {
                Console.Write("\nLe credenziali inserite non sono corrette. ");
                string? scelta;
                do
                {
                    Console.WriteLine("Digita:");
                    Console.WriteLine("1 - per RIPROVARE\n2 - per tornare alla SCHERMATA INIZIALE\n");
                    scelta = Console.ReadLine();
                    switch (scelta)
                    {
                        case "1":
                            AccessoUser();
                            return;
                        case "2":
                            SchermataIniziale();
                            return;
                        default:
                            Console.Write("\nScelta non VALIDA. ");
                            break;
                    }
                }
                while (true);

            }
        }

        private static void AreaPersonale()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine($"\nArea personale di {_user.NickName}");
            Console.WriteLine($"\nStatistiche divisionali:\n{_user.NumeroDivisionaliCollezionate()}/{_coinManager.NumeroTotaleDivisionali()}\n{string.Format("{0:F2}", _user.ValoreDivisionaliCollezionate())}/{string.Format("{0:F2}", _coinManager.ValoreTotaleDivisionali())} $\n");
            Console.WriteLine($"Statistiche commemorative:\n{_user.NumeroCommemorativeCollezionate()}/{_coinManager.NumeroTotaleCommemorative()}\n{string.Format("{0:F2}", _user.ValoreCommemorativeCollezionate())}/{string.Format("{0:F2}", _coinManager.ValoreTotaleCommemorative())} $\n");
            Console.WriteLine($"Statistiche totali:\n{_user.NumeroMoneteCollezionate()}/{_coinManager.NumeroMoneteTotale()}\n{string.Format("{0:F2}", _user.ValoreMoneteCollezionate())}/{string.Format("{0:F2}", _coinManager.ValoreMoneteTotale())} $");

            Console.WriteLine();
            string? scelta;
            do
            {
                Console.WriteLine("Digita:");
                Console.WriteLine("1 - per ispezionare le tue monete DIVISIONALI\n2 - per ispezionare le tue monete COMMEMORATIVE\n3 - per effettuare il LOGOUT\n");

                scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        IspezionaDivisionali();
                        return;
                    case "2":
                        IspezioneCommemorative();
                        return;
                    case "3":
                        //Nel caso di Logout, l'oggetto _user viene posto a null
                        _user = null;
                        //ritorna alla schermata iniziale
                        SchermataIniziale();
                        return;
                    default:
                        Console.Write("\nScelta non VALIDA. ");
                        break;
                }
            }
            while (true);
        }

        private static void IspezionaDivisionali()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------\n");
            string? scelta;

            do
            {
                Console.WriteLine("Per scegliere il taglio di moneta da visionare, digita:");
                Console.WriteLine("1 - per 0.01 $");
                Console.WriteLine("2 - per 0.02 $");
                Console.WriteLine("3 - per 0.05 $");
                Console.WriteLine("4 - per 0.10 $");
                Console.WriteLine("5 - per 0.20 $");
                Console.WriteLine("6 - per 0.50 $");
                Console.WriteLine("7 - per 1.00 $");
                Console.WriteLine("8 - per 2.00 $");
                Console.WriteLine("9 - per tornare INDIETRO\n");

                scelta = Console.ReadLine();
                switch (scelta)
                {
                    case "1":
                        VisualizzaTaglio(Taglio.UnCentesimo, 0.01);
                        return;

                    case "2":
                        VisualizzaTaglio(Taglio.DueCentesimi, 0.02);
                        return;

                    case "3":
                        VisualizzaTaglio(Taglio.CinqueCentesimi, 0.05);
                        return;

                    case "4":
                        VisualizzaTaglio(Taglio.DieciCentesimi, 0.10);
                        return;

                    case "5":
                        VisualizzaTaglio(Taglio.VentiCentesimi, 0.20);
                        return;

                    case "6":
                        VisualizzaTaglio(Taglio.CinquantaCentesimi, 0.50);
                        return;

                    case "7":
                        VisualizzaTaglio(Taglio.UnEuro, 1.00);
                        return;

                    case "8":
                        VisualizzaTaglio(Taglio.DueEuro, 2.00);
                        return;

                    case "9":
                        AreaPersonale();
                        return;

                    default:
                        Console.Write("\nScelta non VALIDA. ");
                        break;
                }
            }
            while (true);
        }

        private static void VisualizzaTaglio(Taglio taglio, double valore)
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine($"\n{string.Format("{0:F2}", valore)} $\n");

            foreach (Coin coin in _coinManager.MoneteDivisionaliPerTaglio(taglio))
            {
                Console.Write($"{coin.Anno} - ");
                if (_user.MonetaPresente(coin))
                {
                    Console.Write("X");
                }
                Console.Write("\n");
            }

            string? scelta;
            Console.WriteLine();
            do
            {
                Console.WriteLine("Digita:");
                Console.WriteLine("1 - per inserire un'annata mancante\n2 - per eliminare un'annata già collezionata\n3 - per tornare INDIETRO\n");
                scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        AggiungiAnnata(taglio, valore);
                        return;
                    case "2":
                        RimuoviAnnata(taglio, valore);
                        return;
                    case "3":
                        IspezionaDivisionali();
                        return;
                    default:
                        Console.Write("\nScelta non valita. ");
                        break;
                }
            }
            while (true);
        }

        private static void AggiungiAnnata(Taglio taglio, double valore)
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nDigita l'annata da inserire:\n");

            string? scelta1 = Console.ReadLine();

            int annataScelta;

            //verifica che la stringa inserita dall'user è un intero
            if (int.TryParse(scelta1, out annataScelta))
            {
                //verifica se esiste l'annata specificata del taglio divisionale
                if (_coinManager.AnnataInseritaPresente(taglio, annataScelta))
                {
                    //verifica se l'user non ha già la moneta che sta tentando di inserire
                    if (_user.AggiungiMonetaAllaCollezione(_coinManager.CercaMonetaDivisionale(taglio, valore, annataScelta)))
                    {
                        Console.WriteLine("\nMoneta inserita con successo!\n");
                        //Si salva la nuove informazione sul database
                        _dB.Salva();
                        VisualizzaTaglio(taglio, valore);
                    }
                    else
                    {
                        Console.Write("\nPossiedi già la moneta che stai provando ad inserire. ");
                        string? scelta4;

                        do
                        {
                            Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                            scelta4 = Console.ReadLine();

                            switch (scelta4)
                            {
                                case "1":
                                    AggiungiAnnata(taglio, valore);
                                    return;
                                case "2":
                                    VisualizzaTaglio(taglio, valore);
                                    return;
                                default:
                                    Console.Write("\nScelta non valida. ");
                                    break;
                            }
                        }
                        while (true);

                    }
                }
                else
                {
                    Console.Write("\nNon esiste una moneta con l'anno da te inserito. ");
                    string? scelta3;
                    do
                    {
                        Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta3 = Console.ReadLine();

                        switch (scelta3)
                        {
                            case "1":
                                AggiungiAnnata(taglio, valore);
                                return;
                            case "2":
                                VisualizzaTaglio(taglio, valore);
                                return;
                            default:
                                Console.Write("\nScelta non valida. ");
                                break;
                        }
                    }
                    while (true);
                }
            }
            else
            {
                Console.Write("\nNon hai inserito un annata.");
                string? scelta2;
                do
                {
                    Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                    scelta2 = Console.ReadLine();

                    switch (scelta2)
                    {
                        case "1":
                            AggiungiAnnata(taglio, valore);
                            return;
                        case "2":
                            VisualizzaTaglio(taglio, valore);
                            return;
                        default:
                            Console.Write("\nScelta non valida.");
                            break;
                    }
                }
                while (true);
            }
        }

        private static void RimuoviAnnata(Taglio taglio, double valore)

        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nDigita l'annata da eliminare:\n");

            string? scelta1 = Console.ReadLine();

            int annataScelta;

            //verifica che la stringa inserita dall'user è un intero
            if (int.TryParse(scelta1, out annataScelta))
            {
                //verifica se esiste l'annata specificata del taglio divisionale
                if (_coinManager.AnnataInseritaPresente(taglio, annataScelta))
                {
                    //verifica se l'user ha la moneta che sta tentando di cancellare dalla sua collezione
                    if (_user.RimuoviMonetaDallaCollezione(_coinManager.CercaMonetaDivisionale(taglio, valore, annataScelta)))
                    {
                        Console.WriteLine("\nMoneta eliminata con successo!\n");
                        //si salva la nuova informazione sul database
                        _dB.Salva();
                        VisualizzaTaglio(taglio, valore);
                    }
                    else
                    {
                        Console.Write("\nNon possiedi la moneta che stai tentando di eliminare. ");
                        string? scelta4;

                        do
                        {
                            Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                            scelta4 = Console.ReadLine();

                            switch (scelta4)
                            {
                                case "1":
                                    RimuoviAnnata(taglio, valore);
                                    return;
                                case "2":
                                    VisualizzaTaglio(taglio, valore);
                                    return;
                                default:
                                    Console.Write("\nScelta non valida. ");
                                    break;
                            }
                        }
                        while (true);

                    }
                }
                else
                {
                    Console.Write("\nNon esiste una moneta con l'anno da te inserito. ");
                    string? scelta3;
                    do
                    {
                        Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta3 = Console.ReadLine();

                        switch (scelta3)
                        {
                            case "1":
                                RimuoviAnnata(taglio, valore);
                                return;
                            case "2":
                                VisualizzaTaglio(taglio, valore);
                                return;
                            default:
                                Console.Write("\nScelta non valida. ");
                                break;
                        }
                    }
                    while (true);
                }
            }
            else
            {
                Console.Write("\nNon hai inserito un annata.");
                string? scelta2;
                do
                {
                    Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                    scelta2 = Console.ReadLine();

                    switch (scelta2)
                    {
                        case "1":
                            RimuoviAnnata(taglio, valore);
                            return;
                        case "2":
                            VisualizzaTaglio(taglio, valore);
                            return;
                        default:
                            Console.Write("\nScelta non valida.");
                            break;
                    }
                }
                while (true);
            }
        }

        private static void IspezioneCommemorative()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nLe tue monete commemorative:\n");

            foreach (Coin c in _coinManager.MonetaCommemorativeOrdinate())
            {
                if (_user.MonetaPresente(c))
                {
                    Console.Write("X -");
                }
                else
                {
                    Console.Write("  -");
                }
                Console.Write($" ANNO: {c.Anno} NOME MONETA: {c.Nome}\n");
            }
            string? scelta;
            Console.WriteLine();
            do
            {
                Console.WriteLine("Digita\n1 - per inserire una commemorativa mancante\n2 - per eliminare una commemorativa già collezionata\n3 - per tornare INDIETRO\n");
                scelta = Console.ReadLine();
                switch (scelta)
                {
                    case "1":
                        AggiungiCommemorativa();
                        return;
                    case "2":
                        RimuoviCommemorativa();
                        return;
                    case "3":
                        AreaPersonale();
                        return;
                    default:
                        Console.Write("\nScelta non valida. ");
                        break;
                }
            }
            while (true);
        }

        private static void AggiungiCommemorativa()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nDigita il nome della commemorativa da inserire:\n");

            string? nomeCommemorativa = Console.ReadLine();

            //verifica se esiste una moneta commemorativa del catalogo avente per nome quanto inserito dall'user
            if (_coinManager.CommemorativaPerNomePresente(nomeCommemorativa))
            {
                //verifica se l'user possiede già la moneta commemorativa che sta tentando di aggiungere alla sua collezione
                if (_user.AggiungiMonetaAllaCollezione(_coinManager.CercaMonetaCommemorativa(nomeCommemorativa)))
                {
                    Console.WriteLine("\nMoneta inserita con successo!\n");
                    //si salva la nuova informazione sul database
                    _dB.Salva();
                    IspezioneCommemorative();
                }
                else
                {
                    Console.Write("\nPossiedi già la moneta che stai provando ad inserire. ");
                    string? scelta4;
                    do
                    {
                        Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta4 = Console.ReadLine();

                        switch (scelta4)
                        {
                            case "1":
                                AggiungiCommemorativa();
                                return;
                            case "2":
                                IspezioneCommemorative();
                                return;
                            default:
                                Console.Write("\nScelta non valida. ");
                                break;
                        }

                    }
                    while (true);
                }
            }
            else
            {
                Console.Write("\nNessuna commemorativa presente ha il nome da te inserito. ");
                string? scelta1;
                do
                {
                    Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                    scelta1 = Console.ReadLine();

                    switch (scelta1)
                    {
                        case "1":
                            AggiungiCommemorativa();
                            return;
                        case "2":
                            IspezioneCommemorative();
                            return;
                        default:
                            Console.Write("\nScelta non valida. ");
                            break;
                    }
                }
                while (true);
            }
        }

        private static void RimuoviCommemorativa()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nDigita il nome della commemorativa da eliminare:\n");

            string? nomeCommemorativa = Console.ReadLine();

            //verifica se esiste una moneta commemorativa del catalogo avente per nome quanto inserito dall'user
            if (_coinManager.CommemorativaPerNomePresente(nomeCommemorativa))
            {
                //verifica se l'user possiede già la moneta commemorativa che sta tentando di eliminare dalla sua collezione
                if (_user.RimuoviMonetaDallaCollezione(_coinManager.CercaMonetaCommemorativa(nomeCommemorativa)))
                {
                    Console.WriteLine("\nMoneta eliminata con successo!\n");
                    //si salva la nuova informazione sul database
                    _dB.Salva();
                    IspezioneCommemorative();
                }
                else
                {
                    Console.Write("\nNon possiedi la moneta che stai tentando di eliminare. ");
                    string? scelta4;
                    do
                    {
                        Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta4 = Console.ReadLine();

                        switch (scelta4)
                        {
                            case "1":
                                RimuoviCommemorativa();
                                return;
                            case "2":
                                IspezioneCommemorative();
                                return;
                            default:
                                Console.Write("\nScelta non valida. ");
                                break;
                        }

                    }
                    while (true);
                }
            }
            else
            {
                Console.Write("\nNessuna commemorativa presente ha il nome da te inserito. ");
                string? scelta1;
                do
                {
                    Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                    scelta1 = Console.ReadLine();

                    switch (scelta1)
                    {
                        case "1":
                            RimuoviCommemorativa();
                            return;
                        case "2":
                            IspezioneCommemorative();
                            return;
                        default:
                            Console.Write("\nScelta non valida. ");
                            break;
                    }
                }
                while (true);
            }
        }
        #endregion

        #region AREA_REGISTRAZIONE

        private static void Registrazione()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nREGISTRATI");
            Console.WriteLine("\nScegli il tuo NickName:\n");
            string? nickname = Console.ReadLine();
            Console.WriteLine("\nScegli la tua password:\n");
            string? password = Console.ReadLine();

            //verifica che non esiste alcun altro utente già loggato con lo stesso nickname
            if (_userManager.VerificaRegistrazione(nickname))
            {
                _userManager.AddNewUser(new User()
                {
                    NickName = nickname,
                    Password = password,
                    Id = Guid.NewGuid(),
                    Coins = new List<Coin>(),
                    Permessi = new List<Permesso>() { _dB.ListaPermessi[0] }
                }
                );
                Console.WriteLine("\nRegistrazione avvenuta con successo, effettua ora l'accesso \ncon le tue credenziali appena scelte");

                //reindirizzamento alla schermata di accesso come user
                AccessoUser();
            }
            else
            {
                Console.WriteLine();
                string? scelta;
                do
                {
                    Console.WriteLine("Il nickname scelto è gia usato, digita:");
                    Console.WriteLine("1 - per RIPROVARE\n2 - per tornare alla SCHERMATA PRINCIPALE\n");
                    scelta = Console.ReadLine();

                    switch (scelta)
                    {
                        case "1":
                            Registrazione();
                            return;
                        case "2":
                            SchermataIniziale();
                            return;
                        default:
                            Console.Write("\n\nScelta non VALIDA. ");
                            break;
                    }
                }
                while (true);

            }
        }
        #endregion

        #region AREA_ADMIN

        private static void AccessoAdmin()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nAccedi come ADMIN");
            Console.WriteLine("\nInserisci il tuo NickName:\n");
            string? nickname = Console.ReadLine();
            Console.WriteLine("\nInserisci la tua password:\n");
            string? password = Console.ReadLine();

            //verifica se le credenziali inserite sono quelle di un admin
            if (_userManager.VerificaAdmin(nickname, password))
            {
                PannelloAdmin();
            }
            else
            {
                string? scelta;
                Console.Write("\nLe credenziali inserite non sono corrette. ");
                do
                {
                    Console.WriteLine("Digita:");
                    Console.WriteLine("1 - per RIPROVARE\n2 - per tornare alla SCHERMATA INIZIALE\n");
                    scelta = Console.ReadLine();

                    switch (scelta)
                    {
                        case "1":
                            AccessoAdmin();
                            return;
                        case "2":
                            SchermataIniziale();
                            return;
                        default:
                            Console.Write("\nScelta non valida. ");
                            break;
                    }
                }
                while (true);
            }
        }

        private static void PannelloAdmin()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nArea amministrativa\n");

            string? scelta;
            do
            {
                Console.WriteLine("Digita:\n1 - per creare le divisionali di una nuova annata\n2 - per creare una nuova commemorativa\n3 - per effettuare il LOGOUT\n");
                scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        CreaDivisionaliNuovaAnnata();
                        return;
                    case "2":
                        CreaNuovaCommemorativa();
                        return;
                    case "3":
                        SchermataIniziale();
                        return;
                    default:
                        Console.Write("\nScelta non valida. ");
                        break;
                }
            }
            while (true);

        }

        private static void CreaDivisionaliNuovaAnnata()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");

            //Si ricava l'annata delle nuove monete da inserire, ovvero la successiva annata alla più recente
            int annataDaInserire = _coinManager.AnnataDivisionaleDaInserire();
            Console.Write($"\nSicuro di voler inserire l'annata {annataDaInserire} per tutti i tagli di monete divisionali. ");

            string? scelta;
            do
            {
                Console.WriteLine("Digita:\n1 - per creare le nuove divisionali\n2 - per tornare indietro\n");
                scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        //Se inseriscono le nuove divisionali al catalogo e si salvano le nuove informazioni sul Database
                        _coinManager.AddDivisionaliAnnataSuccessiva(annataDaInserire);
                        Console.WriteLine("\nMonete divisionali inserite con successo");
                        _dB.Salva();
                        PannelloAdmin();
                        return;
                    case "2":
                        PannelloAdmin();
                        return;
                    default:
                        Console.Write("\nScelta non valida. ");
                        break;
                }
            }
            while (true);
        }

        private static void CreaNuovaCommemorativa()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nInserisci l'anno della nuova moneta commemorativa:\n");
            string? annoStringa = Console.ReadLine();
            int anno;

            //Si verifica se la stringa inserita dall'admin è un anno
            if (int.TryParse(annoStringa, out anno))
            {
                //Si verifica se l'annata inserita è uguale o successiva a quella dell'annata più recente del catalogo delle commemorative
                if (_coinManager.AnnoDiInserimentoCommemorativaCoerente(anno))
                {
                    Console.WriteLine("\nInserisci il nome della nuova moneta commemorativa:\n");
                    string? nome = Console.ReadLine();

                    //Si verifica se già esiste sul catalogo un'altra moneta commemorativa con lo stesso nome
                    if (_coinManager.AddNuovaCommemorativa(nome, anno))
                    {
                        Console.WriteLine("\nMoneta inserita correttamente!");
                        //Si salva la creazione della nuova commemorativa nel database
                        _dB.Salva();
                        PannelloAdmin();
                        return;
                    }
                    else
                    {
                        Console.Write("\nIl nome della moneta da inserire deve essere necessariamente diverso dalle monete già presenti.");
                        string? scelta2;
                        do
                        {
                            Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                            scelta2 = Console.ReadLine();

                            switch (scelta2)
                            {
                                case "1":
                                    CreaNuovaCommemorativa();
                                    return;
                                case "2":
                                    PannelloAdmin();
                                    return;
                                default:
                                    Console.Write("\nScelta non valida.");
                                    break;
                            }
                        }
                        while (true);
                    }
                }
                else
                {
                    Console.Write("\nDevi inserire un'annata successiva o uguale a quella della moneta commemorativa più recente.");
                    string? scelta2;
                    do
                    {
                        Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta2 = Console.ReadLine();

                        switch (scelta2)
                        {
                            case "1":
                                CreaNuovaCommemorativa();
                                return;
                            case "2":
                                PannelloAdmin();
                                return;
                            default:
                                Console.Write("\nScelta non valida.");
                                break;
                        }
                    }
                    while (true);
                }
            }
            else
            {
                Console.Write("\nNon hai inserito un annata.");
                string? scelta1;
                do
                {
                    Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                    scelta1 = Console.ReadLine();

                    switch (scelta1)
                    {
                        case "1":
                            CreaNuovaCommemorativa();
                            return;
                        case "2":
                            PannelloAdmin();
                            return;
                        default:
                            Console.Write("\nScelta non valida.");
                            break;
                    }
                }
                while (true);
            }
        }
        #endregion
    }
}
