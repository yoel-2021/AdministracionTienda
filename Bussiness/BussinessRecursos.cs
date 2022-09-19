using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bussiness
{
    public class BussinessRecursos
    {
        public static string GenerarContrasena()
        {
            string contrasena = Guid.NewGuid().ToString("N").Substring(0, 6);
            return contrasena;
        }


        //Encriptacion de TEXTO en SHA256
        public static string ConvertirASha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            //Usar System.Security.Cryptography
            SHA256 hash = SHA256Managed.Create();
            Encoding encoding = Encoding.UTF8;
            byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

            foreach (byte b in result)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("pruebasdotnet2022@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("pruebasdotnet2022@gmail.com", "xqkspidqdbrlhuol"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true

                };

                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }

        public static string ConvertirBase64(string ruta, bool conversion_exitosa)
        {
            string textoBase64 = string.Empty;
            conversion_exitosa = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch(Exception ex)
            {
                conversion_exitosa = false;
            }

            return textoBase64;
        }

    }
}
