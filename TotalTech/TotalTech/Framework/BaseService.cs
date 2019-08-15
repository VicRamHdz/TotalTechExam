using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TotalTech.Models;

namespace TotalTech.Framework
{
    public abstract class BaseService : IDisposable
    {
        #region Private Members
        private HttpClient _client;
        private string _endpointUrl;
        #endregion

        #region Private Methods
        private Uri UriBuilder(string endpoint) => new Uri(string.Concat(_endpointUrl, endpoint));

        #endregion

        #region Protected Methods
        protected BaseService()
        {
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(300)
            };
            _endpointUrl = Constants.BaseApiUrl;
        }

        protected async Task<ResponseResult<ReturnValue>> GetAsync<ReturnValue>(string endpoint, CancellationToken token = new CancellationToken())
        {
            var res = new ResponseResult<ReturnValue>();
            HttpResponseMessage response = new HttpResponseMessage();
            var url = UriBuilder(endpoint);

            try
            {
                response = await _client.GetAsync(url, token);
                res = await GettingStandardResponse<ReturnValue>(response);
            }
            catch (Exception ex)
            {
                ex.InnerException.Source = $"Error parsing: \n Type: {nameof(ReturnValue)} \n At method: GetAsync \n URL: {UriBuilder(endpoint)} \n Json Response: {response.ReasonPhrase}";
                throw ex;
            }

            return res;
        }

        protected async Task<ResponseResult<ReturnValue>> PostAsync<BodyData, ReturnValue>(string endpoint, BodyData requestBody)
        {
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await PostAsync<ReturnValue>(endpoint, content);
        }

        protected async Task<ResponseResult<ReturnValue>> PostAsync<ReturnValue>(string endpoint, HttpContent content)
        {
            var res = new ResponseResult<ReturnValue>();
            HttpResponseMessage response = new HttpResponseMessage();
            var url = UriBuilder(endpoint);

            try
            {
                response = await _client.PostAsync(url, content);
                res = await GettingStandardResponse<ReturnValue>(response);
            }
            catch (Exception ex)
            {
                ex.Data.Add("ErrorInfo", $"Error parsing: \n Type: {nameof(ReturnValue)} \n At method: PostAsync \n URL: {UriBuilder(endpoint)} \n Json Response: {response.ReasonPhrase}");
                throw ex;
            }

            return res;

        }

        protected async Task<ResponseResult<ReturnValue>> PutAsync<BodyData, ReturnValue>(string endpoint, BodyData requestBody)
        {
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await PutAsync<ReturnValue>(endpoint, content);
        }

        protected async Task<ResponseResult<ReturnValue>> PutAsync<ReturnValue>(string endpoint, HttpContent content)
        {
            var res = new ResponseResult<ReturnValue>();
            HttpResponseMessage response = new HttpResponseMessage();
            var url = UriBuilder(endpoint);

            try
            {
                response = await _client.PutAsync(url, content);
                res = await GettingStandardResponse<ReturnValue>(response);
            }
            catch (Exception ex)
            {
                ex.Data.Add("ErrorInfo", $"Error parsing: \n Type: {nameof(ReturnValue)} \n At method: PutAsync \n URL: {UriBuilder(endpoint)} \n Json Response: {response.ReasonPhrase}");
                throw ex;
            }

            return res;

        }

        private async Task<ResponseResult<ReturnValue>> GettingStandardResponse<ReturnValue>(HttpResponseMessage response)
        {
            var res = new ResponseResult<ReturnValue>();
            if (response.IsSuccessStatusCode)
            {
                res.StatusCode = 200;
                res.Status = "Success";
                res.Message = "Call success";
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var responseContent = await response.Content.ReadAsStringAsync();
                res.Data = JsonConvert.DeserializeObject<ReturnValue>(responseContent, settings);
            }
            else if (response.StatusCode >= System.Net.HttpStatusCode.BadRequest && response.StatusCode < System.Net.HttpStatusCode.InternalServerError)
            {
                res.StatusCode = 400;
                res.Status = "BadRequest";
                res.Data = default(ReturnValue);
                var badResponseContent = await response.Content.ReadAsStringAsync();
                res.Message = badResponseContent;
                res.Message = JsonConvert.DeserializeObject<BadRequestResponseResult>(badResponseContent).error;
            }
            else if (response.StatusCode >= System.Net.HttpStatusCode.InternalServerError)
            {
                res.StatusCode = 500;
                res.Status = "InternalServerError";
                res.Data = default(ReturnValue);
                //res.Message = response.ReasonPhrase;
                res.Message = await response.Content.ReadAsStringAsync();
            }
            return res;
        }

        #endregion

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}