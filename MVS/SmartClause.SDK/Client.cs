using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Smartclause.SDK.DTO;
using Smartclause.SDK.Tools;
using SmartClause.SDK.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Smartclause.SDK
{
    public partial class Client
    {
        private string _baseUrl;
        private string _loginEndpoint = "/Token";
        private string _accessTokenCookie = "scm_api_access_token";
        private string _refreshTokenCookie = "scm_api_refresh_token";

        private string Login { get; set; }
        private string Password { get; set; }
        private readonly IHttpContextAccessor _contextAccessor;

        public Client(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Client(string login, string password, string baseUrl)
        {
            this.Login = login;
            this.Password = password;
            this._baseUrl = baseUrl;
        }

        /// <summary>
        /// Create an HttpWebRequest object, used to make authenticated calls to the API
        /// </summary>
        /// <param name="endpoint">The endpoint to call on the API</param>
        /// <param name="requestMethod">The request method, could be "GET", "POST", "PUT", "DELETE"...</param>
        /// <param name="contentType">The content type of the request</param>
        /// <param name="accept">The accept of the request</param>
        /// <param name="bodyObject">the object to pass in the body request if needed</param>
        /// <returns>A new HttpWebRequest object</returns>
        private async Task<HttpWebRequest> CreateHttpWebRequest(string endpoint, string requestMethod,
            string contentType = "application/json", string accept = "application/json, text/javascript, */*; q=0.01",
            object bodyObject = null)
        {
            var accessToken = await Logon();

            var request = WebRequest.CreateHttp(_baseUrl + endpoint);
            request.Headers[HttpRequestHeader.Authorization] = $"Bearer {accessToken}";
            request.Method = requestMethod;
            request.Accept = accept;
            request.ContentType = contentType;

            if (bodyObject != null)
            {
                Stream requestStream = request.GetRequestStream();
                StreamWriter sw = new(requestStream);
                await sw.WriteAsync(JsonConvert.SerializeObject(bodyObject));
                await sw.FlushAsync();
            }

            return request;
        }

        private async Task<HttpWebRequest> CreateHttpWebHookRequest(string endpoint, string contentType = "text/plain", string bodyObject = null)
        {
            HttpWebRequest request = WebRequest.CreateHttp(_baseUrl + endpoint);
            request.Method = "POST";
            request.ContentType = contentType;

            if (bodyObject != null)
            {
                Stream requestStream = request.GetRequestStream();
                StreamWriter sw = new(requestStream);
                await sw.WriteAsync(bodyObject);
                await sw.FlushAsync();
            }

            return request;
        }

        /// <summary>
        /// Make the API call and return the deserialized response
        /// </summary>
        /// <param name="request">the request to make</param>
        /// <typeparam name="T">the deserialize type</typeparam>
        /// <returns>the object deserialized</returns>
        /// <exception cref="Exception">throw an exception if something went wrong</exception>
        private async Task<T> GetResponseAsObject<T>(WebRequest request)
        {
            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            if (responseStream == null)
                throw new Exception("Response stream is invalid");

            var streamReader = new StreamReader(responseStream);
            return JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd());
        }


        public async Task<string> Logon()
        {
            try
            {
                // var fromAccessToken = await tryLogonWithToken();
                // if (fromAccessToken != null)
                //     return fromAccessToken;

                var body = "grant_type=password&username=" + HttpUtility.UrlEncode(Login) + "&password=" +
                           HttpUtility.UrlEncode(Password);

                var b = Encoding.Default.GetBytes(body);

                HttpWebRequest request = HttpWebRequest.CreateHttp(_baseUrl + _loginEndpoint);

                request.Method = "POST";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.ContentType = "application/json";
                var ms = request.GetRequestStream();


                StreamWriter sw = new StreamWriter(ms);
                sw.Write(body);
                sw.Flush();
                var response = await request.GetResponseAsync();
                var responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);

                var res = JsonConvert.DeserializeObject<LogonResult>(sr.ReadToEnd());

                return res.access_token;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<LogonResult> LogonExtended()
        {
            try
            {
                var body = "grant_type=password&username=" + HttpUtility.UrlEncode(Login) + "&password=" +
                           HttpUtility.UrlEncode(Password);

                var b = Encoding.Default.GetBytes(body);

                HttpWebRequest request = HttpWebRequest.CreateHttp(_baseUrl + _loginEndpoint);

                request.Method = "POST";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.ContentType = "application/json";
                var ms = request.GetRequestStream();


                StreamWriter sw = new StreamWriter(ms);
                sw.Write(body);
                sw.Flush();
                var response = await request.GetResponseAsync();
                var responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);

                var res = JsonConvert.DeserializeObject<LogonResult>(sr.ReadToEnd());

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<TenantConfiguration> GetConfiguration(string tenantId)
        {
            var request = await CreateHttpWebRequest("api/user/configuration", "GET");
            request.Headers.Add("TenantId", tenantId);
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<TenantConfiguration>(sr.ReadToEnd());
        }

        public async Task<TenantAssistantAccountKeyVault> GetAssistantAccountKeyVault(string tenantId)
        {
            var request = await CreateHttpWebRequest("api/user/assistantAccountKeyVault", "GET");
            request.Headers.Add("TenantId", tenantId);
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<TenantAssistantAccountKeyVault>(sr.ReadToEnd());
        }

        public async Task<LogonResult> RefreshToken(string refreshToken)
        {
            try
            {
                var body = "grant_type=refresh_token&refresh_token=" + HttpUtility.UrlEncode(refreshToken);

                var b = Encoding.Default.GetBytes(body);

                HttpWebRequest request = HttpWebRequest.CreateHttp(_baseUrl + _loginEndpoint);

                request.Method = "POST";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.ContentType = "application/json";
                var ms = request.GetRequestStream();


                StreamWriter sw = new StreamWriter(ms);
                sw.Write(body);
                sw.Flush();
                var response = await request.GetResponseAsync();
                var responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);

                var res = JsonConvert.DeserializeObject<LogonResult>(sr.ReadToEnd());

                return res;
            }
            catch (WebException ex)
            {
                _contextAccessor.HttpContext.Response.Redirect("/Account/Relog");
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<string> tryLogonWithToken()
        {
            // Check if cookie exist from "AllKey". Not using [_refreshTokenCookie] because it would create it if it doesn't exist
            if (_contextAccessor.HttpContext.Request.Cookies.Any(c => c.Key == _refreshTokenCookie))
            {
                if (_contextAccessor.HttpContext.Request.Cookies[_accessTokenCookie] != null &&
                    checkAccessToken(_contextAccessor.HttpContext.Request.Cookies[_accessTokenCookie]))
                    return _contextAccessor.HttpContext.Request.Cookies[_accessTokenCookie];
                var logon = await RefreshToken(_contextAccessor.HttpContext.Request.Cookies[_refreshTokenCookie]);
                if (logon != null)
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddSeconds(logon.expires_in);
                    _contextAccessor.HttpContext.Response.Cookies.Append(_accessTokenCookie, logon.access_token,
                        option);

                    CookieOptions optionRefresh = new CookieOptions();
                    optionRefresh.Expires = DateTime.Now.AddDays(365);
                    _contextAccessor.HttpContext.Response.Cookies.Append(_refreshTokenCookie, logon.refresh_token,
                        optionRefresh);

                    return logon.access_token;
                }
            }

            return null;
        }

        private bool checkAccessToken(string access_token)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(_baseUrl + "/api/Account/UserInfo");
                request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", access_token));
                request.Method = "GET";
                var response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                return false;
            }
        }

        private Dictionary<string, string> GetParametersDictionary(object requestDto)
        {
            Dictionary<string, string> parameterPairs = requestDto.GetType()
                .GetProperties()
                .ToDictionary(
                    propertyInfo =>
                    {
                        string name = propertyInfo.Name.PascalToCamelCase();
                        return name;
                    },
                    propertyInfo =>
                    {
                        object valueObject = propertyInfo.GetValue(requestDto);
                        string valueString = valueObject?.ToString() ?? "";
                        return valueString;
                    });

            return parameterPairs;
        }
    }
}