using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Web;
using System.Net.Mail;
using APEXAContracting.Common.Interfaces;
using APEXAContracting.Common;

namespace APEXAContracting.Common.Helpers
{
    /// <summary>
    ///  Value is same as email template's file name.
    /// </summary>
    public enum EmailType
    {
        #region
        #endregion
        NationalClaimConfirmationDTC_EN,

        NationalClaimConfirmationUser_EN,
        NationalClaimConfirmationUser_FR,
        NationalClaimConfirmationUser_ES,
        NationalClaimConfirmationUser_ZH,

        NationalClaimConfirmationService_EN,
        NationalClaimConfirmationService_FR,
        NationalClaimConfirmationService_ES,
        NationalClaimConfirmationService_ZH,

        WarrantyClaimSendPDFEmail, //always English template
        WarrantyClaimSendPDFEmailUser_EN,
        WarrantyClaimSendPDFEmailUser_FR,
        WarrantyClaimSendPDFEmailUser_ES,
        WarrantyClaimSendPDFEmailUser_ZH,

        /// <summary>
        ///  Forgot password, system send new password based on user account's email setting.
        ///  Called in Identity server project.
        ///  Got template called "ForgotPasswordEmail.xslt" in APEXAContracting.IdentityServer project.
        /// </summary>
        ForgotPasswordEmail,
        /// <summary>
        ///  Send email to system administrator when new user registration submit.
        /// </summary>
        UserRegisterEmail
    }

    public enum EmailTemplateType
    {
        html,
        xslt
    }

    public interface IEmailSettings
    {
        string FromEmail { get; set; }
        string MailServer { get; set; }
        int SMTPServerPort { get; set; }
        string SMTPUser { get; set; }
        string SMTPPassword { get; set; }
        bool SMTPEnableSsL { get; set; }
    }

    public class EmailSettings : IEmailSettings
    {
        public string FromEmail { get; set; }
        public string MailServer { get; set; }
        public int SMTPServerPort { get; set; }
        public string SMTPUser { get; set; }
        public string SMTPPassword { get; set; }
        public bool SMTPEnableSsL { get; set; }
    }

    public class EmailHelper
    {
        #region private variable
        private string _emailTemplateFolder;
        private EmailType _emailType;
        private string _bodyTeamplate;
        private EmailTemplateType _templateType;
        /// <summary>
        ///  Wrapper for IConfiguration.
        /// </summary>
        public IEmailSettings _config;
        #endregion

        #region Properties
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        #endregion


        /// <summary>
        /// constructor method
        /// </summary>
        /// <param name="emailType"></param>
        public EmailHelper(IConfigSettings config, string templateFolder, EmailType emailType, string teamplate = "")
        {
            _config = new EmailSettings()
            {
                FromEmail = config.FromEmail,
                MailServer = config.MailServer,
                SMTPServerPort= config.SMTPServerPort,
                SMTPUser= config.SMTPUser,
                SMTPPassword= config.SMTPPassword,
                SMTPEnableSsL= config.SMTPEnableSsL
            };

            _emailTemplateFolder = templateFolder;
            _emailType = emailType;
            _bodyTeamplate = teamplate;
        }

        public EmailHelper(IConfigSettings config, string templatePath, EmailType emailType, EmailTemplateType templateType, string teamplate = "")
        {
            _config = new EmailSettings()
            {
                FromEmail = config.FromEmail,
                MailServer = config.MailServer,
                SMTPServerPort = config.SMTPServerPort,
                SMTPUser = config.SMTPUser,
                SMTPPassword = config.SMTPPassword,
                SMTPEnableSsL = config.SMTPEnableSsL
            };

            _emailTemplateFolder = templatePath;
            _emailType = emailType;
            _templateType = templateType;
            _bodyTeamplate = teamplate;
        }

        /// <summary>
        /// constructor method
        /// </summary>
        /// <param name="emailType"></param>
        public EmailHelper(IEmailSettings config, string templateFolder, EmailType emailType, string teamplate = "")
        {
            _config = config;
            _emailTemplateFolder = templateFolder;
            _emailType = emailType;
            _bodyTeamplate = teamplate;
        }

        public EmailHelper(IEmailSettings config, string templatePath, EmailType emailType, EmailTemplateType templateType, string teamplate = "")
        {
            _config = config;
            _emailTemplateFolder = templatePath;
            _emailType = emailType;
            _templateType = templateType;
            _bodyTeamplate = teamplate;
        }


        /// <summary>
        /// send mail out
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="mailTo">Support multiple recipents. Email splited by ';'. Dataformat: email1;email2;...</param>
        /// <param name="attachedFiles"></param>
        public void Send(string mailTo, string mailFrom, Dictionary<string, byte[]> attachment)
        {
            string subject = GetEmailSubject(_emailType);
            string body = GetEmailBody(_emailType, EmailTemplateType.html, null, null);

            this.SendEmail(mailTo, mailFrom, subject, body, attachment);
        }

        /// <summary>
        ///  Work for xslt email template.
        /// </summary>
        /// <param name="mailTo">Support multiple recipents. Email splited by ';'. Dataformat: email1;email2;...</param>
        /// <param name="parameters">Support additional parameters defined in xslt template.</param>
        /// <param name="contentEntity">Business object which used to generate email body.</param>
        /// <param name="attachedFiles"></param>
        public void Send(string mailTo, string mailFrom, Object contentEntity, Dictionary<string, string> parameters, Dictionary<string, byte[]> attachment)
        {
            string subject = GetEmailSubject(_emailType, parameters);

            string body = GetEmailBody(_emailType, EmailTemplateType.xslt, contentEntity, parameters);

            this.SendEmail(mailTo, mailFrom, subject, body, attachment);
        }

        /// <summary>
        /// get email body
        /// </summary>
        /// <param name="emailType">Associated to busienss logic what email need to send. template file name will be same as "emailType" value string.</param>
        /// <param name="emailTemplateType">html or xslt.</param>
        /// <param name="contentEntity">Now, this input parameter only work for "xslt" EmailTemplateType.</param>
        /// <param name="parameters">Now, this input parameter only work for "xslt" EmailTemplateType.</param>
        /// <returns></returns>
        private string GetEmailBody(EmailType emailType, EmailTemplateType emailTemplateType, Object contentEntity, Dictionary<string, string> parameters)
        {
            string template = string.Empty;
            string emailBody = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(this._bodyTeamplate))
                    template = GetEmailTemplate(emailType, emailTemplateType);
                else
                    template = this._bodyTeamplate;

                if (emailTemplateType == EmailTemplateType.html)
                {
                    emailBody = template;

                    //reflect
                    Type type = this.GetType();
                    PropertyInfo[] properties = type.GetProperties();
                    foreach (var prop in properties)
                    {
                        object propValue = prop.GetValue(this, null);
                        if (propValue != null)
                        {
                            emailBody = emailBody.Replace("$" + prop.Name + "$", propValue.ToString());
                        }
                    }
                }
                else if (emailTemplateType == EmailTemplateType.xslt)
                {
                    XsltTransformer transformer = new XsltTransformer(template);

                    emailBody = transformer.Transform(contentEntity, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emailBody;
        }

        /// <summary>
        /// get email teamplate for special email type
        /// </summary>
        /// <param name="emailType"></param>
        /// <returns></returns>
        private string GetEmailTemplate(EmailType emailType, EmailTemplateType emailTemplateType)
        {
            string templatePath = string.Empty;

            templatePath = Path.Combine(this._emailTemplateFolder, string.Format("{0}.{1}", emailType.ToString(), emailTemplateType.ToString()));
            
            return File.ReadAllText(templatePath); ;
        }

        /// <summary>
        /// get email subject 
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="parameters">Now, this input parameter only work for ForgotPasswordEmail.</param>
        /// <returns></returns>
        private string GetEmailSubject(EmailType emailType,Dictionary<string, string> parameters = null)
        {
            if (!string.IsNullOrEmpty(this.EmailSubject))
            {
                //Return the special Email Subject
                return this.EmailSubject;
            }
            else
            {
                switch (emailType)
                {
                    case EmailType.NationalClaimConfirmationDTC_EN:
                        return "National Account Claim Confirmation";
                    case EmailType.NationalClaimConfirmationUser_EN:
                    case EmailType.NationalClaimConfirmationUser_FR:
                    case EmailType.NationalClaimConfirmationUser_ES:
                    case EmailType.NationalClaimConfirmationUser_ZH:
                        return "National Account Claim Confirmation";
                    case EmailType.NationalClaimConfirmationService_EN:
                    case EmailType.NationalClaimConfirmationService_FR:
                    case EmailType.NationalClaimConfirmationService_ES:
                    case EmailType.NationalClaimConfirmationService_ZH:
                        return "National Account Claim Confirmation";
                    case EmailType.ForgotPasswordEmail:
                        string companyName = "";
                        if (parameters.ContainsKey("CompanyName"))
                        {
                            companyName = parameters["CompanyName"];                           
                       }
                        return "{CompanyName} Reset Password".Replace("{CompanyName}", companyName);
                    case EmailType.UserRegisterEmail:
                        return "New User Account Registration Details";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailfrom"></param>
        /// <param name="mailto">Support multiple recipents. Email splited by ';'. Dataformat: email1;email2;...</param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="attachment"></param>
        /// <param name="smtp"></param>
        /// <param name="smtp_userid"></param>
        /// <param name="smtp_password"></param>
        private void SendEmail(string mailto, string sender, string subject, string content, Dictionary<string, byte[]> attachment)
        {
            this.EmailBody = content;

            string mailFrom = sender; 
            //if sender no provided, get the value from configuration
            if(string.IsNullOrEmpty(mailFrom))
                mailFrom = _config.FromEmail;
            string mailServer = _config.MailServer;
            int SMTPServerPort = _config.SMTPServerPort;
            string mailLogonUser = _config.SMTPUser;
            string mailLogonPassword = _config.SMTPPassword;
            bool enabledSSL = _config.SMTPEnableSsL;

            string[] mail_to = mailto.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            MailMessage email = new MailMessage();

            email.From = new MailAddress(mailFrom);

            foreach (string to in mail_to)
            {
                if (string.IsNullOrEmpty(to) || to.Length < 3) continue;
                email.To.Add(new MailAddress(to));
            }

            email.IsBodyHtml = true;
            email.Subject = subject;
            email.Body = content;

            if (attachment != null && attachment.Count > 0)
            {
                foreach(string key in attachment.Keys)
                {
                    Attachment emaiAttach = new Attachment(new MemoryStream(attachment[key]), key);
                    email.Attachments.Add(emaiAttach);
                }
            }
            //handle GMail server.
            bool IsGamilServer = (mailServer.ToLower() == "smtp.gmail.com" || mailFrom.ToLower().EndsWith("@gmail.com"));
            SmtpClient smtp_client = null;

            if (IsGamilServer)
            {
                smtp_client = new SmtpClient(mailServer, SMTPServerPort);
                smtp_client.UseDefaultCredentials = false;
            }
            else
            {
                smtp_client = new SmtpClient(mailServer, SMTPServerPort);
                smtp_client.UseDefaultCredentials = true;
            }

            smtp_client.Credentials = new System.Net.NetworkCredential(mailLogonUser, mailLogonPassword);
            smtp_client.EnableSsl = enabledSSL;
            smtp_client.Timeout = 30000;

            try
            {
                smtp_client.Send(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //release the attacthment file
                foreach (Attachment item in email.Attachments)
                {
                    item.Dispose();
                }

                if (smtp_client != null) {
                    smtp_client.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="smtpUser"></param>
        /// <param name="smtpPassword"></param>
        /// <param name="smtpPort"></param>
        /// <param name="smtpEnableSSL"></param>
        /// <param name="mailFrom"></param>
        /// <param name="mailto">Support multiple recipents. Email splited by ';'. Dataformat: email1;email2;...</param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="attachment"></param>
        public static void SendEmail(string smtpServer,string smtpUser,string smtpPassword,int? smtpPort,bool smtpEnableSSL,
            string mailFrom,  string mailto, string subject, string content, Dictionary<string, byte[]> attachment)
        {
            string[] mail_to = mailto.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            MailMessage email = new MailMessage();

            email.From = new MailAddress(mailFrom);

            foreach (string to in mail_to)
            {
                if (string.IsNullOrEmpty(to) || to.Length < 3) continue;
                email.To.Add(new MailAddress(to));
            }

            email.IsBodyHtml = true;
            email.Subject = subject;
            email.Body = content;

            if (attachment != null && attachment.Count > 0)
            {
                foreach (string key in attachment.Keys)
                {
                    Attachment emaiAttach = new Attachment(new MemoryStream(attachment[key]), key);
                    email.Attachments.Add(emaiAttach);
                }
            }
            //handle GMail server.
            bool IsGamilServer = (smtpServer.ToLower() == "smtp.gmail.com" || mailFrom.ToLower().EndsWith("@gmail.com"));
            using (SmtpClient smtp_client = new SmtpClient())
            {
                smtp_client.Host = smtpServer;

                if (smtpPort.HasValue && smtpPort > 0)
                {
                    smtp_client.Port = smtpPort.Value;
                }

                if (IsGamilServer)
                {
                    smtp_client.UseDefaultCredentials = false;
                }
                else
                {
                    smtp_client.UseDefaultCredentials = true;
                }

                smtp_client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword);
                smtp_client.EnableSsl = smtpEnableSSL;
                smtp_client.Timeout = 30000;

                try
                {
                    smtp_client.Send(email);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //release the attacthment file
                    foreach (Attachment item in email.Attachments)
                    {
                        item.Dispose();
                    }

                    if (smtp_client != null)
                    {
                        smtp_client.Dispose();
                    }
                }
            }
        }

    }

    /// <summary>
    ///  Work for transfer any business entity to string (html or xml) with specified xslt.
    ///  Reference: https://code.msdn.microsoft.com/How-to-transform-XML-using-113b08eb
    /// </summary>
    public class XsltTransformer
    {
        private readonly XslCompiledTransform xslTransform;

        /// <summary>
        ///  Constructor.
        /// </summary>
        /// <param name="xsl"></param>
        public XsltTransformer(string xsl)
        {
            xslTransform = new XslCompiledTransform();

            using (var stringReader = new StringReader(xsl))
            {
                using (var xslt = XmlReader.Create(stringReader))
                {
                    xslTransform.Load(xslt);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity">Business entity contains business data.</param>
        /// <param name="parameters">optional. Additional custom parameters.</param>
        /// <returns></returns>
        public string Transform(object entity, Dictionary<string, string> parameters)
        {
            XsltArgumentList xslArg = null;

            if (entity != null)
            {
                if (parameters != null && parameters.Count > 0)
                {
                    xslArg = new XsltArgumentList();

                    foreach (var item in parameters)
                    {
                        xslArg.AddParam(item.Key, "", item.Value);
                    }
                }

                var xml = Utilities.Serialize(entity);

                using (var writer = new StringWriter())
                {
                    using (var input = XmlReader.Create(new StringReader(xml)))
                    {
                        xslTransform.Transform(input, xslArg, writer);

                        return writer.ToString();
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
