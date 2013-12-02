using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Security.Cryptography;

namespace PieroKeyMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var keys = KeyMaker.Make("test", DateTime.Now);
            Console.WriteLine("private key : " + keys[0]);
            Console.WriteLine("public key : " + keys[1]);
            Console.WriteLine("key string : " + keys[2]);
        }
    }



    /// <summary>
    /// 密钥由4部分组成：
    /// 验证值，盐值，是否使用延迟天数，使用的日期（延迟的天数）
    /// 验证值使用客户名称以及一个将客户名称补全至16位的随机数组成 再对整体进行哈希
    /// 是否使用延迟天数一位：0表示是有有效时间，1表示后面几位表示在现有的有效时间后加上几天
    /// 
    /// 全文使用rsa加密，用户持有私钥，制作方持有公钥
    /// 
    /// </summary>

    class KeyMaker
    {
        /// <summary>
        /// 创建新密钥
        /// </summary>
        /// <returns>一个包含密钥信息的数组：私钥（客户），公钥（开发者），密钥字符串</returns>
        static public string[] Make(string customerName, DateTime lastDate)
        {
            RSA rsa = RSA.Create();
            var rServer = new RSACryptoServiceProvider();
            var pri = rServer.ToXmlString(true);
            var pub = rServer.ToXmlString(false);

            Random random = new Random();
            string randomString = random.Next(int.MaxValue).ToString().PadLeft(9) + random.Next(int.MaxValue).ToString().PadLeft(9);
            var checkString = customerName + randomString.Substring(2, 8) + randomString.Substring(8, 8);
            var salt = checkString.Remove(0, customerName.Length);
            var md5 = MD5.Create(); ;
            var retCharArr = new Byte[100];
            md5.TransformBlock(Encoding.ASCII.GetBytes(checkString.ToArray<char>()), 0, 16, retCharArr, 0);
            //! err
            var eCheckString = Encoding.ASCII.GetString(retCharArr,0,16);

            var decString = eCheckString + salt + '0' + lastDate.Year.ToString() + lastDate.Month.ToString().PadLeft(2, '0') + lastDate.Day.ToString().PadLeft(2, '0');
            rsa.FromXmlString(pub);
            //err
            var enString = Encoding.ASCII.GetString(rsa.EncryptValue(Encoding.ASCII.GetBytes(decString)));
            return new string[] { pri, pub, enString };
        }
        static public string Make(string publicKey, string customerName, DateTime lastDate)
        {
            return "";
        }


    }
}


