using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using System;
using System.Collections.Generic;
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

        #endregion

        #region Font

        private static readonly Font title_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
        private static readonly Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

        #endregion

        #region Table

        private readonly PdfPTable Title;
        private readonly PdfPTable Header;
        //private readonly PdfPTable Body;

        #endregion

        public WarpingProductionReportPdfTemplate(WarpingProductionReportListDto reportModel)
        {
            #region Header

            List<string> headerStatic = new List<string> { A, B, C, D, E, F, G, TOTAL };

            List<string> headerDynamic = new List<string> { reportModel.AGroupTotal.ToString(),
                                                                reportModel.BGroupTotal.ToString(),
                                                                reportModel.CGroupTotal.ToString(),
                                                                reportModel.DGroupTotal.ToString(),
                                                                reportModel.EGroupTotal.ToString(),
                                                                reportModel.FGroupTotal.ToString(),
                                                                reportModel.GGroupTotal.ToString(),
                                                                reportModel.TotalAll.ToString() };

            #endregion

            #region Body

            List<List<string>> bodyData = new List<List<string>>
            {
                reportModel.PerOperatorList.Select(x => x.ProductionDate.ToString()).ToList(),
                reportModel.PerOperatorList.Select(x => x.AGroup.ToString()).ToList(),
                reportModel.PerOperatorList.Select(x => x.BGroup.ToString()).ToList(),
                reportModel.PerOperatorList.Select(x => x.CGroup.ToString()).ToList(),
                reportModel.PerOperatorList.Select(x => x.DGroup.ToString()).ToList(),
                reportModel.PerOperatorList.Select(x => x.EGroup.ToString()).ToList(),
                reportModel.PerOperatorList.Select(x => x.FGroup.ToString()).ToList(),
                reportModel.PerOperatorList.Select(x => x.GGroup.ToString()).ToList(),
                reportModel.PerOperatorList.Select(x => x.Total.ToString()).ToList(),
            };

            #endregion

            this.Title = GetTitle();
            this.Header = GetHeader(headerStatic, headerDynamic);
            //this.Body = GetBody(bodyData);
        }

        private PdfPTable GetTitle()
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

            return title;
        }

        private PdfPTable GetHeader(List<string> staticData, List<string> dynamicData)
        {
            PdfPTable bodyTable = new PdfPTable(10);
            float[] bodyTableWidths = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };
            bodyTable.SetWidths(bodyTableWidths);
            bodyTable.WidthPercentage = 100;

            PdfPCell bodyCell = new PdfPCell() { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
            //PdfPCell emptyCell = new PdfPCell() { Border = Rectangle.BOX, FixedHeight = 15f };
            //emptyCell.Phrase = new Phrase(string.Empty, normal_font);

            bodyCell.Phrase = new Phrase(DATE, bold_font);
            bodyCell.Rowspan = 2;
            bodyTable.AddCell(bodyCell);

            foreach (var headerString in staticData)
            {
                bodyCell.Phrase = new Phrase(headerString, bold_font);
                bodyTable.AddCell(bodyCell);
            }

            bodyCell.Phrase = new Phrase(INITIALS, bold_font);
            bodyCell.Rowspan = 2;
            bodyTable.AddCell(bodyCell);

            foreach (var headerValue in dynamicData)
            {
                bodyCell.Phrase = new Phrase(headerValue, normal_font);
                bodyTable.AddCell(bodyCell);
            }

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
            //document.Add(Body);

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
