//  Geliştirici Huzeyfe Coşkun
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

public class ITextWrapper
{
    public ITextWrapper()
    {

    }

    /// <summary>
    /// Verilen image'ı pdf'e çevirir
    /// </summary>
    /// <param name="imagePath"></param>
    /// <param name="destinationFolder"></param>
    public static string ImageToPdf(string imagePath, string destinationFolder, string destFileName)
    {
        PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + destFileName));
        Document document = new Document(pdfDocument);

        ImageData imageData = ImageDataFactory.Create(imagePath);
        Image image = new Image(imageData);
        image.SetWidth(pdfDocument.GetDefaultPageSize().GetWidth() - 50);
        image.SetAutoScaleHeight(true);

        document.Add(image);
        pdfDocument.Close();
        return destinationFolder + destFileName;
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
        PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + fileName));
        Document document = new Document(pdfDocument);
        var extensions = new List<string> { ".jpeg", ".jpg", ".png", ".tiff", "pdf" };
        int page = 1;
        List<string> tempFiles = new List<string>();
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            var extension = fileInfo.Extension;

            //// Desteklenmeyen bir format varsa Merge'i iptal et
            //if (!extensions.Contains(extension))
            //    return;

            if (extension != ".pdf")
            {
                // Convert
                var path = ImageToPdf(file,
                    destinationFolder, Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8) + ".pdf");
                tempFiles.Add(path);
                var read = new PdfDocument(new PdfReader(path));
                Document x = new Document(read);

                read.CopyPagesTo(1, read.GetNumberOfPages(), pdfDocument);
                page += read.GetNumberOfPages();
                read.Close();
            }
            else
            {
                var read = new PdfDocument(new PdfReader(file));
                Document x = new Document(read);
                read.CopyPagesTo(1, read.GetNumberOfPages(), pdfDocument);
                read.Close();
            }
        }
        pdfDocument.Close();

        // Delete Converted PDF of images
        foreach (var tempFile in tempFiles)
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    /// <summary>
    /// Pdf dosyasını belirlenen yerden böl
    /// Usage : Split ( "d:\\", "d:\\", "\\[PDF].pdf", [SplitPoint] );
    /// </summary>
    /// <param name="sourceFolder">Kaynak klasör</param>
    /// <param name="destinationFolder">Hedef klasör</param>
    /// <param name="sourceFile">Sadece dosyanın adı yazılır</param>
    /// <param name="splitPage">Sayfanın bölüneceği kısım</param>
    public static void Split(string sourceFolder, string destinationFolder, string sourceFile, int splitPage)
    {
        var source = new PdfDocument(new PdfReader(sourceFolder + sourceFile));
        Document source_document = new Document(source);

        var write_one = new PdfDocument(new PdfWriter(sourceFolder + "\\parca_1.pdf"));
        Document write_one_document = new Document(write_one);

        var write_two = new PdfDocument(new PdfWriter(sourceFolder + "\\parca_2.pdf"));
        Document write_two_document = new Document(write_one);

        if (splitPage > source.GetNumberOfPages())
            return;

        source.CopyPagesTo(1, splitPage, write_one);
        source.CopyPagesTo(splitPage, source.GetNumberOfPages(), write_two);

        source.Close();
        write_one.Close();
        write_two.Close();
    }

    /// <summary>
    /// Insert Pdf to file
    /// </summary>
    /// <param name="sourceFile">Kaynak dosya</param>
    /// <param name="insertFile">Insert edilecek dosya</param>
    /// <param name="destinationFile">Hedef dosya</param>
    /// <param name="insertPoint">Eklenecek nokta</param>
    public static void InsertInto(string sourceFile, string insertFile, string destinationFile, int insertPoint)
    {
        var source = new PdfDocument(new PdfReader(sourceFile));
        Document source_document = new Document(source);

        if (source.GetNumberOfPages() < insertPoint)
            return;
        if (insertPoint < 1)
            return;

        var insert = new PdfDocument(new PdfReader(insertFile));
        Document insert_document = new Document(insert);

        var destination = new PdfDocument(new PdfWriter(destinationFile));
        Document destination_document = new Document(destination);

        source.CopyPagesTo(1, insertPoint, destination);
        insert.CopyPagesTo(1, insert.GetNumberOfPages(), destination);
        source.CopyPagesTo(insertPoint, source.GetNumberOfPages(), destination);

        source.Close();
        insert.Close();
        destination.Close();
    }

}
