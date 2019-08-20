namespace Manufactures.Application.Helpers
{
    public class MachineStatus
    {
        //Daily Operation - Machine Status (Loom, Sizing, Warping)
        public static string ONENTRY = "ENTRY";
        public static string ONSTART = "START";
        public static string ONSTOP = "STOP";
        public static string ONRESUME = "CONTINUE";
        public static string ONFINISH = "FINISH";
        public static string ONCOMPLETE = "COMPLETED";
        public static string ONCHANGESHIFT = "CHANGE-SHIFT";

        //Daily Operation Machine Status (Reaching, Tying)
        public static string ONSTARTREACHING = "REACHING-START";
        public static string ONFINISHREACHING = "REACHING-FINISH";
        public static string ONSTARTTYING= "TYING-START";
        public static string ONFINISHTYING= "TYING-FINISH";
    }
}
