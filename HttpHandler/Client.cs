using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpHandler
{
    public class DataResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }


    }
    public class Client : IDisposable
    {
        private string BaseUrl = string.Empty;
        private Dictionary<string, string> headers = null;


        /// <summary>
        /// Initialize Client with Base Url of API
        /// </summary>
        /// <param name="baseUrl">API Base Url</param>
        public Client(string baseUrl) { BaseUrl = baseUrl; }

        public Client() { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                BaseUrl = string.Empty;
                headers = null;
            }
        }

        /// <summary>
        /// Add HTTP Header
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public void AddHeader(string key, string value)
        {
            if (headers == null) { headers = new Dictionary<string, string>(); }
            headers.Add(key, value);
        }


        /// <summary>
        /// Remove a Header
        /// </summary>
        /// <param name="key">Key</param>
        public void RemoveHeader(string key)
        {
            if (headers != null) { if (headers.ContainsKey(key)) { headers.Remove(key); } }
        }


        /// <summary>
        /// Clear all HTTP Headers
        /// </summary>
        public void ClearHeaders()
        {
            if (headers != null) { headers.Clear(); }
        }


        /// <summary>
        /// Get Data from your api 
        /// </summary>
        /// <typeparam name="T">Object that api return</typeparam>
        /// <param name="api">API Endpoint</param>
        /// <returns>Object of Data</returns>
        public T GetData<T>(string api)
        {
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode) { content = response.Content.ReadAsStringAsync().Result; }
            }
            return JsonConvert.DeserializeObject<T>(content);
        }


        /// <summary>
        /// Get Data Async from your api 
        /// </summary>
        /// <typeparam name="T">Object that api return</typeparam>
        /// <param name="api">API Endpoint</param>
        /// <returns>Object of Data</returns>
        public async Task<T> GetDataAsync<T>(string api)
        {
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(api);
                if (response.IsSuccessStatusCode) { content = await response.Content.ReadAsStringAsync(); }
            }
            return JsonConvert.DeserializeObject<T>(content);
        }


        /// <summary>
        /// Get Json string from your api 
        /// </summary>
        /// <param name="api">API Endpoint</param>
        /// <returns>Json String</returns>
        public async Task<string> GetJsonAsync(string api)
        {
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(api);
                if (response.IsSuccessStatusCode) { content = await response.Content.ReadAsStringAsync(); }
            }
            return content;
        }


        /// <summary>
        /// Get Data With Status Code
        /// </summary>
        /// <typeparam name="T">Object that api return</typeparam>
        /// <param name="api">API Endpoint</param>
        /// <returns>Data</returns>
        public DataResult<T> GetData_WithStatusCode<T>(string api)
        {
            var result = new DataResult<T>();
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode) { content = response.Content.ReadAsStringAsync().Result; }
                else { result.Content = response.Content.ReadAsStringAsync().Result; }
                result.Success = response.IsSuccessStatusCode;
                result.Message = response.ReasonPhrase;
                result.Data = JsonConvert.DeserializeObject<T>(content);
            }
            return result;
        }




        /// <summary>
        /// Get Data With Status Code Async
        /// </summary>
        /// <typeparam name="T">Object that api return</typeparam>
        /// <param name="api">API Endpoint</param>
        /// <returns>Data with Status</returns>
        public async Task<DataResult<T>> GetData_WithStatusCode_Async<T>(string api)
        {
            var result = new DataResult<T>();
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {                
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(api);
                if (response.IsSuccessStatusCode) { content = await response.Content.ReadAsStringAsync(); }
                else { result.Content = await response.Content.ReadAsStringAsync(); }
                result.Success = response.IsSuccessStatusCode;
                result.Message = response.ReasonPhrase;
                result.Data = JsonConvert.DeserializeObject<T>(content);
            }
            return result;
        }


        /// <summary>
        /// Get Json string With StatusCode
        /// </summary>
        /// <param name="api">API Endpoint</param>
        /// <returns>Json string with Status</returns>
        public async Task<DataResult<string>> GetJson_WithStatusCode_Async(string api)
        {
            var result = new DataResult<string>();
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(api);
                if (response.IsSuccessStatusCode) { content = await response.Content.ReadAsStringAsync(); }
                else { result.Content = await response.Content.ReadAsStringAsync(); }
                result.Success = response.IsSuccessStatusCode;
                result.Message = response.ReasonPhrase;
                result.Data = content;
            }
            return result;
        }


        /// <summary>
        ///  Post Data
        /// </summary>
        /// <typeparam name="T">Object that api return</typeparam>
        /// <param name="api">API Endpoint</param>
        /// <param name="obj">Data to POST on API</param>
        /// <returns>Object of Data</returns>
        public T PostData<T>(string api, object obj)
        {
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string data = JsonConvert.SerializeObject(obj);
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent httpContent = new StringContent(data, UTF8Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(api, httpContent).Result;
                if (response.IsSuccessStatusCode) { content = response.Content.ReadAsStringAsync().Result; }
            }
            return JsonConvert.DeserializeObject<T>(content);
        }


        /// <summary>
        /// Post Data Async
        /// </summary>
        /// <typeparam name="T">Object that api return</typeparam>
        /// <param name="api">API Endpoint</param>
        /// <param name="obj">Data to POST on API</param>
        /// <returns>Object of Data</returns>
        public async Task<T> PostDataAsync<T>(string api, object obj)
        {
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string data = JsonConvert.SerializeObject(obj);
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent httpContent = new StringContent(data, UTF8Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(api, httpContent);
                if (response.IsSuccessStatusCode) { content = await response.Content.ReadAsStringAsync(); }
            }
            return JsonConvert.DeserializeObject<T>(content);
        }


        /// <summary>
        /// Post Data With Status Code
        /// </summary>
        /// <typeparam name="T">Object that api return</typeparam>
        /// <param name="api">API Endpoint</param>
        /// <param name="obj">Data to POST on API</param>
        /// <returns>Data with Status</returns>
        public DataResult<T> PostData_WithStatusCode<T>(string api, object obj)
        {
            var result = new DataResult<T>();
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string data = JsonConvert.SerializeObject(obj);
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent httpContent = new StringContent(data, UTF8Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(api, httpContent).Result;
                if (response.IsSuccessStatusCode) { content = response.Content.ReadAsStringAsync().Result; }
                else { result.Content = response.Content.ReadAsStringAsync().Result; }
                result.Success = response.IsSuccessStatusCode;
                result.Message = response.ReasonPhrase;
                result.Data = JsonConvert.DeserializeObject<T>(content);
            }
            return result;
        }


        /// <summary>
        /// Post Data With Status Code Async
        /// </summary>
        /// <typeparam name="T">Object that api return</typeparam>
        /// <param name="api">API Endpoint</param>
        /// <param name="obj">Data to POST on API </param>
        /// <returns>Data with Status</returns>
        public async Task<DataResult<T>> PostData_WithStatusCode_Async<T>(string api, object obj)
        {
            var result = new DataResult<T>();
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string data = JsonConvert.SerializeObject(obj);
                if (!string.IsNullOrEmpty(BaseUrl)) { client.BaseAddress = new Uri(BaseUrl); }
                if (headers != null) { foreach (var x in headers) { client.DefaultRequestHeaders.TryAddWithoutValidation(x.Key, x.Value); } }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent httpContent = new StringContent(data, UTF8Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(api, httpContent);
                if (response.IsSuccessStatusCode) { content = await response.Content.ReadAsStringAsync(); }
                else { result.Content = await response.Content.ReadAsStringAsync(); }
                result.Success = response.IsSuccessStatusCode;
                result.Message = response.ReasonPhrase;
                result.Data = JsonConvert.DeserializeObject<T>(content);
            }
            return result;
        }
    }
}
