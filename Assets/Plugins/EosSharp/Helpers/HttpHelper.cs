using Cryptography.ECDSA;
using EosSharp.Exceptions;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace EosSharp.Helpers
{
    public class HttpHelper
    {
        private static int http_timeout = 5;
        private static int try_again_timeout = 5;

        private static HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(http_timeout) };
        private static Dictionary<string, object> ResponseCache { get; set; } = new Dictionary<string, object>();

        public static void ClearResponseCache()
        {             
            ResponseCache.Clear();
        }

        public static async Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, JsonSerializerSettings jsonSettings = null)
        {
            Stream result = null;

            var retryPolicy = Policy
             .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<ArgumentNullException>()
            .Or<InvalidOperationException>()
            .Or<EosSharp.Exceptions.ApiException>()
            .WaitAndRetryForeverAsync(retryAttempt =>
            TimeSpan.FromSeconds(try_again_timeout), (exception, timeSpan, context) =>
            {
                /*
                EosSharp.Exceptions.ApiException e = (EosSharp.Exceptions.ApiException)exception;
                Debug.Log("+++ NETWORK ERROR - " + exception.Message);
                Debug.Log("+++ API ERROR - " + e.Content);
                */
            });

            await retryPolicy.ExecuteAsync(async () =>
            {
                HttpRequestMessage request = BuildJsonRequestMessage(url, data);
                result = await SendAsync(request);
            });

            return DeserializeJsonFromStream<TResponseData>(result);
        }

        public static async Task<TResponseData> PostJsonAsync<TResponseData>(string url, object data, CancellationToken cancellationToken, JsonSerializerSettings jsonSettings = null)
        {
            Stream result = null;

            var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<ArgumentNullException>()
            .Or<InvalidOperationException>()
            .Or<EosSharp.Exceptions.ApiException>()
            .WaitAndRetryForeverAsync(retryAttempt =>
            TimeSpan.FromSeconds(try_again_timeout), (exception, timeSpan, context) =>
            {
                /*
                EosSharp.Exceptions.ApiException e = (EosSharp.Exceptions.ApiException)exception;
                Debug.Log("+++ NETWORK ERROR - " + exception.Message);
                Debug.Log("+++ API ERROR - " + e.Content);
                */
            });

            await retryPolicy.ExecuteAsync(async () =>
            {
                /*
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("Operation aborted by token");
                    return DeserializeJsonFromStream<TResponseData>(result);
                }
                */
                HttpRequestMessage request = BuildJsonRequestMessage(url, data);
                result = await SendAsync(request, cancellationToken);
            });

            return DeserializeJsonFromStream<TResponseData>(result);
        }

        public static async Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, bool reload = false, JsonSerializerSettings jsonSettings = null)
        {
            string hashKey = GetRequestHashKey(url, data);

            if (!reload)
            {
                object value;
                if (ResponseCache.TryGetValue(hashKey, out value))
                    return (TResponseData)value;
            }

            Stream result = null;

            var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<ArgumentNullException>()
            .Or<InvalidOperationException>()
            .Or<EosSharp.Exceptions.ApiException>()
            .WaitAndRetryForeverAsync(retryAttempt =>
            TimeSpan.FromSeconds(try_again_timeout), (exception, timeSpan, context) =>
            {
                /*
                EosSharp.Exceptions.ApiException e = (EosSharp.Exceptions.ApiException)exception;
                Debug.Log("+++ NETWORK ERROR - " + exception.Message);
                Debug.Log("+++ API ERROR - " + e.Content);
                */
            });

            await retryPolicy.ExecuteAsync(async () =>
            {
                HttpRequestMessage request = BuildJsonRequestMessage(url, data);
                result = await SendAsync(request);
            });

            var responseData = DeserializeJsonFromStream<TResponseData>(result);
            UpdateResponseDataCache(hashKey, responseData);

            return responseData;
        }

        public static async Task<TResponseData> PostJsonWithCacheAsync<TResponseData>(string url, object data, CancellationToken cancellationToken, bool reload = false, JsonSerializerSettings jsonSettings = null)
        {
            string hashKey = GetRequestHashKey(url, data);

            if (!reload)
            {
                object value;
                if (ResponseCache.TryGetValue(hashKey, out value))
                    return (TResponseData)value;
            }

            Stream result = null;

            var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<ArgumentNullException>()
            .Or<InvalidOperationException>()
            .Or<EosSharp.Exceptions.ApiException>()
            .WaitAndRetryForeverAsync(retryAttempt =>
            TimeSpan.FromSeconds(try_again_timeout), (exception, timeSpan, context) =>
            {
                /*
                EosSharp.Exceptions.ApiException e = (EosSharp.Exceptions.ApiException)exception;
                Debug.Log("+++ NETWORK ERROR - " + exception.Message);
                Debug.Log("+++ API ERROR - " + e.Content);
                */
            });

            await retryPolicy.ExecuteAsync(async () =>
            {
                /*
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("Operation aborted by token");
                    return;
                }
                */
                HttpRequestMessage request = BuildJsonRequestMessage(url, data);
                result = await SendAsync(request, cancellationToken);
            });

            var responseData = DeserializeJsonFromStream<TResponseData>(result);
            UpdateResponseDataCache(hashKey, responseData);

            return responseData;
        }

        public static async Task<TResponseData> GetJsonAsync<TResponseData>(string url, JsonSerializerSettings jsonSettings = null)
        {
            Stream result = null;

            var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<ArgumentNullException>()
            .Or<InvalidOperationException>()
            .Or<EosSharp.Exceptions.ApiException>()
            .WaitAndRetryForeverAsync(retryAttempt =>
            TimeSpan.FromSeconds(try_again_timeout), (exception, timeSpan, context) =>
            {
                /*
                EosSharp.Exceptions.ApiException e = (EosSharp.Exceptions.ApiException)exception;
                Debug.Log("+++ NETWORK ERROR - " + exception.Message);
                Debug.Log("+++ API ERROR - " + e.Content);
                */
            });

            await retryPolicy.ExecuteAsync(async () =>
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                result = await SendAsync(request);
            });

            return DeserializeJsonFromStream<TResponseData>(result, jsonSettings);
        }

        public static async Task<TResponseData> GetJsonAsync<TResponseData>(string url, CancellationToken cancellationToken, JsonSerializerSettings jsonSettings = null)
        {
            Stream result = null;

            var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<ArgumentNullException>()
            .Or<InvalidOperationException>()
            .Or<EosSharp.Exceptions.ApiException>()
            .WaitAndRetryForeverAsync(retryAttempt =>
            TimeSpan.FromSeconds(try_again_timeout), (exception, timeSpan, context) =>
            {
                /*
                EosSharp.Exceptions.ApiException e = (EosSharp.Exceptions.ApiException)exception;
                Debug.Log("+++ NETWORK ERROR - " + exception.Message);
                Debug.Log("+++ API ERROR - " + e.Content);
                */
            });

            
            await retryPolicy.ExecuteAsync(async () =>
            {
                /*
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("Operation aborted by token");
                    return;
                }
                */
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                result = await SendAsync(request, cancellationToken);
            });

            return DeserializeJsonFromStream<TResponseData>(result, jsonSettings);
        }

        public static async Task<Stream> SendAsync(HttpRequestMessage request)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await BuildSendResponse(response);
        }

        public static async Task<Stream> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return await BuildSendResponse(response);
        }

        public static TData DeserializeJsonFromStream<TData>(Stream stream, JsonSerializerSettings jsonSettings = null)
        {
            if (stream == null || stream.CanRead == false)
                return default(TData);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                return JsonSerializer.Create(jsonSettings).Deserialize<TData>(jtr);
            }
        }

        public static HttpRequestMessage BuildJsonRequestMessage(string url, object data)
        {
            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
        }


        private static void UpdateResponseDataCache<TResponseData>(string hashKey, TResponseData responseData)
        {
            if (ResponseCache.ContainsKey(hashKey))
            {
                ResponseCache[hashKey] = responseData;
            }
            else
            {
                ResponseCache.Add(hashKey, responseData);
            }
        }

        private static string GetRequestHashKey(string url, object data)
        {
            var keyBytes = new List<byte[]>()
            {
                Encoding.UTF8.GetBytes(url),
                SerializationHelper.ObjectToByteArray(data)
            };
            return Encoding.Default.GetString(Sha256Manager.GetHash(SerializationHelper.Combine(keyBytes)));
        }

        private static async Task<Stream> BuildSendResponse(HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
                return stream;

            var content = await StreamToStringAsync(stream);
            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content
            };
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }
    }
}
