using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Framework.Infrastructure.Models.DocumentHandlers;
using Framework.Infrastructure.Utils;
using OpenXmlPowerTools;
using A = DocumentFormat.OpenXml.Drawing;
using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace Framework.Infrastructure.Handlers
{
    public class DocXHandler
    {
        private readonly MemoryStream docxMs;
        private readonly Placeholders rep;
        private int imageCounter;

        public DocXHandler(string docXTemplateFilename, Placeholders rep)
        {
            docxMs = Utils.FileUtils.GetFileAsMemoryStream(docXTemplateFilename);
            this.rep = rep;
        }

        public MemoryStream ReplaceAll()
        {
            if (rep != null)
            {
                ReplaceTexts();

                ReplaceHyperlinks();

                ReplaceTableRows();

                ReplaceImages();
            }

            docxMs.Position = 0;

            return docxMs;
        }

        public MemoryStream ReplaceTexts()
        {
            if (rep.TextPlaceholders == null || rep.TextPlaceholders.Count == 0)
                return null;

            using (var doc = WordprocessingDocument.Open(docxMs, true))
            {
                CleanMarkup(doc);

                // Search in body, headers and footers
                var documentTexts = doc.MainDocumentPart.Document.Descendants<Text>();
                var headerTexts = doc.MainDocumentPart.HeaderParts.SelectMany(h => h.Header.Descendants<Text>());
                var footerTexts = doc.MainDocumentPart.FooterParts.SelectMany(f => f.Footer.Descendants<Text>());

                // <<< Here
                foreach (var text in documentTexts.Concat(headerTexts).Concat(footerTexts))
                {
                    foreach (var replace in rep.TextPlaceholders)
                    {
                        if (text.Text.Contains(rep.TextPlaceholderStartTag + replace.Key + rep.TextPlaceholderEndTag))
                        {
                            //If we have line breaks present
                            if (!string.IsNullOrEmpty(replace.Value) && replace.Value.Contains(rep.NewLineTag))
                            {
                                string[] repArray = replace.Value.Split(new string[] { rep.NewLineTag }, StringSplitOptions.None);

                                var lastInsertedText = text;
                                Break lastInsertedBreak;

                                for (var i = 0; i < repArray.Length; i++)
                                {
                                    //The text is only replaced with the first part of the replacement array
                                    if (i == 0)
                                    {
                                        text.Text = text.Text.Replace(rep.TextPlaceholderStartTag + replace.Key + rep.TextPlaceholderEndTag, repArray[i]);
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
                                text.Text = text.Text.Replace(rep.TextPlaceholderStartTag + replace.Key + rep.TextPlaceholderEndTag, replace.Value ?? string.Empty);
                            }
                        }
                    }
                }
            }

            docxMs.Position = 0;
            return docxMs;
        }

        public MemoryStream ReplaceHyperlinks()
        {
            if (rep.HyperlinkPlaceholders == null || rep.HyperlinkPlaceholders.Count == 0)
                return null;

            using (var doc = WordprocessingDocument.Open(docxMs, true))
            {
                CleanMarkup(doc);

                // Search in body, headers and footers
                var documentTexts = doc.MainDocumentPart.Document.Descendants<Text>();

                foreach (var text in documentTexts)
                {
                    foreach (var replace in rep.HyperlinkPlaceholders)
                    {
                        var pl = rep.HyperlinkPlaceholderStartTag + replace.Key + rep.HyperlinkPlaceholderEndTag;
                        if (text.Text.Contains(pl))
                        {
                            var run = text.Ancestors<Run>().First();

                            if (text.Text.StartsWith(pl))
                            {
                                var newAfterRun = (Run)run.Clone();
                                string afterText = text.Text.Substring(pl.Length, text.Text.Length - pl.Length);
                                Text newAfterRunText = newAfterRun.GetFirstChild<Text>();
                                newAfterRunText.Space = SpaceProcessingModeValues.Preserve;
                                newAfterRunText.Text = afterText;

                                run.Parent.InsertAfter(newAfterRun, run);
                            }
                            else if (text.Text.EndsWith(pl))
                            {
                                var newBeforeRun = (Run)run.Clone();
                                string beforeText = text.Text.Substring(0, text.Text.Length - pl.Length);
                                Text newBeforeRunText = newBeforeRun.GetFirstChild<Text>();
                                newBeforeRunText.Space = SpaceProcessingModeValues.Preserve;
                                newBeforeRunText.Text = beforeText;

                                run.Parent.InsertBefore(newBeforeRun, run);
                            }
                            else
                            {
                                //Break the texts into the part before and after image. Then create separate runs for them
                                var pos = text.Text.IndexOf(pl, StringComparison.CurrentCulture);

                                var newBeforeRun = (Run)run.Clone();
                                string beforeText = text.Text.Substring(0, pos);
                                Text newBeforeRunText = newBeforeRun.GetFirstChild<Text>();
                                newBeforeRunText.Space = SpaceProcessingModeValues.Preserve;
                                newBeforeRunText.Text = beforeText;
                                run.Parent.InsertBefore(newBeforeRun, run);

                                var newAfterRun = (Run)run.Clone();
                                string afterText = text.Text.Substring(pos + pl.Length, text.Text.Length - pos - pl.Length);
                                Text newAfterRunText = newAfterRun.GetFirstChild<Text>();
                                newAfterRunText.Space = SpaceProcessingModeValues.Preserve;
                                newAfterRunText.Text = afterText;
                                run.Parent.InsertAfter(newAfterRun, run);
                            }

                            var relation = doc.MainDocumentPart.AddHyperlinkRelationship(new Uri(replace.Value.Link, UriKind.RelativeOrAbsolute), true);
                            string relationid = relation.Id;
                            var linkText = string.IsNullOrEmpty(replace.Value.Text) ? replace.Value.Link : replace.Value.Text;
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
                            var hyper =
                                new Hyperlink(
                                    new Run(
                                        new RunProperties(new RunStyle() { Val = "Hyperlink" }),
                                        new Text(linkText)))
                                {
                                    Id = relationid,
                                    History = OnOffValue.FromBoolean(true)
                                };
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"

                            run.Parent.InsertBefore(hyper, run);
                            run.Remove();
                        }
                    }
                }
            }

            docxMs.Position = 0;
            return docxMs;
        }

        public MemoryStream ReplaceTableRows()
        {
            if (rep.TablePlaceholders == null || rep.TablePlaceholders.Count == 0)
                return null;

            using (var doc = WordprocessingDocument.Open(docxMs, true))
            {
                CleanMarkup(doc);

                //Take a Row (one Dictionary) at a time
                foreach (var trDict in rep.TablePlaceholders)
                {
                    var trCol0 = trDict.First();
                    // Find the text elements matching one of the search strings
                    // where the text is inside a table cell --> these are the rows we are searching for.
                    var textElements =
                        doc.MainDocumentPart.Document.Body.Descendants<Text>()
                        .Concat(doc.MainDocumentPart.HeaderParts.SelectMany(h => h.Header.Descendants<Text>()))
                        .Concat(doc.MainDocumentPart.FooterParts.SelectMany(f => f.Footer.Descendants<Text>()))
                        .Where(t =>
                            trDict.Keys.Select(key => rep.TablePlaceholderStartTag + trCol0.Key + rep.TablePlaceholderEndTag).Any(s => t.Text.Contains(s)) &&
                            t.Ancestors<DocumentFormat.OpenXml.Wordprocessing.TableCell>().Any());

                    // Loop through all found rows
                    foreach (var textElement in textElements)
                    {
                        var newTableRows = new List<TableRow>();
                        var tableRow = textElement.Ancestors<TableRow>().First();

                        //Lets create row by row and replace placeholders
                        for (var j = 0; j < trCol0.Value.Length; j++)
                        {
                            newTableRows.Add((TableRow)tableRow.CloneNode(true));
                            var tableRowCopy = newTableRows[newTableRows.Count - 1];

                            //Cycle through the cells of the row to replace from the Dictionary value ( string array)
                            foreach (var text in tableRow.Descendants<Text>())
                            {
                                //Now cycle through the "columns" (keys) of the Dictionary and replace item by item
                                for (var index = 0; index < trDict.Count; index++)
                                {
                                    var item = trDict.ElementAt(index);

                                    if (text.Text.Contains(rep.TablePlaceholderStartTag + item.Key + rep.TablePlaceholderEndTag))
                                    {
                                        //If we have line breaks present
                                        if (item.Value[j].Contains(rep.NewLineTag))
                                        {
                                            string[] repArray = item.Value[j].Split(new string[] { rep.NewLineTag }, StringSplitOptions.None);
                                            var lastInsertedText = text;
                                            Break lastInsertedBreak;

                                            for (var i = 0; i < repArray.Length; i++)
                                            {
                                                //The text is only replaced with the first part of the replacement array
                                                if (i == 0)
                                                {
                                                    text.Text = text.Text.Replace(
                                                        rep.TablePlaceholderStartTag + item.Key +
                                                        rep.TablePlaceholderEndTag, repArray[i]);
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
                                            text.Text = text.Text.Replace(
                                                rep.TablePlaceholderStartTag + item.Key + rep.TablePlaceholderEndTag,
                                                item.Value[j]);
                                        }
                                    }
                                }
                            }

                            tableRow.Parent.InsertAfter(tableRowCopy, tableRow);
                            tableRow = tableRowCopy;
                        }

                        tableRow.Remove();
                    }
                }
            }

            docxMs.Position = 0;
            return docxMs;
        }

        public MemoryStream ReplaceImages()
        {
            if (rep.ImagePlaceholders == null || rep.ImagePlaceholders.Count == 0)
                return null;

            using (var doc = WordprocessingDocument.Open(docxMs, true))
            {
                CleanMarkup(doc);

                var documentTexts = doc.MainDocumentPart.Document.Descendants<Text>();

                foreach (var text in documentTexts)
                {
                    foreach (var replace in rep.ImagePlaceholders)
                    {
                        string pl = rep.ImagePlaceholderStartTag + replace.Key + rep.ImagePlaceholderEndTag;
                        imageCounter++;
                        if (text.Text.Contains(pl))
                        {
                            var run = text.Ancestors<Run>().First();
                            var newRunForImage = new Run();
                            //Break the texts into the part before and after image. Then create separate runs for them
                            var pos = text.Text.IndexOf(pl, StringComparison.CurrentCulture);

                            if (pos == 0)
                            {
                                var newAfterRun = (Run)run.Clone();
                                string afterText = text.Text.Substring(pl.Length, text.Text.Length - pl.Length);
                                Text newAfterRunText = newAfterRun.GetFirstChild<Text>();
                                newAfterRunText.Space = SpaceProcessingModeValues.Preserve;
                                newAfterRunText.Text = afterText;

                                run.Parent.InsertAfter(newAfterRun, run);
                            }
                            else if (text.Text.EndsWith(pl))
                            {
                                var newBeforeRun = (Run)run.Clone();
                                string beforeText = text.Text.Substring(0, pos);
                                Text newBeforeRunText = newBeforeRun.GetFirstChild<Text>();
                                newBeforeRunText.Space = SpaceProcessingModeValues.Preserve;
                                newBeforeRunText.Text = beforeText;

                                run.Parent.InsertBefore(newBeforeRun, run);
                            }
                            else
                            {
                                var newBeforeRun = (Run)run.Clone();
                                string beforeText = text.Text.Substring(0, pos);
                                Text newBeforeRunText = newBeforeRun.GetFirstChild<Text>();
                                newBeforeRunText.Space = SpaceProcessingModeValues.Preserve;
                                newBeforeRunText.Text = beforeText;
                                run.Parent.InsertBefore(newBeforeRun, run);

                                var newAfterRun = (Run)run.Clone();
                                string afterText = text.Text.Substring(pos + pl.Length, text.Text.Length - pos - pl.Length);
                                Text newAfterRunText = newAfterRun.GetFirstChild<Text>();
                                newAfterRunText.Space = SpaceProcessingModeValues.Preserve;
                                newAfterRunText.Text = afterText;
                                run.Parent.InsertAfter(newAfterRun, run);
                            }

                            run.Parent.InsertBefore(newRunForImage, run);
                            run.Remove();

                            AppendImageToElement(replace, newRunForImage, doc);
                        }
                    }
                }
            }

            docxMs.Position = 0;
            return docxMs;
        }

        private static void CleanMarkup(WordprocessingDocument doc)
        {
            //REMOVE THESE Markups, because they break up the text into multiple pieces,
            //thereby preventing simple search and replace
            var settings = new SimplifyMarkupSettings
            {
                RemoveComments = true,
                RemoveContentControls = true,
                RemoveEndAndFootNotes = true,
                RemoveFieldCodes = false,
                RemoveLastRenderedPageBreak = true,
                RemovePermissions = true,
                RemoveProof = true,
                RemoveRsidInfo = true,
                RemoveSmartTags = true,
                RemoveSoftHyphens = true,
                ReplaceTabsWithSpaces = true,
                RemoveBookmarks = true
            };
            MarkupSimplifier.SimplifyMarkup(doc, settings);
        }

        private void AppendImageToElement(KeyValuePair<string, ImageElement> placeholder, OpenXmlElement element, WordprocessingDocument wordprocessingDocument)
        {
            string imageExtension = ImageUtils.GetImageType(placeholder.Value.MemStream);

            MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;

            var imageUri = new Uri($"/word/media/{placeholder.Key}{imageCounter}.{imageExtension}", UriKind.Relative);

            // Create "image" part in /word/media
            // Change content type for other image types.
            PackagePart packageImagePart = wordprocessingDocument.Package.CreatePart(imageUri, "Image/" + imageExtension);

            // Feed data.
            placeholder.Value.MemStream.Position = 0;
            byte[] imageBytes = placeholder.Value.MemStream.ToArray();
            packageImagePart.GetStream().Write(imageBytes, 0, imageBytes.Length);

            PackagePart documentPackagePart = mainPart.OpenXmlPackage.Package.GetPart(new Uri("/word/document.xml", UriKind.Relative));

            // URI to the image is relative to relationship document.
            PackageRelationship imageRelationshipPart = documentPackagePart.CreateRelationship(new Uri("media/" + placeholder.Key + imageCounter + "." + imageExtension, UriKind.Relative),TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");

            var imgTmp = placeholder.Value.MemStream.GetImage();

            var drawing = GetImageElement(imageRelationshipPart.Id, placeholder.Key, "picture", imgTmp.Width, imgTmp.Height, placeholder.Value.Dpi);
            element.AppendChild(drawing);
        }

        private Drawing GetImageElement(
            string imagePartId,
            string fileName,
            string pictureName,
            double width,
            double height,
            double ppi)
        {
            double englishMetricUnitsPerInch = 914400;
            double pixelsPerInch = ppi;

            //calculate size in emu
            double emuWidth = width * englishMetricUnitsPerInch / pixelsPerInch;
            double emuHeight = height * englishMetricUnitsPerInch / pixelsPerInch;

#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
            var element = new Drawing(
                new DW.Inline(
                    new DW.Extent { Cx = (Int64Value)emuWidth, Cy = (Int64Value)emuHeight },
                    new DW.EffectExtent { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                    new DW.DocProperties { Id = (UInt32Value)1U, Name = pictureName + imageCounter },
                    new DW.NonVisualGraphicFrameDrawingProperties(
                    new A.GraphicFrameLocks { NoChangeAspect = true }),
                    new A.Graphic(
                        new A.GraphicData(
                            new PIC.Picture(
                                new PIC.NonVisualPictureProperties(
                                    new PIC.NonVisualDrawingProperties { Id = (UInt32Value)0U, Name = fileName },
                                    new PIC.NonVisualPictureDrawingProperties()),
                                new PIC.BlipFill(
                                    new A.Blip(
                                        new A.BlipExtensionList(
                                            new A.BlipExtension { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" }))
                                    {
                                        Embed = imagePartId,
                                        CompressionState = A.BlipCompressionValues.Print
                                    },
                                    new A.Stretch(new A.FillRectangle())),
                                new PIC.ShapeProperties(
                                    new A.Transform2D(
                                        new A.Offset { X = 0L, Y = 0L },
                                        new A.Extents { Cx = (Int64Value)emuWidth, Cy = (Int64Value)emuHeight }),
                                    new A.PresetGeometry(
                                        new A.AdjustValueList())
                                    { Preset = A.ShapeTypeValues.Rectangle })))
                        {
                            Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture"
                        }))
                {
                    DistanceFromTop = (UInt32Value)0U,
                    DistanceFromBottom = (UInt32Value)0U,
                    DistanceFromLeft = (UInt32Value)0U,
                    DistanceFromRight = (UInt32Value)0U,
                    EditId = "50D07946"
                });
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"
            return element;
        }
    }
}
