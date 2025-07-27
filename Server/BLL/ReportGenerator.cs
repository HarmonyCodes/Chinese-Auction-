using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Collections.Generic;

namespace Server.BLL
{
    public class ReportGenerator
    {
        public static void CreateWinnersReport(Dictionary<string, string> giftWinners, Stream outputStream)
        {
            var document = new PdfDocument();
            document.Info.Title = "Winners Report";
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            var fontBody = new XFont("Verdana", 12, XFontStyle.Regular);

            int y = 40;
            gfx.DrawString("Gift Winners Report", fontTitle, XBrushes.Black, new XRect(0, y, page.Width, page.Height), XStringFormats.TopCenter);
            y += 40;

            foreach (var gift in giftWinners)
            {
                string line = $"Gift: {gift.Key}   |   Winner: {gift.Value}";
                gfx.DrawString(line, fontBody, XBrushes.Black, new XRect(40, y, page.Width - 80, page.Height), XStringFormats.TopLeft);
                y += 25;
            }

            document.Save(outputStream);
        }

        public static void CreateRevenueReport(decimal totalRevenue, Stream outputStream)
        {
            var document = new PdfDocument();
            document.Info.Title = "Revenue Report";

            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            var fontBody = new XFont("Verdana", 12, XFontStyle.Regular);

            int y = 40;
            gfx.DrawString("Total Revenue Report", fontTitle, XBrushes.Black,
                new XRect(0, y, page.Width, page.Height), XStringFormats.TopCenter);

            y += 60;

            string revenueText = $"Total Revenue: {totalRevenue:C}"; // C => Currency format
            gfx.DrawString(revenueText, fontBody, XBrushes.Black,
                new XRect(60, y, page.Width - 120, page.Height), XStringFormats.TopLeft);

            document.Save(outputStream);
        }
    }
}
