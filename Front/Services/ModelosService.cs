using Front.Models;
using Newtonsoft.Json;
using System.Text;

namespace Front.Services
{
    // hereda de RestApp
    public class ModelosService : RestApp
    {

        protected string urlController { get; set; } = "predecir";

        // creamos el objeto llamando al constructor RestApp
        public ModelosService(HttpClient httpClient) : base(httpClient)
        {       
        }

        // Llama al EndPoint para predecir las señales de compra y venta con el modelo seleccionado
        public async Task<Dictionary<string, List<Ticker>>> GetPrediccionAsync(string ticker, string modelo, string intervalo, string fecha_ini, string fecha_fin)
        {
            var url = $"{baseUrl}/{urlController}";

            try
            {
                var requestBody = new
                {
                    ticker = ticker,
                    intervalo = intervalo,
                    fechaInicio = fecha_ini,
                    fechaFinal = fecha_fin,
                    modelo = modelo
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
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
