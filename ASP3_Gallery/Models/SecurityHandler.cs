using System;
using System.Collections.Generic;
using System.Linq;
//using System.Security;
using System.Web;

namespace ASP3_Gallery.Models
{
    static class SecurityHandler
    {
        /// <summary>
        /// Make hashed password with Salt and hard coded block for improving security
        /// </summary>
        /// <param name="shelter"></param>
        /// <param name="Salted"></param>
        /// <returns></returns>
        public static string GetHash(System.Security.SecureString inputpassword, string Salted)
        {
            IntPtr unsafePtr = IntPtr.Zero;
            byte[] MergedArray = null;

            try
            {
                unsafePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocAnsi(inputpassword);
                byte[] myByteArray = new byte[inputpassword.Length];
                System.Runtime.InteropServices.Marshal.Copy(unsafePtr, myByteArray, 0, inputpassword.Length);
                byte[] saltbuf = System.Text.Encoding.ASCII.GetBytes(Salted);
                byte[] hardcode = System.Text.Encoding.ASCII.GetBytes("U-m63W4HP2QCOz+u7UL9awx01Zg=");
                MergedArray = new byte[saltbuf.Length + myByteArray.Length + hardcode.Length];
                System.Buffer.BlockCopy(saltbuf, 0, MergedArray, 0, saltbuf.Length);
                System.Buffer.BlockCopy(myByteArray, 0, MergedArray, saltbuf.Length, myByteArray.Length);
                System.Buffer.BlockCopy(hardcode, 0, MergedArray, saltbuf.Length + myByteArray.Length, hardcode.Length);
                MergedArray = new System.Security.Cryptography.SHA384Managed().ComputeHash(MergedArray);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocAnsi(unsafePtr);
                inputpassword.Dispose();
            }
            return Convert.ToBase64String(MergedArray);
        }


        /// <summary>
        /// Overloaded method with string parameters
        /// </summary>
        /// <param name="inputpassword">String parameter</param>
        /// <param name="Salted">String parameter</param>
        /// <returns></returns>
        public static string GetHash(string inputpassword, string Salted)
        {
            return GetHash(MakeSecureString(inputpassword), Salted);
        }

        /// <summary>
        /// Make Salt
        /// </summary>
        /// <returns></returns>
        public static string GetSalt()
        {
            byte[] salt = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Compare two passwords
        /// </summary>
        /// <param name="shelter"></param>
        /// <param name="storepassword"></param>
        /// <param name="Salted"></param>
        /// <returns></returns>
        public static bool Compare(System.Security.SecureString inputpassword, string hashedpassword, string Salted)
        {
            var hashed = GetHash(inputpassword, Salted);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashedpassword, hashed) == 0;
        }

        /// <summary>
        /// Compare two passwords Recommended to use SecureString for non hashed password
        /// </summary>
        /// <param name="inputpassword">Input</param>
        /// <param name="hashedpassword">From Database</param>
        /// <param name="Salted">Salt from DB</param>
        /// <returns></returns>
        public static bool Compare(string inputpassword, string hashedpassword, string Salted)
        {
            return Compare(MakeSecureString(inputpassword), hashedpassword, Salted);
        }



        private static System.Security.SecureString MakeSecureString(string password)
        {
            System.Security.SecureString secureString = new System.Security.SecureString();
            foreach (char ch in password)
                secureString.AppendChar(ch);
            return secureString;
        }
    }


}