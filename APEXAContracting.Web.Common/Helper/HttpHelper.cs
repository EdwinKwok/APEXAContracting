using APEXAContracting.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APEXAContracting.Web.Common.Helper
{
    public static class HttpHelper
    {





        /// <summary>
        ///  Http Post to call web api and return response. For create new entity process.
        ///  Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client?WT.mc_id=DT-MVP-5003235
        /// </summary>
        /// <typeparam name="T">Define the returned result's entity type.</typeparam>
        /// <typeparam name="TT">Define the input parameter's entity type.</typeparam>
        /// <param name="entity">Post an entity object as parameter.</param>
        /// <param name="rootUrl">Root url for http request.</param>
        /// <param name="url">Relative web api method call url.</param>
        /// <returns></returns>
        public static async Task<BusinessResult<T>> Post<T, TT>(string rootUrl, string url, TT entity)
        {
            BusinessResult<T> result = new BusinessResult<T>(ResultStatus.Failure);
            using (HttpClient client = GetHttpJsonClient(rootUrl))
            {
                // string inputJson = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                // HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsJsonAsync<TT>(url, entity);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsJsonAsync<BusinessResult<T>>();
                }

                return result;
            }
        }

        /// <summary>
        ///  Http Get to call web api and return response.
        ///  Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client?WT.mc_id=DT-MVP-5003235
        /// </summary>
        /// <param name="rootUrl">Root url for http request.</param>
        /// <param name="url">Relative web api method call url.</param>
        /// <typeparam name="T">Define the returned result's entity type.</typeparam>
        /// <returns></returns>
        public static async Task<BusinessResult<T>> Get<T>(string rootUrl, string url)
        {
            BusinessResult<T> result = new BusinessResult<T>(ResultStatus.Failure);
            using (HttpClient client = GetHttpJsonClient(rootUrl))
            {

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsJsonAsync<BusinessResult<T>>();
                }

                return result;
            }
        }


        /// <summary>
        ///  Http Get to call web api and return response.
        ///  Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client?WT.mc_id=DT-MVP-5003235
        /// </summary>
        /// <param name="url">Relative web api method call url.</param>
        /// <typeparam name="T">Define the returned result's entity type.</typeparam>
        /// <param name="parameters">Input parameters collection, which will be passed into web api through url parameters.</param>
        /// <returns></returns>
        public static async Task<BusinessResult<T>> Get<T>(string rootUrl, string url, Dictionary<string, string> parameters)
        {
            BusinessResult<T> result = new BusinessResult<T>(ResultStatus.Failure);
            string query = string.Empty;
            if (parameters != null)
            {
                query = new FormUrlEncodedContent(parameters).ReadAsStringAsync().Result;
            }

            if (!string.IsNullOrEmpty(query))
            {
                url += "?" + query;
            }

            result = await Get<T>(rootUrl, url);

            return result;
        }


        /// <summary>
        ///  Http Put to call web api and return response. For update entity process.
        ///  Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client?WT.mc_id=DT-MVP-5003235
        /// </summary>
        /// <typeparam name="T">Define the returned result's entity type.</typeparam>
        /// <typeparam name="TT">Define the input parameter's entity type.</typeparam>
        /// <param name="entity">Put an entity for update porcess.</param>
        /// <param name="url">Relative web api method call url.</param>
        /// <returns></returns>
        public static async Task<BusinessResult<T>> Put<T, TT>(string rootUrl, string url, TT entity)
        {
            BusinessResult<T> result = new BusinessResult<T>(ResultStatus.Failure);
            using (HttpClient client = GetHttpJsonClient(rootUrl))
            {

                HttpResponseMessage response = await client.PutAsJsonAsync<TT>(url, entity);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsJsonAsync<BusinessResult<T>>();
                }

                return result;
            }
        }


        /// <summary>
        ///  Http Delete to call web api and return response. For delete entity process.
        ///  Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client?WT.mc_id=DT-MVP-5003235
        /// </summary>
        /// <typeparam name="T">Define the returned result's entity type.</typeparam>
        /// <param name="rootUrl">Http request root url.</param>
        /// <param name="url">Relative web api method call url.</param>
        /// <returns></returns>
        public static async Task<BusinessResult<T>> Delete<T>(string rootUrl, string url)
        {
            BusinessResult<T> result = new BusinessResult<T>(ResultStatus.Failure);
            using (HttpClient client = GetHttpJsonClient(rootUrl))
            {
                HttpResponseMessage response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string x = await response.Content.ReadAsStringAsync();
                    result = await response.Content.ReadAsJsonAsync<BusinessResult<T>>();
                    
                }

                return result;
            }
        }


        public static HttpClient GetHttpJsonClient(string apiRootUrl)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiRootUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        /// <summary>
        ///  Http Post with Json object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url">It is relative http request url.</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
           this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content);
        }

        /// <summary>
        /// Http Put with Json object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url">It is relative http request url.</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(
           this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PutAsync(url, content);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            T value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }

    }
}
