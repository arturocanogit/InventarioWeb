using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;

namespace Global
{
    public class Logs
    {

    }
    public class Utilerias
    {
        /// <summary>
        /// Mapea todas las propiedades que hacen match de un objeto con otro
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="tipoHabitacionDto"></param>
        /// <returns></returns>
        public static TResult Mapeador<TResult, TSource>(TSource tipoHabitacionDto) where TResult : new()
        {
            TResult result = Activator.CreateInstance<TResult>();
            Type typeSource = typeof(TSource);
            foreach (var property in result.GetType().GetProperties())
            {
                //Las propiedades virtuales no se mapean
                if (property.PropertyType.IsPrimitive || 
                    property.PropertyType == typeof(string) ||
                    property.PropertyType == typeof(DateTime))
                {
                    var propertySource = typeSource.GetProperty(property.Name);
                    if (propertySource != null)
                    {
                        property.SetValue(result,
                        typeSource.GetProperty(property.Name).GetValue(tipoHabitacionDto));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Convierte un stream en un array de bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] StreamToByteArray(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
        /// <summary>
        /// Metodo para envio de peticiones http
        /// </summary>
        /// <typeparam name="Result"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<Result> HttpRequest<Result>(string url, HttpMethod method)
        {
            HttpClient client = new HttpClient();
            var response = await client.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = method
            });
            var res = await response.Content.ReadAsStringAsync();
            return Deserialize<Result>(res);
        }
        /// <summary>
        /// Metodo para deserializar una cadena
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string content)
        {
            return new JavaScriptSerializer().Deserialize<T>(content);
        }
        /// <summary>
        /// Metodo para serializar una cadena
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Serialize<T>(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }
        /// <summary>
        /// Envio de correos de manera asincrona
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="to"></param>
        /// <param name="body"></param>
        /// <param name="eventHandler"></param>
        public static void EnvioMailAsync(string subject, string to, string body, SendCompletedEventHandler eventHandler = null)
        {
            string from = GetAppSetting("EMAIL_FROM");
            string smtp = GetAppSetting("EMAIL_SMTP");

            // Command-line argument must be the SMTP host.
            SmtpClient client = new SmtpClient(smtp);
            // Specify the email sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress _from = new MailAddress(from);
            // Set destinations for the email message.
            MailAddress _to = new MailAddress(to);
            // Specify the message content.
            MailMessage message = new MailMessage(_from, _to);
            message.Body = body;
            // Include some non-ASCII characters in body and subject.
            string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            if (eventHandler != null)
            {
                client.SendCompleted += eventHandler;
            }
            //client.SendCompleted += new
            //SendCompletedEventHandler(SendCompletedCallback);
            // The userState can be any object that allows your callback
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = "test message1";
            client.SendAsync(message, userState);
            //Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            //string answer = Console.ReadLine();
            // If the user canceled the send, and mail hasn't been sent yet,
            // then cancel the pending operation.
            //if (answer.StartsWith("c") && mailSent == false)
            //{
            //    client.SendAsyncCancel();
            //}
            // Clean up.
            message.Dispose();
            //Console.WriteLine("Goodbye.");
        }
        /// <summary>
        /// Codifica texto html 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HTMLEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }
        /// <summary>
        /// Obtiene un app key del archivo de configuración de la app,
        /// si no se encuentra manda una excepcion con el nombre de la key
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAppSetting(string name)
        {
            string config = ConfigurationManager.AppSettings.Get(name);
            if (string.IsNullOrEmpty(config))
            {
                throw new NullReferenceException($"Se requiere de una configuración para {name}");
            }
            return config;
        }
        /// <summary>
        /// Convierte un datatable en lista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
    class Seguridad
    {
        /// <summary>
        /// Codifica texto html 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HTMLEncode(string html)
        {
            return Utilerias.HTMLEncode(html);
        }
        /// <summary>
        /// Genera llaves aleatorias para encirptar/desencriptar en aes
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GenerarLlavesAES()
        {
            using (Aes aesAlg = Aes.Create())
            {
                return new Dictionary<string, string>
                {
                    { "Key", Convert.ToBase64String(aesAlg.Key) },
                    { "IV", Convert.ToBase64String(aesAlg.IV) }
                };
            }
        }
        public static string EncriptAES(string text)
        {
            var key = Convert.FromBase64String(Utilerias.GetAppSetting("AES_KEY"));
            var iv = Convert.FromBase64String(Utilerias.GetAppSetting("AES_IV"));
            return Convert.ToBase64String(
                EncryptStringToBytes_Aes(text, key, iv));
        }
        public static string DencriptAES(string text)
        {
            var key = Convert.FromBase64String(Utilerias.GetAppSetting("AES_KEY"));
            var iv = Convert.FromBase64String(Utilerias.GetAppSetting("AES_IV"));
            return DecryptStringFromBytes_Aes(
                Convert.FromBase64String(text), key, iv);
        }
        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
    public static class Extensiones
    {
        /// <summary>
        /// Codifica una cadena html
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HTMLEncode(this string text)
        {
            return Utilerias.HTMLEncode(text);
        }
        /// <summary>
        /// Encripta la cadena con algoritmo aes
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToEncriptAES(this string text)
        {
            return Seguridad.EncriptAES(text);
        }
        /// <summary>
        /// Desencripta la cadena con algoritmo aes
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToDencriptAES(this string text)
        {
            return Seguridad.DencriptAES(text);
        }
        /// <summary>
        /// Convierte un datatable en lista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dataTable)
        {
            return Utilerias.ConvertDataTable<T>(dataTable);
        }
        /// <summary>
        /// Valida si una cadena es alfanumerica, los espacios son validos
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static bool IsAlphaNumeric(this string cadena)
        {
            return Regex.IsMatch(cadena, @"[A-z0-9]+");
        }
        /// <summary>
        /// Valida si una cadena hace match con una fecha
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static bool IsDate(this string cadena)
        {
            return Regex.IsMatch(cadena, @"[0-9]{2}\/\/[0-9]{2}\/\/[0.9]{4}");
        }
        /// <summary>
        /// Valida si una cadena puede convertirse en un numero
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string cadena)
        {
            return double.TryParse(cadena, out _);
        }
        /// <summary>
        /// Valida si un objeto es no nulo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }
        /// <summary>
        /// Valida si un objeto es nulo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
    public static class Configuracion
    {
        /// <summary>
        /// Obtiene la informacion almacenada en un xml 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static string GetConfiguracion(string fileName, string tagName)
        {
            XmlReader reader = XmlReader.Create(@"filepath");
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    //return only when you have START tag  
                    switch (reader.Name.ToString())
                    {
                        case "Name":
                            Console.WriteLine("The Name of the Student is " + reader.ReadString());
                            break;
                        case "Grade":
                            Console.WriteLine("The Grade of the Student is " + reader.ReadString());
                            break;
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// Guarda la información en un xml si ya existe lo actualiza
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static void SetConfiguracion(string fileName, XmlNode xmlNode)
        {
            if (File.Exists(fileName))
            {
                XmlReader reader = XmlReader.Create(@"filepath");
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == xmlNode.Name)
                        {

                        }
                        //return only when you have START tag  
                        switch (reader.Name.ToString())
                        {
                            case "Name":
                                Console.WriteLine("The Name of the Student is " + reader.ReadString());
                                break;
                            case "Grade":
                                Console.WriteLine("The Grade of the Student is " + reader.ReadString());
                                break;
                        }
                    }
                }
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);
                doc.AppendChild(xmlNode);
                doc.Save(fileName);
            }
        }
    }
}



