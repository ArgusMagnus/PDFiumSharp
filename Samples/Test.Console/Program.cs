using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFiumSharp;
using System.IO;
using System.Security.Cryptography;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var doc = new PdfDocument("TestDoc.pdf", "password"))
            {
                int i = 0;
                foreach (var page in doc.Pages)
                {
                    using (var bitmap = new PDFiumBitmap((int)page.Width, (int)page.Height, true))
                    using (var stream = new FileStream($"{i++}.bmp", FileMode.Create))
                    {
                        page.Render(bitmap);
                        bitmap.Save(stream);
                    }
                }
            }
            TestSplit();
            TestMerge();
            Console.ReadKey();
        }

        static void TestSplit()
        {
            using (var doc = new PdfDocument("TestDoc.pdf", "password"))
            {
                if (!Directory.Exists("TestDoc"))
                {
                    Directory.CreateDirectory("TestDoc");
                }
                for (global::System.Int32 i = 0; i < doc.Pages.Count; i++)
                {
                    var num = i + 1;
                    using (var stream = PdfSupport.GetPDFPage(doc, num))
                    {
                        File.WriteAllBytes($@"TestDoc\{num}.pdf", stream.ToArray());
                    }   
                }
            }
        }

        static void TestMerge()
        {
            var files = Directory.GetFiles("TestDoc", "*.pdf")?.ToList().
                ConvertAll(file => new PdfDocument(file));
            using (var stream = PdfSupport.MergePDF(files.ToArray()))
            {
                File.WriteAllBytes("TestDocNew.pdf", stream.ToArray());
            }
        }
    }
}
