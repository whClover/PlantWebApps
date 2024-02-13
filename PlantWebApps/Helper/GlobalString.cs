namespace PlantWebApps.Helper
{
    public class GlobalString
    {
        public static string dbname() { return "COMPONENT_DUMMY"; }
        public static string dblive() { return "COMPONENT"; }
        public static String conString() { return "Server=BPNSQL02.thiess.aus;Database=" + dbname() + ";User ID=sadummy;Password=thiess000"; }
        public static String conStringLive() { return "Server=BPNSQL02.thiess.aus;Database=" + dblive() + ";User ID=sacomp;Password=Thiess12345"; }
        public static String _username { get; set; }
        public static String _usernamesession { get; set; }
    }
}
