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

        //Daily Operation - Machine Status (Reaching)
        public static string ONSTARTREACHINGIN = "REACHING-IN-START";
        public static string ONFINISHREACHINGIN = "REACHING-IN-FINISH";
        public static string CHANGEOPERATORREACHINGIN = "REACHING-IN-CHANGE-OPERATOR";
        public static string ONSTARTCOMB= "COMB-START";
        public static string CHANGEOPERATORCOMB = "COMB-CHANGE-OPERATOR";
        public static string ONFINISHCOMB= "COMB-FINISH";
    }
}
