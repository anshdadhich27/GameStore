using GameStore.Models.Entity;
using GameStore.Models.Interface;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Models
{
    public class Utility
    {
        

        public static bool IsValidEmail(string email)
        {
            bool result = true;
            try { var obj = new MailAddress(email); }
            catch { result = false; }
            return result;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Get_Hash(string input, string secretkey)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] secretBytes = Encoding.UTF8.GetBytes(secretkey);
            using (HMACSHA256 hmac = new HMACSHA256(secretBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                return ByteToString(hashBytes);
            }                          
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = 1;
                if (showSrNo)
                {
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }
                // add the content into the Excel file
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    try
                    {
                        ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                        int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                        if (maxLength < 150)
                        {
                            workSheet.Column(columnIndex).AutoFit();
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    columnIndex++;
                }

                // format header - bold, yellow on black
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FDE9D9"));

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }
                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false)
        {
            return ExportExcel(ListToDataTable<T>(data), Heading, showSlno);
        }


        public static string Create_Invoice(string content, string id)
        {
            var invoice_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", "Invoice", string.Format("{0}.pdf", id));
            if (!File.Exists(invoice_path)) { var file = File.Create(invoice_path); file.Close(); }
            using (FileStream file = new FileStream(invoice_path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                StringReader sr = new StringReader(content);
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 35f, 0f);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, file);
                pdfDoc.Open();
                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                file.Close();
            }
            return string.Format("/Resources/Invoice/{0}.pdf", id);
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return sbinary;
        }

        /// <summary>
        /// Get SKU For Product
        /// {0}{1}-{2}{3}-{4}-{5}
        /// 0. Initial of product category. (Ex. G for Game, C for Console etc)
        /// 1. Product Id
        /// 2. Sub Category of product. (Ex. P for Platform)
        /// 3. Product Type Id
        /// 4. Quantity
        /// 5. Date (DDMMYYYY)
        /// </summary>
        /// <param name="obj">Product Details</param>
        /// <returns>SKU</returns>
        public static string Get_SKU(CartItem obj)
        {
            string initial = string.Empty;
            string type = string.Empty;
            switch (obj.Product_Type)
            {
                case Constant.ACCESSORIES:
                    initial = "A";
                    type = "T";
                    break;
                case Constant.CONSOLE:
                    initial = "C";
                    type = "T";
                    break;
                case Constant.GAME:
                    initial = "G";
                    type = "P";
                    break;
                default:
                    initial = "S";
                    type = "P";
                    break;
            }
            return string.Format("{0}{1}-{2}{3}-{4}-{5}", initial, obj.Product_Id, type, obj.Product_TypeId, obj.Quantity, DateTime.Now.ToString("ddMMyyyy"));
        }
        

        public static string GetInTitleCase(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        public static DateTime GetDateTime(long unixDate)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return date.AddMilliseconds(unixDate).ToLocalTime();
        }

        public static long GetUnixTime()
        {
            TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (long)timeSpan.TotalSeconds;
        }

        private static async Task<Image> GetImage(IHostingEnvironment env, string url)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            Stream inputStream = await response.Content.ReadAsStreamAsync();
            return Image.FromStream(inputStream);
        }

        
        public static async Task<string> SaveImageAndGetUrl(IHostingEnvironment env, IImageRepository img_repo, Images img, string resource_id)
        {
            bool result = true;            
            DataProvider.DbResult dbResult = null;
            string igdb = "https://images.igdb.com/igdb/image/upload/";
            string fileName = string.Format("{0}.jpg", Guid.NewGuid().ToString());

            img.ImageType = Constant.ImageType.Small;
            img.Url = string.Format("/{0}/{1}/{2}/{3}", Constant.Resources, img.ImageOf, img.ImageType, fileName);
            result = await SaveImageAndGetUrl(env, string.Format("{0}/{1}/{2}.jpg", igdb, Constant.ImageType.IGdb.Logo, resource_id), img.ImageType, img.ImageOf, fileName);
            if (result) { dbResult = await img_repo.Add_New_ImageAsync(img); }

            img.ImageType = Constant.ImageType.Thumbnail;
            img.Url = string.Format("/{0}/{1}/{2}/{3}", Constant.Resources, img.ImageOf, img.ImageType, fileName);
            result = await SaveImageAndGetUrl(env, string.Format("{0}/{1}/{2}.jpg", igdb, Constant.ImageType.IGdb.Thumbnail, resource_id), img.ImageType, img.ImageOf, fileName);
            if (result) { dbResult = await img_repo.Add_New_ImageAsync(img); }

            img.ImageType = Constant.ImageType.Medium;
            img.Url = string.Format("/{0}/{1}/{2}/{3}", Constant.Resources, img.ImageOf, img.ImageType, fileName);
            result = await SaveImageAndGetUrl(env, string.Format("{0}/{1}/{2}.jpg", igdb, Constant.ImageType.IGdb.Medium, resource_id), img.ImageType, img.ImageOf, fileName);
            if (result) { dbResult = await img_repo.Add_New_ImageAsync(img); }

            img.ImageType = Constant.ImageType.Large;
            img.Url = string.Format("/{0}/{1}/{2}/{3}", Constant.Resources, img.ImageOf, img.ImageType, fileName);
            result = await SaveImageAndGetUrl(env, string.Format("{0}/{1}/{2}.jpg", igdb, Constant.ImageType.IGdb.Large, resource_id), img.ImageType, img.ImageOf, fileName);
            if (result) { dbResult = await img_repo.Add_New_ImageAsync(img); }
            
            return fileName;
        }
        
        public static async Task<string> SaveCoverImageAndGetUrl(IHostingEnvironment env, IImageRepository img_repo, Images img, string resource_id)
        {
            bool result = true;
            DataProvider.DbResult dbResult = null;
            string igdb = "https://images.igdb.com/igdb/image/upload/";
            string fileName = string.Format("{0}.jpg", Guid.NewGuid().ToString());
            
            img.ImageType = Constant.ImageType.Thumbnail;
            img.Url = string.Format("/{0}/{1}/{2}/{3}", Constant.Resources, img.ImageOf, img.ImageType, fileName);
            result = await SaveImageAndGetUrl(env, string.Format("{0}/{1}/{2}.jpg", igdb, Constant.ImageType.IGdb.Thumbnail, resource_id), img.ImageType, img.ImageOf, fileName);
            if (result) { dbResult = await img_repo.Add_New_ImageAsync(img); }
            
            return fileName;
        }

        public static async Task<string> SaveScreenshotAndGetUrl(IHostingEnvironment env, IImageRepository img_repo, Images img, string resource_id)
        {
            bool result = true;
            DataProvider.DbResult dbResult = null;
            string igdb = "https://images.igdb.com/igdb/image/upload/";
            string fileName = string.Format("{0}.jpg", Guid.NewGuid().ToString());
            
            img.ImageType = Constant.ImageType.Medium;
            img.Url = string.Format("/{0}/{1}/{2}/{3}", Constant.Resources, img.ImageOf, img.ImageType, fileName);
            result = await SaveImageAndGetUrl(env, string.Format("{0}/{1}/{2}.jpg", igdb, Constant.ImageType.IGdb.Medium, resource_id), img.ImageType, img.ImageOf, fileName);
            if (result) { dbResult = await img_repo.Add_New_ImageAsync(img); }

            img.ImageType = Constant.ImageType.Large;
            img.Url = string.Format("/{0}/{1}/{2}/{3}", Constant.Resources, img.ImageOf, img.ImageType, fileName);
            result = await SaveImageAndGetUrl(env, string.Format("{0}/{1}/{2}.jpg", igdb, Constant.ImageType.IGdb.Large, resource_id), img.ImageType, img.ImageOf, fileName);
            if (result) { dbResult = await img_repo.Add_New_ImageAsync(img); }

            return fileName;
        }

        public static async Task<string> SaveLogoAndGetUrl(IHostingEnvironment env, IImageRepository img_repo, Images img, string resource_id)
        {
            bool result = true;
            DataProvider.DbResult dbResult = null;
            string igdb = "https://images.igdb.com/igdb/image/upload/";
            string fileName = string.Format("{0}.jpg", Guid.NewGuid().ToString());

            img.ImageType = Constant.ImageType.Small;
            img.Url = string.Format("/{0}/{1}/{2}/{3}", Constant.Resources, img.ImageOf, img.ImageType, fileName);
            result = await SaveImageAndGetUrl(env, string.Format("{0}/{1}/{2}.jpg", igdb, Constant.ImageType.IGdb.Logo, resource_id), img.ImageType, img.ImageOf, fileName);
            if (result) { dbResult = await img_repo.Add_New_ImageAsync(img); }
            
            return fileName;
        }
        
        public static async Task<bool> SaveImageAndGetUrl(IHostingEnvironment env, string url, string imageType, string imageOf, string fileName)
        {
            bool result = true; 
            try
            {
                using (var client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (Stream inputStream = await response.Content.ReadAsStreamAsync())
                            {
                                using (var image = Image.FromStream(inputStream))
                                {
                                    string folderPath = string.Format(@"{0}\{1}\{2}\{3}", env.WebRootPath, Constant.Resources, imageOf, imageType);
                                    if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
                                    string filePath = string.Format(@"{0}\{1}\{2}\{3}\{4}", env.WebRootPath, Constant.Resources, imageOf, imageType, fileName);
                                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }                                   
                            }
                        }
                        else { result = false; }
                    }                        
                }                                    
            }
            catch (Exception ex) { var msg = ex.Message; result = false; }
            return result;
        }

        
    }

    internal static class RandomNumber
    {
        private static Random r = new Random();
        private static object locker = new object();
        private static Random globalRandom = new Random();
        [ThreadStatic]
        private static Random localRandom;
        public static int GenerateNewRandom(int min, int max)
        {
            return new Random().Next(min, max);
        }
        public static int GenerateLockedRandom(int min, int max)
        {
            int result;
            lock (locker)
            {
                result = r.Next(min, max);
            }
            return result;
        }
        public static int GenerateRandom(int min, int max)
        {
            Random random = localRandom;
            if (random == null)
            {
                int seed;
                lock (globalRandom)
                {
                    seed = globalRandom.Next();
                }
                random = (localRandom = new Random(seed));
            }
            return random.Next(min, max);
        }
    }
}
