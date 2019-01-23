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
            const int MARGIN = 10;

            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            Document document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            document.Open();

            #region Header

            PdfPTable headerTable = new PdfPTable(2);
            headerTable.SetWidths(new float[] { 10f, 10f });
            headerTable.WidthPercentage = 100;
            PdfPTable headerTable1 = new PdfPTable(1);
            PdfPTable headerTable2 = new PdfPTable(2);
            headerTable2.SetWidths(new float[] { 15f, 40f });
            headerTable2.WidthPercentage = 100;

            PdfPCell cellHeader1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody = new PdfPCell() { Border = Rectangle.NO_BORDER };

            PdfPCell cellHeaderCS2 = new PdfPCell() { Border = Rectangle.NO_BORDER, Colspan = 2 };


            cellHeaderCS2.Phrase = new Phrase("BUKTI PENGELUARAN BANK", bold_font);
            cellHeaderCS2.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.AddCell(cellHeaderCS2);

            cellHeaderBody.Phrase = new Phrase("PT. DANLIRIS", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Kel. Banaran, Kec. Grogol", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Sukoharjo - 57100", normal_font);
            headerTable1.AddCell(cellHeaderBody);

            cellHeader1.AddElement(headerTable1);
            headerTable.AddCell(cellHeader1);

            cellHeaderCS2.Phrase = new Phrase("", bold_font);
            headerTable2.AddCell(cellHeaderCS2);

            cellHeaderBody.Phrase = new Phrase("Tanggal", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": ", normal_font);
            headerTable2.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("NO", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": ", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            
            cellHeaderBody.Phrase = new Phrase("Dibayarkan ke", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": ", normal_font);
            headerTable2.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("Bank", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": ", normal_font);
            headerTable2.AddCell(cellHeaderBody);

            cellHeader2.AddElement(headerTable2);
            headerTable.AddCell(cellHeader2);

            cellHeaderCS2.Phrase = new Phrase("", normal_font);
            headerTable.AddCell(cellHeaderCS2);

            document.Add(headerTable);

            #endregion Header

            

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            //try
            //{
            //    stream.Write(byteInfo, 0, byteInfo.Length);
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            stream.Position = 0;

            return stream;
        }
    }
}
