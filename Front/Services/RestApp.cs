using Front.Models;
using System.Text.Json;
using Front.Respuestas;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace Front.Services
{


    public class RestApp
    {
        protected HttpClient client;
        protected JsonSerializerOptions _serializerOptions;
        protected string baseUrl = "http://localhost:5000/api";
        protected Respuesta respuesta;

        public RestApp(HttpClient httpClient) {

            Setting.BaseUrl = baseUrl;

            client = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
        }
    }
}
