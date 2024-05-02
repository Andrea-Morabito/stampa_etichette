using System.Globalization;

namespace AssetLabel
{
    /**
 * <summary>Classe Record, 
 * la classe record descrive la "struttura" delle righe lette dal CsvHelper e assegna dei valori alle variabili 
 * ( it1, anno, reparto, PCNumber, PFNumber e dataDiProduzione</summary>
 * */
    public class Record
    {
        public const string QRCode = "\"QRCODE\",1,1,3,2,4 : ";                           //
        public const string FTNumber = "FT \"Swiss 721 BT\",7:MAG 1,1:NI: DIR 1: PT ";    //Stringhe formattate correttamente per essere stampate dalla stampante
        public const string BFNumber = "BF \"Swiss 721 BT\",7,0,1,1,1,100,2 ON: BARSET "; //
        
        public string? CodiceAzienda {   get;    set;   }
        public string? annoDiProduzione{   get;    set;    }
        public string? reparto{   get;    set;    }
        public string? cespite{   get;    set;    }
        public string? numeroDiEtichette{   get;    set;    }
        public string? dataDiProduzione{   get;    set;    }

        public Record()
        {

        }

        public Record(Object[] var) 
        {
            CodiceAzienda = var[0].ToString() +"";
            annoDiProduzione = var[1].ToString() + "";
            reparto = var[2].ToString() + "";
            cespite = var[3].ToString() + "";
            var numEtichette = var[4].ToString().Split(',');
            numeroDiEtichette = numEtichette[0]+ "";
            dataDiProduzione = var[5].ToString() + "";
        }
        /**
         * <summary> Questo metodo ha il compito di stampare le informazioni di un Record</summary>
         * <returns></returns>
         * */
        public string printEtichetta()
        {
            NumberFormatInfo provider = new NumberFormatInfo(); // Creo un nuovo Oggetto di tipo NumberFormatInfo
            provider.CurrencyDecimalSeparator = "."; // Definisco il tipo di separatore del provider
            int IntPf = Convert.ToInt32(Convert.ToDecimal(numeroDiEtichette, provider));//Converto la stringa in un decimale che viene a sua volta convertito in un int
            
            return "CLL\r\n " + "PP 00110,0120 :AN 5: " + FTNumber + "\"SAURO\"" + "\r\n" + "PP 00110,0005 :AN 2: " +
            BFNumber + QRCode + "PB " + "\"" + cespite + "\"" + "\r\n" + "PF " + IntPf.ToString()+"\r\n";
        }
    }
}
