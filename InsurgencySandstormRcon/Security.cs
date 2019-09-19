using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace InsurgencySandstormRcon
{
    public static class Security
    {
        public static string DecryptPassword(string encPwd)
        {
            try
            {
                string pwd = Encoding.Unicode.GetString(
                    ProtectedData.Unprotect(Convert.FromBase64String(encPwd), null, DataProtectionScope.CurrentUser));
                return pwd;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to decrypt rcon password : " + ex.Message);
            }
            return string.Empty;
        }

        public static string EncryptPassword(string pwd)
        {
            try
            {
                string encPwd = Convert.ToBase64String(ProtectedData.Protect(
                    Encoding.Unicode.GetBytes(pwd), null, DataProtectionScope.CurrentUser));
                return encPwd;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to encrypt rcon password : " + ex.Message);
            }
            //TODO : Decide if we should use unprotected password, notify user, give user option to not encrypt passwords?
            return string.Empty;
        }
    }
}
