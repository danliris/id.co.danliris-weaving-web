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

            Document document = new Document(PageSize.A4_LANDSCAPE, MARGIN, MARGIN, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            #region Header

            PdfPTable headerTable = new PdfPTable(2);
            PdfPCell headerCell = new PdfPCell(new Phrase("DAFTAR LAPORAN SURAT ORDER PRODUKSI", bold_font));
            headerCell.Rowspan = 2;
            headerCell.HorizontalAlignment = 1;
            headerTable.AddCell(headerCell);

            #endregion Header

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
