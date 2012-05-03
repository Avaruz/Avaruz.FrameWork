using System;
using System.Linq;
using System.Net.Mail;

namespace Avaruz.FrameWork.Infraestructure
{

    public class Mail
    {
        private readonly SmtpClient _smtp;

        public MailAddressCollection ToRecipients;

        public MailAddressCollection CCRecipients;

        public MailAddress FromEmail { get; set; }

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


        public void SendMail(string asunto, string cuerpoMensaje)
        {
            var mail = new MailMessage {Subject = asunto, Body = cuerpoMensaje, IsBodyHtml = true};
            _smtp.Send(mail);

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

       }

    }
}
