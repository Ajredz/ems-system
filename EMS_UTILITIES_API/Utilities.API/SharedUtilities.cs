using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.API
{
 
    public static class SharedUtilities
    {
        public static ResultView _resultView = new ResultView();

        public enum SYSTEM_MODULE
        {
            SECURITY,
            PLANTILLA,
            IPM,
            MANPOWER,
            RECRUITMENT,
            ONBOARDING,
            WORKFLOW
        }

        public enum RESULT_TYPE
        {
            PASS_FAIL,
            APPROVE_REJECT,
            PASS_FAIL_SKIP,
            APPROVE_REJECT_SKIP
        }

        public enum MRF_APPROVER_STATUS
        {
            FOR_APPROVAL,
            APPROVED,
            REJECTED,
            CANCELLED
        }

        public enum MRF_STATUS
        {
            OPEN,
            CLOSED,
            REJECTED,
            CANCELLED
        }

        public enum APPROVE_REJECT
        {
            APPROVED,
            REJECTED
        }

        public enum UNIT_OF_TIME
        { 
            NONE,
            SECOND,
            MINUTE,
            HOUR,
            DAY
        }

        public static (DateTime? From, DateTime? To) GetDateRange(UNIT_OF_TIME unit, int value)
        {
            DateTime? from = null;
            DateTime? to = DateTime.Now;

            switch (unit)
            {
                case UNIT_OF_TIME.NONE:
                    from = null;
                    to = null;
                    break;
                case UNIT_OF_TIME.SECOND:
                    from = to.Value.AddSeconds(value * (-1));
                    break;
                case UNIT_OF_TIME.MINUTE:
                    from = to.Value.AddMinutes(value * (-1));
                    break;
                case UNIT_OF_TIME.HOUR:
                    from = to.Value.AddHours(value * (-1));
                    break;
                case UNIT_OF_TIME.DAY:
                    from = to.Value.AddDays(value * (-1));
                    break;
                default:
                    from = null;
                    to = null;
                    break;
            }
            return (from, to);
        }

        /// <summary>
        /// Executes HttpClient GetAsync from API by URL.
        /// </summary>
        /// <typeparam name="TModel">Model to be deserialized with.</typeparam>
        /// <param name="result">Model to be deserialized with.</param>
        /// <param name="URL">The URL the request is sent to.</param>
        /// <returns>APIResult: Deserialized JSON, IsSuccess: determine if request is success or not, ErrorMessage: Error Message to be displayed on Front End.</returns>
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

        //public static Task PutFromAPI(object bulkRemove, string uRL)
        //{
        //    throw new NotImplementedException();
        //}

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

        /// <summary>
        /// Assigning List collection to a SelectListItem.
        /// </summary>
        /// <typeparam name="TEntity">Model of the List parameter.</typeparam>
        /// <param name="list">Parameter to be transformed into a SelectListItem.</param>
        /// <param name="valueField">Name of the field to be placed on 'Value'.</param>
        /// <param name="mainTextField">Name of the field to be placed on 'Text'.</param>
        /// <param name="subTextField">Name of the field to be placed on 'Text' together with the mainTextField delimited by '-'.</param>
        /// <param name="selectedValue">Set the selected value of the Dropdown.</param>
        /// <returns></returns>
        public static List<SelectListItem> GetDropdown<TEntity>(List<TEntity> list, string valueField, string mainTextField, string subTextField = null, object selectedValue = null)
        {
            return list.Select(x => new SelectListItem
            {
                Value = Convert.ToString(x.GetType().GetProperty(valueField).GetValue(x)),
                Text =
                    string.Concat(
                        Convert.ToString(x.GetType().GetProperty(mainTextField).GetValue(x)),
                        string.IsNullOrEmpty(subTextField) ? "" :
                            string.Concat(" - ", Convert.ToString(x.GetType().GetProperty(subTextField).GetValue(x)))
                    ),
                Selected = Equals(Convert.ToString(x.GetType().GetProperty(valueField).GetValue(x)), Convert.ToString(selectedValue))
            }).OrderBy(x => x.Text).ToList();
        }


        public static void CreateWorkerServiceLog(string errorLogPath, string fileName, string message)
        {
            try
            {
                StringBuilder Output = new StringBuilder();
                Output.Append(message);
                //Output.Append(Environment.NewLine);

                if (!Directory.Exists(errorLogPath))
                {
                    System.IO.Directory.CreateDirectory(errorLogPath);
                }

                if (System.IO.File.Exists(errorLogPath + fileName))
                {
                    using TextWriter tr = System.IO.File.AppendText(errorLogPath + fileName);
                    tr.WriteLine(Output);
                }
                else
                {
                    using TextWriter tw = System.IO.File.CreateText(errorLogPath + fileName);
                    tw.WriteLine(Output);
                }
            }
            catch { }
        }
        public static string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new System.Security.Cryptography.SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }
    }
}