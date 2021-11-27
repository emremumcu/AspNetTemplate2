// https://tutexchange.com/action-results-in-asp-net-mvc-core/

namespace AspNetTemplate2.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Text;
    using System.Text.Json;

    public class ContentController : Controller
    {
        public IActionResult Index()
        {
            StringBuilder html = new StringBuilder();

            html.Append("<a href='Content/GetContentResultText'>Content Result: TEXT</a>").Append("<br/>");
            html.Append("<a href='Content/GetContentResultHtml'>Content Result: HTML</a>").Append("<br/>");
            html.Append("<a href='Content/GetContentResultXML'>Content Result: XML</a>").Append("<br/>");
            html.Append("<a href='Content/GetContentResultJson'>Content Result: JSON</a>").Append("<br/>");
            html.Append("<a href='Content/GetContentText'>Content: TEXT</a>").Append("<br/>");
            html.Append("<a href='Content/GetEmptyResult'>Empty Result</a>").Append("<br/>");

            ContentResult result = new ContentResult();
            result.Content = html.ToString();
            result.ContentType = "text/html";
            result.StatusCode = (int)HttpStatusCode.OK;
            return result;
        }

        public ContentResult GetContentResultText()
        {
            string text = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,";

            ContentResult result = new ContentResult();
            result.Content = text;
            result.ContentType = "text/plain"; // "text/plain; charset=utf-8";
            result.StatusCode = (int)HttpStatusCode.OK;
            return result;
        }

        public ContentResult GetContentResultHtml()
        {
            string html = @"<!DOCTYPE html>
                             <html>
                             <head>
                             <title>Page Title</title>
                             </head>
                             <body>
                             <h1>This is a Heading</h1>
                             <p>This is a paragraph.</p>
                             </body>
                             </html>";

            ContentResult result = new ContentResult();
            result.Content = html;
            result.ContentType = "text/html"; 
            result.StatusCode = (int)HttpStatusCode.OK;
            return result;
        }

        public ContentResult GetContentResultXML()
        {
            string xml = @"<Contact>  
                <Name>Saineshwar</Name>  
                <Phone Type=""home"">206-555-0144</Phone>  
                <Phone type=""work"">425-555-0145</Phone>  
                <Address>  
                <Street1>123 Main St</Street1>  
                <City>Mercer Island</City>  
                <State>WA</State>  
                <Postal>68042</Postal>  
                </Address>  
                <NetWorth>10</NetWorth>  
                </Contact>";

            ContentResult result = new ContentResult();
            result.Content = xml;
            result.ContentType = "application/xml"; 
            result.StatusCode = (int)HttpStatusCode.OK;
            return result;
        }

        public ContentResult GetContentResultJson()
        {
            var json = new { Id = 123, Name = "Hero" };

            ContentResult result = new ContentResult();
            result.Content = JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
            result.ContentType = "application/json";
            result.StatusCode = StatusCodes.Status200OK;
            return result;
        }

        public ContentResult GetContentText()
        {
            string data = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,";
            return Content(data, "text/plain");
            // return Content(data);
        }

        public EmptyResult GetEmptyResult()
        {
            //code to execute some logic
            return new EmptyResult();
        }
    }
}
