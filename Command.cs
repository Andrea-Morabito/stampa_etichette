namespace AssetLabel
{

    internal class Command
    { 

        private string _selection = "SELECT \r\n[CPY_0] \r\n,year([AASIPTDAT_0]) as Anno\r\n,[CCE_0]\r\n,[AASREF_0]\r\n,[AASQTY_0]\r\n,[AASIPTDAT_0]\r\n";
        private string _databaseLocation = "FROM x3.SAURO.FXDASSETS\n";
        private string _yearfilter = "WHERE year([AASIPTDAT_0]) in"; 
        private string _CMPSTAfilter = "AND([CMPSTA_0] in (1, 2))\r\n";
        private string _order = "ORDER BY  [CPY_0], year([AASIPTDAT_0]), [CCE_0],[AASIPTDAT_0], AASREF_0";
        private string _yearOption = "";
        private string _repOption = "";


        public Command(string _yearOption, string _repOption)
        {

            if(_yearOption != "") 
            {
                _yearfilter += " ('" + _yearOption + "')";
            } else
            {
                _yearfilter += " ('2020', '2021', '2022') "; 
            }
            if(_repOption != "")
            {
                _CMPSTAfilter = " AND CCE_0 = '"+_repOption+"'" + " AND ([CMPSTA_0] in (1,2))\n";
            } 
        }

        public string buildSqlCommand()
        {
            string Sql = _selection + _databaseLocation + _yearfilter + _CMPSTAfilter + _order;
            return Sql;
        }
    }
}
