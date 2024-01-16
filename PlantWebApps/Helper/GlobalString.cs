namespace PlantWebApps.Helper
{
    public class GlobalString
    {
        public static string dbname() { return "COMPONENT_DUMMY"; }
        public static String conString() { return "Server=BPNSQL02.thiess.aus;Database=" + dbname() + ";User ID=sadummy;Password=thiess000"; }
        public static String _username { get; set; }
    }
}
