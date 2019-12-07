using jsreport.Binary;
using jsreport.Client;
using jsreport.Local;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace WebApplication2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public class DetailsModel
        {
            public long year { get; set; }
            public int month { get; set; }
            public string person { get; set; }
        }

        public class ItemModel
        {
            public string Name { get; set; }
            public long Price { get; set; }
        }

        public class PayModel
        {
            public string Name { get; set; }
            public long Price { get; set; }
        } 
        protected async void Button1_Click(object sender, EventArgs e)
        {
            var rs = new LocalReporting()
                        .UseBinary(JsReportBinary.GetBinary())
                        .Configure(cfg => cfg.FileSystemStore().BaseUrlAsWorkingDirectory())
                        .AsUtility()
                        .Create();

            var report3 = await rs.RenderByNameAsync("Payslip", new
            {
                details = new List<DetailsModel>()
                {
                    new DetailsModel()
                    {
                        person = "Dragon",
                        year = 2017,
                        month = 1
                    }
                },
                deductions = new List<PayModel>()
                {
                    new PayModel()
                    {
                        Name = "Unpaid Leave",
                        Price = 300
                    },
                    new PayModel()
                    {
                        Name = "Social Security",
                        Price = 400
                    },
                },
                additions = new List<PayModel>()
                {
                    new PayModel()
                    {
                        Name = "Petrol Claim",
                        Price = 0
                    },
                    new PayModel()
                    {
                        Name = "Monthly Allowance",
                        Price = 400
                    }
                },
                basepay = new List<PayModel>()
                {
                    new PayModel()
                    {
                        Name = "Base",
                        Price = 2100
                    }
                }
            });

            using (var fs = File.Create("C:\\Users\\Dragon\\source\\repos\\WebApplication2\\WebApplication2\\Reports\\out.pdf"))
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