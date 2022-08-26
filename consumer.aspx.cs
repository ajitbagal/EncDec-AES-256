using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EncDec
{
    public partial class consumer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["param"] != null)
            {
                lblCiphertext.Text = Request.QueryString["param"].ToString();
                var appSettings = ConfigurationManager.AppSettings;
                string AESkey = appSettings["AESkey"] ?? "Not Found";
                if (AESkey != "Not Found")
                {
                    lblPlainText.Text = Decrypt(Request.QueryString["param"].ToString(), AESkey);
                }
                else
                {
                    return;
                }
            }
            else
            { 
            
            }


        }

        protected void btnbacktosender_Click(object sender, EventArgs e)
        {
            Response.Redirect("sender.aspx");
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