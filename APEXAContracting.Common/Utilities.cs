using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Text;
using APEXAContracting.Common.Interfaces;
using System.Globalization;
using static APEXAContracting.Common.Enumerations;

namespace APEXAContracting.Common
{
    /// <summary>
    ///  Put any shared methods here.
    ///  Check Dapasoft.Core.Extensions.String.DotNetStandard library which already contains shared methods.
    /// </summary>
    public static class Utilities
    {
        #region Password Management. Transfer logic from old system. 
        //
        private const int SECURITY_SYSTEMSALT_LENGTH = 16;
        private const int SECURITY_SALT_LENGTH = 8;

        /// <summary>
        /// Generates System wide Salt for encrypting Passwords and other data
        /// </summary>
        /// <param name="size"> the length of salte </param>
        /// <returns></returns>
        public static byte[] GenerateSalt(int size)
        {
            byte[] data = new byte[size - 1];
            System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetBytes(data);
            return data;
        }

        /// <summary>
        /// Generates Salt for encrypting user Passwords. 
        /// Length take SECURITY_USERSALT_LENGTH
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateSalt()
        {
            return GenerateSalt(SECURITY_SALT_LENGTH);
        }

        /// <summary>
        /// Generates Salt for encrypting usystem data
        /// Length take SECURITY_USERSALT_LENGTH
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateSystemSalt()
        {
            return GenerateSalt(SECURITY_SYSTEMSALT_LENGTH);
        }

        /// <summary>
        /// Encrypts Password in SHA384
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>Return Hash value of password</returns>
        public static byte[] HashPassword(string password, byte[] salt)
        {
            string tmpPass = string.Empty;
            tmpPass = Convert.ToBase64String(salt) + password;
            UTF8Encoding tConvert = new UTF8Encoding();
            byte[] passBytes = tConvert.GetBytes(tmpPass);
            return SHA384Managed.Create().ComputeHash(passBytes);
        }

        public static string RegenratePassword(int size)
        {
            StringBuilder sb = new StringBuilder();
            const int minvalue = 0;
            const int maxvalue = 4;
            int tmp, tmp1 = 0;
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                tmp = random.Next(minvalue, maxvalue);
                if (i != 0)
                    tmp = GetNum(tmp, tmp1, minvalue, maxvalue, random);
                switch (tmp)
                {
                    case 0:
                        sb.Append(CreateNum());
                        break;
                    case 1:
                        sb.Append(CreateBigAbc());
                        break;
                    case 2:
                        sb.Append(CreateSmallAbc());
                        break;
                    case 3:
                        sb.Append(CreateSymbol());
                        break;
                    default:
                        break;
                }
                tmp1 = tmp;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Compare 2 password
        /// </summary>
        /// <param name="pass1"></param>
        /// <param name="pass2"></param>
        /// <returns>if match, return true; if not match, return false</returns>
        public static bool PasswordMatch(Byte[] pass1, Byte[] pass2)
        {
            int i;
            try
            {
                for (i = 0; i < pass1.Length - 1; i++)
                {
                    if (pass1[i] != pass2[i])
                        return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        public static string Serialize(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());

            var writerSettings =
            new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };

            var emptyNameSpace = new XmlSerializerNamespaces();
            emptyNameSpace.Add(string.Empty, string.Empty);

            var stringWriter = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(stringWriter, writerSettings))
            {
                serializer.Serialize(xmlWriter, obj, emptyNameSpace);

                return stringWriter.ToString();
            }
        }

        private static int CreateNum()
        {
            Random random = new Random();
            int num = random.Next(0, 10);
            return num;
        }
        private static char CreateBigAbc()
        {
            Random random = new Random();
            int num = random.Next(65, 91);
            char ABC = Convert.ToChar(num);
            return ABC;
        }
        private static char CreateSmallAbc()
        {
            Random random = new Random();
            int num = random.Next(97, 123);
            char abc = Convert.ToChar(num);
            return abc;
        }
        private static char CreateSymbol()
        {
            Random random = new Random();
            int num = random.Next(33, 48);
            char symbol = Convert.ToChar(num);
            return symbol;
        }
        private static int GetNum(int tmp, int tmp1, int minValue, int maxValue, Random ra)
        {
            while (tmp == tmp1)
            {
                tmp = ra.Next(minValue, maxValue);
            }
            return tmp;
        }

        /// <summary>
        ///  Work for logging error.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GetParametersString(this IDictionary<string, object> parameters)
        {
            string result = string.Empty;
           
            if (parameters != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var parameter in parameters)
                {
                    sb.AppendLine(parameter.Key + ": " + parameter.Value + ", ");
                }
                result = sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///  Try to cache error in convert string to decimal.
        ///  Issues happened when culture changed.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            decimal decimalValue = 0;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    //
                    // Maybe user selected other language from the top.
                    //
                    // Referene: https://stackoverflow.com/questions/22644352/wont-convert-string-to-decimal-c-sharp-input-string-was-not-in-a-correct-forma
                    //
                    var culture = new CultureInfo("en-US");
                    decimalValue = Convert.ToDecimal(value, culture);
                }
                catch
                {
                    // hide exception.
                }
            }

            return decimalValue;
        }

        /// <summary>
        ///  Work for combine multiple string items to be one string with specified splitor such as ","
        ///  Work for combine address information.
        /// </summary>
        /// <param name="splitor"></param>
        /// <param name="inputStrings"></param>
        /// <returns></returns>
        public static string CombineStrings(string splitor, params string[] inputStrings) {
            string result = string.Empty;

           if (inputStrings!=null && inputStrings.Length > 0)
            {
                List<string> items = new List<string>();
                foreach (string item in inputStrings) {
                    if (!string.IsNullOrEmpty(item)) {
                        items.Add(item.Trim());
                    }
                }

                result = string.Join(splitor, items);
            }

            return result;
        }

        public static int CalculateAge(DateTime? DateOfBirth)
        {
            string result = string.Empty;

            int age = 0;
            DateTime currentDate = DateTime.Now;
            if (DateOfBirth.HasValue)
            {
                age = currentDate.Year - DateOfBirth.Value.Year;
                if (currentDate.Month < DateOfBirth.Value.Month || (currentDate.Month == DateOfBirth.Value.Month && currentDate.Day < DateOfBirth.Value.Day))
                {
                    age--;
                }
            }
            return age < 0 ? 0 : age;
        }

        /// <summary>
        ///  Convert input datetime to utc datetime. Further, return the date's start time and end time of day.
        /// </summary>
        /// <param name="inputDateTime"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertDateTimeToUTC(this DateTime inputDateTime, out DateTime startDateTime, out DateTime endDateTime) {

            DateTime utcDate = inputDateTime.ToUniversalTime();

            startDateTime = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, 0, 0, 0);
            endDateTime = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, 23, 59, 59);

            return utcDate;
        }

        /// <summary>
        ///  Convert input datetime to utc datetime. 
        /// </summary>
        /// <param name="inputDateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertDateTimeToUTC(this DateTime inputDateTime)
        {
            DateTime utcDate = inputDateTime.ToUniversalTime();

            return utcDate;
        }

        /// <summary>
        ///  Convert datetime string with format:
        ///  Sat Jun 01 2019 00:00:00 GMT-0400 (Eastern Daylight Time) 
        ///  Thu Jul 02 2015 00:00:00 GMT+0100 (GMT Standard Time)
        ///  to datetime value.
        /// </summary>
        /// <param name="inputDateString"></param>
        /// <returns></returns>
        public static bool ConvertToDateTime(this string inputDateString, out DateTime  outputDate)
        {
            bool result = false;
            outputDate = new DateTime();
            string format = "ddd MMM dd yyyy HH:mm:ss 'GMT'K";
            try
            {
                inputDateString = inputDateString.Replace("(Eastern Daylight Time)", "").Trim();
                inputDateString = inputDateString.Replace("(GMT Standard Time)", "").Trim();
                result = DateTime.TryParseExact(inputDateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out outputDate);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        
        public static byte[] ReadStream(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}