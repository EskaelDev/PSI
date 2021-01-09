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
        public static string PATH => Directory.GetCurrentDirectory() + "/temp.pdf";
        public static Document Document(bool horizontal = false)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(PATH, FileMode.Create, FileAccess.Write)));

            if (horizontal) { pdfDocument.SetDefaultPageSize(pdfDocument.GetDefaultPageSize().Rotate()); }

            Document doc = new Document(pdfDocument);
            doc.SetFont(PdfHelper.FONT);
            return doc;
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


        public static void test()
        {
            //https://www.c-sharpcorner.com/blogs/create-table-in-pdf-using-c-sharp-and-itextsharp
            //PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(Directory.GetCurrentDirectory() + "/temp.pdf", FileMode.Create, FileAccess.Write)));
            //Document document = new Document(pdfDocument);
            using (Document document = Document())
            {
                string line = "Super fajny pdf z rzeczami";
                document.Add(new Paragraph(line));
                List<string> h = new List<string>() { "Kod", "Rok", "Semestr", "Wydział" };
                List<string[]> c = new List<string[]>() { };
                List<List<string>> s = new List<List<string>>();
                //s.Add({ "KD/232", "2018", "II", "Telekomunikacji" });
                //c.Add(s);
                //s = new string[] { "XCZ-39", "2077", "VI", "Informatyki" };
                //c.Add(s);
                //Table table = Table(h, c);

                line = "ryba po grecku";
                document.Add(new Paragraph(line));
                h = new List<string>() { "Pierwszy punkt", "Drugi punkt", "Halo" };
                List list = List(h, ListNumberingType.GREEK_UPPER);
                document.Add(list);

                h = new List<string>() { "Pieczarki", "Jajka", "Plastelina" };
                list = List(h);

                list.Add(new ListItem(""));
                //document.Add(table);
                document.Add(list);
                document.Add(new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));
            }
            //string line = "Super fajny pdf z rzeczami";
            //document.Add(new Paragraph(line));
            //List<string> h = new List<string>() { "Kod", "Rok", "Semestr", "Wydział" };
            //List<string[]> c = new List<string[]>(){};
            //string[] s = new string[] { "KD/232", "2018", "II", "Telekomunikacji" };
            //c.Add(s);
            //s = new string[] { "XCZ-39", "2077", "VI", "Informatyki" };
            //c.Add(s);
            //Table table = Table(h, c);

            //line = "ryba po grecku";
            //document.Add(new Paragraph(line));
            //h = new List<string>() { "Pierwszy punkt", "Drugi punkt", "Halo" };
            //List list = List(h, ListNumberingType.GREEK_UPPER);
            //document.Add(list);

            //h = new List<string>() { "Pieczarki", "Jajka", "Plastelina" };
            //list = List(h);

            //list.Add(new ListItem(""));
            //document.Add(table);
            //document.Add(list);
            //document.Add(new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));
            //document.Close();
            Console.WriteLine("Awesome PDF just got created.");
        }
    }

}
