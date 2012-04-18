using System;
using System.Linq;
using System.Net.Mail;

namespace Avaruz.FrameWork.Infraestructure
{

    public class Mail
    {
        private readonly SmtpClient _smtp;

        public Mail(SmtpDeliveryMethod deliveryMethod, string host, bool useDefaultCredentials)
        {
            _smtp = new SmtpClient
            {
                Host = host,
                DeliveryMethod = deliveryMethod,
                UseDefaultCredentials = useDefaultCredentials
            };
            //Or Your SMTP Server Address
            //smtp.Credentials = new NetworkCredential
            //     ("BOMP\asoriagalvarro", "U$umaki2011");
            //Or your Smtp Email ID and Password

        }

        public Mail()
            : this(SmtpDeliveryMethod.Network, "PIAP010SCZ", true)
        {

        }


        public void SendMail(string toEmail, string asunto, string cuerpoMensaje, string fromEmail, bool masDeUnMail)
        {
            var mail = new MailMessage();
            if (masDeUnMail)
            {
                foreach (var buzon in toEmail.Split(',').Where(buzon => !String.IsNullOrWhiteSpace(buzon.Trim())).Where(buzon => !String.IsNullOrWhiteSpace(buzon)))
                {
                    mail.To.Add(buzon);
                }
            }
            else
            {
                mail.To.Add(toEmail);
            }

            mail.From = new MailAddress(fromEmail);
            mail.Subject = asunto;
            mail.Body = cuerpoMensaje;

            mail.IsBodyHtml = true;

            //var smtp = new SmtpClient
            //               {
            //                   Host = "smtp.gmail.com",
            //                   Credentials = new System.Net.NetworkCredential
            //                       ("adhemar.sgv@gmail.com", "Matia$2809"),
            //                   EnableSsl = true
            //               };
            ////Or Your SMTP Server Address
            //////Or your Smtp Email ID and Password
            //smtp.Send(mail);



            _smtp.Send(mail);
        }

    }
}
