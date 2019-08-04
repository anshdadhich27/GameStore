using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Models.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using GameStore.Models;
using System.Xml.Linq;

namespace GameStore.Services
{
    public class TelrApi
    {
        private const string BaseUrl = "https://secure.telr.com/tools/api/xml/";

        private static HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Constant.TelrAuthToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            return client;
        }

        public static async Task<IList<TransactionDetails>> Get_Recent_Transactions()
        {
            string content = string.Empty;
            using (var client = GetHttpClient())
            {   
                HttpResponseMessage response = await client.GetAsync("transaction");
                if (response.IsSuccessStatusCode) { content = await response.Content.ReadAsStringAsync(); }
            }
            return Xml_Helper.Get_Transactions(content);
        }

        public static async Task<IList<TransactionDetails>> Get_Transactions_By_Cart(string id)
        {
            string content = string.Empty;
            using (var client = GetHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("transaction/{0}/cart", id));
                if (response.IsSuccessStatusCode) { content = await response.Content.ReadAsStringAsync(); }
            }
            return Xml_Helper.Get_Transactions(content);
        }

        public static async Task<TransactionDetails> Get_Transaction_By_Id(string id)
        {
            string content = string.Empty;
            using (var client = GetHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("transaction/{0}", id));
                if (response.IsSuccessStatusCode) { content = await response.Content.ReadAsStringAsync(); }
            }
            return Xml_Helper.Get_Transaction(content);
        }
    }

    public class Xml_Helper
    {
        public static IList<TransactionDetails> Get_Transactions(string xml_content)
        {
            var list = new List<TransactionDetails>();
            try
            {
                XDocument xdoc = XDocument.Parse(xml_content);
                list = (from node in xdoc.Element(Xml_Nodes.TRANSACTIONS).Elements(Xml_Nodes.TRANSACTION)
                        select new TransactionDetails
                        {
                            id = node.Element(Xml_Nodes.ID)?.Value,
                            prev_id = node.Element(Xml_Nodes.PREV_ID)?.Value,
                            init_id = node.Element(Xml_Nodes.INIT_ID)?.Value,                            
                            amount = node.Element(Xml_Nodes.AMOUNT)?.Value,
                            currency = node.Element(Xml_Nodes.CURRENCY)?.Value,
                            description = node.Element(Xml_Nodes.DECRIPTION)?.Value,
                            cartid = node.Element(Xml_Nodes.CARTID)?.Value,
                            test = node.Element(Xml_Nodes.TEST)?.Value,
                            auth_code = node.Element(Xml_Nodes.AUTH).Element(Xml_Nodes.CODE)?.Value,
                            auth_message = node.Element(Xml_Nodes.AUTH).Element(Xml_Nodes.MESSAGE)?.Value,
                            auth_status = node.Element(Xml_Nodes.AUTH).Element(Xml_Nodes.STATUS)?.Value,
                            auth_statustxt = node.Element(Xml_Nodes.AUTH).Element(Xml_Nodes.STATUS_TXT)?.Value,
                            card_number = node.Element(Xml_Nodes.CARD).Element(Xml_Nodes.NUMBER)?.Value,
                            card_type = node.Element(Xml_Nodes.CARD).Element(Xml_Nodes.TYPE)?.Value,
                            card_country = node.Element(Xml_Nodes.CARD).Element(Xml_Nodes.COUNTRY)?.Value,
                            card_country_iso = node.Element(Xml_Nodes.CARD).Element(Xml_Nodes.COUNTRY_ISO)?.Value,
                            
                            billing = new Address
                            {
                                Title = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.TITLE)?.Value,
                                First_Name = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.FNAME)?.Value,
                                Last_Name = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.SNAME)?.Value,
                                Address_Line = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.ADDR1)?.Value,
                                Address_Line2 = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.ADDR2)?.Value,
                                Address_Line3 = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.ADDR3)?.Value,
                                City = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.CITY)?.Value,
                                State = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.REGION)?.Value,
                                Country = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.COUNTRY)?.Value,
                                ZipCode = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.ZIP)?.Value,
                                Mobile = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.TEL)?.Value,
                                Email = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.EMAIL)?.Value,
                                IP = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.IP)?.Value
                            }                            
                        }
                      ).ToList();
            }
            catch {  }
            return list;
        }

        public static TransactionDetails Get_Transaction(string xml_content)
        {
            var list = new TransactionDetails();
            try
            {
                XDocument xdoc = XDocument.Parse(xml_content);
                list = (from node in xdoc.Elements(Xml_Nodes.TRANSACTION)
                        select new TransactionDetails
                        {
                            id = node.Element(Xml_Nodes.ID)?.Value,
                            prev_id = node.Element(Xml_Nodes.PREV_ID)?.Value,
                            init_id = node.Element(Xml_Nodes.INIT_ID)?.Value,
                            amount = node.Element(Xml_Nodes.AMOUNT)?.Value,
                            currency = node.Element(Xml_Nodes.CURRENCY)?.Value,
                            description = node.Element(Xml_Nodes.DECRIPTION)?.Value,
                            cartid = node.Element(Xml_Nodes.CARTID)?.Value,
                            test = node.Element(Xml_Nodes.TEST)?.Value,
                            auth_code = node.Element(Xml_Nodes.AUTH).Element(Xml_Nodes.CODE)?.Value,
                            auth_message = node.Element(Xml_Nodes.AUTH).Element(Xml_Nodes.MESSAGE)?.Value,
                            auth_status = node.Element(Xml_Nodes.AUTH).Element(Xml_Nodes.STATUS)?.Value,
                            auth_statustxt = node.Element(Xml_Nodes.AUTH).Element(Xml_Nodes.STATUS_TXT)?.Value,
                            card_number = node.Element(Xml_Nodes.CARD).Element(Xml_Nodes.NUMBER)?.Value,
                            card_type = node.Element(Xml_Nodes.CARD).Element(Xml_Nodes.TYPE)?.Value,
                            card_country = node.Element(Xml_Nodes.CARD).Element(Xml_Nodes.COUNTRY)?.Value,
                            card_country_iso = node.Element(Xml_Nodes.CARD).Element(Xml_Nodes.COUNTRY_ISO)?.Value,

                            billing = new Address
                            {
                                Title = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.TITLE)?.Value,
                                First_Name = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.FNAME)?.Value,
                                Last_Name = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.SNAME)?.Value,
                                Address_Line = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.ADDR1)?.Value,
                                Address_Line2 = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.ADDR2)?.Value,
                                Address_Line3 = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.ADDR3)?.Value,
                                City = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.CITY)?.Value,
                                State = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.REGION)?.Value,
                                Country = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.COUNTRY)?.Value,
                                ZipCode = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.ZIP)?.Value,
                                Mobile = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.TEL)?.Value,
                                Email = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.EMAIL)?.Value,
                                IP = node.Element(Xml_Nodes.BILLING).Element(Xml_Nodes.IP)?.Value
                            }
                        }
                      ).FirstOrDefault();
            }
            catch { }
            return list;
        }
    }
}
