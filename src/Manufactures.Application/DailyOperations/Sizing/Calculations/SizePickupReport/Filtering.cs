using Manufactures.Application.Helpers;

namespace Manufactures.Application.DailyOperations.Sizing.Calculations.SizePickupReport
{
    public class Filtering
    {
        public string ComparingPCCVC(double spu)
        {
            string result = "";
            if (spu < SizePickupSPUConstants.PC_CVCLOWERLIMIT)
            {
                result = "Lower Limit";
            }
            if (spu >= SizePickupSPUConstants.PC_CVCLOWERLIMIT && spu <= SizePickupSPUConstants.PC_CVCUPPERLIMIT)
            {
                result = "Standard";
            }
            if (spu > SizePickupSPUConstants.PC_CVCUPPERLIMIT)
            {
                result = "Upper Limit";
            }
            return result;
        }

        public string ComparingCotton(double spu)
        {
            string result = "";
            if (spu < SizePickupSPUConstants.COTTONLOWERLIMIT)
            {
                result = "Lower Limit";
            }
            if (spu >= SizePickupSPUConstants.COTTONLOWERLIMIT && spu <= SizePickupSPUConstants.COTTONUPPERLIMIT)
            {
                result = "Standard";
            }
            if (spu > SizePickupSPUConstants.COTTONUPPERLIMIT)
            {
                result = "Upper Limit";
            }
            return result;
        }

        public string ComparingPE(double spu)
        {
            string result = "";
            if (spu < SizePickupSPUConstants.PELOWERLIMIT)
            {
                result = "Lower Limit";
            }
            if (spu >= SizePickupSPUConstants.PELOWERLIMIT && spu <= SizePickupSPUConstants.PEUPPERLIMIT)
            {
                result = "Standard";
            }
            if (spu > SizePickupSPUConstants.PEUPPERLIMIT)
            {
                result = "Upper Limit";
            }
            return result;
        }

        public string ComparingRayon(double spu)
        {
            string result = "";
            if (spu < SizePickupSPUConstants.RAYONLOWERLIMIT)
            {
                result = "Lower Limit";
            }
            if (spu >= SizePickupSPUConstants.RAYONLOWERLIMIT && spu <= SizePickupSPUConstants.RAYONUPPERLIMIT)
            {
                result = "Standard";
            }
            if (spu > SizePickupSPUConstants.RAYONUPPERLIMIT)
            {
                result = "Upper Limit";
            }
            return result;
        }
    }
}
