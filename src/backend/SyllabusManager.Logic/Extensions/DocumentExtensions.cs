using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyllabusManager.Logic.Extensions
{
    public static class DocumentExtensions
    {
        public static Paragraph Paragraph(this Document document, string text)
        {
            var p = new Paragraph(text);
            document.Add(p);
            return p;
        }
    }
}
