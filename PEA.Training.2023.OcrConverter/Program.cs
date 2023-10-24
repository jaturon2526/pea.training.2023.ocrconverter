using iText.Kernel.Pdf;
using iText.Pdfocr;
using iText.Pdfocr.Tesseract4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA.Training._2023.OcrConverter
{
    internal class Program
    {
        private static readonly Tesseract4OcrEngineProperties tesseract4OcrEngineProperties = new Tesseract4OcrEngineProperties();
        private static readonly string TESS_DATA_FOLDER = @"..\..\resources\tesseract data";
        private static string OUTPUT_PDF = @"..\..\resources\output\output.pdf";
        private const string DEFAULT_RGB_COLOR_PROFILE_PATH = @"..\..\profiles\sRGB_CS_profile.icm";
        private static readonly IList<FileInfo> LIST_IMAGES_OCR = new List<FileInfo>
    {
        new FileInfo(@"..\..\resources\input\input.jpg")
    };
        static void Main(string[] args)
        {
            try
            {
                var tesseractReader = new Tesseract4LibOcrEngine(tesseract4OcrEngineProperties);
                tesseract4OcrEngineProperties.SetPathToTessData(new FileInfo(TESS_DATA_FOLDER));

                var properties = new OcrPdfCreatorProperties();
                properties.SetPdfLang("th"); //we need to define a language to make it PDF/A compliant

                var ocrPdfCreator = new OcrPdfCreator(tesseractReader, properties);
                Directory.CreateDirectory(Path.GetDirectoryName(OUTPUT_PDF));

                using (var writer = new PdfWriter(OUTPUT_PDF))
                {
                    ocrPdfCreator.CreatePdfA(LIST_IMAGES_OCR, writer, GetRgbPdfOutputIntent()).Close();
                }

                Console.WriteLine("Convert is success");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }
        static PdfOutputIntent GetRgbPdfOutputIntent()
        {
            Stream @is = new FileStream(DEFAULT_RGB_COLOR_PROFILE_PATH, FileMode.Open, FileAccess.Read);
            return new PdfOutputIntent("", "", "", "sRGB IEC61966-2.1", @is);
        }
    }
}
