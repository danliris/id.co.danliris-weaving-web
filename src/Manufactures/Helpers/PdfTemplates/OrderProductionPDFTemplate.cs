using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Application.Orders.DataTransferObjects.OrderReport;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Manufactures.Helpers.PdfTemplates
{
    public class OrderProductionPdfTemplate
    {
        #region Static Data

        const int MARGIN = 16;
        const string TITLE = "DAFTAR LAPORAN SURAT PERINTAH PRODUKSI";
        const string UNIT = "Unit";
        const string SPLITTER = " : ";
        const string EMPTY_SPOT = " ";
        const string NUMBER = "No.";
        const string DATE_ORDERED = "Tanggal SPP";
        const string CONSTRUCTION = "Konstruksi";
        const string YARN_NUMBER = "No. Benang";
        const string BLENDED_WARP_POLY = "Lusi" + "\n" + "Poly(%)";
        const string BLENDED_WARP_COTTON = "Lusi" + "\n" + "Cotton(%)";
        const string BLENDED_WARP_OTHERS = "Lusi" + "\n" + "Lainnya(%)";
        const string BLENDED_WEFT_POLY = "Pakan" + "\n" + "Poly(%)";
        const string BLENDED_WEFT_COTTON = "Pakan" + "\n" + "Cotton(%)";
        const string BLENDED_WEFT_OTHERS = "Pakan" + "\n" + "Lainnya(%)";
        const string ESTIMATED_GRADE_A = "Grade A";
        const string ESTIMATED_GRADE_B = "Grade B";
        const string ESTIMATED_GRADE_C = "Grade C";
        const string ESTIMATED_GRADE_D = "Grade D";
        const string WHOLE_GRADE = "Total ALL";
        const string FABRIC_CONSTRUCTION_WARP = "Kebutuhan" + "\n" + "Lusi";
        const string FABRIC_CONSTRUCTION_WEFT = "Kebutuhan" + "\n" + "Pakan";
        const string FABRIC_CONSTRUCTION_TOTAL_YARN = "Total" + "\n" + "Kebutuhan";
        const string KNOWING = "Mengetahui,";
        const string MADE_BY = "Dibuat Oleh,";
        const string BLANK_SPOT = "(....................................)";
        const string HEAD_OF_DIVISION = "KEPALA BAGIAN";
        const string MAINTENANCE_PRODUCTION = "MAINTENANCE/ PRODUKSI";
        const string PPIC_WEAVING = "PPIC WEAVING";

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
        private readonly PdfPTable BodyFooter;
        private readonly PdfPTable Footer;

        #endregion

        public OrderProductionPdfTemplate(List<OrderReportListDto> reportModel)
        {
            //#region Header

            //List<string> headerData = new List<string> { UNIT };

            //#endregion

            #region Body

            List<string> bodyColumn = new List<string> { DATE_ORDERED, CONSTRUCTION, YARN_NUMBER, BLENDED_WARP_POLY, BLENDED_WARP_COTTON, BLENDED_WARP_OTHERS, BLENDED_WEFT_POLY, BLENDED_WEFT_COTTON, BLENDED_WEFT_OTHERS, ESTIMATED_GRADE_A, ESTIMATED_GRADE_B, ESTIMATED_GRADE_C, ESTIMATED_GRADE_D, WHOLE_GRADE, FABRIC_CONSTRUCTION_WARP, FABRIC_CONSTRUCTION_WEFT, FABRIC_CONSTRUCTION_TOTAL_YARN };

            List<List<string>> bodyData = new List<List<string>>
            {
                reportModel.Select(x => x.DateOrdered.Date.ToString("dd/MM/yyyy")).ToList(),
                reportModel.Select(x => x.ConstructionNumber).ToList(),
                reportModel.Select(x => x.YarnNumber).ToList(),
                reportModel.Select(x => x.WarpCompositionPoly.ToString()).ToList(),
                reportModel.Select(x => x.WarpCompositionCotton.ToString()).ToList(),
                reportModel.Select(x => x.WarpCompositionOthers.ToString()).ToList(),
                reportModel.Select(x => x.WeftCompositionPoly.ToString()).ToList(),
                reportModel.Select(x => x.WeftCompositionCotton.ToString()).ToList(),
                reportModel.Select(x => x.WeftCompositionOthers.ToString()).ToList(),
                reportModel.Select(x => x.EstimatedProductionGradeA.ToString()).ToList(),
                reportModel.Select(x => x.EstimatedProductionGradeB.ToString()).ToList(),
                reportModel.Select(x => x.EstimatedProductionGradeC.ToString()).ToList(),
                reportModel.Select(x => x.EstimatedProductionGradeD.ToString()).ToList(),
                reportModel.Select(x => x.TotalEstimatedProduction.ToString()).ToList(),
                reportModel.Select(x => x.AmountOfWarp.ToString()).ToList(),
                reportModel.Select(x => x.AmountOfWeft.ToString()).ToList(),
                reportModel.Select(x => x.TotalYarn.ToString()).ToList()
            };

            string unitName = reportModel.Select(u => u.UnitName).FirstOrDefault();

            #endregion

            this.Title = GetTitle();
            this.Header = GetHeader(unitName);
            this.Body = GetBody(bodyColumn, bodyData);
            this.BodyFooter = this.GetBodyFooter();
            this.Footer = GetFooter();
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

        private PdfPTable GetHeader(string headerData)
        {
            PdfPTable header = new PdfPTable(4);
            float[] headerTableWidths = new float[] { 1f, 1f, 3f, 34f };
            header.SetWidths(headerTableWidths);
            header.WidthPercentage = 100;
            PdfPCell cellHeader1 = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 10f
            };
            cellHeader1.Phrase = new Phrase(UNIT, normal_font);
            header.AddCell(cellHeader1);

            PdfPCell cellHeader2 = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 10f
            };
            cellHeader2.Phrase = new Phrase(SPLITTER, normal_font);
            header.AddCell(cellHeader2);

            PdfPCell cellHeader3 = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 10f
            };
            cellHeader3.Phrase = new Phrase(headerData, normal_font);
            header.AddCell(cellHeader3);

            PdfPCell cellHeader4 = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                PaddingBottom = 10f
            };
            cellHeader4.Phrase = new Phrase(EMPTY_SPOT, normal_font);
            header.AddCell(cellHeader4);

            return header;
        }

        private PdfPTable GetBody(List<string> bodyColumn, List<List<string>> bodyData)
        {
            PdfPTable bodyTable = new PdfPTable(18);
            float[] bodyTableWidths = new float[] { 3f, 7f, 12f, 8f, 7f, 7f, 7f, 7f, 7f, 7f, 5f, 5f, 5f, 5f, 6f, 7f, 7f, 7f };
            bodyTable.SetWidths(bodyTableWidths);
            bodyTable.WidthPercentage = 100;

            PdfPCell bodyCell = new PdfPCell() { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
            PdfPCell emptyCell = new PdfPCell() { Border = Rectangle.BOX, FixedHeight = 15f };
            emptyCell.Phrase = new Phrase(string.Empty, normal_font);

            bodyCell.Phrase = new Phrase("No", bold_font);
            bodyTable.AddCell(bodyCell);

            foreach (var column in bodyColumn)
            {
                bodyCell.Phrase = new Phrase(column, bold_font);
                bodyTable.AddCell(bodyCell);
            }

            for (int rowNo = 0; rowNo < bodyData.FirstOrDefault().Count; rowNo++)
            {
                bodyCell.Phrase = new Phrase((rowNo + 1).ToString("#,#", CultureInfo.InvariantCulture), normal_font);
                bodyTable.AddCell(bodyCell);

                for (int colNo = 0; colNo < bodyData.Count; colNo++)
                {
                    bodyCell.Phrase = new Phrase(bodyData[colNo][rowNo], normal_font);
                    bodyTable.AddCell(bodyCell);
                }
            }

            return bodyTable;
        }

        private PdfPTable GetBodyFooter()
        {
            PdfPTable bodyFooterTable = new PdfPTable(2);
            bodyFooterTable.SetWidths(new float[] { 1f, 2f });
            bodyFooterTable.WidthPercentage = 100;

            PdfPTable subBodyFooterTable = new PdfPTable(1);
            subBodyFooterTable.WidthPercentage = 100;
            subBodyFooterTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell bodyFooterCell = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            bodyFooterCell.PaddingTop = 10f;
            PdfPCell subBodyFooterCell = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell emptyCell = new PdfPCell() { Border = Rectangle.NO_BORDER, FixedHeight = 15f };
            emptyCell.Phrase = new Phrase(string.Empty, normal_font);

            subBodyFooterCell.Phrase = new Phrase(KNOWING, normal_font);
            subBodyFooterTable.AddCell(subBodyFooterCell);
            bodyFooterCell.AddElement(subBodyFooterTable);

            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            subBodyFooterTable = new PdfPTable(1);
            subBodyFooterTable.WidthPercentage = 100;
            subBodyFooterTable.HorizontalAlignment = Element.ALIGN_CENTER;

            subBodyFooterCell.Phrase = new Phrase(MADE_BY, normal_font);
            subBodyFooterTable.AddCell(subBodyFooterCell);
            bodyFooterCell.AddElement(subBodyFooterTable);

            bodyFooterTable.AddCell(bodyFooterCell);

            return bodyFooterTable;
        }

        private PdfPTable GetFooter()
        {
            PdfPTable footerTable = new PdfPTable(3);
            footerTable.SetWidths(new float[] { 1f, 1f, 1f });
            footerTable.WidthPercentage = 100;

            PdfPTable subFooterTable = new PdfPTable(1);
            subFooterTable.WidthPercentage = 100;
            subFooterTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell footerCell = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell subFooterCell = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell emptyCell = new PdfPCell() { Border = Rectangle.NO_BORDER, FixedHeight = 15f };
            emptyCell.Phrase = new Phrase(string.Empty, normal_font);

            subFooterTable.AddCell(emptyCell);
            subFooterTable.AddCell(emptyCell);
            subFooterCell.Phrase = new Phrase(BLANK_SPOT, normal_font);
            subFooterTable.AddCell(subFooterCell);
            subFooterCell.Phrase = new Phrase(HEAD_OF_DIVISION, normal_font);
            subFooterTable.AddCell(subFooterCell);
            footerCell.AddElement(subFooterTable);

            footerTable.AddCell(footerCell);

            footerCell = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            subFooterTable = new PdfPTable(1);
            subFooterTable.WidthPercentage = 100;
            subFooterTable.HorizontalAlignment = Element.ALIGN_CENTER;

            subFooterTable.AddCell(emptyCell);
            subFooterTable.AddCell(emptyCell);
            subFooterCell.Phrase = new Phrase(BLANK_SPOT, normal_font);
            subFooterTable.AddCell(subFooterCell);
            subFooterCell.Phrase = new Phrase(MAINTENANCE_PRODUCTION, normal_font);
            subFooterTable.AddCell(subFooterCell);
            footerCell.AddElement(subFooterTable);

            footerTable.AddCell(footerCell);

            footerCell = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            subFooterTable = new PdfPTable(1);
            subFooterTable.WidthPercentage = 100;
            subFooterTable.HorizontalAlignment = Element.ALIGN_CENTER;

            subFooterTable.AddCell(emptyCell);
            subFooterTable.AddCell(emptyCell);
            subFooterCell.Phrase = new Phrase(BLANK_SPOT, normal_font);
            subFooterTable.AddCell(subFooterCell);
            subFooterCell.Phrase = new Phrase(PPIC_WEAVING, normal_font);
            subFooterTable.AddCell(subFooterCell);
            footerCell.AddElement(subFooterTable);

            footerTable.AddCell(footerCell);

            return footerTable;
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
            document.Add(BodyFooter);
            document.Add(Footer);

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
