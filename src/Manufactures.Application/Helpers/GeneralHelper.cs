using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Helpers
{
    public class GeneralHelper
    {
        public DateTime ConvertDateTime(ExcelRangeBase value)
        {
            var date = DateTime.Parse(value.Text.ToString().Trim());
            var dateFormated = date.ToString("dd/MM/yyyy");
            return DateTime.ParseExact(dateFormated, "dd/MM/yyyy", null);
        }

        public DateTime? ConvertNullDateTime(ExcelRangeBase value)
        {
            if (value.Text != "" && !string.IsNullOrWhiteSpace(value.Text))
            {
                var date = DateTime.Parse(value.Text.ToString().Trim());
                var dateFormated = date.ToString("dd/MM/yyyy");
                return DateTime.ParseExact(dateFormated, "dd/MM/yyyy", null);
            }
            else
            {
                return null;
            }
        }

        public string GenerateId(ExcelRangeBase date, ExcelRangeBase no)
        {
            var dateTime = ConvertDateTime(date);
            var convertNo = Convert.ToInt32(no.Value);

            return dateTime.ToString("yyyyMM") + convertNo.ToString("000");
        }

        public void ValidatePeriode(ExcelRangeBase value, DateTime periode)
        {
            var dateSheet = ConvertDateTime(value);
            var monthSheet = dateSheet.ToString("MM/yyyy");
            var monthPeriode = periode.ToString("MM/yyyy");
            if (monthPeriode != monthSheet)
            {
                throw new Exception($"Periode yang dipilih tidak sesuai dengan periode data excel");
            }
        }
    }
}
