using System.Globalization;
using System.Data;
using Microsoft.Data.SqlClient;
using CsvHelper;
using CsvHelper.Configuration;

namespace AssetLabel
{

    /**
     * <summary>Classe Programm, la classe contiene il metodo Main</summary>
     * */
    public class Program
    {
        /**
         * <summary>Metodo Main</summary>
         * */
        public static void Main(string[] args)
        {
            Arguments argomenti = new Arguments(args);                  // Creo un Oggetto Arguments per raccogliere i parametri inseriti da terminale/console, gli passo un array di stringhe
            Label Stampante = new Label(argomenti.getValue("-IP"));     //Creo un oggetto Label per scrivere e stampare le etichette, gli passo una stringa che rappresenta l'ip della stampante
            List<Record> ListaRecord = new List<Record>();              // Creo una lista vuota di Record

            if (argomenti.getValue("-SRC").ToUpper() == "DB")
            {
                try
                {
                    SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder();

                    SqlBuilder.DataSource = "*************";                                      //
                    SqlBuilder.UserID = "**********";                                             //
                    SqlBuilder.Password = "**********";                                           //Credienziali di accesso e opzioni per lo String Builder
                    SqlBuilder.InitialCatalog = "*******";                                        //
                    SqlBuilder.TrustServerCertificate = true;                                     //

                    SqlConnection connection = new SqlConnection(SqlBuilder.ConnectionString);
                    connection.Open();

                    Command SqlCommand = new Command(argomenti.getValue("-ANNO"), argomenti.getValue("-REP"));  //
                    SqlCommand QueryCommand = new SqlCommand(SqlCommand.buildSqlCommand(), connection);         //Creo il comando per la query e lo eseguo
                    SqlDataReader SqlReader = QueryCommand.ExecuteReader();                                     //

                    DataTable SqlData = new DataTable();
                    SqlData.Load(SqlReader);

                    List<DataRow> ListaDatiSql = SqlData.Select().ToList(); //Trasformo SqlData in una lista di DataRow

                    foreach (var Elem in ListaDatiSql)
                    {
                        Record recordCespite = new Record(Elem.ItemArray);//Creo un record
                        ListaRecord.Add(recordCespite);                   //

                    }

                    Stampante.stampaEtichetta(argomenti, ListaRecord);
                }
                catch (SqlException _SqlExcept)
                {
                    Console.WriteLine("Impossibile svolgere la query\n\n" + _SqlExcept.Message);
                    System.Environment.Exit(-1);
                }
            }
            else
            {
                if(argomenti.getValue("-SRC").ToUpper() == "CSV")
                {
                    string filepath = Environment.CurrentDirectory +"\\..\\..\\..";
                    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)    //Creo il file di configurazione per leggere il file excel
                    {
                        HasHeaderRecord = false, //
                        AllowComments = false,   //Opzioni File excel 
                        Delimiter = ";",         //
                    };

                    StreamReader reader = null; //
                    CsvReader csvReader = null; // Creo Scrittori e Lettori
                    StreamWriter writer = null; //

                    try
                    {
                        reader = new StreamReader(argomenti.getValue("-INPUT"));    // Creo lo StreamReader, gli passo come parametro una stringa che rappresenta la cartella con il file di input
                        csvReader = new CsvReader(reader, config);                  // Creo l'oggetto CsvReader, gli passo come parametri il lettore di flusso e il file di configurazione config
                        if (argomenti.getValue("-OUTPUT") == "")
                        {
                            writer = new StreamWriter(filepath + "\\CSV\\OutputAssetLabel2020-2021-2022.txt");      // Creo l'oggetto StreamWriter, come parametro passo una stringa che rappresenta la cartella dove sara creato il file di output
                        }
                        else
                        {
                            writer = new StreamWriter(argomenti.getValue("-OUTPUT"));
                        }
                    }
                    catch (System.IO.IOException e) // Intercetto l'eccezione di IO ( System.IO.IOException )
                    {
                        
                        Console.WriteLine("Directory non esistente o errata");
                        System.Environment.Exit(-1);
                        
                    }

                    var insiemeEtichette = csvReader.GetRecords<Record>();  // Creo un variabile che contiene tutti i Record del file .csv 
                    

                    foreach (var etichetta in insiemeEtichette)// Ciclo foreach che scorre tutti gli elementi della variabile insiemeEtichette
                    {
                        ListaRecord.Add(etichetta);// aggiunge un elemento Record alla variabile ListaRecord
                    }

                    Stampante.scriviEtichetta(argomenti, writer, ListaRecord);
                }
                else
                {
                    Console.WriteLine("Scelta non valida, Scegliere un opzione valida (db o csv)");
                    System.Environment.Exit(-1);
                }
            }
        }

    }
}