using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace YiSha.Model
{
    public class SystemConfigCategoryAttribute : Attribute
    {
        public const string Basic = "basic";
        public const string Rsa = "rsa";
        public const string Mail = "mail";


        public string Category
        {
            get; set;
        }
        public SystemConfigCategoryAttribute(string category)
        {
            this.Category = category;
        }
    }
    public class SystemConfigModel
    {
        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Basic)]
        public string CorporateName
        {
            get; set;
        }

        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Basic)]
        public string CorporateShortName
        {
            get; set;
        }
        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Rsa)]
        public string PasswordPublicKey
        {
            get; set;
        }
        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Rsa)]
        public string PasswordPrivateKey
        {
            get; set;
        }
        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Rsa)]
        public string VarPasswordPublicKey
        {
            get; set;
        }
        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Rsa)]
        public string VarPasswordPrivateKey
        {
            get; set;
        }

        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Mail)]
        public string MailHost
        {
            get; set;
        }
        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Mail)]
        public string MailPort
        {
            get; set;
        }
        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Mail)]
        public string MailEnableSsl
        {
            get; set;
        }
        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Mail)]
        public string MailUserName
        {
            get; set;
        }
        [SystemConfigCategoryAttribute(SystemConfigCategoryAttribute.Mail)]
        public string MailPassword
        {
            get; set;
        }
      
    }
}
