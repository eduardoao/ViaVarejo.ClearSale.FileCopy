namespace ViaVarejo.ClearSale.FileCopy
{
    public class ConfigurationClearSale
    {
        public string uriClearSaleFtp;
        public string userClearSaleFtp;
        public string passClearSaleFtp;

        public ConfigurationClearSale(string uriClearSaleFtp, string portClearSaleFtp, string userClearSaleFtp, string passClearSaleFtp)
        {
            this.uriClearSaleFtp = uriClearSaleFtp;
            this.userClearSaleFtp = userClearSaleFtp;
            this.passClearSaleFtp = passClearSaleFtp;
        }
    }
}