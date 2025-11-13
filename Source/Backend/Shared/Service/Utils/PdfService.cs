using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Framework.Infrastructure.Logging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;
using iTextSharpPdf = iTextSharp.text.pdf;

namespace Manifest.Service.Utils
{
    public class PdfService
    {
        protected readonly ILog log;

        public PdfService(ILog log)
        {
            this.log = log;
        }

        public int GetNumberOfPages(Stream pdfStream)
        {
            using (PdfDocument document = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import))
            {
                return document.Pages.Count;
            }
        }

        public int? GetPageNumberOfFirstOccurence(Stream pdfStream, List<string> searchTexts)
        {
            if (pdfStream == null)
                return null;

            try
            {
                using (PdfDocument document = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import))
                {
                    return GetPageNumberFromTexts(document, searchTexts);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public void AddWatermark(string pdfInputFilePath, string pdfOutputFilePath)
        {
            iTextSharpPdf.PdfReader pdfReader = null;
            iTextSharpPdf.PdfStamper pdfStamper = null;

            try
            {
                pdfReader = new iTextSharpPdf.PdfReader(pdfInputFilePath);

                var numberOfPages = pdfReader.NumberOfPages;
                var psize = pdfReader.GetPageSize(1);
                var width = psize.Width;
                var height = psize.Height;

                using (var outputStream = new FileStream(pdfOutputFilePath, FileMode.Create))
                {
                    pdfStamper = new iTextSharpPdf.PdfStamper(pdfReader, outputStream);

                    iTextSharpPdf.PdfContentByte waterMarkContent;

                    var imageString = "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";
                    var image = iTextSharp.text.Image.GetInstance(Convert.FromBase64String(imageString));

                    image.GrayFill = 20;
                    image.SetAbsolutePosition((width - image.Width) / 2, (height - image.Height) / 2);

                    for (int i = 1; i <= numberOfPages; i++)
                    {
                        waterMarkContent = pdfStamper.GetUnderContent(i);
                        waterMarkContent.AddImage(image);
                    }

                    pdfStamper.Close();
                }
            }
            finally
            {
                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
            }
        }

        public int? GetPageNumberFromTexts(PdfDocument document, List<string> searchTexts)
        {
            for (int i = 1; i <= document.Pages.Count; i++)
            {
                var content = ContentReader.ReadContent(document.Pages[i]);
                var pageData = string.Join("", ExtractTextFromPdfSharpContent(content));

                if (searchTexts.Any(x => pageData?.ToLower().Contains(x.ToLower()) ?? false))
                    return i;
            }

            return 0;
        }

        public IEnumerable<string> ExtractTextFromPdfSharpContent(CObject cObject)
        {
            if (cObject is COperator)
            {
                var cOperator = cObject as COperator;
                if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
                    cOperator.OpCode.Name == OpCodeName.TJ.ToString())
                {
                    foreach (var cOperand in cOperator.Operands)
                        foreach (var txt in ExtractTextFromPdfSharpContent(cOperand))
                            yield return txt;
                }
            }
            else if (cObject is CSequence)
            {
                var cSequence = cObject as CSequence;
                foreach (var element in cSequence)
                    foreach (var txt in ExtractTextFromPdfSharpContent(element))
                        yield return txt;
            }
            else if (cObject is CString)
            {
                var cString = cObject as CString;
                yield return cString.Value;
            }
        }

        //public void AddWatermark(string pdfFilePath, string watermarkText = "DRAFT")
        //{
        //    try
        //    {
        //        var document = PdfReader.Open(pdfFilePath, PdfDocumentOpenMode.Modify);
        //        var font = new XFont("Arial", 130, XFontStyle.Regular);

        //        foreach (var page in document.Pages)
        //        {
        //            // Variation 1: Draw a watermark as a text string.
        //            // Get an XGraphics object for drawing beneath the existing content.
        //            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

        //            // Get the size (in points) of the text.
        //            var size = gfx.MeasureString(watermarkText, font);

        //            // Define a rotation transformation at the center of the page.
        //            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
        //            gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
        //            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

        //            // Create a string format.
        //            var format = new XStringFormat();
        //            format.Alignment = XStringAlignment.Near;
        //            format.LineAlignment = XLineAlignment.Near;

        //            // Create a dimmed gray brush.
        //            XBrush brush = new XSolidBrush(XColor.FromArgb(128, 128, 128, 128));

        //            // Draw the string.
        //            gfx.DrawString(watermarkText, font, brush, new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2), format);
        //        }

        //        document.Save(pdfFilePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex, "Error when adding watermark to pdf");
        //        throw;
        //    }
        //}
    }
}
