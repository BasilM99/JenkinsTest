using ArabyAds.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebHDFS;
namespace ArabyAds.AdFalcon.Domain.Utilities
{
    public class WebHDFSUtil
    {
        private WebHDFS.WebHDFSClient _WebHDFSClient1;
        private WebHDFS.WebHDFSClient _WebHDFSClient2;


        private string _BaseAPi1;
        private string _BaseApi2;
        private NetworkCredential _networkCreditional;

        private string _BaseDirectory;
        public WebHDFSUtil(string BaseAPi1, string BaseApi2, string BaseDirectory, NetworkCredential networkCreditional)
        {

            this._BaseAPi1 = BaseAPi1;
            this._BaseApi2 = BaseApi2;
            this._networkCreditional = networkCreditional;
            this._BaseDirectory = BaseDirectory;

            _WebHDFSClient1 = new WebHDFSClient(BaseAPi1, networkCreditional);
            _WebHDFSClient2 = new WebHDFSClient(BaseApi2, networkCreditional);

        }


        public async Task<bool> WriteStreamAsync(
            Stream stream,
            string path,
            bool overwrite = false,
            long? blocksize = null,
            short? replication = null,
            string permission = null,
            int? buffersize = null)
        {

            try
            {
                return await _WebHDFSClient1.WriteStream(stream, path, overwrite, blocksize, replication, permission, buffersize);
            }
            catch (Exception ex)
            {
                return await _WebHDFSClient2.WriteStream(stream, path, overwrite, blocksize, replication, permission, buffersize);

            }
            finally
            {

            }
        }

        public bool WriteStream(
           Stream stream,
           string path,
           bool overwrite = false,
           long? blocksize = null,
           short? replication = null,
           string permission = null,
           int? buffersize = null)
        {

            try
            {
             
                path = HttpUtility.UrlEncode(path, Encoding.UTF8);
                path = path.Replace("%2f", "/");
                var task = _WebHDFSClient1.WriteStream(stream, path, overwrite, blocksize, replication, permission, buffersize);
                //task.Wait();
                var result = task.Result;
                return result;
            }
            catch (Exception ex)
            {
                var task = _WebHDFSClient2.WriteStream(stream, path, overwrite, blocksize, replication, permission, buffersize);
                //   task.Wait();
                var result = task.Result;
                return result;

            }
            finally
            {

            }
        }
        public WebHDFS.FileStatus GetFileStatus(

   string path
  )
        {

            try
            {
                var task = _WebHDFSClient1.GetFileStatus(path);
                //task.Wait();
                var result = task.Result;
                return result;
            }
            catch (Exception ex)
            {
                var task = _WebHDFSClient2.GetFileStatus(path);
                //  task.Wait();
                var result = task.Result;
                return result;

            }
            finally
            {

            }
        }



        public bool ReadStream(
Stream stream,
    string path,
    long? offset = null,
    long? length = null,
    int? buffersize = null)
        {

            try
            {
                var task = _WebHDFSClient1.ReadStream(stream, path, offset, length, buffersize);
                // task.Wait();

                var result = task.Result;
                return result;
            }
            catch (Exception ex)
            {
                var task = _WebHDFSClient2.ReadStream(stream, path, offset, length, buffersize);
                // task.Wait();
                var result = task.Result;
                return result;


            }
            finally
            {

            }

        }
        public async Task<bool> ReadStreamAsync(
   Stream stream,
            string path,
            long? offset = null,
            long? length = null,
            int? buffersize = null)
        {

            try
            {
                return await _WebHDFSClient1.ReadStream(stream, path, offset, length, buffersize);
            }
            catch (Exception ex)
            {
                return await _WebHDFSClient2.ReadStream(stream, path, offset, length, buffersize);

            }
            finally
            {

            }

        }


        public async Task<bool> MakeDirectoryAsync(
       string path,
       string permission = null)
        {

            try
            {
                var result = await _WebHDFSClient1.MakeDirectory(path, permission);

                return result.boolean;
            }
            catch (Exception ex)
            {
                var result = await _WebHDFSClient2.MakeDirectory(path, permission);
                return result.boolean;

            }
            finally
            {

            }

        }




        public bool MakeDirectory(
     string path,
     string permission = null)
        {

            try
            {
                var task = _WebHDFSClient1.MakeDirectory(path, permission);
                var result = task.Result;
                //eturn result;
                return result.boolean;
            }
            catch (Exception ex)
            {
                var task = _WebHDFSClient2.MakeDirectory(path, permission);
                var result = task.Result;
                return result.boolean;

            }
            finally
            {

            }

        }


        public byte[] ReadFileByResponse(string path)
        {
            path = HttpUtility.UrlEncode(path, Encoding.UTF8);
            path = path.Replace("%2f", "/");
            var dowloadPath = this._BaseAPi1 + path + "?op=OPEN";
            var dowloadPath2 = this._BaseApi2 + path + "?op=OPEN";
            var cc = new CredentialCache();

            HttpWebResponse resp = null;
            HttpWebRequest req = null;
            try
            {

                ApplicationContext.Instance.Logger.Info(string.Format("Download Path- {0}", dowloadPath));
                req = (HttpWebRequest)WebRequest.Create(dowloadPath);
                req.AllowAutoRedirect = true;
                req.Credentials = this._networkCreditional;



                try
                {


                    resp = (HttpWebResponse)req.GetResponse();
                }
                catch (Exception ex)
                {
                    ApplicationContext.Instance.Logger.Info(string.Format("Download Path2- {0}", dowloadPath2));
                    req = (HttpWebRequest)WebRequest.Create(dowloadPath2);
                    req.AllowAutoRedirect = true;
                    req.Credentials = this._networkCreditional;
                    resp = (HttpWebResponse)req.GetResponse();
                }
            }
            finally
            {


            }
            BinaryReader sr = new BinaryReader(resp.GetResponseStream());
            // byte[] allData = sr.ReadBytes(int.MaxValue);

            try
            {
                // long startBytes = 0;


                //Dividing the data in 1024 bytes package
                // int maxCount = (int)Math.Ceiling((resp.ContentLength - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                // int i;
                // for (i = 0; i < maxCount ; i++)
                //{
                byte[] allData = sr.ReadBytes((int)resp.ContentLength);

                // }
                return allData;
            }
            finally
            {

                sr.Close();
            }
        }

    }
}
