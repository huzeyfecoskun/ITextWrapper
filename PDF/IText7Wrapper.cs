//  Geliştirici Huzeyfe Coşkun

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace PDF
{
    public class ITextWrapper
    {
        /// <summary>
        /// Verilen image'ı pdf'e çevirir
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="destinationFile"></param>
        public static string ImageToPdf(string imagePath, string destinationFile)
        {
            var pdfDocument = new PdfDocument(new PdfWriter(destinationFile));
            var document = new Document(pdfDocument);

            var imageData = ImageDataFactory.Create(imagePath);
            var image = new Image(imageData);
            image.SetWidth(pdfDocument.GetDefaultPageSize().GetWidth() - 50);
            image.SetAutoScaleHeight(true);

            document.Add(image);
            pdfDocument.Close();
            return destinationFile;
        }

        /// <summary>
        /// Pdf belgesinin kaç sayfa olduğunu gösterir
        /// Return page count of PDF File
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public static int GetPageCount(string sourceFile)
        {
            var fileInfo = new FileInfo(sourceFile);
            if (fileInfo.Extension != ".pdf")
                return -1;
            var source = new PdfDocument(new PdfReader(sourceFile));
            Document sourceDocument = new Document(source);

            int pageCount = source.GetNumberOfPages();
            source.Close();
            return pageCount;
        }

        /// <summary>
        /// Merge Given Files
        /// Verile dosyaları birleştir
        /// MergeFiles ( [PATH_ARRAY], "d:\\", "[OUTPUT].pdf" )
        /// </summary>
        /// <param name="files"></param>
        /// <param name="destinationFolder"></param>
        /// <param name="fileName"></param>
        public static void MergeFiles(string[] files, string destinationFolder, string fileName)
        {
            var pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + fileName));
            var document = new Document(pdfDocument);
            var extensions = new List<string> { ".jpeg", ".jpg", ".png", ".tiff", "pdf" };
            var tempFiles = new List<string>();
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                var extension = fileInfo.Extension;

                if (extension != ".pdf")
                {
                    if (extensions.FirstOrDefault(n => n == extension) == null)
                        return;
                    var path = ImageToPdf(file,
                        destinationFolder + "\\" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8) + ".pdf");
                    tempFiles.Add(path);
                    var read = new PdfDocument(new PdfReader(path));
                    var x = new Document(read);

                    read.CopyPagesTo(1, read.GetNumberOfPages(), pdfDocument);
                    read.GetNumberOfPages();
                    read.Close();
                }
                else
                {
                    var read = new PdfDocument(new PdfReader(file));
                    var x = new Document(read);
                    read.CopyPagesTo(1, read.GetNumberOfPages(), pdfDocument);
                    read.Close();
                }
            }
            pdfDocument.Close();

            // Delete Converted PDF of images
            foreach (var tempFile in tempFiles.Where(File.Exists))
            {
                File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Pdf dosyasını belirlenen yerden böl
        /// Usage : Split (  "[PDF].pdf", [SplitPage], [DESTINATION_FOLDER] );
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="splitPage"></param>
        /// <param name="destinationFolder"></param>
        public static void Split(string sourceFile, int splitPage, string destinationFolder)
        {
            var fileInfo = new FileInfo(sourceFile);
            var source = new PdfDocument(new PdfReader(sourceFile));
            Document sourceDocument = new Document(source);

            var writeOne = new PdfDocument(new PdfWriter(fileInfo.DirectoryName + "\\parca_1.pdf"));
            Document writeOneDocument = new Document(writeOne);

            var writeTwo = new PdfDocument(new PdfWriter(fileInfo.DirectoryName + "\\parca_2.pdf"));
            Document writeTwoDocument = new Document(writeOne);

            if (splitPage > source.GetNumberOfPages())
                return;

            source.CopyPagesTo(1, splitPage, writeOne);
            source.CopyPagesTo(splitPage, source.GetNumberOfPages(), writeTwo);

            source.Close();
            writeOne.Close();
            writeTwo.Close();
        }

        /// <summary>
        /// Insert Pdf to file
        /// </summary>
        /// <param name="sourceFile">Kaynak dosya</param>
        /// <param name="insertFile">Insert edilecek dosya</param>
        /// <param name="insertPoint">Eklenecek nokta</param>
        /// <param name="destinationFile">Hedef dosya</param>
        public static void InsertInto(string sourceFile, string insertFile, int insertPoint, string destinationFile)
        {
            var source = new PdfDocument(new PdfReader(sourceFile));
            Document sourceDocument = new Document(source);

            if (source.GetNumberOfPages() < insertPoint)
                return;
            if (insertPoint < 1)
                return;

            var insert = new PdfDocument(new PdfReader(insertFile));
            Document insertDocument = new Document(insert);

            var destination = new PdfDocument(new PdfWriter(destinationFile));
            Document destinationDocument = new Document(destination);

            source.CopyPagesTo(1, insertPoint, destination);
            insert.CopyPagesTo(1, insert.GetNumberOfPages(), destination);
            source.CopyPagesTo(insertPoint, source.GetNumberOfPages(), destination);

            source.Close();
            insert.Close();
            destination.Close();
        }

    }
}