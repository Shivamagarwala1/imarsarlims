using iMARSARLIMS.Interface;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
//using PdfSharpCore.Pdf.Actions;
//using PdfSharpCore.Pdf.IO;

//using Microsoft.Playwright;
//using PuppeteerSharp;
//using PuppeteerSharp.Media;

using System.Drawing;
//using PDFiumSharp;
//using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IempMasterServices _empMasterServices;
       // private readonly IConverter _converter;

        // Constructor for dependency injection
        public LoginController(IempMasterServices empMasterServices )
        {
            _empMasterServices = empMasterServices;
            // _converter = converter; IConverter converter
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceStatusResponseModel>> EmpLogin([FromBody] LoginRequestModel loginRequestModel)
        {
            if (loginRequestModel == null)
                return BadRequest("Invalid login request model.");

            try
            {
                var result = await _empMasterServices.EmpLogin(loginRequestModel);

                if (result == null)
                    return Unauthorized("Invalid username or password.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is set up in your project)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("forgetPassword")]
        public async Task<ActionResult<ServiceStatusResponseModel>> forgetPassword(string Username)
        {
            if (Username == "")
                return BadRequest("Please Enter Username");

            try
            {
                var result = await _empMasterServices.forgetPassword(Username);

                if (result == null)
                    return Unauthorized("Invalid username");

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is set up in your project)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //start

        //[HttpPost("convert")]
        //public async Task<IActionResult> ConvertHtmlToPdf([FromBody] PdfRequest request)
        //{
        //    try
        //    {
        //        // Initialize Playwright and launch a Chromium browser
        //        using var playwright = await Playwright.CreateAsync();
        //        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });



        //        var page = await browser.NewPageAsync();

        //        // Set the HTML content
        //        await page.SetContentAsync(request.HtmlContent);

        //        // Generate the PDF as a byte array
        //        var pdfBytes = await page.PdfAsync(new PagePdfOptions
        //        {
        //            Format = "A4",
        //            DisplayHeaderFooter = true,
        //            HeaderTemplate = "<span style='font-size: 12px; margin-left: 10px;'>Header</span>",
        //            FooterTemplate = "<span style='font-size: 12px; margin-left: 10px;'>Footer</span>",
        //            Margin = new Margin { Top = "20px", Bottom = "20px", Left = "10px", Right = "10px" }
        //        });

        //        // Return the PDF file as a response
        //        var fileName = "GeneratedDocument.pdf";
        //        return File(pdfBytes, "application/pdf", fileName);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}
        //[HttpPost("generate-pdf")]
        //public async Task<IActionResult> GeneratePdf([FromBody] PdfRequest request)
        //{
        //    if (string.IsNullOrWhiteSpace(request.HtmlContent))
        //    {
        //        return BadRequest("HTML content is required.");
        //    }

        //    // Download the Chromium browser if not already done
        //    var browserFetcher = new BrowserFetcher();
        //    var revisionInfo = await browserFetcher.DownloadAsync(); // Downloads the default Chromium version

        //    // Launch the browser
        //    await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        //    {
        //        Headless = true // Run in headless mode
        //    });

        //    // Open a new page
        //    await using var page = await browser.NewPageAsync();

        //    // Set the page content to the provided HTML
        //    await page.SetContentAsync(request.HtmlContent);

        //    // Generate the PDF with header and footer
        //    var pdfStream = await page.PdfStreamAsync(new PdfOptions
        //    {
        //        Format = PaperFormat.A4,
        //        PrintBackground = true, // Include background styles
        //        DisplayHeaderFooter = true, // Enable header and footer
        //        HeaderTemplate = "<span style=\"font-size: 12px; font-weight: bold;\">Custom Header</span>", // Custom header
        //        FooterTemplate = "<span style=\"font-size: 10px;\">Page <span class=\"pageNumber\"></span> of <span class=\"totalPages\"></span></span>", // Custom footer with page numbers
        //        MarginOptions = new MarginOptions
        //        {
        //            Top = "2cm",
        //            Bottom = "2cm",
        //            Left = "1cm",
        //            Right = "1cm"
        //        }
        //    });
        //    // Close the browser
        //    await browser.CloseAsync();

        //    // Return the PDF file as a response
        //    return File(pdfStream, "application/pdf", "GeneratedDocument.pdf");

        //}

        //[HttpPost("GeneratePdfWithImages")]
        //public IActionResult GeneratePdfWithImages([FromBody] PdfRequest request)
        //{
        //    if (string.IsNullOrWhiteSpace(request.HtmlContent))
        //    {
        //        return BadRequest("HTML content is required.");
        //    }

        //    // Step 1: Configure the PDF renderer with header, footer, and margins
        //    var renderer = new ChromePdfRenderer
        //    {
        //        RenderingOptions = new ChromePdfRenderOptions
        //        {
        //            MarginTop = 30,  // Top margin in mm
        //            MarginBottom = 40, // Bottom margin in mm
        //            MarginLeft = 10, // Left margin in mm
        //            MarginRight = 10, // Right margin in mm
        //            HtmlHeader = new HtmlHeaderFooter
        //            {
        //                MaxHeight = 20, // Height of the header in mm
        //                HtmlFragment = "<span style='font-size:12px; font-weight:bold;'>PDF Header - Generated Report</span>"
        //            },
        //            HtmlFooter = new HtmlHeaderFooter
        //            {
        //                MaxHeight = 20, // Height of the footer in mm
        //                HtmlFragment = "<span style='font-size:10px;'>Page {page} of {total-pages}</span>"
        //            }
        //        }
        //    };

        //    // Step 2: Generate the PDF from HTML
        //    var pdfDocument = renderer.RenderHtmlAsPdf(request.HtmlContent);

        //    // Step 3: Convert the entire PDF to images (at default DPI)
        //    var imagePaths = pdfDocument.RasterizeToImageFiles("300"); // This generates image files for each page

        //    // Step 4: Create a new PDF document
        //    var newPdf = new PdfDocument(null); // Empty PDF to append images into

        //    foreach (var imagePath in imagePaths)
        //    {
        //        // Load the image as a Bitmap
        //        using (var bitmap = new Bitmap(imagePath))
        //        {
        //            // Create a PDF page from the Bitmap image
        //            var pageImagePdf = PdfDocument.FromImage(bitmap);

        //            // Append the page to the new PDF
        //            newPdf.AppendPdf(pageImagePdf);
        //        }
        //    }

        //    // Step 5: Save the new PDF to a memory stream
        //    var outputStream = new MemoryStream();
        //    newPdf.SaveAs(outputStream);
        //    outputStream.Position = 0;

        //    // Step 6: Return the new PDF
        //    return File(outputStream, "application/pdf", "ImageBasedPdfWithHeaderFooter.pdf");


        //}

        //end
        public class PdfRequest
        {
            public string HtmlContent { get; set; }
        }
    }
}
