using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manifest.Common.Models.LetterTemplateParams;
using Manifest.Service.App;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using OpenXmlPowerTools;
using FileUtils = Framework.Infrastructure.Utils.FileUtils;
using HorizontalAnchorValues = DocumentFormat.OpenXml.Vml.Wordprocessing.HorizontalAnchorValues;
using Lock = DocumentFormat.OpenXml.Vml.Office.Lock;
using Shape = DocumentFormat.OpenXml.Vml.Shape;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using V = DocumentFormat.OpenXml.Vml;
using VerticalAnchorValues = DocumentFormat.OpenXml.Vml.Wordprocessing.VerticalAnchorValues;

namespace Manifest.Service.Utils
{
    public class WordService
    {
        protected readonly ILog log;
        protected readonly DocumentConverterService documentConverterService;

        public WordService(ILog log, DocumentConverterService documentConverterService)
        {
            this.log = log;
            this.documentConverterService = documentConverterService;
        }

        public WordService()
        {
        }

        public bool IsWordFileValid(string fileName)
        {
            try
            {
                GetWordXMLFromAWordFile(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetWordXMLFromAWordFile(string fileName)
        {
            //docx to xml
            using (var wordprocessingDocument = WordprocessingDocument.Open(fileName, true))
            {
                return wordprocessingDocument.ToFlatOpcString();
            }
        }

        public bool IsWordFileStreamValid(Stream fileStream)
        {
            try
            {
                if (fileStream == null)
                    return false;
                GetWordXMLFromAWordFileStream(fileStream);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetWordXMLFromAWordFileStream(Stream fileStream)
        {
            //docx to xml
            fileStream.Position = 0;

            using (var wordprocessingDocument = WordprocessingDocument.Open(fileStream, false))
            {
                return wordprocessingDocument.ToFlatOpcString();
            }
        }

        public bool SaveWordXMLToWordFile(string wordXMLContent, string fileName)
        {
            //xml to docx
            using (var wordprocessingDocument = WordprocessingDocument.FromFlatOpcString(wordXMLContent))
            {
                wordprocessingDocument.Clone(fileName);
            }

            return true;
        }

        public bool IsWordXMLStringValid(string wordXMLContent)
        {
            try
            {
                using (var wordprocessingDocument = WordprocessingDocument.FromFlatOpcString(wordXMLContent))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool SaveWordXMLToWordFileStream(string wordXMLContent, Stream fileStream)
        {
            //xml to docx
            fileStream.Position = 0;
            using (var wordprocessingDocument = WordprocessingDocument.FromFlatOpcString(wordXMLContent))
            {
                wordprocessingDocument.Clone(fileStream, false);
            }

            return true;
        }

        public string GetWordDocFileAsHtml(string fileName)
        {
            using (var wordprocessingDocument = WordprocessingDocument.Open(fileName, false))
            {
                var settings = new HtmlConverterSettings();
                var html = HtmlConverter.ConvertToHtml(wordprocessingDocument, settings);
                return html.ToStringNewLineOnAttributes();
            }
        }

        public WordprocessingDocument CreateDocument(string textContent)
        {
            WordprocessingDocument wordDocument;

            using (var memoryStream = new MemoryStream())
            {
                wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document, true);
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
                new Document(new Body()).Save(mainPart);
                Body body = mainPart.Document.Body;
                body.Append(new Paragraph(new Run(new Text(textContent))));
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"
                mainPart.Document.Save();
            }

            return wordDocument;
        }

        public WordprocessingDocument CreateDocument(List<OpenXmlElement> content)
        {
            using (var memoryStream = new MemoryStream())
            {
                WordprocessingDocument document = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document);
                MainDocumentPart mainPart = document.AddMainDocumentPart();
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
                mainPart.Document = new Document(new Body());
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"

                foreach (var element in content)
                {
                    mainPart.Document.Body.AddChild(element.CloneNode(true));
                }

                document.Save();

                return document;
            }
        }

        public MemoryStream CreateDocumentAsMemoryStream(string textContent)
        {
            var mem = new MemoryStream();
            using (var wordDocument = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
                new Document(new Body()).Save(mainPart);
                Body body = mainPart.Document.Body;
                body.Append(new Paragraph(new Run(new Text(textContent))));
                mainPart.Document.Save();
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"
            }

            mem.Position = 0;
            return mem;
        }

        public ReturnModel<string> GetDocumentContentFromWordXML(string wordXML)
        {
            if (!IsWordXMLStringValid(wordXML))
            {
                return log.ErrorAndReturnModel<string>("Invalid word XML string");
            }

            using (var wordprocessingDocument = WordprocessingDocument.FromFlatOpcString(wordXML))
            {
                return ReturnModel<string>.Success(GetDocumentContent(wordprocessingDocument));
            }
        }

        public ReturnModel<string> GetDocumentContentFromWordStream(Stream fileStream, bool isEditable = true)
        {
            if (!IsWordFileStreamValid(fileStream))
            {
                return log.ErrorAndReturnModel<string>("Invalid word file");
            }

            using (var wordprocessingDocument = WordprocessingDocument.Open(fileStream, isEditable))
            {
                return ReturnModel<string>.Success(GetDocumentContent(wordprocessingDocument));
            }
        }

        public bool AddDraftWatermark(Stream fileStream, bool addImageWatermark = false)
        {
            if (!IsWordFileStreamValid(fileStream))
                throw new Exception("Invalid word file");

            fileStream.Position = 0;

            using (var wordprocessingDocument = WordprocessingDocument.Open(fileStream, true))
            {
                if (addImageWatermark)
                {
                    return AddImageWatermark(wordprocessingDocument);
                }
                else
                {
                    return AddWatermark(wordprocessingDocument, "DRAFT");
                }
            }
        }

        public bool AddDraftWatermark(string filePath, bool addImageWatermark = false)
        {
            if (!IsWordFileValid(filePath))
                throw new Exception("Invalid word file");

            using (var wordprocessingDocument = WordprocessingDocument.Open(filePath, true))
            {
                if (addImageWatermark)
                {
                    return AddImageWatermark(wordprocessingDocument);
                }
                else
                {
                    return AddWatermark(wordprocessingDocument, "DRAFT");
                }
            }
        }

        public bool RemoveWatermark(Stream fileStream)
        {
            if (!IsWordFileStreamValid(fileStream))
                throw new Exception("Invalid word file");

            fileStream.Position = 0;

            using (var wordprocessingDocument = WordprocessingDocument.Open(fileStream, true))
            {
                return RemoveWatermark(wordprocessingDocument);
            }
        }

        public bool RemoveWatermark(string filePath)
        {
            if (!IsWordFileValid(filePath))
                throw new Exception("Invalid word file");

            using (var wordprocessingDocument = WordprocessingDocument.Open(filePath, true))
            {
                return RemoveWatermark(wordprocessingDocument);
            }
        }

        public async Task<Stream> ReplaceTexts(Stream stream, PlaceHolders placeholders)
        {
            try
            {
                if (placeholders != null)
                {
                    if (stream == null)
                        return null;

                    using (var doc = WordprocessingDocument.Open(stream, true))
                    {
                        //CleanMarkup(doc);

                        var documentTexts = doc.MainDocumentPart.Document.Descendants<Text>();
                        var headerTexts = doc.MainDocumentPart.HeaderParts.SelectMany(h => h.Header.Descendants<Text>());
                        var footerTexts = doc.MainDocumentPart.FooterParts.SelectMany(f => f.Footer.Descendants<Text>());

                        var texts = documentTexts.Concat(headerTexts).Concat(footerTexts);

                        var prevText = new Text();

                        if (texts != null)
                        {
                            foreach (var text in texts)
                            {
                                if (prevText != null && prevText.Text != null && text != null && text.Text != null && prevText.Text.Trim().EndsWith(PlaceHolders.StartTag))
                                {
                                    var origText = prevText.Text;
                                    var pos = origText.LastIndexOf(PlaceHolders.StartTag);

                                    if (pos == 0)
                                    {
                                        prevText.Text = "";
                                    }
                                    else
                                    {
                                        prevText.Text = origText.Substring(0, pos - 1);
                                    }

                                    text.Text = origText.Substring(pos, origText.Length - pos).AppendWith(text.Text);
                                }

                                if (prevText != null && prevText.Text != null && text != null && text.Text != null && text.Text.Trim().StartsWith(PlaceHolders.EndTag))
                                {
                                    var origText = text.Text;
                                    var pos = text.Text.IndexOf(PlaceHolders.EndTag);

                                    if (pos == origText.Length - 1)
                                    {
                                        text.Text = "";
                                    }
                                    else
                                    {
                                        text.Text = origText.Substring(pos + 1, origText.Length - pos - 1);
                                    }

                                    prevText.Text = prevText.Text.AppendWith(origText.Substring(0, pos + 1));
                                }

                                prevText = text;
                            }

                            foreach (var text in texts)
                            {
                                foreach (var replace in placeholders.TextPlaceholders)
                                {
                                    if (text.Text.Contains(PlaceHolders.StartTag + replace.Key + PlaceHolders.EndTag))
                                    {
                                        if (!string.IsNullOrEmpty(replace.Value) && replace.Value.Contains(PlaceHolders.NewLineTag))
                                        {
                                            string[] repArray = replace.Value.Split(new string[] { PlaceHolders.NewLineTag }, StringSplitOptions.None);

                                            var lastInsertedText = text;
                                            Break lastInsertedBreak;

                                            for (var i = 0; i < repArray.Length; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    text.Text = text.Text.Replace(PlaceHolders.StartTag + replace.Key + PlaceHolders.EndTag, repArray[i]);
                                                }
                                                else
                                                {
                                                    var tmpText = new Text(repArray[i]);
                                                    var tmpBreak = new Break();
                                                    text.Parent.InsertAfter(tmpBreak, lastInsertedText);
                                                    lastInsertedBreak = tmpBreak;
                                                    text.Parent.InsertAfter(tmpText, lastInsertedBreak);
                                                    lastInsertedText = tmpText;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            text.Text = text.Text.Replace(PlaceHolders.StartTag + replace.Key + PlaceHolders.EndTag, replace.Value ?? string.Empty);
                                        }
                                    }
                                }

                                foreach (var replaceTable in placeholders.TablePlaceholders)
                                {
                                    if (text.Text.Contains(PlaceHolders.StartTag + replaceTable.Key + PlaceHolders.EndTag))
                                    {
                                        if (replaceTable.Value == null || replaceTable.Value.RowData.IsNullOrEmpty() || replaceTable.Value.RowData.Count <= 1)
                                        {
                                            text.Text = text.Text.Replace(PlaceHolders.StartTag + replaceTable.Key + PlaceHolders.EndTag, "<NO DATA AVAILABLE>");
                                        }
                                        else
                                        {
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
                                            var table = new Table();
                                            var tableProp = new TableProperties();

                                            var tableStyle = new TableStyle() { Val = "TableGrid" };
                                            var tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
                                            var tableBorders = new TableBorders(
                                                new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 10, Space = 0, Color = "000000" },
                                                new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 10, Space = 0, Color = "000000" },
                                                new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 10, Space = 0, Color = "000000" },
                                                new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 10, Space = 0, Color = "000000" },
                                                new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 10, Space = 0, Color = "000000" },
                                                new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 10, Space = 0, Color = "000000" });
                                            tableProp.Append(tableStyle, tableWidth, tableBorders);
                                            table.AppendChild(tableProp);

                                            var isFirstRow = true;

                                            foreach (var row in replaceTable.Value.RowData)
                                            {
                                                var tableRow = new TableRow();
                                                foreach (var col in row.Split(PlaceHolders.TableRowColumnSplitTag))
                                                {
                                                    tableRow.Append(CreateTableCell(col, bold: isFirstRow));
                                                }

                                                table.AppendChild(tableRow);
                                                isFirstRow = false;
                                            }

                                            text.Text = text.Text.Replace(PlaceHolders.StartTag + replaceTable.Key + PlaceHolders.EndTag, string.Empty);
                                            text.Ancestors<Paragraph>().First().Parent.InsertAfter(table, text.Ancestors<Paragraph>().First());
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                stream.Position = 0;
                return await Task.FromResult(stream);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

#pragma warning disable S1144 // Unused private types or members should be removed
        private string ConvertDocumentToHtml(List<OpenXmlElement> content)
#pragma warning restore S1144 // Unused private types or members should be removed
        {
            var tmpFile = FileUtils.GetRandomFileNameInTempDirectory();
            var tmpDocFile = System.IO.Path.ChangeExtension(tmpFile, ".docx");

            using (var memoryStream = File.Open(tmpDocFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                WordprocessingDocument document = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document);
                MainDocumentPart mainPart = document.AddMainDocumentPart();
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
                mainPart.Document = new Document(new Body());
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"

                foreach (var element in content)
                {
                    mainPart.Document.Body.AddChild(element.CloneNode(true));
                }

                document.Save();
            }

            var anotherTmpFile = FileUtils.GetRandomFileNameInTempDirectory();
            var tmpHtmlFile = System.IO.Path.ChangeExtension(anotherTmpFile, ".html");

            try
            {
                documentConverterService.ConvertDocument(tmpDocFile, tmpHtmlFile);
                return File.ReadAllText(tmpHtmlFile);
            }
            finally
            {
                FileUtils.DeleteFileSafely(tmpFile);
                FileUtils.DeleteFileSafely(tmpDocFile);
                FileUtils.DeleteFileSafely(anotherTmpFile);
                FileUtils.DeleteFileSafely(tmpHtmlFile);
            }
        }

        private string GetDocumentContent(WordprocessingDocument wordprocessingDocument)
        {
            var stringBuilder = new StringBuilder();
            foreach (var element in wordprocessingDocument.MainDocumentPart.Document.Elements())
            {
                stringBuilder.Append(GetXmlElementText(element));
            }

            return stringBuilder.ToString();
        }

        private string GetXmlElementText(OpenXmlElement openXmlElement)
        {
            var text = new StringBuilder();

            foreach (var element in openXmlElement.Elements())
            {
                if (element.GetType() == typeof(Text))
                {
                    text.Append(element.InnerText);
                }
                else if (element.GetType() == typeof(Break))
                {
                    text.Append(Environment.NewLine);
                }
                else if (element.GetType() == typeof(OpenXmlUnknownElement))
                {
                    if (element.InnerText != null && element.InnerText.Length != 0)
                    {
                        text.Append(Environment.NewLine + element.InnerText);
                    }
                }
                else
                {
                    if (element.GetType() == typeof(Paragraph) && text.Length != 0)
                    {
                        text.Append(Environment.NewLine);
                    }

                    text.Append(GetXmlElementText(element));
                }
            }

            return text.ToString();
        }

        private IEnumerable<OpenXmlElement> ContentControls(OpenXmlPart part)
        {
            return part.RootElement.Descendants().Where(e => e is SdtBlock || e is SdtRun || e is SdtCell);
        }

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable S1144 // Unused private types or members should be removed
        private IEnumerable<OpenXmlElement> ContentControls(WordprocessingDocument doc)
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning restore IDE0051 // Remove unused private members
        {
            foreach (var cc in ContentControls(doc.MainDocumentPart))
                yield return cc;
            foreach (var header in doc.MainDocumentPart.HeaderParts)
                foreach (var cc in ContentControls(header))
                    yield return cc;
            foreach (var footer in doc.MainDocumentPart.FooterParts)
                foreach (var cc in ContentControls(footer))
                    yield return cc;
            if (doc.MainDocumentPart.FootnotesPart != null)
                foreach (var cc in ContentControls(doc.MainDocumentPart.FootnotesPart))
                    yield return cc;
            if (doc.MainDocumentPart.EndnotesPart != null)
                foreach (var cc in ContentControls(doc.MainDocumentPart.EndnotesPart))
                    yield return cc;
        }

        private bool RemoveWatermark(WordprocessingDocument wordprocessingDocument)
        {
            try
            {
                var watermarkPart = GetWatermarkPart(wordprocessingDocument);
                if (watermarkPart == null)
                {
                    return false;
                }

                var header = watermarkPart.Ancestors<Header>().First();

                var watermarkSdtBlock = watermarkPart.Ancestors<SdtBlock>().FirstOrDefault();
                if (watermarkSdtBlock != null)
                {
                    watermarkSdtBlock.Remove();
                }

                var watermarkSdtRun = watermarkPart.Ancestors<SdtRun>().FirstOrDefault();
                if (watermarkSdtRun != null)
                {
                    watermarkSdtRun.Remove();
                }

                var watermarkPara = watermarkPart.Ancestors<Paragraph>().FirstOrDefault();
                if (watermarkPara != null && watermarkPara.InnerText == "")
                {
                    watermarkPara.Remove();
                }

                header.HeaderPart.DeletePart("roppwId999");
                header.Save();

                if (!header.HasChildren)
                {
                    var rId = wordprocessingDocument.MainDocumentPart.GetIdOfPart(header.HeaderPart);
                    var headerRef = wordprocessingDocument.MainDocumentPart.Document.Body.Descendants<HeaderReference>().FirstOrDefault(x => x.Id == rId);

                    if (headerRef != null)
                    {
                        var sectionProp = headerRef.Ancestors<SectionProperties>().First();
                        if (sectionProp != null)
                        {
                            sectionProp.RemoveChild(headerRef);
                        }
                    }

                    wordprocessingDocument.MainDocumentPart.DeletePart(header.HeaderPart);
                }
            }
            catch
            {
                //This is not needed as per @sing.
            }

            wordprocessingDocument.Save();
            return true;
        }

        private Shape GetWatermarkPart(WordprocessingDocument doc)
        {
            try
            {
                if (doc.MainDocumentPart.HeaderParts.Any())
                {
                    foreach (var watermarkPart in doc.MainDocumentPart.HeaderParts.SelectMany(x => x.Header.Descendants<Shape>()))
                    {
                        if (watermarkPart != null && (watermarkPart.Id == "WordPictureWatermark75517470" || watermarkPart.Id == "PowerPlusWaterMarkObject357831064"))
                        {
                            return watermarkPart;
                        }

                        if (watermarkPart != null && (watermarkPart.Id == "PowerPlusWaterMarkObject357476642" || (watermarkPart.Alternate != null && watermarkPart.Alternate == "PowerPlusWaterMarkObject357831064")))
                        {
                            watermarkPart.Id = "PowerPlusWaterMarkObject357831064";
                            doc.Save();

                            return watermarkPart;
                        }
                    }
                }
            }
            catch
            {
                //This is not needed as per @sing.
            }

#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
        }

        private bool AddWatermark(WordprocessingDocument wordprocessingDocument, string watermarkText)
        {
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
            var watermarkPart = GetWatermarkPart(wordprocessingDocument);
            if (watermarkPart != null)
            {
                return true;
            }

            var headerPart = wordprocessingDocument.MainDocumentPart.AddNewPart<HeaderPart>();
            var rId = wordprocessingDocument.MainDocumentPart.GetIdOfPart(headerPart);
            var headerRef = new HeaderReference();
            headerRef.Id = rId;
            var sectionProps = wordprocessingDocument.MainDocumentPart.Document.Body.Elements<SectionProperties>().LastOrDefault();
            if (sectionProps == null)
            {
                sectionProps = new SectionProperties();
                wordprocessingDocument.MainDocumentPart.Document.Body.Append(sectionProps);
            }

            sectionProps.Append(headerRef);

            headerPart.Header = new Header();
            headerPart.Header.Save();

            var sdtBlock1 = new SdtBlock();
            var sdtProperties1 = new SdtProperties(new SdtId() { Val = 87908844 }, new SdtContentDocPartObject(new DocPartGallery() { Val = "Watermarks" }, new DocPartUnique()));

            var sdtContentBlock1 = new SdtContentBlock();
            var paragraph2 = new Paragraph()
            {
                RsidParagraphAddition = "00656E18",
                RsidRunAdditionDefault = "00656E18"
            };
            var paragraphProperties2 = new ParagraphProperties(new ParagraphStyleId() { Val = "Header" });
            var run1 = new Run();
            var runProperties1 = new RunProperties(new NoProof(), new Languages() { EastAsia = "zh-TW" });
            var picture1 = new Picture();

            var shapetype1 = new Shapetype()
            {
                Id = "_x0000_t136",
                CoordinateSize = "21600,21600",
                OptionalNumber = 136,
                Adjustment = "10800",
                EdgePath = "m@7,l@8,m@5,21600l@6,21600e"
            };

            var formulas1 = new Formulas();
            var formula1 = new Formula() { Equation = "sum #0 0 10800" };
            var formula2 = new Formula() { Equation = "prod #0 2 1" };
            var formula3 = new Formula() { Equation = "sum 21600 0 @1" };
            var formula4 = new Formula() { Equation = "sum 0 0 @2" };
            var formula5 = new Formula() { Equation = "sum 21600 0 @3" };
            var formula6 = new Formula() { Equation = "if @0 @3 0" };
            var formula7 = new Formula() { Equation = "if @0 21600 @1" };
            var formula8 = new Formula() { Equation = "if @0 0 @2" };
            var formula9 = new Formula() { Equation = "if @0 @4 21600" };
            var formula10 = new Formula() { Equation = "mid @5 @6" };
            var formula11 = new Formula() { Equation = "mid @8 @5" };
            var formula12 = new Formula() { Equation = "mid @7 @8" };
            var formula13 = new Formula() { Equation = "mid @6 @7" };
            var formula14 = new Formula() { Equation = "sum @6 0 @5" };

            formulas1.Append(formula1, formula2, formula3, formula4, formula5, formula6, formula7, formula8, formula9, formula10, formula11, formula12, formula13, formula14);
            var path1 = new DocumentFormat.OpenXml.Vml.Path()
            {
                AllowTextPath = true,
                ConnectionPointType = V.Office.ConnectValues.Custom,
                ConnectionPoints = "@9,0;@10,10800;@11,21600;@12,10800",
                ConnectAngles = "270,180,90,0"
            };
            var textPath1 = new TextPath()
            {
                On = true,
                FitShape = true
            };
            var shapeHandles1 = new ShapeHandles();

            var shapeHandle1 = new ShapeHandle()
            {
                Position = "#0,bottomRight",
                XRange = "6629,14971"
            };

            shapeHandles1.Append(shapeHandle1);

            var lock1 = new Lock
            {
                Extension = ExtensionHandlingBehaviorValues.Edit,
                TextLock = true,
                ShapeType = true
            };

            shapetype1.Append(formulas1);
            shapetype1.Append(path1);
            shapetype1.Append(textPath1);
            shapetype1.Append(shapeHandles1);
            shapetype1.Append(lock1);
            var shape1 = new Shape()
            {
                Id = "PowerPlusWaterMarkObject357831064",
                Style = "position:absolute;left:0;text-align:left;margin-left:0;margin-top:0;width:527.85pt;height:131.95pt;rotation:315;z-index:-251656192;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin",
                OptionalString = "_x0000_s2049",
                AllowInCell = false,
                FillColor = "silver",
                Stroked = false,
                Type = "#_x0000_t136",
                Alternate = "PowerPlusWaterMarkObject357831064"
            };

            var fill1 = new Fill() { Opacity = ".5" };
            var textPath2 = new TextPath()
            {
                Style = "font-family:\"Calibri\";font-size:1pt",
                String = watermarkText
            };

            var textWrap1 = new V.Wordprocessing.TextWrap()
            {
                AnchorX = HorizontalAnchorValues.Margin,
                AnchorY = VerticalAnchorValues.Margin
            };

            shape1.Append(fill1);
            shape1.Append(textPath2);
            shape1.Append(textWrap1);
            picture1.Append(shapetype1);
            picture1.Append(shape1);
            run1.Append(runProperties1);
            run1.Append(picture1);
            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run1);
            sdtContentBlock1.Append(paragraph2);
            sdtBlock1.Append(sdtProperties1);
            sdtBlock1.Append(sdtContentBlock1);
            headerPart.Header.Append(sdtBlock1);
            headerPart.Header.Save();

            wordprocessingDocument.Save();
            return true;
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"
        }

        private bool AddImageWatermark(WordprocessingDocument wordprocessingDocument)
        {
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
            var watermarkPart = GetWatermarkPart(wordprocessingDocument);
            if (watermarkPart != null)
            {
                return true;
            }

            var headerPart = wordprocessingDocument.MainDocumentPart.AddNewPart<HeaderPart>();
            var rId = wordprocessingDocument.MainDocumentPart.GetIdOfPart(headerPart);
            var headerRef = new HeaderReference();
            headerRef.Id = rId;
            var sectionProps = wordprocessingDocument.MainDocumentPart.Document.Body.Elements<SectionProperties>().LastOrDefault();
            if (sectionProps == null)
            {
                sectionProps = new SectionProperties();
                wordprocessingDocument.MainDocumentPart.Document.Body.Append(sectionProps);
            }

            sectionProps.Append(headerRef);

            headerPart.Header = new Header();
            headerPart.Header.Save();

            var watermarkPara = new Paragraph();
            var watermarkRun = new Run();
            var watermarkPicture = new Picture();
            var watermarShape = new Shape()
            {
                Id = "WordPictureWatermark75517470",
                Style = "position:absolute;left:0;text-align:left;margin-left:0;margin-top:0;width:415.2pt;height:456.15pt;z-index:-251656192;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin",
                OptionalString = "_x0000_s2051",
                AllowInCell = false,
                Type = "#_x0000_t75"
            };

            var watermarkImageData = new ImageData()
            {
                Gain = "19661f",
                BlackLevel = "22938f",
                Title = "??",
                RelationshipId = "roppwId999"
            };

            watermarShape.Append(watermarkImageData);
            watermarkPicture.Append(watermarShape);
            watermarkRun.Append(watermarkPicture);
            watermarkPara.Append(watermarkRun);
            headerPart.Header.Append(watermarkPara);
            headerPart.Header.Save();

            var watermarkImagePart = headerPart.AddNewPart<ImagePart>("image/jpeg", "roppwId999");
            var imageString = "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";
            var data = new MemoryStream(Convert.FromBase64String(imageString));
            watermarkImagePart.FeedData(data);
            data.Close();

            wordprocessingDocument.Save();
            return true;
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"
        }

#pragma warning disable S1144 // Unused private types or members should be removed
        private TableCell CreateTableCell(string cellText = "", bool bold = false, bool underline = false)
#pragma warning restore S1144 // Unused private types or members should be removed
        {
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
            var tableCell = new TableCell();

            tableCell.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" },
                new TableCellVerticalAlignment() { Val = new EnumValue<TableVerticalAlignmentValues>(TableVerticalAlignmentValues.Center) }));

            tableCell.Append(CreateParagraph(cellText, bold, underline));

            return tableCell;
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"
        }

        private Paragraph CreateParagraph(string text = "", bool bold = false, bool underline = false)
        {
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
            var run = new Run();

            if (bold && underline)
            {
                run.Append(new RunProperties(new Bold(), new Underline()));
            }
            else if (bold)
            {
                run.Append(new RunProperties(new Bold()));
            }
            else if (underline)
            {
                run.Append(new RunProperties(new Underline()));
            }

            if (text == null)
            {
                text = "";
            }

            string[] textArray = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var first = true;

            foreach (string line in textArray)
            {
                if (!first)
                {
                    run.Append(new Break());
                }

                first = false;
                run.Append(new Text(line));
            }

            return new Paragraph(run);
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"
        }
    }
}