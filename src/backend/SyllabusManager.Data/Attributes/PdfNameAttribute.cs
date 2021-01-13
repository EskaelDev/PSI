using System;
using System.Collections.Generic;
using System.Text;

namespace SyllabusManager.Data.Attributes
{

    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Enum | System.AttributeTargets.Field)]
    public class PdfNameAttribute : System.Attribute
    {
        public string name;

        public PdfNameAttribute(string name)
        {
            this.name = name;
        }
    }
}
