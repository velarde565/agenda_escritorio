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

    // Constructor que inicializa RSACryptoServiceProvider con 3048 bits
    public CryptographyService()
    {
        rsa = new RSACryptoServiceProvider(3072); // Inicializa RSA con una clave de 3048 bits
    }

    // Exporta la clave pública en formato Base64
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



    // Encripta los datos con la clave pública del cliente (que es la clave pública RSA del cliente)
    public byte[] EncryptDataWithClientKey(byte[] data)
    {
        return rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
    }

    // Desencripta los datos con la clave privada del cliente
    public byte[] DecryptDataWithPrivateKey(byte[] encryptedData)
    {
        return rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
    }

    // Método para generar un nuevo par de claves RSA si es necesario
    public void GenerateNewKeyPair()
    {
        rsa = new RSACryptoServiceProvider(3072); // Regenera un par de claves RSA de 3048 bits
    }


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

    public byte[] GetAESKey()
    {
        if (aesKey == null || aesKey.Length == 0)
            throw new InvalidOperationException("La clave AES no ha sido establecida.");

        return aesKey;
    }


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
