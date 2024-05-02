using System.Net.Sockets;
using System.Text;

namespace AssetLabel
{
    /**
   * Classe Label, questa classe contiene un parametro di tipo Record, la classe si occupa di stampare l'output ( string ) di un Record con i metodi stampaEtichetta e stampaEtichettaReparto
   * */
    public class Label
    {
        private string ip = "";
        private int port = 9100;
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /**
         * <summary>Costruttore vuoto</summary>
         * */
        public Label(string ip)
        {
            this.ip = ip;
            try
            {
                socket.Connect(this.ip, port);
            }
            catch(Exception e)
            {
                Console.WriteLine("Stampante spenta o non raggiungibile\n\n" + e.Message);
            }
        }


        /**
         * <summary> Ritorna la lista di Record che dovrà poi essere stampata alla stampante o scritta nel file di testo</summary>
         * <param name="args"></param>
         * <param name="dataQuery"></param>
         * * */
        public List<Record> GetQuery(Arguments args, List<Record> dataQuery)
        {
            List<Record> query = new List<Record>();
            foreach(Record data in dataQuery)
            {
                if(args.getValue("-ANNO") != null && args.getValue("-ANNO") != data.annoDiProduzione)
                {
                    query.Add(data);
                }
            }

            List<Record> queryReparti = new List<Record>();
            foreach(var data in query)
            {
                queryReparti.Add(data);
            }
            query = null;

            foreach(Record data in queryReparti)
            {
                if(args.getValue("-REPARTO") != null && args.getValue("-REPARTO") != data.reparto)
                {
                    query.Add(data);
                }
            }

            return query;
        }


        /**
         *<summary>Stampa l'etichetta di un ogetto Record, il metodo controlla il cambio di reparto usando la variabile prec e ne stampa l'etichetta di reparto</summary>
         * <param name="args"></param>
         * <param name="target">Stream Writer, oggeto per scrivere strighe di testo in un file </param>
         * <param name="data"></param>
         * 
         * */
        public void stampaEtichetta(Arguments args, List<Record> dataQuery)
        {

            string ContenutoEtichetta;
            string prec = ""; // prec rappresenta il Record precedente

            List<Record> query = GetQuery(args, dataQuery);


            foreach(var r in query){
                if (r.reparto.Equals(prec))
                {
                     ContenutoEtichetta = r.printEtichetta() + "\n"; // Assegno a valoreEtichetta il valore di Record trasformato in stringa
                }
                else
                {
                     ContenutoEtichetta = stampaEtichettaReparto(r) + "\n" + r.printEtichetta();
                 }

                 byte[] bytes = Encoding.ASCII.GetBytes(ContenutoEtichetta);


                 if (ContenutoEtichetta != "")
                  {
                        socket.Send(bytes); // metodo della classe Socket che serve a mandare un array di byte ad un dispositivo ( nel nostro caso una stampante )
                  }

                    prec = r.reparto; //Assegno a prec il valore di Record
                }
        }

        public void scriviEtichetta(Arguments args, StreamWriter target, List<Record> dataQuery)
        {
            string ContenutoEtichetta;
            string prec = "";
            List<Record> query = GetQuery(args, dataQuery);
            foreach(var r in query)
            {
                bool isDaStampare = false;

                if (r.reparto.Equals(prec))
                {
                    ContenutoEtichetta = r.printEtichetta() + "\n"; // Assegno a valoreEtichetta il valore di Record trasformato in stringa
                }
                else
                {
                    ContenutoEtichetta = stampaEtichettaReparto(r) + "\n" + r.printEtichetta();
                }

                prec = r.reparto;

                if (ContenutoEtichetta != "" &&isDaStampare == true)
                {
                    target.Write(ContenutoEtichetta);
                    target.Flush();
                }

            }

            target.Close();
        }

        /**
         * <summary>Stampa l'etichetto nal caso di cambio reparto</summary>
         * <returns></returns>
         * */
        private string stampaEtichettaReparto(Record r)
        {
            return "CLL\r\n " + "PP 00110,0050 :AN 5: " + "FT " + "\"Swiss 721 BT\",7:MAG 1,1:NI: " +
              "DIR 1: " + "PT " + "\"" + r.reparto + "\"" + "\r\n" + "PF 001\r\n";
        }
    }
}
