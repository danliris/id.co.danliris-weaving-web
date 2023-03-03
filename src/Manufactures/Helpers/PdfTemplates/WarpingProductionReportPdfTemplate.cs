using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Manufactures.Helpers.PdfTemplates
{
    public class WarpingProductionReportPdfTemplate
    {
        #region Static Data

        const int MARGIN = 16;
        const string TITLE = "MONITOR PRODUKSI WARPING/ OPERATOR";
        const string DATE = "TGL";
        const string A = "A";
        const string B = "B";
        const string C = "C";
        const string D = "D";
        const string E = "E";
        const string F = "F";
        const string G = "G";
        const string TOTAL = "TOTAL";
        const string INITIALS = "PARAF";
        const string EMPTY_SPACE = "";

        #endregion

        #region Font

        private static readonly Font title_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
        private static readonly Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

        #endregion

        #region Table

        private readonly PdfPTable Title;
        private readonly PdfPTable Header;
        private readonly PdfPTable Body;
        private int colCount;
        private List<float> bodyCount;

        #endregion

        public WarpingProductionReportPdfTemplate(WarpingProductionReportListDto reportModel)
        {
            #region Title

            var titleMonthValue = reportModel.Month;
            var titleYearValue = reportModel.Year;

            #endregion

            #region Header

            var headerDynamic = reportModel.Headers.Select(x => new WarpingProductionReportHeaderDto(x.Group, x.Name)).OrderBy(x => x.Group).ThenBy(x => x.Name);

            #endregion

            #region Body
            var productionValue = reportModel.ProcessedList.SelectMany(x => x.DailyProcessedPerOperator).Select(x => new DailyProcessedPerOperatorDto(x.Group, x.Name, x.Total)).OrderBy(x => x.Group).ThenBy(x => x.Name);

            #endregion

            this.Title = GetTitle(titleMonthValue, titleYearValue);
            this.Header = GetHeader(headerDynamic.ToList());
            this.Body = GetBody(headerDynamic.ToList(), reportModel.ProcessedList);
        }

        private PdfPTable GetTitle(string month, string year)
        {
            PdfPTable title = new PdfPTable(1)
            {
                WidthPercentage = 100
            };
            PdfPCell cellTitle = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingBottom = 10f
            };
            cellTitle.Phrase = new Phrase(TITLE, title_font);
            title.AddCell(cellTitle);

            cellTitle.Phrase = new Phrase(month.ToUpper() + " " + year, title_font);
            title.AddCell(cellTitle);

            return title;
        }

        private PdfPTable GetHeader(List<WarpingProductionReportHeaderDto> dynamicData)
        {
            colCount = dynamicData.Count() + 3;

            PdfPTable headerTable = new PdfPTable(colCount);
            bodyCount = new List<float>() { 1f, 1f, 1f };
            dynamicData.ForEach((item) => bodyCount.Add(1f));
            headerTable.SetWidths(bodyCount.ToArray());
            headerTable.WidthPercentage = 100;

            PdfPCell RowSpan2Cell = new PdfPCell()
            {
                Border = Rectangle.BOX,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Rowspan = 2
            };

            RowSpan2Cell.Phrase = new Phrase(DATE, bold_font);
            headerTable.AddCell(RowSpan2Cell);

            PdfPCell StaticHeaderCell = new PdfPCell()
            {
                Border = Rectangle.BOX,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };

            var groupList = dynamicData.GroupBy(x => x.Group)
                                .Select(g => g.Key);

            foreach (var item in groupList)
            {
                StaticHeaderCell.Colspan = dynamicData.Count(x => x.Group == item);
                StaticHeaderCell.Phrase = new Phrase(item, bold_font);
                headerTable.AddCell(StaticHeaderCell);

            }

            RowSpan2Cell.Phrase = new Phrase(TOTAL, bold_font);
            headerTable.AddCell(RowSpan2Cell);

            RowSpan2Cell.Phrase = new Phrase(INITIALS, bold_font);
            headerTable.AddCell(RowSpan2Cell);

            StaticHeaderCell.Colspan = 1;
            foreach (var operatorName in dynamicData)
            {
                StaticHeaderCell.Phrase = new Phrase(operatorName.Name, bold_font);
                headerTable.AddCell(StaticHeaderCell);
            }
            StaticHeaderCell.Colspan = 1;
            StaticHeaderCell.Phrase = new Phrase("", normal_font);
            headerTable.AddCell(StaticHeaderCell);
            StaticHeaderCell.Colspan = 1;

            return headerTable;
        }

        private PdfPTable GetBody(List<WarpingProductionReportHeaderDto> dynamicData, List<WarpingProductionReportProcessedListDto> bodyData)
        {
            PdfPTable bodyTable = new PdfPTable(colCount);
            bodyTable.SetWidths(bodyCount.ToArray());
            bodyTable.WidthPercentage = 100;

            PdfPCell bodyCell = new PdfPCell() { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };

            foreach (var bodyValue in bodyData)
            {
                bodyCell.Phrase = new Phrase(bodyValue.Day.ToString(), normal_font);
                bodyTable.AddCell(bodyCell);

                var group = bodyValue.DailyProcessedPerOperator.GroupBy(x => new { x.Group, x.Name })
                    .Select(x => new DailyProcessedPerOperatorDto(x.Key.Group, x.Key.Name, x.Sum(y => y.Total))).OrderBy(x => x.Group).ThenBy(x => x.Name);

                foreach (var item in dynamicData)
                {
                    var currentCell = group.FirstOrDefault(x => x.Group == item.Group && x.Name == item.Name);
                    if (currentCell != null)
                    {
                        bodyCell.Phrase = new Phrase(currentCell.Total.ToString(), normal_font);
                        bodyTable.AddCell(bodyCell);
                    }
                    else
                    {
                        bodyCell.Phrase = new Phrase("0", normal_font);
                        bodyTable.AddCell(bodyCell);
                    }
                }

                bodyCell.Phrase = new Phrase(group.Sum(x => x.Total).ToString(), normal_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("", normal_font);
                bodyTable.AddCell(bodyCell);
            }

            bodyCell.Phrase = new Phrase("", normal_font);
            bodyTable.AddCell(bodyCell);

            return bodyTable;
        }

        public MemoryStream GeneratePdfTemplate()
        {

            Document document = new Document(PageSize.A4.Rotate(), MARGIN, MARGIN, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            document.Add(Title);
            document.Add(Header);
            document.Add(Body);

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
