using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Helpers
{
    public class SizePickupSPUConstants
    {
        public const string PCCONSTRUCTION = "PC";
        public const string CVCCONSTRUCTION = "CVC";
        public static double PC_CVCSTANDARD = 11;
        public static double PC_CVCLOWERLIMIT = 8;
        public static double PC_CVCUPPERLIMIT = 14;

        public const string COTTONCONSTRUCTION = "COTTON";
        public static double COTTONSTANDARD = 10;
        public static double COTTONLOWERLIMIT = 7;
        public static double COTTONUPPERLIMIT = 13;

        public const string PECONSTRUCTION = "PE";
        public static double PESTANDARD = 14;
        public static double PELOWERLIMIT = 12;
        public static double PEUPPERLIMIT = 16;

        public const string RAYONCONSTRUCTION = "RAYON";
        public static double RAYONSTANDARD = 1;
        public static double RAYONLOWERLIMIT = -2;
        public static double RAYONUPPERLIMIT = 4;
    }
}
