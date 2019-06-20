namespace Manufactures.Application.Helpers
{
    public class DailyOperationMachineStatus
    {
        //Machine Status (Loom, Sizing, Warping)
        public static string ONENTRY = "ENTRY";
        public static string ONSTART = "START";
        public static string ONSTOP = "STOP";
        public static string ONRESUME = "CONTINUE";
        public static string ONCOMPLETE = "COMPLETED";
        public static string ONCHANGESHIFT = "CHANGE-SHIFT";

        //Operation Status (Loom, Sizing, Warping)
        public static string ONPROCESS = "PROCESSING";
        public static string ONFINISH = "FINISH";
    }
}
