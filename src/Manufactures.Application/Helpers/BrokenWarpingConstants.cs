using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Helpers
{
    public class BrokenWarpingConstants
    {
        public static double BROKEN_WARPING_DEFAULT_CONST = 10000000;
        public static double MTR_CONST = 0.9144;

        public enum FooterBroken
        {
            TOTAL_BROKEN,
            MAX_BROKEN,
            MIN_BROKEN,
            AVERAGE_BROKEN
        }
    }
}
