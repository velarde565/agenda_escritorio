using System;
using System.Security.Cryptography;
using System.Windows;
using System.Security.Cryptography.X509Certificates;
using System.IO;

public class CryptographyService
{
    private byte[] aesKey;
    private RSACryptoServiceProvider rsa;
    private string serverPublicKeyBase64;  // Variable para almacenar la clave pública del servidor

    /// <summary>
    /// Constructor que inicializa el servicio de criptografía usando RSA con una clave de 3072 bits.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una instancia de <see cref="RSACryptoServiceProvider"/> con una longitud de clave de 3072 bits,
    /// que proporciona un nivel de seguridad más alto que la longitud estándar de 2048 bits. La clave se utiliza para operaciones 
    /// de cifrado y descifrado RSA.
    /// </remarks>
    public CryptographyService()
    {
        rsa = new RSACryptoServiceProvider(3072); // Inicializa RSA con una clave de 3072 bits
    }


    /// <summary>
    /// Exporta la clave pública del servicio de criptografía en formato Base64.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la clave pública en formato Base64, que puede ser enviada a un servidor.
    /// </returns>
    /// <remarks>
    /// Este método exporta la clave pública en formato X.509 (que es el estándar para las claves públicas) y la convierte
    /// en una cadena Base64. Además, muestra un cuadro de mensaje con la clave pública antes de devolverla.
    /// </remarks>
    public string GetPublicKey()
    {
        // Exporta la clave pública en formato X.509 (que es el estándar para las claves públicas)
        string publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());

        // Muestra la clave pública en un MessageBox antes de enviarla
        MessageBox.Show("Clave pública que se envía al servidor: " + publicKey,
                        "Clave Pública del Cliente",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

        return publicKey;
    }


    /// <summary>
    /// Importa la clave pública del servidor a partir de una cadena en formato Base64.
    /// </summary>
    /// <param name="publicKeyBase64">
    /// La clave pública del servidor en formato Base64 que será importada.
    /// </param>
    /// <remarks>
    /// Este método convierte la clave pública en Base64 a un arreglo de bytes y luego intenta importar la clave pública
    /// en formato X.509 (PKCS#8) usando el algoritmo RSA. Si la importación es exitosa, se muestra un mensaje indicando
    /// que la clave pública se ha importado correctamente. En caso de error, se manejan excepciones específicas y se
    /// muestra un mensaje de error detallado.
    /// </remarks>
    public void ImportServerPublicKey(string publicKeyBase64)
    {
        try
        {
            // Muestra la clave pública del servidor recibida en Base64
            MessageBox.Show("Clave pública del servidor recibida: " + publicKeyBase64,
                             "Clave Pública del Servidor",
                             MessageBoxButton.OK,
                             MessageBoxImage.Information);

            // Convierte la clave pública en Base64 a bytes
            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyBase64);

            // Intenta importar la clave pública en formato X.509 (PKCS#8)
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

            // Si la clave pública se importa correctamente
            MessageBox.Show("Clave pública del servidor importada correctamente.");
        }
        catch (FormatException ex)
        {
            MessageBox.Show($"Error al convertir la clave pública: {ex.Message}");
        }
        catch (CryptographicException ex)
        {
            MessageBox.Show($"Error al importar la clave pública RSA: {ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al importar la clave pública: {ex.Message}");
        }
    }



    /// <summary>
    /// Encripta los datos con la clave pública del cliente (RSA).
    /// </summary>
    /// <param name="data">Datos a encriptar en formato de bytes.</param>
    /// <returns>Un arreglo de bytes que contiene los datos encriptados.</returns>
    /// <remarks>
    /// Este método utiliza la clave pública del cliente (RSA) para encriptar los datos proporcionados.
    /// Se aplica el esquema de padding PKCS#1 para garantizar la compatibilidad con el estándar de encriptación RSA.
    /// </remarks>
    public byte[] EncryptDataWithClientKey(byte[] data)
    {
        return rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
    }





    /// <summary>
    /// Desencripta los datos con la clave privada del cliente (RSA).
    /// </summary>
    /// <param name="encryptedData">Datos encriptados en formato de bytes.</param>
    /// <returns>Un arreglo de bytes que contiene los datos desencriptados.</returns>
    /// <remarks>
    /// Este método utiliza la clave privada del cliente (RSA) para desencriptar los datos proporcionados.
    /// Se aplica el esquema de padding PKCS#1 para garantizar la compatibilidad con el estándar de desencriptación RSA.
    /// </remarks>
    public byte[] DecryptDataWithPrivateKey(byte[] encryptedData)
    {
        return rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
    }




    /// <summary>
    /// Genera un nuevo par de claves RSA si es necesario.
    /// </summary>
    /// <remarks>
    /// Este método inicializa un nuevo objeto RSACryptoServiceProvider con una longitud de clave de 3072 bits.
    /// La generación de nuevas claves RSA es útil cuando se necesita un nuevo par de claves (pública y privada) para la encriptación y desencriptación.
    /// </remarks>
    public void GenerateNewKeyPair()
    {
        rsa = new RSACryptoServiceProvider(3072); // Regenera un par de claves RSA de 3072 bits
    }




    /// <summary>
    /// Establece la clave AES para su uso en operaciones de encriptación y desencriptación.
    /// </summary>
    /// <param name="key">La clave AES que se va a establecer, representada como un arreglo de bytes.</param>
    /// <remarks>
    /// Este método valida que la clave no sea nula ni vacía, y luego almacena la clave AES en una variable interna.
    /// La clave AES se utiliza para encriptar o desencriptar datos con el algoritmo AES.
    /// Después de almacenar la clave, se muestra un mensaje con la clave en formato Base64.
    /// </remarks>
    /// <exception cref="ArgumentException">Lanzado si la clave AES es nula o vacía.</exception>
    public void SetAESKey(byte[] key)
    {
        if (key == null || key.Length == 0)
            throw new ArgumentException("La clave AES no puede ser nula o vacía.");

        aesKey = key;

        MessageBox.Show($"Clave AES almacenada: {Convert.ToBase64String(aesKey)}",
                        "Clave AES en CryptographyService",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
    }





    /// <summary>
    /// Obtiene la clave AES almacenada para su uso en operaciones de encriptación y desencriptación.
    /// </summary>
    /// <returns>La clave AES almacenada, representada como un arreglo de bytes.</returns>
    /// <remarks>
    /// Este método devuelve la clave AES que se ha almacenado previamente mediante el método <see cref="SetAESKey(byte[])"/>.
    /// Si la clave no ha sido establecida previamente, se lanzará una excepción <see cref="InvalidOperationException"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Lanzado si la clave AES no ha sido establecida.</exception>
    public byte[] GetAESKey()
    {
        if (aesKey == null || aesKey.Length == 0)
            throw new InvalidOperationException("La clave AES no ha sido establecida.");

        return aesKey;
    }



    /// <summary>
    /// Encripta los datos proporcionados utilizando el algoritmo AES con una clave previamente establecida.
    /// </summary>
    /// <param name="data">Datos en formato de arreglo de bytes que serán cifrados.</param>
    /// <returns>Datos cifrados en formato de arreglo de bytes.</returns>
    /// <remarks>
    /// Este método utiliza el modo ECB (Electronic Codebook) de AES, sin IV (Vector de Inicialización), lo que implica que el mismo bloque de texto cifrado siempre generará la misma salida para los mismos datos de entrada.
    /// Se utiliza el padding PKCS7 para asegurar que los datos sean de un tamaño múltiplo del tamaño de bloque de AES (128 bits).
    /// </remarks>
    /// <exception cref="ArgumentNullException">Lanzado si los datos proporcionados son nulos.</exception>
    public byte[] EncryptDataWithAES(byte[] data)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = aesKey;  // Asignar la clave AES

            // Usar ECB, sin IV. Aseguramos que no se utilice el IV
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;  // Establecer el padding adecuado

            // Cifrar los datos
            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, null))  // No se pasa IV, null está permitido para ECB
            {
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                    }
                    byte[] encryptedData = msEncrypt.ToArray();
                    return encryptedData;  // Solo devolver los datos cifrados
                }
            }
        }
    }


    /// <summary>
    /// Desencripta los datos proporcionados utilizando el algoritmo AES con una clave previamente establecida.
    /// </summary>
    /// <param name="data">Datos cifrados en formato de arreglo de bytes que serán desencriptados.</param>
    /// <returns>Datos desencriptados en formato de arreglo de bytes.</returns>
    /// <remarks>
    /// Este método utiliza el modo ECB (Electronic Codebook) de AES, sin IV (Vector de Inicialización), lo que implica que el mismo bloque de texto cifrado siempre generará la misma salida para los mismos datos de entrada.
    /// Se utiliza el padding PKCS7 para asegurar que los datos cifrados se ajusten a un tamaño múltiplo del tamaño de bloque de AES (128 bits).
    /// </remarks>
    /// <exception cref="ArgumentNullException">Lanzado si los datos proporcionados son nulos.</exception>
    public byte[] DecryptDataWithAES(byte[] data)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = aesKey;  // Usar la clave AES

            // Usar ECB, sin IV
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;  // Establecer el padding adecuado

            // Desencriptar los datos
            using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, null))  // No se pasa IV
            {
                using (MemoryStream msDecrypt = new MemoryStream(data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream msDecrypted = new MemoryStream())
                        {
                            csDecrypt.CopyTo(msDecrypted);  // Copiar los datos desencriptados a un MemoryStream
                            return msDecrypted.ToArray();  // Retornar los datos desencriptados
                        }
                    }
                }
            }
        }
    }




}
