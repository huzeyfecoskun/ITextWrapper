using System;

public class Class1
{
	public Class1()
	{

        public void ImageToPdf()
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(OUTPUT_FOLDER + "ImageToPdf.pdf"));
            Document document = new Document(pdfDocument);

            ImageData imageData = ImageDataFactory.create(ORIG);
            Image image = new Image(imageData);
            image.setWidth(pdfDocument.getDefaultPageSize().getWidth() - 50);
            image.setAutoScaleHeight(true);

            document.add(image);
            pdfDocument.close();
        }

	}
}
