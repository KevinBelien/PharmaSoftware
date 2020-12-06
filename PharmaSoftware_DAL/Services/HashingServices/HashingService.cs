using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.Services.HashingServices
{
    public class HashingService : IHashingService
    {
        public string DecryptString(string decr)
        {
            try
            {
                if (decr != null)
                {


                    //Converteren 64 bits naar 8 bits;
                    byte[] data = Convert.FromBase64String(decr);
                    using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
                    {
                        byte[] keys = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes("1Yhd_)283A2.90hdsoHDS902983__0kjdks394_#fff1204HEbsKDPf8Ile=27HgFVHfdç!..90)éài?34"));
                        using (AesCryptoServiceProvider tripdes = new AesCryptoServiceProvider { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.ISO10126 })
                        {//Een symmetrisch decryptor object aanmaken 
                            ICryptoTransform transform = tripdes.CreateDecryptor();
                            byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                            //byte array omzetten in string
                            return UTF8Encoding.UTF8.GetString(results);
                        }
                    }
                }
            }
            catch (FormatException)
            {
                return "";
            }
            return "";
        }

        public string EncryptString(string encr)
        {

            //array van bytes opvullen met de bits van elke char van wachtwoord
            byte[] data = UTF8Encoding.UTF8.GetBytes(encr);
            //provider gebruiken om met het SHA256 algoritme te werken
            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
            {
                //Converteert de invoerreeks naar een bytearray  + Berekent de hash-waarde
                byte[] keys = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes("1Yhd_)283A2.90hdsoHDS902983__0kjdks394_#fff1204HEbsKDPf8Ile=27HgFVHfdç!..90)éài?34"));
                //Met behulp van cryptografisch interface, symmetrische encryptie mogelijk maken
                //Paddin = AES werkt met fixed bit sizes. Specifies the type of padding to apply when the message data block is shorter than the full number of bytes needed for a cryptographic operation.
                using (AesCryptoServiceProvider tripdes = new AesCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.ISO10126 })
                {
                    //Een symmetrisch encryptor object aanmaken 
                    ICryptoTransform transform = tripdes.CreateEncryptor();
                    //Transformeren van het opgegeven gebied van de opgegeven bytearray.
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    //Teruggeven van een 64 bit string
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }
    }
}
