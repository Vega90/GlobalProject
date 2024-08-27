namespace Front.Respuestas
{
    public class Respuesta
    {
        public int success { get; set; }
        public CodigosRespuesta codigoRes { get; set; }
        public string mensaje { get; set; } = "";
        public object? Datos { get; set; }

        public int? idObjectBD { get; set; }
    }
}
