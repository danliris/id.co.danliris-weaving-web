using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Manufactures.Application.Helpers
{
    public class ConverterChecker
    {
        public string GenerateValueString(ExcelRangeBase value)
        {
            if (value.Value == null || value.Value.Equals("-") || string.IsNullOrWhiteSpace(value.Text) || value.Text.Trim().Equals("-"))
            {
                return null;
            }
            return value.Value.ToString().Trim();
            //return value.Value.ToString();
        }

        public int? GenerateValueInt(ExcelRangeBase value)
        {
            if (value.Value == null || value.Value.Equals("-") || string.IsNullOrWhiteSpace(value.Text) || value.Text.Trim().Equals("-"))
            {
                return null;
            }
            return Convert.ToInt32(value.Value.ToString().Trim());
        }

        public double? GenerateValueDouble(ExcelRangeBase value)
        {
            try
            {
                if (value.Value == null || value.Value.Equals("-") || string.IsNullOrWhiteSpace(value.Text) || value.Text.Trim().Equals("-"))
                {
                    return null;
                }
                return Convert.ToDouble(value.Value.ToString().Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal memproses value -{value.Value} {ex.Message}");
            }
        }

        public decimal? GenerateValueDecimal(ExcelRangeBase value)
        {
            if (value.Value == null || value.Value.Equals("-") || string.IsNullOrWhiteSpace(value.Text) || value.Text.Trim().Equals("-"))
            {
                return null;
            }
            return Convert.ToDecimal(value.Value.ToString().Trim());
        }

        public char? GenerateValueChar(ExcelRangeBase value)
        {
            if (value.Value == null || value.Value.Equals("-") || string.IsNullOrWhiteSpace(value.Text) || value.Text.Trim().Equals("-"))
            {
                return null;
            }
            return Convert.ToChar(value.Value.ToString().Trim());
        }

        public DateTime? GenerateValueDate(ExcelRangeBase value)
        {
            if (value.Value == null || value.Value.Equals("-") || string.IsNullOrWhiteSpace(value.Text) || value.Text.Trim().Equals("-"))
            {
                return null;
            }
            var date = DateTime.Parse(value.Text.ToString().Trim());
            var dateFormated = date.ToString("dd/MM/yyyy");
            return DateTime.ParseExact(dateFormated, "dd/MM/yyyy", null);
        }

        public string GeneratePureString(ExcelRangeBase value)
        {
            if (value.Value == null || value.Value.Equals("-") || value.Text.Trim().Equals("-") || string.IsNullOrWhiteSpace(value.Text))
            {
                return null;
            }
            return value.Text;
        }
        public double? GeneratePureDouble(ExcelRangeBase value)
        {
            try
            {
                if (value.Value == null || value.Value.Equals("-") || value.Text.Trim().Equals("-") || string.IsNullOrWhiteSpace(value.Text))
                {
                    return null;
                }
                return Convert.ToDouble(value.Text);
            }
            catch
            {
                return null;
            }
        }

        public DateTime? GeneratePureDateTime(ExcelRangeBase value)
        {
            if (value.Value == null || value.Value.Equals("-") || value.Text.Trim().Equals("-") || string.IsNullOrWhiteSpace(value.Text))
            {
                return null;
            }
            var date = DateTime.Parse(value.Text.ToString().Trim());
            var dateFormated = date.ToString("dd/MM/yyyy");
            return DateTime.ParseExact(dateFormated, "dd/MM/yyyy", null);
        }

        public DateTime? GeneratePureTime(ExcelRangeBase value)
        {
            if (value.Value == null || value.Value.Equals("-") || value.Text.Trim().Equals("-") || string.IsNullOrWhiteSpace(value.Text))
            {
                return null;
            }

            var richTime = DateTime.Parse(value.Text);
            var time = richTime.ToString("HH:mm");
            var anu = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);
            return DateTime.ParseExact(time, "HH:mm", null);
        }
    }
}
