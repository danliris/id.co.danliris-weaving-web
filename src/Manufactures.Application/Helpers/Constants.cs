namespace Manufactures.Application.Helpers
{
    public class Constants
    {
        // Daily Operation Status
        public static string PROCESS = "PROCESSING";
        public static string STOP = "STOP";
        public static string RESUME = "CONTINUE";
        public static string FINISH = "FINISH";

        // Beam Status
        public static string AVAILABLE = "AVAILABLE";
        public static string USED = "USED";
        public static string UNUSED = "UNUSED";

        // Type Of Yarn
        public static string WARP = "LUSI";
        public static string WEFT = "PAKAN";

        // Type of ring
        public static string SPUN = "Spun/Ring";
        public static string OPENEND = "Open End";
        public static string FILAMENT = "Filament";

        // Status on Manufacture Order
        public static string ALL = "ALL";
        public static string ONORDER = "OPEN-ORDER";
        public static string ONESTIMATED = "ESTIMATED-PRODUCTION";
    }
}
