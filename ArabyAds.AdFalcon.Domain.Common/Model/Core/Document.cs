using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Domain.Utilities;
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Core
{
    public class Document : IEntity<int>
    {

       static WebHDFSUtil _WebHDFSUtil = null;


        static long? blocksize = null;
        static short? replication = null;
        static string permission = null;
        static int? buffersize = null;
        static long? offset = null;
        static long? length = null;
        private string _NewName = string.Empty;
        private static WebHDFSUtil WebHDFSUtil
        {
            get
            {
                if (_WebHDFSUtil == null)
                {
                    _WebHDFSUtil = new WebHDFSUtil(ConfigurationManager.AppSettings["LogImpPath"], ConfigurationManager.AppSettings["LogImpPath2"], "", new NetworkCredential(ConfigurationManager.AppSettings["WebHDFSUserName"], ConfigurationManager.AppSettings["WebHDFSPassword"], ConfigurationManager.AppSettings["WebHDFSDomain"]));
                }
                return _WebHDFSUtil;
            }
        }


        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string UsedName { get {

                if (string.IsNullOrEmpty(NewName))
                {
                    return Name;

                }
                return NewName;
            } set { } }
        public virtual string NewName { get { return _NewName; } set { 
                
                _NewName = value;
                if (!this.IsWebHDFS)
                    Name = _NewName;

            } }
        public virtual DocumentType DocumentType { get; set; }
        public virtual string Extension { get; set; }
        public virtual int Size { get; set; }
        public virtual DateTime UploadedDate { get; set; }
        public virtual byte[] Content { get; set; }

        public virtual bool IsWebHDFS { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual int Usage { get; set; }


        /*public virtual byte[] ReadContent()
        {


            var task = ReadContent();
            var result = task.Result;
            return result;
        }

        public virtual bool WriteContent(byte[] Content)
        {


            var task = WriteContent(Content);
            var result = task.Result;
            return result;
        }*/
        public virtual byte[] ReadContent()



        {
            if (!this.IsWebHDFS)
            {
                return this.Content;
            }
            else
            {
      
               return  WebHDFSUtil.ReadFileByResponse(Name);
              


            }

        }

        public virtual bool WriteContent(byte[] Content)



        {
            if (this.IsWebHDFS)
            {
                this.Content = Content;
                Stream stream = new MemoryStream(this.Content);
                _ = WebHDFSUtil.WriteStream(stream, Name, true, blocksize, replication, permission, buffersize);

                this.Content = null;
            }
            else
            {
                this.Content = Content;
            }
            return false;
        }
        public virtual void ftpUpload(string ftpUrl)//, string cdnUrl)
        {
            if (!ftpUrl.EndsWith(FullNameUpdate(), StringComparison.OrdinalIgnoreCase))
            {
                //delete old file
                Framework.Utilities.FtpHelper.Delete(ftpUrl);
            }
            Framework.Utilities.FtpHelper.Upload(ftpUrl, ReadContent());
        }

        public virtual string ftpUpload(string directory, string cdnUrl)
        {
            //directory += "/";
            //Framework.Utilities.FtpHelper.CreateDirectory(directory);

            //directory += subDirectory + "/";
            //Framework.Utilities.FtpHelper.CreateDirectory(directory);
            var fileName = FullNameUpdate().Replace(" ", "_");
            Framework.Utilities.FtpHelper.Upload(directory + "/" + fileName, ReadContent());
            return (cdnUrl + "/" + fileName);
        }

        public virtual void UpdateUsage(bool isRemove=false)
        {
            if(!isRemove)
            {
                Usage++;
            }
            else
            {
                Usage--;
            }
        }
        public virtual string FullName()
        {
            return string.Format("{0}{1}", this.UsedName, Extension);
        }

        public virtual string FullNameUpdate()
        {
            return string.Format("{0}{1}", this.GetNameWithNoExtension(), Extension);
        }

        public virtual string GetNameWithNoExtension()
        {
            string currentName = this.UsedName;
            if (this.IsWebHDFS)
            {
                currentName = this.UsedName.Substring(this.UsedName.LastIndexOf("/") + 1);

                currentName= currentName.Replace(this.Extension, string.Empty);
            }
           return currentName;
        }
        public virtual void Delete()
        {
            IsDeleted = true;
        }
       
        public virtual string GetDescription()
        {
            return Name;
        }

        public virtual string StructureTheName(string nameWithExtnsion)
        {


            var subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");

            return ConfigurationManager.AppSettings["WebHDFSDirectory"] + "/" + subFolder + "/" + Configuration.Id64Generator.GenerateId().ToString()+"/"+ nameWithExtnsion; 

        }
    }
}

