using System;
using System.IO;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace pdf2text
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ERROR: To continue you need the path to the folder containing the pdf files...");
                return;
            }


            DirectoryInfo dir = new DirectoryInfo(args[0]);
            if (!dir.Exists)
            {
                Console.WriteLine($"ERROR: Direcoty {args[0]} not exists");
                return;
            }


            FileInfo[] listPdfFiles = dir.GetFiles("*.pdf", SearchOption.AllDirectories);
            if (listPdfFiles.Length == 0)
            {
                Console.WriteLine($"WARNING: Not found pdf files");
                return;
            }

            string outputDirPath = System.IO.Path.Combine(args[0], "output");
            if (!Directory.Exists(outputDirPath)) Directory.CreateDirectory(outputDirPath);

            foreach (FileInfo pdf in listPdfFiles)
            {
                Console.WriteLine($"Converting file: {pdf.Name}");
                try
                {
                    string textPlain = GetTextFromAllPages(pdf.FullName);
                    string outputPath = System.IO.Path.Combine(outputDirPath, pdf.Name.Replace(pdf.Extension, "") + ".txt");
                    File.WriteAllText(outputPath, textPlain);
                }
                catch (Exception e)
                {

                    Console.WriteLine($"\t\tError: {e.Message}");
                    throw;
                }
            }


            Console.WriteLine("Complete");
        }


        /// <summary>
        /// Return all text from file pdf
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        public static string GetTextFromAllPages(string pdfPath)
        {
            StringWriter output = new();
            PdfDocument document = PdfDocument.Open(pdfPath);

            foreach (Page page in document.GetPages())
            {
                output.WriteLine(page.Text);
            }

            return output.ToString();
        }

    }//end class: Program
}

