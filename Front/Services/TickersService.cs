using Front.Models;
using Front.Respuestas;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Front.Services
{
    public class TickersService : RestApp
    {
        protected string urlController { get; set; } = "/tickers";


        public TickersService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<Dictionary<string, string>> GetTickersAsync()
        {
            var url = $"{baseUrl}/{urlController}";

            try
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var deserializado = JsonConvert.DeserializeObject< Dictionary<string, string> >(content);
                    return deserializado;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) {
                return null;
            }

        }

        // Método para obtener los detalles del ticker seleccionado
        public async Task<Dictionary<string, List<Ticker>>> GetTickerDataAsync(string nombre, string ticker, string periodo, string intervalo)
        {
            var url = $"{baseUrl}/{urlController}/details";

            try
            {
                var requestBody = new
                {
                    name = nombre,
                    ticker = ticker,
                    periodo = periodo, 
                    intervalo = intervalo
                };

                var serial = JsonConvert.SerializeObject(requestBody);
                var jsonContent = new StringContent(serial, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var deserialized = JsonConvert.DeserializeObject<Dictionary<string, List<Ticker>>>(content);
                    return deserialized;
                }
                else
                {
                    // Manejo del error según sea necesario, puedes registrar el error o lanzar una excepción personalizada
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, puedes registrar el error o lanzar una excepción personalizada
                return null;
            }
        }

    }
}
