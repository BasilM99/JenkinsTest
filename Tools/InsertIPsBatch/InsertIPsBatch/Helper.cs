using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InsertIPsBatch
{
    public static class Helper
    {
        #region Heleper
        public static long ConvertIPToNumber(string ip)
        {
            long ipnum = 0;
            IPAddress addr = null;
            if (IPAddress.TryParse(ip, out addr))
            {
                byte[] address = addr.GetAddressBytes();
                for (int i = 0; i < 4; ++i)
                {
                    long y = address[i];
                    if (y < 0)
                    {
                        y += 256;
                    }
                    ipnum += y << ((3 - i) * 8);
                }
            }
            return ipnum;
        }
        public static List<IP> ReadFile(string path)
        {
            //read file text
            var text = System.IO.File.ReadAllText(path);
            //Convert to IP Object
            return text.Split('\n').Select(x => x.Trim('\r')).Select(ip => new IP(ip)).ToList(); ;

        }

        public static void WriteFile(string path, IEnumerable<IP> data)
        {
            var sb = new StringBuilder();

            foreach (var ip in data)
            {
                sb.AppendLine(ip.Value);
            }
            System.IO.File.WriteAllText(path, sb.ToString());
        }
        #endregion
    }
}
