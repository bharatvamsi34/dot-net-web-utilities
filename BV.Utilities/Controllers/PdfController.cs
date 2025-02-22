using BV.Utilities.Models;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BV.Utilities.Controllers
{
    public class PdfController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        // GET: Pdf/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Pdf/RemovePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePassword(PdfModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Process the uploaded PDF
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.PdfFile.CopyToAsync(memoryStream);

                        if (model?.Password != null && memoryStream != null && memoryStream.Length > 0) {
                            // Rewind the memory stream to the start
                            memoryStream.Seek(0, SeekOrigin.Begin);

                            // Convert the password string to a byte array
                            byte[] passwordBytes = Encoding.UTF8.GetBytes(model.Password);

                            // Attempt to open the protected PDF with the provided password
                            PdfReader reader = new PdfReader(memoryStream, new ReaderProperties().SetPassword(passwordBytes));
                            using (var outputMemoryStream = new MemoryStream())
                            {
                                // Create a writer to output the unprotected PDF
                                PdfWriter writer = new PdfWriter(outputMemoryStream);
                                PdfDocument pdfDoc = new PdfDocument(reader, writer);

                                // Close the PdfDocument to finalize the writing
                                pdfDoc.Close();

                                // Get the original file name (without path or extension if needed)
                                var originalFileName = Path.GetFileName(model.PdfFile.FileName);

                                // Return the unprotected PDF as a downloadable file
                                return File(outputMemoryStream.ToArray(), "application/pdf", originalFileName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Return an error if something goes wrong
                    ModelState.AddModelError("", "Error processing PDF: " + ex.Message);
                }
            }

            // If the model state is invalid, return to the view with the model
            return View("Upload", model);
        }
    }
}
