using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Transaction
    {
        
        public string auth_status { get; set; }
                
        public string auth_code { get; set; }
                
        public string auth_message { get; set; }
                
        public string auth_tranref { get; set; }
                
        public string auth_cvv { get; set; }
                
        public string auth_avs { get; set; }
        
        public string card_code { get; set; }
               
        public string card_desc { get; set; }
               
        public string cart_id { get; set; }
                
        public string cart_desc { get; set; }
                
        public string cart_currency { get; set; }
                
        public string cart_amount { get; set; }
                
        public string tran_currency { get; set; }
                
        public string tran_amount { get; set; }
              
        public string tran_cust_ip { get; set; }
               
        public string auth_hash { get; set; }
                
        public string bill_title { get; set; }
                
        public string bill_fname { get; set; }
                
        public string bill_sname { get; set; }
                
        public string bill_addr1 { get; set; }
                
        public string bill_addr2 { get; set; }
                
        public string bill_addr3 { get; set; }
                
        public string bill_city { get; set; }
                
        public string bill_region { get; set; }
                
        public string bill_country { get; set; }
               
        public string bill_zip { get; set; }
                
        public string bill_phone1 { get; set; }
                
        public string bill_email { get; set; }
               
        public string bill_hash { get; set; }
                
        public string delv_title { get; set; }
                
        public string delv_fname { get; set; }
                
        public string delv_sname { get; set; }
        
        public string delv_addr1 { get; set; }
        
        public string delv_addr2 { get; set; }
        
        public string delv_addr3 { get; set; }
        
        public string delv_city { get; set; }
        
        public string delv_region { get; set; }
        
        public string delv_Ccountry { get; set; }
        
        public string delv_zip { get; set; }
        
        public string delv_phone1 { get; set; }
        
        public string delv_hash { get; set; }
    }

    public class TransactionDetails
    {
        public long SNo { get; set; }
        public string id { get; set; }
        public string prev_id { get; set; }
        public string init_id { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string cartid { get; set; }
        public string test { get; set; }
        public string date { get; set; }
        public Address billing { get; set; }

        public string auth_status { get; set; }
        public string auth_statustxt { get; set; }
        public string auth_code { get; set; }
        public string auth_message { get; set; }

        public string card_number { get; set; }
        public string card_type { get; set; }
        public string card_country { get; set; }
        public string card_country_iso { get; set; }

        public TransactionDetails()
        {
            billing = new Address();
        }

    }

    

    
}
