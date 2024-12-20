using System;

namespace AgendaEscritorio.service
{
    public static class DataExtractor
    {
        /// <summary>
        /// Extrae una porción de datos de la respuesta, comenzando en el índice proporcionado.
        /// </summary>
        /// <param name="response">La respuesta completa de la que se extraerán los datos.</param>
        /// <param name="index">Índice desde el cual se comenzará a extraer la porción de datos. Este valor se actualizará después de la extracción.</param>
        /// <returns>La porción de datos extraída de la respuesta.</returns>
        /// <remarks>
        /// El método asume que la respuesta sigue un formato en el cual los primeros dos caracteres representan el tamaño de la porción de datos a extraer. 
        /// Luego de extraer los datos, el índice proporcionado se actualiza para apuntar al siguiente segmento de la respuesta.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">Lanzado si el índice está fuera del rango de la cadena de respuesta.</exception>
        public static string ExtractData(string response, ref int index)
        {
            // Leer el valor del tamaño de los datos a extraer (dos caracteres iniciales)
            int offset = int.Parse(response.Substring(index, 2));  // Asume que el tamaño está codificado en los primeros dos caracteres
            index += 2;  // Avanza el índice para pasar el tamaño

            // Extraer los datos con el tamaño especificado
            string data = response.Substring(index, offset);  // Extrae los datos del tamaño especificado
            index += offset;  // Actualiza el índice para apuntar al siguiente segmento

            return data;  // Retorna los datos extraídos
        }


        /// <summary>
        /// Extrae una porción de datos de la respuesta, comenzando en el índice proporcionado, con un tamaño especificado por los primeros cuatro caracteres.
        /// </summary>
        /// <param name="response">La respuesta completa de la que se extraerán los datos.</param>
        /// <param name="index">Índice desde el cual se comenzará a extraer la porción de datos. Este valor se actualizará después de la extracción.</param>
        /// <returns>La porción de datos extraída de la respuesta.</returns>
        /// <remarks>
        /// El método asume que la respuesta sigue un formato en el cual los primeros cuatro caracteres representan el tamaño de la porción de datos a extraer. 
        /// Luego de extraer los datos, el índice proporcionado se actualiza para apuntar al siguiente segmento de la respuesta.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">Lanzado si el índice está fuera del rango de la cadena de respuesta.</exception>
        public static string ExtractOtherData(string response, ref int index)
        {
            // Leer el valor del tamaño de los datos a extraer (cuatro caracteres iniciales)
            int offset = int.Parse(response.Substring(index, 4));  // Asume que el tamaño está codificado en los primeros cuatro caracteres
            index += 4;  // Avanza el índice para pasar el tamaño

            // Extraer los datos con el tamaño especificado
            string data = response.Substring(index, offset);  // Extrae los datos del tamaño especificado
            index += offset;  // Actualiza el índice para apuntar al siguiente segmento

            return data;  // Retorna los datos extraídos
        }


        /// <summary>
        /// Extrae un token de la cadena de mensaje, utilizando los dos primeros caracteres para determinar el tamaño del token.
        /// </summary>
        /// <param name="message">El mensaje completo del que se extraerá el token.</param>
        /// <returns>El token extraído del mensaje.</returns>
        /// <remarks>
        /// El método supone que los primeros dos caracteres del mensaje indican el tamaño del token a extraer. 
        /// A continuación, extrae el token según el tamaño especificado y lo retorna.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">Lanzado si el mensaje no tiene suficiente longitud para extraer el token con el tamaño indicado.</exception>
        public static string ExtractToken(string message)
        {
            // Leer el valor del tamaño del token (dos caracteres iniciales)
            int tokenOffset = int.Parse(message.Substring(0, 2));  // El tamaño del token está codificado en los primeros dos caracteres

            // Extraer el token con el tamaño especificado
            return message.Substring(2, tokenOffset);  // Extrae el token con el tamaño indicado y lo retorna
        }

    }
}
