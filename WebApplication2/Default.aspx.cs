using jsreport.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace WebApplication2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        ReportingService rs;

        protected void Page_Load(object sender, EventArgs e)
        {
            rs = new ReportingService("http://localhost:5488");
        }

        public class CompanyModel
        {
            public string Name { get; set; }
            public string Road { get; set; }
            public string Country { get; set; }
        }

        public class ItemModel
        {
            public string Name { get; set; }
            public long Price { get; set; }
        }
        protected async void Button1_Click(object sender, EventArgs e)
        {
            var report3 = await rs.RenderByNameAsync("Invoice", new
            {
                Number = "123",
                Seller = new CompanyModel()
                {
                    Name = "Next Step Webs, Inc.",
                    Road = "12345 Sunny Road",
                    Country = "Sunnyville, TX 12345"
                },
                Buyer = new CompanyModel()
                {
                    Name = "Acme Corp.",
                    Road = "16 Johnson Road",
                    Country = "Paris, France 8060"
                },
                Items = new List<ItemModel>()
                    {
                        new ItemModel()
                        {
                            Name = "Website design",
                            Price = 300
                        },
                        new ItemModel()
                        {
                            Name = "Implementing specific components",
                            Price = 600
                        },
                        new ItemModel()
                        {
                            Name = "Maintenance and support",
                            Price = 150
                        }
                    }
            });

            using (var fs = File.Create("\\Reports\\out.pdf"))
            {
                report3.Content.CopyTo(fs);
            }

            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + "outpdf.pdf" + "\"");
            byte[] data = req.DownloadData(Server.MapPath("\\Reports\\out.pdf"));
            response.BinaryWrite(data);
            response.End();
        }
    }
}