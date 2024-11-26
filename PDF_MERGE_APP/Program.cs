using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PDF_MERGE_APP
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceFolder = @"C:\PDFLER"; // PDF'lerin bulunduğu klasör yolu
            string outputFilePath = @"C:\PDFLER\BirlestirilmisPDF.pdf"; // Birleştirilmiş PDF için dosya yolu

            try
            {
                MergePDFs(sourceFolder, outputFilePath);
                Console.WriteLine("PDF dosyaları başarıyla birleştirildi!");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
            }
        }

        static void MergePDFs(string folderPath, string outputFilePath)
        {
            // Tüm PDF dosyalarını al
            var pdfFiles = Directory.GetFiles(folderPath, "*.pdf");
            if (pdfFiles.Length == 0)
            {
                Console.WriteLine("Klasörde PDF dosyası bulunamadı.");
                
                return;
              
            }

            FileStream stream = null;
            Document document = null;
            PdfCopy pdfCopy = null;
            try
            {
                stream = new FileStream(outputFilePath, FileMode.Create);
                document = new Document();
                pdfCopy = new PdfCopy(document, stream);
                document.Open();

                foreach (string file in pdfFiles)
                {
                    PdfReader pdfReader = null;
                    try
                    {
                        pdfReader = new PdfReader(file);
                        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                        {
                            pdfCopy.AddPage(pdfCopy.GetImportedPage(pdfReader, page));
                        }
                    }
                    finally
                    {
                        pdfReader?.Close();
                    }
                }
            }
            finally
            {
                // Nesneleri manuel olarak kapatıyoruz
                document?.Close();
                pdfCopy?.Close();
                stream?.Close();
            }
           
        }
        
    }
}
