namespace AssetLabel
{
    /**
    * <summary>Classe Arguments, questa classe ha il compito di ottenere,  elaborare e gestire gli argometi inseriti da terminale/console</summary>
    * */
    public class Arguments
    {
        private Dictionary<string, string> arguments = new Dictionary<string, string>();//Creo un Oggetto Dictionary che abbiana una chiave di tipo string a un valore di tipo string

        /**
         * <summary>Il costruttore ottiene i parametri inseriti da terminale e li mette in un dizionario <string, string></summary>
         * <param name="args">Array di string, argomenti inseriti da terminale</param>
         * */
        public Arguments(string[] args)
        {
            for (int i = 0; i < args.Length; i = i + 2)
            {
                arguments.Add(args[i].ToUpper(), args[i + 1]);// assegno al dizionario la chiave ( trasformata in caratteri maiuscoli ) al valore

            }
        }

        /**
         * <summary> Questa classe ha il compito di restituire il valore abbinato ad una chiave</summary>
         * <param name="chiave"></param>
         * */
        public string getValue(string chiave)
        {
            if (arguments.ContainsKey(chiave))
            {
                return arguments[chiave];
            }
            return "";
        }
    }
}
