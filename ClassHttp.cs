using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace LC_Service
{
    public class ClassHttp
    {
        private string _mLastErrorMessage = string.Empty;

        public bool SendThingPlugRequest(string pUrl, ClassLORA.HEADER_METHOD pMethod, ClassLORA.HEADER_ACCEPT pAccept,
            List<KeyValuePair<string, string>> pHeaders, out ClassLORA.HTTP_RESPONSE pResponse)
        {
            bool result = false;
            pResponse = new ClassLORA.HTTP_RESPONSE();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);

            ServicePointManager.Expect100Continue = true;
            // .Net Framework 4.5
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            // .Net Framework 4.0
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                request.AllowWriteStreamBuffering = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.KeepAlive = true;
                switch (pMethod)
                {
                    case ClassLORA.HEADER_METHOD.HTTP_GET: request.Method = "GET"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_POST: request.Method = "POST"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_PUT: request.Method = "PUT"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_DELETE: request.Method = "DELETE"; break;
                }

                switch (pAccept)
                {
                    case ClassLORA.HEADER_ACCEPT.RESPONSE_XML: request.Accept = "application/xml"; break;
                    case ClassLORA.HEADER_ACCEPT.RESPONSE_JSON: request.Accept = "application/json"; break;
                }

                foreach (KeyValuePair<string, string> header in pHeaders)
                {
                    request.Headers[header.Key] = header.Value;
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < response.Headers.Count; i++)
                {
                    headerList.Add(new KeyValuePair<string, string>(response.Headers.Keys[i], response.Headers[i]));
                }
                string sBody = new StreamReader(response.GetResponseStream()).ReadToEnd();

                pResponse.HEADERS = headerList;
                pResponse.BODY = sBody;

                response.Close();
                result = true;
            }
            catch (WebException webExp)
            {
                string error = string.Empty;
                if (webExp.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)webExp.Response)
                    {
                        List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
                        for (int i = 0; i < errorResponse.Headers.Count; i++)
                        {
                            headerList.Add(new KeyValuePair<string, string>(errorResponse.Headers.Keys[i], errorResponse.Headers[i]));
                        }
                        string sBody = new StreamReader(errorResponse.GetResponseStream()).ReadToEnd();

                        pResponse.HEADERS = headerList;
                        pResponse.BODY = string.Empty;

                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }

                if (string.IsNullOrEmpty(error))
                    _mLastErrorMessage = webExp.Message;
                else
                    _mLastErrorMessage = error;
            }
            catch (Exception Exp)
            {
                _mLastErrorMessage = Exp.Message;
            }

            return result;
        }

        public bool SendThingPlugRequest(string pUrl, ClassLORA.HEADER_METHOD pMethod, ClassLORA.HEADER_ACCEPT pAccept,
            List<KeyValuePair<string, string>> pHeaders, string pContentType, string pBody, out ClassLORA.HTTP_RESPONSE pResponse)
        {
            bool result = false;
            pResponse = new ClassLORA.HTTP_RESPONSE();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);

            ServicePointManager.Expect100Continue = true;
            // .Net Framework 4.5
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            // .Net Framework 4.0
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                request.AllowWriteStreamBuffering = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.KeepAlive = true;
                switch (pMethod)
                {
                    case ClassLORA.HEADER_METHOD.HTTP_GET: request.Method = "GET"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_POST: request.Method = "POST"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_PUT: request.Method = "PUT"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_DELETE: request.Method = "DELETE"; break;
                }

                switch (pAccept)
                {
                    case ClassLORA.HEADER_ACCEPT.RESPONSE_XML: request.Accept = "application/xml"; break;
                    case ClassLORA.HEADER_ACCEPT.RESPONSE_JSON: request.Accept = "application/json"; break;
                }

                foreach (KeyValuePair<string, string> header in pHeaders)
                {
                    request.Headers[header.Key] = header.Value;
                }
                request.ContentType = pContentType;
                request.ContentLength = pBody.Length;

                var parameters = Encoding.UTF8.GetBytes(pBody);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(parameters, 0, parameters.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < response.Headers.Count; i++)
                {
                    headerList.Add(new KeyValuePair<string, string>(response.Headers.Keys[i], response.Headers[i]));
                }
                string sBody = new StreamReader(response.GetResponseStream()).ReadToEnd();

                pResponse.HEADERS = headerList;
                pResponse.BODY = sBody;

                response.Close();
                result = true;
            }
            catch (WebException webExp)
            {
                string error = string.Empty;
                if (webExp.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)webExp.Response)
                    {
                        List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
                        for (int i = 0; i < errorResponse.Headers.Count; i++)
                        {
                            headerList.Add(new KeyValuePair<string, string>(errorResponse.Headers.Keys[i], errorResponse.Headers[i]));
                        }
                        string sBody = new StreamReader(errorResponse.GetResponseStream()).ReadToEnd();

                        pResponse.HEADERS = headerList;
                        pResponse.BODY = string.Empty;

                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }

                if (string.IsNullOrEmpty(error))
                    _mLastErrorMessage = webExp.Message;
                else
                    _mLastErrorMessage = error;
            }
            catch (Exception Exp)
            {
                _mLastErrorMessage = Exp.Message;
            }

            return result;
        }

        public bool SendContelaRequest(string pUrl, ClassLORA.HEADER_METHOD pMethod, List<KeyValuePair<string, string>> pHeaders, out ClassLORA.HTTP_RESPONSE pResponse)
        {
            bool result = false;
            pResponse = new ClassLORA.HTTP_RESPONSE();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);

            ServicePointManager.Expect100Continue = true;
            // .Net Framework 4.5
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            // .Net Framework 4.0
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                request.AllowWriteStreamBuffering = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.KeepAlive = true;
                switch (pMethod)
                {
                    case ClassLORA.HEADER_METHOD.HTTP_GET: request.Method = "GET"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_POST: request.Method = "POST"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_PUT: request.Method = "PUT"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_DELETE: request.Method = "DELETE"; break;
                }

                request.Accept = "application/json";
                request.ContentType = "application/json";

                foreach (KeyValuePair<string, string> header in pHeaders)
                {
                    request.Headers[header.Key] = header.Value;
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < response.Headers.Count; i++)
                {
                    headerList.Add(new KeyValuePair<string, string>(response.Headers.Keys[i], response.Headers[i]));
                }
                string sBody = new StreamReader(response.GetResponseStream()).ReadToEnd();

                pResponse.HEADERS = headerList;
                pResponse.BODY = sBody;

                response.Close();
                result = true;
            }
            catch (WebException webExp)
            {
                string error = string.Empty;
                if (webExp.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)webExp.Response)
                    {
                        List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
                        for (int i = 0; i < errorResponse.Headers.Count; i++)
                        {
                            headerList.Add(new KeyValuePair<string, string>(errorResponse.Headers.Keys[i], errorResponse.Headers[i]));
                        }
                        string sBody = new StreamReader(errorResponse.GetResponseStream()).ReadToEnd();

                        pResponse.HEADERS = headerList;
                        pResponse.BODY = string.Empty;

                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }

                if (string.IsNullOrEmpty(error))
                    _mLastErrorMessage = webExp.Message;
                else
                    _mLastErrorMessage = error;
            }
            catch (Exception Exp)
            {
                _mLastErrorMessage = Exp.Message;
            }

            return result;
        }

        public bool SendContelaRequest(string pUrl, ClassLORA.HEADER_METHOD pMethod, List<KeyValuePair<string, string>> pHeaders, string pBody, out ClassLORA.HTTP_RESPONSE pResponse)
        {
            bool result = false;
            pResponse = new ClassLORA.HTTP_RESPONSE();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);

            ServicePointManager.Expect100Continue = true;
            // .Net Framework 4.5
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            // .Net Framework 4.0
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                request.AllowWriteStreamBuffering = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.KeepAlive = true;
                switch (pMethod)
                {
                    case ClassLORA.HEADER_METHOD.HTTP_GET: request.Method = "GET"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_POST: request.Method = "POST"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_PUT: request.Method = "PUT"; break;
                    case ClassLORA.HEADER_METHOD.HTTP_DELETE: request.Method = "DELETE"; break;
                }

                request.Accept = "application/json";
                request.ContentType = "application/json";

                foreach (KeyValuePair<string, string> header in pHeaders)
                {
                    request.Headers[header.Key] = header.Value;
                }
                request.ContentLength = pBody.Length;

                var parameters = Encoding.UTF8.GetBytes(pBody);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(parameters, 0, parameters.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < response.Headers.Count; i++)
                {
                    headerList.Add(new KeyValuePair<string, string>(response.Headers.Keys[i], response.Headers[i]));
                }
                string sBody = new StreamReader(response.GetResponseStream()).ReadToEnd();

                pResponse.HEADERS = headerList;
                pResponse.BODY = sBody;

                response.Close();
                result = true;
            }
            catch (WebException webExp)
            {
                string error = string.Empty;
                if (webExp.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)webExp.Response)
                    {
                        List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
                        for (int i = 0; i < errorResponse.Headers.Count; i++)
                        {
                            headerList.Add(new KeyValuePair<string, string>(errorResponse.Headers.Keys[i], errorResponse.Headers[i]));
                        }
                        string sBody = new StreamReader(errorResponse.GetResponseStream()).ReadToEnd();

                        pResponse.HEADERS = headerList;
                        pResponse.BODY = string.Empty;

                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }

                if (string.IsNullOrEmpty(error))
                    _mLastErrorMessage = webExp.Message;
                else
                    _mLastErrorMessage = error;
            }
            catch (Exception Exp)
            {
                _mLastErrorMessage = Exp.Message;
            }

            return result;
        }

        public string GetLastErrorMessage()
        {
            return _mLastErrorMessage;
        }
    }
}