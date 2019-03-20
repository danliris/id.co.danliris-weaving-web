using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Domain.Orders;
using Manufactures.Dtos;
using Manufactures.Dtos.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Helpers.PdfTemplates
{
    public class OrderProductionReportPDFTemplate
    {
        public MemoryStream GenerateSOPReportPdf(List<OrderReportBySearchDto> weavingModel)
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
            cellSubHeader.Phrase = new Phrase("Unit : ", normal_font);
            cellSubHeader.HorizontalAlignment = Element.ALIGN_LEFT;
            subHeaderTable.AddCell(cellSubHeader);
            //Empty Content
            cellSubHeader.Phrase = new Phrase("", normal_font);
            subHeaderTable.AddCell(cellSubHeader);

            //---------------------------------------------------------------------------------//
            //Add Element to Table
            document.Add(headerTable);
            document.Add(subHeaderTable);

            #endregion Header

            #region Body

            //Body (1st Row Table)
            PdfPTable bodyTableFirst = new PdfPTable(9);
            float[] bodyTableFirstWidths = new float[] { 4f, 10f, 16f, 8f, 15f, 15f, 20f, 8f, 15f };
            bodyTableFirst.SetWidths(bodyTableFirstWidths);
            bodyTableFirst.WidthPercentage = 100;

            //Add Cell (1st Row Table)
            PdfPCell bodyCellFirstNumber = new PdfPCell();
            PdfPCell bodyCellFirstDate = new PdfPCell();
            PdfPCell bodyCellFirstConstructionNumber = new PdfPCell();
            PdfPCell bodyCellFirstYarnType = new PdfPCell();
            PdfPCell bodyCellFirstWarpBlended = new PdfPCell();
            PdfPCell bodyCellFirstWeftBlended = new PdfPCell();
            PdfPCell bodyCellFirstEstimatedProduction = new PdfPCell();
            PdfPCell bodyCellFirstTotal = new PdfPCell();
            PdfPCell bodyCellFirstWoven = new PdfPCell();


            //Fill Cell with Content (1st Row Table)
            bodyCellFirstNumber.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstDate.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstConstructionNumber.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstYarnType.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstWarpBlended.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellFirstWeftBlended.HorizontalAlignment = Element.ALIGN_CENTER;
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

            bodyCellFirstYarnType.Phrase = new Phrase("No. Benang", bold_font);
            bodyCellFirstYarnType.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER;
            bodyTableFirst.AddCell(bodyCellFirstYarnType);

            bodyCellFirstWarpBlended.Phrase = new Phrase("Komposisi Lusi(%)", bold_font);
            bodyTableFirst.AddCell(bodyCellFirstWarpBlended);

            bodyCellFirstWeftBlended.Phrase = new Phrase("Komposisi Pakan(%)", bold_font);
            bodyTableFirst.AddCell(bodyCellFirstWeftBlended);

            bodyCellFirstEstimatedProduction.Phrase = new Phrase("Estimasi Produksi", bold_font);
            bodyTableFirst.AddCell(bodyCellFirstEstimatedProduction);

            bodyCellFirstTotal.Phrase = new Phrase("Total ALL", bold_font);
            bodyCellFirstTotal.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER;
            bodyTableFirst.AddCell(bodyCellFirstTotal);

            bodyCellFirstWoven.Phrase = new Phrase("Kebutuhan Benang", bold_font);
            bodyTableFirst.AddCell(bodyCellFirstWoven);

            //Body (2nd Row Table)
            PdfPTable bodyTableSecond = new PdfPTable(18);
            float[] bodyTableSecondWidths = new float[] { 4f, 10f, 16f, 8f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 8f, 5f, 5f, 5f };
            bodyTableSecond.SetWidths(bodyTableSecondWidths);
            bodyTableSecond.WidthPercentage = 100;

            //Add Cell (2nd Row Table)
            PdfPCell bodyCellSecondNumber = new PdfPCell();
            PdfPCell bodyCellSecondDate = new PdfPCell();
            PdfPCell bodyCellSecondConstructionNumber = new PdfPCell();
            PdfPCell bodyCellSecondYarnType = new PdfPCell();
            PdfPCell bodyCellSecondWarpBlendedPoly = new PdfPCell();
            PdfPCell bodyCellSecondWarpBlendedCotton = new PdfPCell();
            PdfPCell bodyCellSecondWarpBlendedOther = new PdfPCell();
            PdfPCell bodyCellSecondWeftBlendedPoly = new PdfPCell();
            PdfPCell bodyCellSecondWeftBlendedCotton = new PdfPCell();
            PdfPCell bodyCellSecondWeftBlendedOther = new PdfPCell();
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
            bodyCellSecondYarnType.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondWarpBlendedPoly.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondWarpBlendedCotton.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondWarpBlendedOther.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondWeftBlendedPoly.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondWeftBlendedCotton.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyCellSecondWeftBlendedOther.HorizontalAlignment = Element.ALIGN_CENTER;
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

            bodyCellSecondYarnType.Phrase = new Phrase("", normal_font);
            bodyCellSecondYarnType.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //bodyCellFirst.Border = Rectangle.NO_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            bodyTableSecond.AddCell(bodyCellSecondYarnType);

            bodyCellSecondWarpBlendedPoly.Phrase = new Phrase("Poly", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondWarpBlendedPoly);

            bodyCellSecondWarpBlendedCotton.Phrase = new Phrase("Cotton", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondWarpBlendedCotton);

            bodyCellSecondWarpBlendedOther.Phrase = new Phrase("Lainnya", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondWarpBlendedOther);

            bodyCellSecondWeftBlendedPoly.Phrase = new Phrase("Poly", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondWeftBlendedPoly);

            bodyCellSecondWeftBlendedCotton.Phrase = new Phrase("Cotton", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondWeftBlendedCotton);

            bodyCellSecondWeftBlendedOther.Phrase = new Phrase("Lainnya", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondWeftBlendedOther);

            bodyCellSecondEstimatedProductionA.Phrase = new Phrase("Grade A", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondEstimatedProductionA);

            bodyCellSecondEstimatedProductionB.Phrase = new Phrase("Grade B", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondEstimatedProductionB);

            bodyCellSecondEstimatedProductionC.Phrase = new Phrase("Grade C", bold_font);
            bodyTableSecond.AddCell(bodyCellSecondEstimatedProductionC);

            bodyCellSecondEstimatedProductionAval.Phrase = new Phrase("Grade D", bold_font);
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
            float[] footerTableWidths = new float[] { 13f, 26f };
            footerTable.SetWidths(footerTableWidths);
            footerTable.WidthPercentage = 100;

            PdfPTable footerTableSign = new PdfPTable(3);
            float[] footerTableSignWidths = new float[] { 10f, 10f, 10f };
            footerTableSign.SetWidths(footerTableSignWidths);
            footerTableSign.WidthPercentage = 100;

            //Add Cell (3rd Row Table)
            PdfPCell footerCellFirst = new PdfPCell();
            footerCellFirst.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellFirst.Border = Rectangle.NO_BORDER;

            PdfPCell footerCellSecond = new PdfPCell();
            footerCellSecond.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellSecond.Border = Rectangle.NO_BORDER;

            PdfPCell footerCellSignHead = new PdfPCell();
            footerCellSignHead.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellSignHead.Border = Rectangle.NO_BORDER;

            PdfPCell footerCellSignProduction = new PdfPCell();
            footerCellSignProduction.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellSignProduction.Border = Rectangle.NO_BORDER;

            PdfPCell footerCellSignPPIC = new PdfPCell();
            footerCellSignPPIC.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellSignPPIC.Border = Rectangle.NO_BORDER;

            PdfPCell footerCellEmpty = new PdfPCell();
            footerCellEmpty.HorizontalAlignment = Element.ALIGN_CENTER;
            footerCellEmpty.Border = Rectangle.NO_BORDER;

            //Fill Cell with Content (3rd Row Table)

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellFirst.Phrase = new Phrase("Mengetahui,", normal_font);
            footerTable.AddCell(footerCellFirst);

            footerCellSecond.Phrase = new Phrase("Dibuat Oleh,", normal_font);
            footerTable.AddCell(footerCellSecond);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            footerCellEmpty.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(footerCellEmpty);

            //Atur Sesuai Form
            footerCellSignHead.Phrase = new Phrase("(....................................)", normal_font);
            footerTableSign.AddCell(footerCellSignHead);

            footerCellSignProduction.Phrase = new Phrase("(....................................)", normal_font);
            footerTableSign.AddCell(footerCellSignProduction);

            footerCellSignPPIC.Phrase = new Phrase("(....................................)", normal_font);
            footerTableSign.AddCell(footerCellSignPPIC);

            footerCellSignHead.Phrase = new Phrase("KEPALA BAGIAN", normal_font);
            footerTableSign.AddCell(footerCellSignHead);

            footerCellSignProduction.Phrase = new Phrase("MAINTENANCE/ PRODUKSI", normal_font);
            footerTableSign.AddCell(footerCellSignProduction);

            footerCellSignPPIC.Phrase = new Phrase("PPIC WEAVING", normal_font);
            footerTableSign.AddCell(footerCellSignPPIC);

            document.Add(footerTable);
            document.Add(footerTableSign);

            #endregion Footer

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
