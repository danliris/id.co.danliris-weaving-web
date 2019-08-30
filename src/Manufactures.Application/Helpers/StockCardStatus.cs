namespace Manufactures.Application.Helpers
{
    public class StockCardStatus
    {
        // Stock Type
        public static string WARPING_STOCK = "warping";
        public static string SIZING_STOCK = "sizing";
        public static string LOOM_STOCK = "loom";
        public static string REACHING_STOCK = "reaching";
        public static string TYING_STOCK = "tying";

        // Stock Status
        public static string MOVEIN_STOCK = "move-in";
        public static string MOVEOUT_STOCK = "move-out";
        public static string ADJUSTMENT = "stock-adjustment";
    }
}
