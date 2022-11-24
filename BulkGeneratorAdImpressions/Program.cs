using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BulkGeneratorAdImpressions
{
    class Program
    {
        public static string between2finer(string line, string delimiterFirst, string delimiterLast)
        {
            string[] splitterFirst = new string[] { delimiterFirst };
            string[] splitterLast = new string[] { delimiterLast };
            string[] splitRes;
            string buildBuffer;
            splitRes = line.Split(splitterFirst, 100000, System.StringSplitOptions.RemoveEmptyEntries);
            buildBuffer = splitRes[1];
            splitRes = buildBuffer.Split(splitterLast, 100000, System.StringSplitOptions.RemoveEmptyEntries);
            return splitRes[0];
        }
        static void Main(string[] args)
        {
            
            long pAdImpressionsCount=0;
            long pAdClicksCount=0;
            int SleepTime = 0;
            string URL=System.Configuration.ConfigurationManager.AppSettings["RequestURL"];
                 string AdImpressionsCount=System.Configuration.ConfigurationManager.AppSettings["AdImpressionsCount"];
                 string AdClicksCount=System.Configuration.ConfigurationManager.AppSettings["AdClicksCount"];
                 string apiURLValue = System.Configuration.ConfigurationManager.AppSettings["apiURLValue"];
                 string SleepTimeStr = System.Configuration.ConfigurationManager.AppSettings["SleepTime"];
            long.TryParse(AdImpressionsCount,out pAdImpressionsCount);
             long.TryParse(AdClicksCount,out pAdClicksCount);
             int.TryParse(SleepTimeStr, out SleepTime);
//             WebRequest request = WebRequest.Create (URL);
          

try
{
   long clicCount=0;
   string responseText;

    for(long c=0 ;c<pAdImpressionsCount;c++ )
    {
        HttpWebRequest httpWebRequest = WebRequest.Create(URL) as HttpWebRequest;

        c++;
        httpWebRequest.Method = WebRequestMethods.Http.Get;
        httpWebRequest.Accept = "application/json";
        if (c%10000==0)
            {
                Thread.Sleep(SleepTime);
            }
        using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
        {
            

            WebHeaderCollection header = response.Headers;
           
            var encoding = ASCIIEncoding.UTF8;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                 responseText = reader.ReadToEnd();
            }
            if (!responseText.Contains("No Available Ad"))
            {
                if (responseText.Contains(apiURLValue+"B/"))
                {
                    string URLB = apiURLValue + "B/" + between2finer(responseText, apiURLValue + "B/", @"\");
                    HttpWebRequest httpWebRequestAdImp= WebRequest.Create(URLB) as HttpWebRequest;


                    httpWebRequestAdImp.Method = WebRequestMethods.Http.Get;
                    httpWebRequestAdImp.Accept = "application/json";
                    using (HttpWebResponse responseClick = httpWebRequestAdImp.GetResponse() as HttpWebResponse)
                    {

                    }
                
                }
                if (responseText.Contains(apiURLValue + "C/"))
                {
                    string URLC = apiURLValue + "C/" + between2finer(responseText, apiURLValue + "C/",@"\") +"?R="+DateTime.Now.Ticks;
                   
                    if (clicCount <= pAdClicksCount)
                    {
                        clicCount++;
                        HttpWebRequest httpWebRequestClick = WebRequest.Create(URLC) as HttpWebRequest;


                        httpWebRequestClick.Method = WebRequestMethods.Http.Get;
                        //httpWebRequestClick.Accept = "application/json";
                        httpWebRequestClick.BeginGetResponse(null, null);
       
                    }

                }
            }

        }
    }
  
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    
}
        }
        }


    
}
