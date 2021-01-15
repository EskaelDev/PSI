using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;


using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Events;

namespace SyllabusManager.Logic.Helpers
{
    public static class PdfHelper
    {
        public static readonly PdfNumber PORTRAIT = new PdfNumber(0);
        public static readonly PdfNumber LANDSCAPE = new PdfNumber(90);
        public static readonly PdfNumber INVERTEDPORTRAIT = new PdfNumber(180);
        public static readonly PdfNumber SEASCAPE = new PdfNumber(270);

        private static PdfFont FONT => PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.CP1250);
        private static string PATH => Directory.GetCurrentDirectory() + "/temp.pdf";
        public static string PATH_PAGED => Directory.GetCurrentDirectory() + "/temp_paged.pdf";
        public static Document Document(bool horizontal = false)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(PATH, FileMode.Create, FileAccess.Write)));

            if (horizontal) { pdfDocument.SetDefaultPageSize(pdfDocument.GetDefaultPageSize().Rotate()); }

            Document doc = new Document(pdfDocument);
            doc.SetFont(FONT);
            return doc;
        }


        public static void AddPages()
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(PATH), new PdfWriter(new FileStream(PATH_PAGED, FileMode.Create, FileAccess.Write)));
            Document doc = new Document(pdfDocument);

            int numberOfPages = pdfDocument.GetNumberOfPages();
            var size = pdfDocument.GetPage(1).GetPageSize();
            

            for (int i = 1; i <= numberOfPages; i++)
            {
                // Write aligned text to the specified by parameters point
                doc.ShowTextAligned(new Paragraph("Strona " + i + " z " + numberOfPages),
                        size.GetWidth()-50, 20, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
            }

            doc.Close();
        }

        public static Table Table(List<string> headers, List<List<string>> cells)
        {

            Table table = new Table(headers.Count);

            headers.ForEach(h => table.AddHeaderCell(h));
            cells.ForEach(c =>
            {
                foreach (var item in c)
                {
                    table.AddCell(new Cell().Add(new Paragraph(item)));
                }
            });

            return table;
        }

        public static List List(List<string> items, ListNumberingType type = ListNumberingType.DECIMAL)
        {
            List list = new List(type);
            items.ForEach(i => list.Add(new ListItem(i)));
            return list;
        }

    }

}
