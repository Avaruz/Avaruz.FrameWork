﻿using System;
using System.Linq;
using System.Net.Mail;

namespace Avaruz.FrameWork.Infraestructure
{

    public class Mail
    {
        private readonly SmtpClient _smtp;

        public MailAddressCollection ToRecipients;

        public MailAddressCollection CCRecipients;
        
        public AttachmentCollection Attachments;

        public MailAddress FromEmail { get; set; }


        /// <summary>
        /// Constructor donde uno puede especificar el metodo de entrega, el server y si se usan los credenciales por defecto.
        /// </summary>
        /// <param name="deliveryMethod"></param>
        /// <param name="host"></param>
        /// <param name="useDefaultCredentials"></param>
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


        /// <summary>
        /// Constructor que por defecto llama al PIAP011SCZ como SMTP
        /// </summary>
        public Mail()
            : this(SmtpDeliveryMethod.Network, "PIAP011SCZ", true)
        {

        }


        public void SendMail(string asunto, string cuerpoMensaje)
        {
            var mail = new MailMessage { Subject = asunto, Body = cuerpoMensaje, IsBodyHtml = true};

            foreach (var recipient in ToRecipients)
            {
                mail.To.Add(recipient);
            }

            mail.From = FromEmail;
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
