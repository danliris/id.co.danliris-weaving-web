using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Domain.Orders;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.PdfTemplates
{
    public class OrderProductionReportPDFTemplate
    {
        public MemoryStream GenerateSOPReportPdf(WeavingOrderDocumentDto[] weavingModel)
        {
            const int MARGIN = 16;

            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            Document document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN);
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            document.Open();

            #region Header

            //Header
            PdfPTable headerTable = new PdfPTable(1);

            //Add Cell
            PdfPCell cellHeader = new PdfPCell() { Border = Rectangle.NO_BORDER };

            //Fill Cell with Content
            cellHeader.Phrase = new Phrase("DAFTAR LAPORAN SURAT ORDER PRODUKSI", bold_font);
            cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.AddCell(cellHeader);
            //Empty Content
            cellHeader.Phrase = new Phrase("", normal_font);
            headerTable.AddCell(cellHeader);

            //---------------------------------------------------------------------------------//

            //Sub-header
            PdfPTable subHeaderTable = new PdfPTable(1);

            //Add Cell
            PdfPCell cellSubHeader = new PdfPCell() { Border = Rectangle.NO_BORDER };

            //Fill Cell with Content
            cellSubHeader.Phrase = new Phrase("Unit :", normal_font);
            cellSubHeader.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeaderTable.AddCell(cellSubHeader);
            //Empty Content
            cellSubHeader.Phrase = new Phrase("", normal_font);
            subHeaderTable.AddCell(cellSubHeader);

            document.Add(headerTable);
            document.Add(subHeaderTable);

            #endregion Header

            #region Body

            //Body (1st Row Table)
            PdfPTable bodyTableFirst = new PdfPTable(8);
            float[] bodyTableFirstWidths = new float[] { 4f, 10f, 16f, 8f, 15f, 20f, 8f, 15f };
            bodyTableFirst.SetWidths(bodyTableFirstWidths);
            bodyTableFirst.WidthPercentage = 100;

            //Add Cell (1st Row Table)
            PdfPCell bodyCellFirstNumber = new PdfPCell();
            PdfPCell bodyCellFirstDate = new PdfPCell();
            PdfPCell bodyCellFirstConstructionNumber = new PdfPCell();
            PdfPCell bodyCellFirstMaterialType = new PdfPCell();
            PdfPCell bodyCellFirstBlended = new PdfPCell();
            PdfPCell bodyCellFirstEstimatedProduction = new PdfPCell();
            PdfPCell bodyCellFirstTotal = new PdfPCell();
            PdfPCell bodyCellFirstWoven = new PdfPCell();


            //Fill Cell with Content (1st Row Table)
            bodyCellFirstNumber.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstDate.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstConstructionNumber.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstMaterialType.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstBlended.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstEstimatedProduction.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstTotal.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstWoven.HorizontalAlignment = Element.ALIGN_CENTER;

            bodyCellFirstNumber.Phrase = new Phrase("No.", bold_font);
            bodyCellFirstNumber.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER;
            bodyTableFirst.AddCell(bodyCellFirstNumber);

            bodyCellFirstDate.Phrase = new Phrase("Tanggal SP", bold_font);
            bodyCellFirstDate.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER;
            bodyTableFirst.AddCell(bodyCellFirstDate);

            bodyCellFirstConstructionNumber.Phrase = new Phrase("Konstruksi", bold_font);
            bodyCellFirstConstructionNumber.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER;
            bodyTableFirst.AddCell(bodyCellFirstConstructionNumber);

            bodyCellFirstMaterialType.Phrase = new Phrase("No. Benang", bold_font);
            bodyCellFirstMaterialType.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER;
            bodyTableFirst.AddCell(bodyCellFirstMaterialType);

            bodyCellFirstBlended.Phrase = new Phrase("Blended (%)", bold_font);
            bodyTableFirst.AddCell(bodyCellFirstBlended);

            bodyCellFirstEstimatedProduction.Phrase = new Phrase("Estimasi Produksi", bold_font);
            bodyTableFirst.AddCell(bodyCellFirstEstimatedProduction);

            bodyCellFirstTotal.Phrase = new Phrase("Total ALL", bold_font);
            bodyCellFirstTotal.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER;
            bodyTableFirst.AddCell(bodyCellFirstTotal);

            bodyCellFirstWoven.Phrase = new Phrase("Kebutuhan Benang", bold_font);
            bodyTableFirst.AddCell(bodyCellFirstWoven);

            //Body (2nd Row Table)
            PdfPTable bodyTableSecond = new PdfPTable(15);
            float[] bodyTableSecondWidths = new float[] { 4f, 10f, 16f, 8f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 8f, 5f, 5f, 5f };
            bodyTableSecond.SetWidths(bodyTableSecondWidths);
            bodyTableSecond.WidthPercentage = 100;

            //Add Cell (2nd Row Table)
            PdfPCell bodyCellSecondNumber = new PdfPCell();
            PdfPCell bodyCellSecondDate = new PdfPCell();
            PdfPCell bodyCellSecondConstructionNumber = new PdfPCell();
            PdfPCell bodyCellSecondMaterialType = new PdfPCell();
            PdfPCell bodyCellSecondBlendedPoly = new PdfPCell();
            PdfPCell bodyCellSecondBlendedCotton = new PdfPCell();
            PdfPCell bodyCellSecondBlendedOther = new PdfPCell();
            PdfPCell bodyCellSecondEstimatedProductionA = new PdfPCell();
            PdfPCell bodyCellSecondEstimatedProductionB = new PdfPCell();
            PdfPCell bodyCellSecondEstimatedProductionC = new PdfPCell();
            PdfPCell bodyCellSecondEstimatedProductionAval = new PdfPCell();
            PdfPCell bodyCellSecondTotal = new PdfPCell();
            PdfPCell bodyCellSecondWarp = new PdfPCell();
            PdfPCell bodyCellSecondWeft = new PdfPCell();
            PdfPCell bodyCellSecondWovenTotal = new PdfPCell();

            //Fill Cell with Content (2nd Row Table)
            bodyCellSecondNumber.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondDate.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondConstructionNumber.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondMaterialType.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondBlendedPoly.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondBlendedCotton.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondBlendedOther.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondEstimatedProductionA.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondEstimatedProductionB.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondEstimatedProductionC.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondEstimatedProductionAval.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondTotal.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondWarp.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondWeft.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondWovenTotal.HorizontalAlignment = Element.ALIGN_CENTER;

            bodyCellSecondNumber.Phrase = new Phrase("", normal_font);
            bodyCellSecondNumber.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //bodyCellFirst.Border = Rectangle.NO_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            bodyTableSecond.AddCell(bodyCellSecondNumber);

            bodyCellSecondDate.Phrase = new Phrase("", normal_font);
            bodyCellSecondDate.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //bodyCellFirst.Border = Rectangle.NO_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            bodyTableSecond.AddCell(bodyCellSecondDate);

            bodyCellSecondConstructionNumber.Phrase = new Phrase("", normal_font);
            bodyCellSecondConstructionNumber.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //bodyCellFirst.Border = Rectangle.NO_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            bodyTableSecond.AddCell(bodyCellSecondConstructionNumber);

            bodyCellSecondMaterialType.Phrase = new Phrase("", normal_font);
            bodyCellSecondMaterialType.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //bodyCellFirst.Border = Rectangle.NO_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            bodyTableSecond.AddCell(bodyCellSecondMaterialType);

            bodyCellSecondBlendedPoly.Phrase = new Phrase("Poly", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondBlendedPoly);

            bodyCellSecondBlendedCotton.Phrase = new Phrase("Cotton", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondBlendedCotton);

            bodyCellSecondBlendedOther.Phrase = new Phrase("Lainnya", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondBlendedOther);

            bodyCellSecondEstimatedProductionA.Phrase = new Phrase("Grade A", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondEstimatedProductionA);

            bodyCellSecondEstimatedProductionB.Phrase = new Phrase("Grade B", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondEstimatedProductionB);

            bodyCellSecondEstimatedProductionC.Phrase = new Phrase("Grade C", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondEstimatedProductionC);

            bodyCellSecondEstimatedProductionAval.Phrase = new Phrase("Aval", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondEstimatedProductionAval);

            bodyCellSecondTotal.Phrase = new Phrase("", bold_font);
            bodyCellSecondTotal.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //bodyCellFirst.Border = Rectangle.NO_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            bodyTableSecond.AddCell(bodyCellSecondTotal);

            bodyCellSecondWarp.Phrase = new Phrase("Lusi", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondWarp);

            bodyCellSecondWeft.Phrase = new Phrase("Pakan", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondWeft);

            bodyCellSecondWovenTotal.Phrase = new Phrase("Total", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondWovenTotal);

            document.Add(bodyTableFirst);
            document.Add(bodyTableSecond);

            #endregion Body

            #region Footer

            //Add Table (3rd Row Table)
            PdfPTable footerTable = new PdfPTable(2);
            float[] footerTableWidths = new float[] { 10f, 25f };
            footerTable.SetWidths(footerTableWidths);
            footerTable.WidthPercentage = 100;

            PdfPTable footerTableSign = new PdfPTable(3);
            float[] footerTableSignWidths = new float[] { 10f, 10f, 10f };
            footerTableSign.SetWidths(footerTableSignWidths);
            footerTableSign.WidthPercentage = 100;

            //Add Cell (3rd Row Table)
            PdfPCell footerCellFirst = new PdfPCell();
            PdfPCell footerCellSecond = new PdfPCell();
            PdfPCell footerCellSignHead = new PdfPCell();
            PdfPCell footerCellSignProduction = new PdfPCell();
            PdfPCell footerCellSignPPIC = new PdfPCell();

            //Fill Cell with Content (3rd Row Table)
            footerCellFirst.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellSecond.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellSignHead.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellSignProduction.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellSignPPIC.HorizontalAlignment = Element.ALIGN_CENTER;

            footerCellFirst.Phrase = new Phrase("Mengetahui,", normal_font);
            footerCellSignPPIC.Border = Rectangle.NO_BORDER;
            footerTable.AddCell(footerCellFirst);

            footerCellSecond.Phrase = new Phrase("Dibuat Oleh,", normal_font);
            footerCellSignPPIC.Border = Rectangle.NO_BORDER;
            footerTable.AddCell(footerCellSecond);

            //Atur Sesuai Form
            footerCellSignHead.Phrase = new Phrase("(....................)", normal_font);
            footerCellSignHead.Border = Rectangle.NO_BORDER;
            footerTableSign.AddCell(footerCellSignHead);

            footerCellSignProduction.Phrase = new Phrase("(....................)", normal_font);
            footerCellSignProduction.Border = Rectangle.NO_BORDER;
            footerTableSign.AddCell(footerCellSignProduction);

            footerCellSignPPIC.Phrase = new Phrase("(....................)", normal_font);
            footerCellSignPPIC.Border = Rectangle.NO_BORDER;
            footerTableSign.AddCell(footerCellSignPPIC);

            #endregion Footer

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
