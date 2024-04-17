using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using EMS.External.API.Shared;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http.Headers;

namespace EMS.External.API.Shared
{
    public class Utilities
    {

        public static async Task<(TModel APIResult, bool IsSuccess, string ErrorMessage)> GetFromAPI<TModel>(TModel result, string URL)
        {
            bool isSuccess = false;
            string errorMessage = "";

            using (HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite)
            })
            using (HttpResponseMessage res = await client.GetAsync(URL))
            {
                isSuccess = res.IsSuccessStatusCode;

                if (isSuccess)
                {
                    using HttpContent content = res.Content;
                    string data = await content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(data))
                    {
                        result = JsonConvert.DeserializeObject<TModel>(data);
                    }
                    else
                    {
                        isSuccess = false;
                        errorMessage = res.ReasonPhrase;
                    }
                }
                else
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(data))
                        {
                            errorMessage = await content.ReadAsStringAsync();
                        }
                        else
                        {
                            errorMessage = res.ReasonPhrase;
                        }
                    }
                    isSuccess = false;
                }
            }

            return (result, isSuccess, errorMessage);
        }

        /// <summary>
        /// Executes HttpClient PostAsync from API by URL.
        /// </summary>
        /// <typeparam name="TModel">Model to be deserialized with.</typeparam>
        /// <typeparam name="TModelParam">Model to be serialized with.</typeparam>
        /// <param name="result">Paremeter to be deserialized with.</param>
        /// <param name="param">Paremeter to be serialized with and to be sent to the request.</param>
        /// <param name="URL">The URL the request is sent to.</param>
        /// <returns>APIResult: Deserialized JSON, IsSuccess: determine if request is success or not, ErrorMessage: Error Message to be displayed on Front End.</returns>
        public static async Task<(TModel APIResult, bool IsSuccess, string ErrorMessage)> PostFromAPI<TModel, TModelParam>(TModel result, TModelParam param, string URL = "")
        {
            bool isSuccess = false;
            string errorMessage = "";

            using (HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite)
            })
            using (HttpResponseMessage res = await client.PostAsync(URL,
                new StringContent(JsonConvert.SerializeObject(param),
                Encoding.UTF8,
                "application/json"))
            )
            {
                isSuccess = res.IsSuccessStatusCode;
                if (res.IsSuccessStatusCode)
                {
                    using HttpContent content = res.Content;
                    string data = await content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(data))
                    {
                        result = JsonConvert.DeserializeObject<TModel>(data);
                    }
                    else
                    {
                        isSuccess = false;
                        errorMessage = res.ReasonPhrase;
                    }
                }
                else
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(data))
                        {
                            errorMessage = await content.ReadAsStringAsync();
                        }
                        else
                        {
                            errorMessage = res.ReasonPhrase;
                        }
                    }
                    isSuccess = false;
                }
            }

            return (result, isSuccess, errorMessage);
        }

        /// <summary>
        /// Executes HttpClient PostAsync from API by URL.
        /// </summary>
        /// <typeparam name="TModelParam">Model to be serialized with.</typeparam>
        /// <param name="param">Paremeter to be serialized with and to be sent to the request.</param>
        /// <param name="URL">The URL the request is sent to.</param>
        /// <returns>IsSuccess: determine if request is success or not, Message: Message to be displayed on Front End.</returns>
        public static async Task<(bool IsSuccess, string Message)> PostFromAPI<TModelParam>(TModelParam param, string URL = "")
        {
            bool isSuccess = false;
            string message = "";

            using (HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite)
            })
            using (HttpResponseMessage res = await client.PostAsync(URL,
                new StringContent(JsonConvert.SerializeObject(param),
                Encoding.UTF8,
                "application/json"))
            )
            {
                using HttpContent content = res.Content;
                string data = await content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                    message = data;
                else
                    message = res.ReasonPhrase;

                isSuccess = res.IsSuccessStatusCode;
            }
            return (isSuccess, message);
        }

        public static async Task<(bool IsSuccess, string Message)> PostFromAPIWithHeader<TModelParam>(TModelParam param, string URL = "", string HeaderName = "", string HeaderValue = "")
        {
            bool isSuccess = false;
            string message = "";

            using (HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite)
            })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add(HeaderName, HeaderValue);
                using (HttpResponseMessage res = await client.PostAsync(URL,
                    new StringContent(JsonConvert.SerializeObject(param),
                    Encoding.UTF8,
                    "application/json"))
                )
                {
                    using HttpContent content = res.Content;
                    string data = await content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(data))
                        message = data;
                    else
                        message = res.ReasonPhrase;

                    isSuccess = res.IsSuccessStatusCode;
                }
            }
            return (isSuccess, message);
        }

        /// <summary>
        /// Executes HttpClient PutAsync from API by URL.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TModelParam">Model to be serialized with.</typeparam>
        /// <param name="result">Paremeter to be deserialized with.</param>
        /// <param name="param">Paremeter to be serialized with and to be sent to the request.</param>
        /// <param name="URL">The URL the request is sent to.</param>
        /// <returns>APIResult: Deserialized JSON, IsSuccess: determine if request is success or not, ErrorMessage: Error Message to be displayed on Front End.</returns>
        public static async Task<(TModel APIResult, bool IsSuccess, string ErrorMessage)> PutFromAPI<TModel, TModelParam>(TModel result, TModelParam param, string URL = "")
        {
            bool isSuccess = false;
            string errorMessage = "";

            using (HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite)
            })
            using (HttpResponseMessage res = await client.PutAsync(URL,
                new StringContent(JsonConvert.SerializeObject(param),
                Encoding.UTF8,
                "application/json"))
            )
            {
                isSuccess = res.IsSuccessStatusCode;
                if (res.IsSuccessStatusCode)
                {
                    using HttpContent content = res.Content;
                    string data = await content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(data))
                    {
                        result = JsonConvert.DeserializeObject<TModel>(data);
                    }
                    else
                    {
                        isSuccess = false;
                        errorMessage = res.ReasonPhrase;
                    }
                }
                else
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(data))
                        {
                            errorMessage = await content.ReadAsStringAsync();
                        }
                        else
                        {
                            errorMessage = res.ReasonPhrase;
                        }
                    }
                    isSuccess = false;
                }
            }

            return (result, isSuccess, errorMessage);
        }

        /*public static Task PutFromAPI(object bulkRemove, string uRL)
        {
            throw new NotImplementedException();
        }*/

        /// <summary>
        /// Executes HttpClient PutAsync from API by URL.
        /// </summary>
        /// <typeparam name="TModelParam">Model to be serialized with.</typeparam>
        /// <param name="param">Paremeter to be serialized with and to be sent to the request.</param>
        /// <param name="URL">The URL the request is sent to.</param>
        /// <returns>IsSuccess: determine if request is success or not, Message: Message to be displayed on Front End.</returns>
        public static async Task<(bool IsSuccess, string Message)> PutFromAPI<TModelParam>(TModelParam param, string URL = "")
        {
            bool isSuccess = false;
            string message = "";

            using (HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite)
            })
            using (HttpResponseMessage res = await client.PutAsync(URL,
                new StringContent(JsonConvert.SerializeObject(param),
                Encoding.UTF8,
                "application/json"))
            )
            {
                using HttpContent content = res.Content;
                string data = await content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                    message = data;
                else
                    message = res.ReasonPhrase;

                isSuccess = res.IsSuccessStatusCode;
            }

            return (isSuccess, message);
        }

        /// <summary>
        /// Executes HttpClient DeleteAsync from API by URL.
        /// </summary>
        /// <param name="URL">The URL the request is sent to.</param>
        /// <returns>IsSuccess: determine if request is success or not, Message: Message to be displayed on Front End.</returns>
        public static async Task<(bool IsSuccess, string Message)> DeleteFromAPI(string URL)
        {
            bool isSuccess = false;
            string message = "";

            using (HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite)
            })
            using (HttpResponseMessage res = await client.DeleteAsync(URL))
            {
                isSuccess = res.IsSuccessStatusCode;

                using HttpContent content = res.Content;
                string data = await content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                    message = data;
                else
                    message = res.ReasonPhrase;

                isSuccess = res.IsSuccessStatusCode;
            }

            return (isSuccess, message);
        }
        public static string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }
    }
}