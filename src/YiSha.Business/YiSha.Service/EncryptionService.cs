using Koo.Utilities.Data;
using Koo.Utilities.Encryption;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YiSha.Service.Cache;

namespace YiSha.Service
{
    public class EncryptionService : BaseSingleton<EncryptionService>
    {
        private ConfigCache configCahce = new ConfigCache();
        public async Task<string> EncryptPassword(string pwd)
        {
            var config = await configCahce.GetConfigModel();
            return RSAEncryption.Encrypt(pwd, config.PasswordPublicKey);
        }
        public async Task<string> EncryptVarPassword(string pwd)
        {
            var config = await configCahce.GetConfigModel();
            return RSAEncryption.Encrypt(pwd, config.VarPasswordPublicKey);
        }
        public async Task<string> DecryptVarPassword(string pwd)
        {
            var config = await configCahce.GetConfigModel();
            return RSAEncryption.Decrypt(pwd, config.VarPasswordPrivateKey);
        }
        protected override void InitOverride()
        {

        }
    }
}
