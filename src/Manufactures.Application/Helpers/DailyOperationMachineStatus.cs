namespace Manufactures.Application.Helpers
{
    public class DailyOperationMachineStatus
    {
        //Machine Loom Status
        public static string ONENTRY = "ENTRY";
        public static string ONSTART = "START";
        public static string ONSTOP = "STOP";
        public static string ONRESUME = "CONTINUE";
        public static string ONCOMPLETE = "COMPLETED";
        public static string ONCHANGESHIFT = "CHANGE-SHIFT";

        //Operation Status
        public static string ONPROCESS = "PROCESSING";
        public static string ONFINISH = "FINISH";
    }
}
