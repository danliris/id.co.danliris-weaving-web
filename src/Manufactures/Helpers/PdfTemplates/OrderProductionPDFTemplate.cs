using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Manufactures.Helpers.PdfTemplates
{
    public class OrderProductionPDFTemplate
    {
        #region Static Data

        const int MARGIN = 16;
        const string TITLE = "DAFTAR LAPORAN SURAT PERINTAH PRODUKSI";
        const string UNIT = "Unit";
        const string NUMBER = "No.";
        const string DATE_ORDERED = "Tanggal SPP";
        const string CONSTRUCTION = "Konstruksi";
        const string YARN_NUMBER = "No. Benang";
        const string BLENDED_WARP_POLY = "Lusi Poly(%)";
        const string BLENDED_WARP_COTTON = "Lusi Cotton(%)";
        const string BLENDED_WARP_OTHERS = "Lusi Lainnya(%)";
        const string BLENDED_WEFT_POLY = "Pakan Poly(%)";
        const string BLENDED_WEFT_COTTON = "Pakan Cotton(%)";
        const string BLENDED_WEFT_OTHERS = "Pakan Lainnya(%)";
        const string ESTIMATED_GRADE_A = "Grade A";
        const string ESTIMATED_GRADE_B = "Grade B";
        const string ESTIMATED_GRADE_C = "Grade C";
        const string ESTIMATED_GRADE_D = "Grade D";
        const string WHOLE_GRADE = "Total ALL";
        const string FABRIC_CONSTRUCTION_WARP = "Kebutuhan Lusi";
        const string FABRIC_CONSTRUCTION_WEFT = "Kebutuhan Pakan";
        const string FABRIC_CONSTRUCTION_TOTAL_YARN = "Total Kebutuhan";
        const string KNOWING = "Mengetahui,";
        const string MADE_BY = "Dibuat Oleh,";
        const string BLANK_SPOT = "(....................................)";
        const string HEAD_OF_DIVISION = "KEPALA BAGIAN";
        const string MAINTENANCE_PRODUCTION = "MAINTENANCE/ PRODUKSI";
        const string PPIC_WEAVING = "PPIC WEAVING";

        #endregion

        #region Font

        private static readonly Font title_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
        private static readonly Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
        private static readonly Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

        #endregion

        #region Table

        private readonly PdfPTable Title;
        private readonly PdfPTable Header;
        private readonly PdfPTable Body;
        private readonly PdfPTable Footer;

        #endregion

        public OrderProductionPDFTemplate(List<OrderReportBySearchDto> reportModel)
        {
            //#region Header

            //List<string> headerLeft = new List<string> { UNIT };

            //#endregion

            #region Body

            List<string> bodyColumn = new List<string> { DATE_ORDERED, CONSTRUCTION, YARN_NUMBER, BLENDED_WARP_POLY, BLENDED_WARP_COTTON, BLENDED_WARP_OTHERS, BLENDED_WEFT_POLY, BLENDED_WEFT_COTTON, BLENDED_WEFT_OTHERS, ESTIMATED_GRADE_A, ESTIMATED_GRADE_B, ESTIMATED_GRADE_C, ESTIMATED_GRADE_D, WHOLE_GRADE, FABRIC_CONSTRUCTION_WARP, FABRIC_CONSTRUCTION_WEFT, FABRIC_CONSTRUCTION_TOTAL_YARN };

            List<List<string>> bodyData = new List<List<string>>
            {
                reportModel.Select(x => x.DateOrdered.ToString()).ToList(),
                reportModel.Select(x=>x.FabricConstructionDocument.ConstructionNumber).ToList(),
                reportModel.Select(x=>x.YarnNumber).ToList(),
                reportModel.Select(x=>x.WarpComposition.CompositionOfPoly.ToString()).ToList(),
                reportModel.Select(x=>x.WarpComposition.CompositionOfCotton.ToString()).ToList(),
                reportModel.Select(x=>x.WarpComposition.OtherComposition.ToString()).ToList(),
                reportModel.Select(x=>x.WeftComposition.CompositionOfPoly.ToString()).ToList(),
                reportModel.Select(x=>x.WeftComposition.CompositionOfCotton.ToString()).ToList(),
                reportModel.Select(x=>x.WeftComposition.OtherComposition.ToString()).ToList(),
                reportModel.Select(x=>x.EstimatedProductionDocument.GradeA.ToString()).ToList(),
                reportModel.Select(x=>x.EstimatedProductionDocument.GradeB.ToString()).ToList(),
                reportModel.Select(x=>x.EstimatedProductionDocument.GradeC.ToString()).ToList(),
                reportModel.Select(x=>x.EstimatedProductionDocument.GradeD.ToString()).ToList(),
                reportModel.Select(x=>x.EstimatedProductionDocument.WholeGrade.ToString()).ToList(),
                reportModel.Select(x=>x.FabricConstructionDocument.AmountOfWarp.ToString()).ToList(),
                reportModel.Select(x=>x.FabricConstructionDocument.AmountOfWeft.ToString()).ToList(),
                reportModel.Select(x=>x.FabricConstructionDocument.TotalYarn.ToString()).ToList()
            };

            #endregion

            this.Title = GetTitle();
            this.Header = GetHeader();
            this.Body = GetBody(bodyColumn, bodyData);
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

        private PdfPTable GetHeader()
        {
            PdfPTable header = new PdfPTable(2)
            {
                WidthPercentage = 100
            };
            PdfPCell cellHeader = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            cellHeader.Phrase = new Phrase(UNIT, header_font);
            header.AddCell(cellHeader);

            return header;
        }

        private PdfPTable GetBody(List<string> bodyColumn, List<List<string>> bodyData)
        {
            PdfPTable bodyTable = new PdfPTable(18);
            float[] bodyTableWidths = new float[] { 4f, 10f, 16f, 8f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 8f, 5f, 5f, 5f };
            bodyTable.SetWidths(bodyTableWidths);
            bodyTable.WidthPercentage = 100;

            PdfPCell bodyCell = new PdfPCell() { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER };
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
            //PdfPCell emptyCellReceiver = new PdfPCell() { Border = Rectangle.NO_BORDER, FixedHeight = 10f };
            emptyCell.Phrase = new Phrase(string.Empty, normal_font);

            //subFooterTable.AddCell(emptyCellReceiver);
            subFooterCell.Phrase = new Phrase(KNOWING, normal_font);
            subFooterTable.AddCell(subFooterCell);
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

            subFooterCell.Phrase = new Phrase(MADE_BY, normal_font);
            subFooterTable.AddCell(subFooterCell);
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
            document.Add(Footer);

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
