using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;

// https://gist.github.com/doncadavona/fd493b6ced456371da8879c22bb1c263
namespace EncDec
{
    public partial class sender : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string AESkey = appSettings["AESkey"] ?? "Not Found";
            if (AESkey != "Not Found" && txtplaintext.Text.Trim() != "")
            {
                lblCipherText.Text = Encrypt(txtplaintext.Text.Trim(), AESkey);
                
                // Cooment below line to stop redirection
                Response.Redirect("consumer.aspx?param="+Encrypt(txtplaintext.Text.Trim(), AESkey));
            }
            else
            {
                return;
            }
        }

        protected void btnClearValues_Click(object sender, EventArgs e)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string AESkey = appSettings["AESkey"] ?? "Not Found";
            if (AESkey != "Not Found" && lblCipherText.Text.Trim() != "")
            {
                lblPlainText.Text = Decrypt(lblCipherText.Text.Trim(), AESkey);
            }
            else
            {
                return;
            }

        }

        public string Encrypt(string plainText, string key)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = encoding.GetBytes(key);
                aes.GenerateIV();

                ICryptoTransform AESEncrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] buffer = encoding.GetBytes(plainText);

                string encryptedText = Convert.ToBase64String(AESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));

                String mac = "";

                mac = BitConverter.ToString(HmacSHA256(Convert.ToBase64String(aes.IV) + encryptedText, key)).Replace("-", "").ToLower();

                var keyValues = new Dictionary<string, object>
                {
                    { "iv", Convert.ToBase64String(aes.IV) },
                    { "value", encryptedText },
                    { "mac", mac },
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                return HttpUtility.HtmlEncode(Convert.ToBase64String(encoding.GetBytes(serializer.Serialize(keyValues))));
            }
            catch (Exception e)
            {
                throw new Exception("Error encrypting: " + e.Message);
            }
        }

        private string Decrypt(string plainText, string key)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = encoding.GetBytes(key);

                // Base 64 decode
                byte[] base64Decoded = Convert.FromBase64String(plainText);
                string base64DecodedStr = encoding.GetString(base64Decoded);

                // JSON Decode base64Str
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var payload = ser.Deserialize<Dictionary<string, string>>(base64DecodedStr);

                aes.IV = Convert.FromBase64String(payload["iv"]);

                ICryptoTransform AESDecrypt = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] buffer = Convert.FromBase64String(payload["value"]);

                return HttpUtility.HtmlDecode(encoding.GetString(AESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length)));
            }
            catch (Exception e)
            {
                throw new Exception("Error decrypting: " + e.Message);
            }
        }

        private byte[] HmacSHA256(String data, String key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
        }

    }
}