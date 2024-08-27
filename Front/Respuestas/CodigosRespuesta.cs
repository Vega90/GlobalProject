namespace Front.Respuestas
{
    public enum CodigosRespuesta
    {
        OK = 200,
        ClaveRepetida = 300,
        ConexionFallida = 301,
        Null = 302,
        IDNoEncontrado = 303,
        ErrorServidor = 400,
        PermisoDenegado = 401,
        ConexionRechazada = 402,
        LoginError = 403,
        LoginNoAutorizado = 404,
        BorradoNoPermitido = 405

    }

    public static class CodigosRespuestaExtension
    {
        //obtenemos un mensaje según el código que hayamos puesto
        public static string GetMensaje(this CodigosRespuesta cod)
        {
            switch (cod)
            {
                case CodigosRespuesta.OK:
                    return "Respuesta correcta del servidor";

                case CodigosRespuesta.ClaveRepetida:
                    return "Clave duplicada";

                case CodigosRespuesta.ConexionFallida:
                    return "Conexión fallida";

                case CodigosRespuesta.Null:
                    return "Objeto Null";

                case CodigosRespuesta.PermisoDenegado:
                    return "No tiene permiso para realizar esta acción";

                case CodigosRespuesta.ConexionRechazada:
                    return "Conexion rechazada por el servidor";

                case CodigosRespuesta.ErrorServidor:
                    return "Error del servidor";

                case CodigosRespuesta.IDNoEncontrado:
                    return "No existe ningún registro con ese identificador";

                case CodigosRespuesta.LoginError:
                    return "Usuario o contraseña incorrecta";

                case CodigosRespuesta.LoginNoAutorizado:
                    return "Login no autorizado, el empleado no está activo";

                case CodigosRespuesta.BorradoNoPermitido:
                    return "El objeto no se puede eliminar, está activo";

                default:
                    return "Ha ocurrido un error en la petición";

            }
        }

    }

}
