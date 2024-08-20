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
            //Costruire i dati la prima volta, altrimenti ricavarli dal file JSON
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

        public static int SchermataIniziale()
        {
            string? scelta;
            int exit;
            do
            {
                exit = 0;
                Console.WriteLine("\n------------------------------------------------------------------------------------------------");
                Console.WriteLine("\nBenvenuto su EUROCOLL\n");
                Console.WriteLine("Digita:\n1 - per ACCEDERE\n2 - per REGISTRARTI\n3 - per ACCEDERE COME ADMIN\n4 - per chiudere l'applicazione\n");
                scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        exit = AccessoUser();
                        break;
                    case "2":
                        exit = Registrazione();
                        break;
                    case "3":
                        exit = AccessoAdmin();
                        break;
                    case "4":
                        return 1;
                    default:
                        Console.Write("\nScelta non valida. ");
                        break;
                }
            }
            while (exit != 1);
            return exit;
        }
        #endregion

        #region AREA_USER

        private static int AccessoUser()
        {
            string? scelta;
            do 
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------");
                Console.WriteLine("\nACCEDI");
                Console.WriteLine("\nInserisci il tuo NickName:\n");
                string? nickname = Console.ReadLine();
                Console.WriteLine("\nInserisci la tua password:\n");
                string? password = Console.ReadLine();

                if (_userManager.VerificaAccesso(nickname, password))
                {
                    _user = _userManager.GetUser(nickname);
                    return AreaPersonale();
                }
                else
                {
                    Console.Write("\nLe credenziali inserite non sono corrette. ");
                    do
                    {
                        Console.WriteLine("Digita:");
                        Console.WriteLine("1 - per RIPROVARE\n2 - per tornare alla SCHERMATA INIZIALE\n");
                        scelta = Console.ReadLine();
                        switch (scelta)
                        {
                            case "1":
                                break;
                            case "2":
                                //torna a SchermataIniziale
                                return 0;
                            default:
                                Console.Write("\nScelta non VALIDA. ");
                                break;
                        }
                    }
                    while (scelta != "1");
                }   
            } 
            while (scelta == "1");

            return -1;
        }

        private static int AreaPersonale()
        {
            int exit;

            do
            {
                exit = 0;
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
                            exit = IspezionaDivisionali();
                            break;
                        case "2":
                            exit = IspezionaCommemorative();
                            break;
                        case "3":
                            //torna a SchermataIniziale
                            _user = null;
                            return 0;
                        default:
                            Console.Write("\nScelta non VALIDA. ");
                            break;
                    }
                }
                while (scelta != "1" && scelta != "2");
            }
            while (exit == 1);

            return -1;
        }

        private static int IspezionaDivisionali()
        {
            int exit;

            do
            {
                exit = 0;
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
                            exit = VisualizzaTaglio(Taglio.UnCentesimo, 0.01);
                            break;

                        case "2":
                            exit = VisualizzaTaglio(Taglio.DueCentesimi, 0.02);
                            break;

                        case "3":
                            exit = VisualizzaTaglio(Taglio.CinqueCentesimi, 0.05);
                            break;

                        case "4":
                            exit = VisualizzaTaglio(Taglio.DieciCentesimi, 0.10);
                            break;

                        case "5":
                            exit = VisualizzaTaglio(Taglio.VentiCentesimi, 0.20);
                            break;

                        case "6":
                            exit = VisualizzaTaglio(Taglio.CinquantaCentesimi, 0.50);
                            break;

                        case "7":
                            exit = VisualizzaTaglio(Taglio.UnEuro, 1.00);
                            break;

                        case "8":
                            exit = VisualizzaTaglio(Taglio.DueEuro, 2.00);
                            break;

                        case "9":
                            //torna a IspezionaDivisionali
                            return 1;

                        default:
                            Console.Write("\nScelta non VALIDA. ");
                            break;
                    }
                }
                while (scelta != "1" && scelta != "2" && scelta != "3" && scelta != "4" && scelta != "5" && scelta != "6" && scelta != "7" && scelta != "8");
            }
            while (exit == 1);
            return -1;
            
        }

        private static int VisualizzaTaglio(Taglio taglio, double valore)
        {
            int exit;

            do
            {
                exit = 0;
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
                            exit = AggiungiAnnata(taglio, valore);


                            break;
                        case "2":
                            exit = RimuoviAnnata(taglio, valore);
                            break;
                        case "3":
                            //torna a IspezionaDivisionali
                            return 1;
                        default:
                            Console.Write("\nScelta non valita. ");
                            break;
                    }
                }
                while (scelta != "1" && scelta != "2");
            }
            while (exit == 1);
            return -1;
            
        }

        private static int AggiungiAnnata(Taglio taglio, double valore)
        {
            string? scelta;

            do
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------");
                Console.WriteLine("\nDigita l'annata da inserire:\n");

                string? annoStringa = Console.ReadLine();

                int annataScelta;

                if (int.TryParse(annoStringa, out annataScelta))
                {
                    if (_coinManager.AnnataInseritaPresente(taglio, annataScelta))
                    {
                        if (_user.AggiungiMonetaAllaCollezione(_coinManager.CercaMonetaDivisionale(taglio, valore, annataScelta)))
                        {
                            Console.WriteLine("\nMoneta inserita con successo!\n");
                            _dB.Salva();
                            //torna a VisualizzaTaglio
                            return 1;
                        }
                        else
                        {
                            Console.Write("\nPossiedi già la moneta che stai provando ad inserire. ");

                            do
                            {
                                Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                                scelta = Console.ReadLine();

                                switch (scelta)
                                {
                                    case "1":
                                        break;
                                    case "2":
                                        //torna a VisualizzaTaglio
                                        return 1;
                                    default:
                                        Console.Write("\nScelta non valida. ");
                                        break;
                                }
                            }
                            while (scelta != "1");

                        }
                    }
                    else
                    {
                        Console.Write("\nNon esiste una moneta con l'anno da te inserito. ");
                        do
                        {
                            Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                            scelta = Console.ReadLine();

                            switch (scelta)
                            {
                                case "1":
                                    break;
                                case "2":
                                    //torna a VisualizzaTaglio
                                    return 1;
                                default:
                                    Console.Write("\nScelta non valida. ");
                                    break;
                            }
                        }
                        while (scelta != "1");
                    }
                }
                else
                {
                    Console.Write("\nNon hai inserito un annata.");
                    do
                    {
                        Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta = Console.ReadLine();

                        switch (scelta)
                        {
                            case "1":
                                break;
                            case "2":
                                //torna a VisualizzaTaglio
                                return 1;
                            default:
                                Console.Write("\nScelta non valida.");
                                break;
                        }
                    }
                    while (scelta != "1");
                }
            }
            while (scelta == "1");

            return -1;
        }

        private static int RimuoviAnnata(Taglio taglio, double valore)
        {
            string? scelta;

            do
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------");
                Console.WriteLine("\nDigita l'annata da eliminare:\n");

                string? annoStringa = Console.ReadLine();

                int annataScelta;

                if (int.TryParse(annoStringa, out annataScelta))
                {
                    if (_coinManager.AnnataInseritaPresente(taglio, annataScelta))
                    {
                        if (_user.RimuoviMonetaDallaCollezione(_coinManager.CercaMonetaDivisionale(taglio, valore, annataScelta)))
                        {
                            Console.WriteLine("\nMoneta eliminata con successo!\n");
                            _dB.Salva();
                            //torna a VisualizzaTaglio
                            return 1;
                        }
                        else
                        {
                            Console.Write("\nNon possiedi la moneta che stai tentando di eliminare. ");

                            do
                            {
                                Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                                scelta = Console.ReadLine();

                                switch (scelta)
                                {
                                    case "1":
                                        break;
                                    case "2":
                                        //torna a VisualizzaTaglio
                                        return 1;
                                    default:
                                        Console.Write("\nScelta non valida. ");
                                        break;
                                }
                            }
                            while (scelta != "1");
                        }
                    }
                    else
                    {
                        Console.Write("\nNon esiste una moneta con l'anno da te inserito. ");
                        do
                        {
                            Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                            scelta = Console.ReadLine();

                            switch (scelta)
                            {
                                case "1":
                                    break;
                                case "2":
                                    //torna a VisualizzaTaglio
                                    return 1;
                                default:
                                    Console.Write("\nScelta non valida. ");
                                    break;
                            }
                        }
                        while (scelta != "1");
                    }
                }
                else
                {
                    Console.Write("\nNon hai inserito un annata.");
                    do
                    {
                        Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta = Console.ReadLine();

                        switch (scelta)
                        {
                            case "1":
                                break;
                            case "2":
                                //torna a VisualizzaTaglio
                                return 1;
                            default:
                                Console.Write("\nScelta non valida.");
                                break;
                        }
                    }
                    while (scelta != "1");
                }
            }
            while (scelta == "1");

            return -1;
        }

        private static int IspezionaCommemorative()
        {
            int exit;

            do
            {
                exit = 0;
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
                            exit = AggiungiCommemorativa();
                            break;
                        case "2":
                            exit = RimuoviCommemorativa();
                            break;
                        case "3":
                            //torna a IspezionaDivisionali
                            return 1;
                        default:
                            Console.Write("\nScelta non valida. ");
                            break;
                    }
                }
                while (scelta != "1" && scelta != "2");
            }
            while (exit == 1);

            return -1;
            
        }

        private static int AggiungiCommemorativa()
        {
            string? scelta;
            do
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------");
                Console.WriteLine("\nDigita il nome della commemorativa da inserire:\n");

                string? nomeCommemorativa = Console.ReadLine();

                if (_coinManager.CommemorativaPerNomePresente(nomeCommemorativa))
                {
                    if (_user.AggiungiMonetaAllaCollezione(_coinManager.CercaMonetaCommemorativa(nomeCommemorativa)))
                    {
                        Console.WriteLine("\nMoneta inserita con successo!\n");
                        _dB.Salva();
                        //torna a IspezionaCommemorative
                        return 1;
                    }
                    else
                    {
                        Console.Write("\nPossiedi già la moneta che stai provando ad inserire. ");

                        do
                        {
                            Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                            scelta = Console.ReadLine();

                            switch (scelta)
                            {
                                case "1":
                                    break;
                                case "2":
                                    //torna a IspezionaCommemorative
                                    return 1;
                                default:
                                    Console.Write("\nScelta non valida. ");
                                    break;
                            }
                        }
                        while (scelta != "1");
                    }
                }
                else
                {
                    Console.Write("\nNessuna commemorativa presente ha il nome da te inserito. ");
                    do
                    {
                        Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta = Console.ReadLine();

                        switch (scelta)
                        {
                            case "1":
                                break;
                            case "2":
                                //torna a IspezionaCommemorative
                                return 1;
                            default:
                                Console.Write("\nScelta non valida. ");
                                break;
                        }
                    }
                    while (scelta != "1");
                }
            }
            while (scelta == "1");

            return -1;
        }

        private static int RimuoviCommemorativa()
        {
            string? scelta;

            do
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------");
                Console.WriteLine("\nDigita il nome della commemorativa da eliminare:\n");

                string? nomeCommemorativa = Console.ReadLine();

                if (_coinManager.CommemorativaPerNomePresente(nomeCommemorativa))
                {
                    if (_user.RimuoviMonetaDallaCollezione(_coinManager.CercaMonetaCommemorativa(nomeCommemorativa)))
                    {
                        Console.WriteLine("\nMoneta eliminata con successo!\n");
                        _dB.Salva();
                        //torna a IspezionaCommemorative
                        return 1;
                    }
                    else
                    {
                        Console.Write("\nNon possiedi la moneta che stai tentando di eliminare. ");

                        do
                        {
                            Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                            scelta = Console.ReadLine();

                            switch (scelta)
                            {
                                case "1":
                                    break;
                                case "2":
                                    //torna a IspezionaCommemorative
                                    return 1;
                                default:
                                    Console.Write("\nScelta non valida. ");
                                    break;
                            }
                        }
                        while (scelta != "1");
                    }
                }
                else
                {
                    Console.Write("\nNessuna commemorativa presente ha il nome da te inserito. ");
                    do
                    {
                        Console.WriteLine("Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta = Console.ReadLine();

                        switch (scelta)
                        {
                            case "1":
                                break;
                            case "2":
                                //torna a IspezionaCommemorative
                                return 1;
                            default:
                                Console.Write("\nScelta non valida. ");
                                break;
                        }
                    }
                    while (scelta != "1");
                }
            }
            while (scelta == "1");

            return -1;
            
        }
        #endregion

        #region AREA_REGISTRAZIONE

        private static int Registrazione()
        {
            string? scelta;
            do
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------");
                Console.WriteLine("\nREGISTRATI");
                Console.WriteLine("\nScegli il tuo NickName:\n");
                string? nickname = Console.ReadLine();
                Console.WriteLine("\nScegli la tua password:\n");
                string? password = Console.ReadLine();

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
                    return AccessoUser();
                }
                else
                {
                    Console.WriteLine();
                    do
                    {
                        Console.WriteLine("Il nickname scelto è gia usato, digita:");
                        Console.WriteLine("1 - per RIPROVARE\n2 - per tornare alla SCHERMATA PRINCIPALE\n");
                        scelta = Console.ReadLine();

                        switch (scelta)
                        {
                            case "1":
                                break;
                            case "2":
                                //torna a SchermataIniziale
                                return 0;
                            default:
                                Console.Write("\n\nScelta non VALIDA. ");
                                break;
                        }
                    }
                    while (scelta != "1");
                }
            }
            while (scelta == "1");
            return -1;
            
        }
        #endregion

        #region AREA_ADMIN

        private static int AccessoAdmin()
        {
            string? scelta;
            do
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------");
                Console.WriteLine("\nAccedi come ADMIN");
                Console.WriteLine("\nInserisci il tuo NickName:\n");
                string? nickname = Console.ReadLine();
                Console.WriteLine("\nInserisci la tua password:\n");
                string? password = Console.ReadLine();


                if (_userManager.VerificaAdmin(nickname, password))
                {
                    return PannelloAdmin();
                }
                else
                {
                    Console.Write("\nLe credenziali inserite non sono corrette. ");
                    do
                    {
                        Console.WriteLine("Digita:");
                        Console.WriteLine("1 - per RIPROVARE\n2 - per tornare alla SCHERMATA INIZIALE\n");
                        scelta = Console.ReadLine();

                        switch (scelta)
                        {
                            case "1":
                                break;
                            case "2":
                                //torna a SchermataIniziale
                                return 0;
                            default:
                                Console.Write("\nScelta non valida. ");
                                break;
                        }
                    }
                    while (scelta != "1");
                }
            }
            while (scelta == "1");
            return -1;
        }

        private static int PannelloAdmin()
        {
            int exit;
            do
            {
                exit = 0;
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
                            exit = CreaDivisionaliNuovaAnnata();
                            break;
                        case "2":
                            exit = CreaNuovaCommemorativa();
                            break;
                        case "3":
                            //torna a SchermataIniziale
                            return 0;
                        default:
                            Console.Write("\nScelta non valida. ");
                            break;
                    }
                }
                while (exit != 1);
            }
            while (exit == 1);

            return -1;
        }

        private static int CreaDivisionaliNuovaAnnata()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------------");
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
                        _coinManager.AddDivisionaliAnnataSuccessiva(annataDaInserire);
                        Console.WriteLine("\nMonete divisionali inserite con successo");
                        _dB.Salva();
                        //torna a PannelloAdmin
                        return 1;
                    case "2":
                        //torna a PannelloAdmin
                        return 1;
                    default:
                        Console.Write("\nScelta non valida. ");
                        break;
                }
            }
            while (true);
        }

        private static int CreaNuovaCommemorativa()
        {
            string? scelta;

            do
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------");
                Console.WriteLine("\nInserisci l'anno della nuova moneta commemorativa:\n");
                string? annoStringa = Console.ReadLine();
                int anno;
                if (int.TryParse(annoStringa, out anno))
                {
                    if (_coinManager.AnnoDiInserimentoCommemorativaCoerente(anno))
                    {
                        Console.WriteLine("\nInserisci il nome della nuova moneta commemorativa:\n");
                        string? nome = Console.ReadLine();

                        if (_coinManager.AddNuovaCommemorativa(nome, anno))
                        {
                            Console.WriteLine("\nMoneta inserita correttamente!");
                            _dB.Salva();
                            //torna a PannelloAdmin
                            return 1;
                        }
                        else
                        {
                            Console.Write("\nIl nome della moneta da inserire deve essere necessariamente diverso dalle monete già presenti.");
                            do
                            {
                                Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                                scelta = Console.ReadLine();

                                switch (scelta)
                                {
                                    case "1":
                                        break;
                                    case "2":
                                        //torna a PannelloAdmin
                                        return 1;
                                    default:
                                        Console.Write("\nScelta non valida.");
                                        break;
                                }
                            }
                            while (scelta != "1");
                        }
                    }
                    else
                    {
                        Console.Write("\nDevi inserire un'annata successiva o uguale a quella della moneta commemorativa più recente.");
                        do
                        {
                            Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                            scelta = Console.ReadLine();

                            switch (scelta)
                            {
                                case "1":
                                    break;
                                case "2":
                                    //torna a PannelloAdmin
                                    return 1;
                                default:
                                    Console.Write("\nScelta non valida.");
                                    break;
                            }
                        }
                        while (scelta != "1");
                    }
                }
                else
                {
                    Console.Write("\nNon hai inserito un annata.");
                    string? scelta1;
                    do
                    {
                        Console.WriteLine(" Digita:\n1 - per RIPROVARE\n2 - per tornare INDIETRO\n");
                        scelta = Console.ReadLine();

                        switch (scelta)
                        {
                            case "1":
                                break;
                            case "2":
                                //torna a PannelloAdmin
                                return 1;
                            default:
                                Console.Write("\nScelta non valida.");
                                break;
                        }
                    }
                    while (scelta != "1");
                }
            }
            while (scelta == "1");
            
            return -1;
        }
        #endregion
    }
}
