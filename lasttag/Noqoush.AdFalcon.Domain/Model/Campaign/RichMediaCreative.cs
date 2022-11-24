using ICSharpCode.SharpZipLib.Zip;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Web;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class RichMediaCreative : AdCreative
    {
        protected IRichMediaRequiredProtocolRepository richMediaRequiredProtocolRepository = Framework.IoC.Instance.Resolve<IRichMediaRequiredProtocolRepository>();
        private const string RichMediaCustomParamValue = "1";
        public RichMediaCreative()
        {
            TypeId = AdTypeIds.RichMedia;

        }

        public virtual void SetRichMediaProtocol(int richMediaProtocol , bool isRequired)
        {
            string name = Enum.Parse(typeof(RichMediaProtocols), richMediaProtocol.ToString()).ToString().ToLower();
            AdCustomParameter adCustomParam = GetRichMediaCustomParameter();
            if (adCustomParam != null)
            {
                if (richMediaProtocol == (int)RichMediaProtocols.None)
                {
                    RemoveAdCustomParameter(adCustomParam);
                }
                else
                {
                    adCustomParam.IsMandatory = isRequired;
                    adCustomParam.Name = name;
                    adCustomParam.IsDeleted = false;
                }
            }
            else
            {
                if (richMediaProtocol != (int)RichMediaProtocols.None)
                {
                    AddAdCustomParameter(name, RichMediaCustomParamValue, isRequired);
                }
            }
        }

        public virtual RichMediaRequiredProtocol GetRichMediaProtocol()
        {
            AdCustomParameter mRAID1 = GetAdCustomParam(RichMediaProtocols.MRAID1.ToString().ToLower());
            if (mRAID1 != null)
            {
                return richMediaRequiredProtocolRepository.Get((int)RichMediaProtocols.MRAID1);
            }
            AdCustomParameter mRAID2 = GetAdCustomParam(RichMediaProtocols.MRAID2.ToString().ToLower());
            if (mRAID2 != null)
            {
                return richMediaRequiredProtocolRepository.Get((int)RichMediaProtocols.MRAID2);
            }
            return null;
        }
        public virtual bool GetisMandRichMediaProtocol()
        {
            AdCustomParameter mRAID1 = GetAdCustomParam(RichMediaProtocols.MRAID1.ToString().ToLower());
            if (mRAID1 != null)
            {
                return mRAID1.IsMandatory;
            }
            AdCustomParameter mRAID2 = GetAdCustomParam(RichMediaProtocols.MRAID2.ToString().ToLower());
            if (mRAID2 != null)
            {
                return mRAID2.IsMandatory;
            }
            return true;
        }
        public virtual AdCustomParameter GetRichMediaCustomParameter()
        {
            string mred1 = RichMediaProtocols.MRAID1.ToString().ToLower();
            string mred2 = RichMediaProtocols.MRAID2.ToString().ToLower();
            return this.AdCustomParameters.Where(x => x.Name == mred1 || x.Name == mred2).FirstOrDefault();
        }

        public override void Approve()
        {
            base.Approve();
            switch (AdSubType)
            {
                case Common.Model.Campaign.AdSubTypes.ExpandableRichMedia:
                    {
                        var ftpBaseDirectory = Configuration.FtpBaseDirectory;
                        var cdnBaseUrl = Configuration.CdnBaseUrl;
                        ftpUpload(ftpBaseDirectory, cdnBaseUrl);
                        break;
                    }
                case Common.Model.Campaign.AdSubTypes.HTML5Interstitial:
                case Common.Model.Campaign.AdSubTypes.HTML5RichMedia:
                    {
                        var ftpBaseDirectory = Configuration.FtpBaseDirectory;
                        var cdnBaseUrl = Configuration.CdnBaseUrl;
                        ftpUpload(ftpBaseDirectory, cdnBaseUrl);
                        break;
                    }
                case Common.Model.Campaign.AdSubTypes.JavaScriptRichMedia:
                case Common.Model.Campaign.AdSubTypes.JavaScriptInterstitial:
                case Common.Model.Campaign.AdSubTypes.ExternalUrlInterstitial:
                    {
                        UploadSnapshots();
                        //nothing for now
                        break;
                    }
            }

        }
        //TODO:this method has some code identical to the one in banner creative , we need to use only one of them
        public virtual void ftpUpload(string baseDirectory, string cdnBaseUrl)
        {
            string temp = string.Empty;
            var subFolder = string.Empty;
            if (!string.IsNullOrWhiteSpace(Group.Campaign.FolderURL))
            {
                subFolder = Group.Campaign.FolderURL;
            }
            else
            {
                //we need to create folder for it
                subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");
                temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                //create folder fo the current date
                Framework.Utilities.FtpHelper.CreateDirectory(temp);
                //create folder for current Campaign
                var isFolderCreated = false;
                while (!isFolderCreated)
                {
                    temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                    var r = RandomNumber(1, 100000);
                    temp = string.Format("{0}/{1}", temp, r);
                    if (!Framework.Utilities.FtpHelper.DirectoryExists(temp))
                    {
                        Framework.Utilities.FtpHelper.CreateDirectory(temp);
                        isFolderCreated = true;
                        subFolder += "/" + r;
                        Group.Campaign.FolderURL = subFolder;
                    }
                }
            }
            if (!Framework.Utilities.FtpHelper.DirectoryExists((baseDirectory + "/" + subFolder)))
            {
                ApplicationContext.Instance.Logger.Info(string.Format("SubFolder:{0}", (baseDirectory + "/" + subFolder)));
                Framework.Utilities.FtpHelper.CreateDirectory((baseDirectory + "/" + subFolder));
            }
            // create folder for current ad if not found
            subFolder = string.Format("{0}/{1}", subFolder, uId);
            temp = string.Format("{0}/{1}", baseDirectory, subFolder);
            Framework.Utilities.FtpHelper.CreateDirectory(temp);

            var directory = string.Format("{0}/{1}", baseDirectory, subFolder);
            var cdnUrl = string.Format("{0}/{1}", cdnBaseUrl, subFolder);

            var duplicates = AdCreativeUnits.GroupBy(s => s.Document.Name).SelectMany(grp => grp.Skip(1));
            foreach (var adCreativeUnit in duplicates)
            {
                adCreativeUnit.Document.NewName = string.Format("{0}_{1}_{2}", adCreativeUnit.Document.GetNameWithNoExtension(),
                                                             adCreativeUnit.CreativeUnit.Width,
                                                             adCreativeUnit.CreativeUnit.Height);
            }
            foreach (var adCreativeUnit in AdCreativeUnits)
            {
                //regenerate the script every approve, we added this to handle the case when the user change the required protocol
                //if (string.IsNullOrWhiteSpace(adCreativeUnit.Content))
                {
                    var docURL = adCreativeUnit.Document.ftpUpload(directory, cdnUrl);
                    string snapDocURL = null;
                    if (adCreativeUnit.SnapshotDocument != null)
                    {
                        snapDocURL = adCreativeUnit.SnapshotDocument.ftpUpload(directory, cdnUrl);
                        docURL = snapDocURL;

                    }
                    if (!adCreativeUnit.KeepShapshot)
                    {
                        adCreativeUnit.SnapshotUrl = docURL;
                        adCreativeUnit.KeepShapshot = true;
                    }


                    if (this.AdSubType==AdSubTypes.HTML5RichMedia || this.AdSubType == AdSubTypes.HTML5Interstitial)
                    {
                        UnZipp(adCreativeUnit.Document.ReadContent(), directory+"/"+"HTML5/",out docURL);
                    }

                   
                   var template = string.Empty;



                    RichMediaRequiredProtocol richMediaProtocols = GetRichMediaProtocol();
                    if (richMediaProtocols != null)
                    {
                        switch (richMediaProtocols.ID)
                        {
                            case (int)RichMediaProtocols.MRAID1:
                                {
                                    template = Configuration.MRAID1ExpandableRichMediaTemplate;
                                    break;
                                }
                            case (int)RichMediaProtocols.MRAID2:
                                {
                                    template = Configuration.MRAID2ExpandableRichMediaTemplate;
                                    break;
                                }
                        }
                    }
                    else
                    {
                        template = Configuration.NormalExpandableRichMediaTemplate;
                    }
                    if (this.AdSubType == AdSubTypes.HTML5RichMedia || this.AdSubType==AdSubTypes.HTML5Interstitial)
                    {
                        template=Configuration.HTML5PreTagScript;

                        docURL = cdnBaseUrl.Replace("/i","/").Replace("http","https") + docURL;
                          
                    }
                    adCreativeUnit.Content = template
                        .Replace("@BANNER_URL", docURL)
                        .Replace("@AD_UNIT_WIDTH", adCreativeUnit.CreativeUnit.Width.ToString())
                        .Replace("@AD_UNIT_HEIGHT", adCreativeUnit.CreativeUnit.Height.ToString())
                        .Replace("@ACTION_URL", (ActionValue!=null && ActionValue.Value!=null)? System.Web.HttpUtility.UrlEncode(ActionValue.Value):string.Empty);

                    if (this.AdSubType == AdSubTypes.HTML5Interstitial)
                    {
                        adCreativeUnit.Content = adCreativeUnit.Content
                        .Replace("@PLACEMENT_TYPE", "interstitial");
                    }
                    else if(this.AdSubType == AdSubTypes.HTML5RichMedia)
                    {
                        adCreativeUnit.Content = adCreativeUnit.Content
                 .Replace("@PLACEMENT_TYPE", "inline");

                    }
                }
            }
        }
        ///// <summary>
        ///// Extracts the content from a .zip file inside an specific folder.
        ///// </summary>
        ///// <param name="FileZipPath"></param>
        ///// <param name="password"></param>
        ///// <param name="OutputFolder"></param>
        //public void ExtractZipContent(string FileZipPath, string password, string OutputFolder)
        //{
            



        //    ZipFile file = null;
        //    try
        //    {
        //        FileStream fs = File.OpenRead(FileZipPath);
        //        file = new ZipFile(fs);

               

        //        foreach (ZipEntry zipEntry in file)
        //        {
        //            if (!zipEntry.IsFile)
        //            {
        //                // Ignore directories
        //                continue;
        //            }

        //            String entryFileName = zipEntry.Name;
        //            // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
        //            // Optionally match entrynames against a selection list here to skip as desired.
        //            // The unpacked length is available in the zipEntry.Size property.

        //            // 4K is optimum
        //            byte[] buffer = new byte[4096];
        //            Stream zipStream = file.GetInputStream(zipEntry);

        //            // Manipulate the output filename here as desired.
        //            String fullZipToPath = Path.Combine(OutputFolder, entryFileName);
        //            string directoryName = Path.GetDirectoryName(fullZipToPath);

        //            if (directoryName.Length > 0)
        //            {
        //                Directory.CreateDirectory(directoryName);
        //            }

        //            // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
        //            // of the file, but does not waste memory.
        //            // The "using" will close the stream even if an exception occurs.
        //            using (FileStream streamWriter = File.Create(fullZipToPath))
        //            {
        //                StreamUtils.Copy(zipStream, streamWriter, buffer);
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        if (file != null)
        //        {
        //            file.IsStreamOwner = true; // Makes close also shut the underlying stream
        //            file.Close(); // Ensure we release resources
        //        }
        //    }
        //}
        protected virtual void UnZipp(byte[] Content, string destDirPath, out string indexPath)
        {
            ZipInputStream zipIn = null;
            MemoryStream streamWriter = null;
            indexPath = string.Empty;
            try
            {
                //Directory.CreateDirectory(Path.GetDirectoryName(destDirPath));

                zipIn = new ZipInputStream(new MemoryStream(Content));
                ZipEntry entry;

                while ((entry = zipIn.GetNextEntry()) != null)
                {
                    //string dirPath = Path.GetDirectoryName(destDirPath + entry.Name);

                    string dirPath = Path.GetDirectoryName(destDirPath + entry.Name);
                    dirPath = dirPath.Replace(@"\", "/") + "/";

                    var EncodedPath = HttpUtility.UrlEncode(dirPath);
                    EncodedPath = dirPath.Replace("%20f", "/");


                    if (!Framework.Utilities.FtpHelper.DirectoryExists(EncodedPath))
                    {
                        Framework.Utilities.FtpHelper.CreateDirectory(EncodedPath);
                    }

                    if (!entry.IsDirectory)
                    {
                        streamWriter = new MemoryStream();
                        int size = 2048;
                        byte[] buffer = new byte[size];

                        while ((size = zipIn.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            streamWriter.Write(buffer, 0, size);
                        }
                        var bytesContent = streamWriter.ToArray();
                        if (entry.Name.ToLower()=="index.html" || entry.Name.ToLower() == "index.htm")
                        {
                            indexPath = EncodedPath + HttpUtility.UrlEncode(entry.Name) + "?clickTag={CLICK_URL_RED_ENC_SECURE}";
                            bytesContent =PrepareIndexHTML5(bytesContent);
                        }
                        Framework.Utilities.FtpHelper.Upload(EncodedPath + HttpUtility.UrlEncode(entry.Name), bytesContent);
                    }

                    streamWriter.Close();
                }
            }
            catch (System.Threading.ThreadAbortException lException)
            {
                // do nothing
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (zipIn != null)
                {
                    zipIn.Close();
                }

                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
            }
        }
        public virtual int getFirstAppearance(string content, int startIndex, char[] seek, out char foundSeek)
        {
           
            if (!(startIndex > 0))
            {
                foundSeek = ' ';
                return -1;
            }
            string lowerrContent = content;
            for (int i = startIndex; i < lowerrContent.Length; i++)
            {
                for (int z = 0; z < seek.Count(); z++)
                {
                    if (lowerrContent[i] == seek[z])
                    {
                        foundSeek = seek[z];
                        return i;
                    }

                }

            }
            foundSeek = ' ';
            return -1;
        }
        public virtual byte[] PrepareIndexHTML5(byte[] Conetent)
        {
            string content = System.Text.Encoding.UTF8.GetString(Conetent);
            int startIndex = 0;
            int endIndex = 0;
            char foundseek ;
            string lowerContent = content.ToLower();
            if (this.ClickTags!=null && this.ClickTags.Count>0)
            {
                foreach(var clickTag in this.ClickTags)
                    {
                    if (!string.IsNullOrEmpty(clickTag.TrackingUrl))
                    {
                        startIndex = lowerContent.IndexOf(clickTag.VariableName.ToLower());
                        startIndex = getFirstAppearance(lowerContent, startIndex, new char[] { '=' }, out foundseek);
                        startIndex = getFirstAppearance(lowerContent, startIndex, new char[] { '\'', '\"' }, out foundseek);
                        endIndex = getFirstAppearance(lowerContent, startIndex + 1, new char[] { foundseek }, out foundseek);
                        if (startIndex > 0 && endIndex > 0)
                            content = content.Substring(0, startIndex) + "adfalcon.getParameter(\"clickTag\")+" + "encodeURIComponent(\"" + clickTag.TrackingUrl + "\")" + content.Substring(endIndex + 1);
                    }
                }
            }
            //content= content.Insert(lowerContent.IndexOf("<head>")+ "<head>".Length, Configuration.HTML5PreTagScript);
            content = content.Insert(lowerContent.IndexOf("<head>") + "<head>".Length, "<script type=\"text/javascript\" src=\"https://cdn01.static.adfalcon.com/static/js/html5/mraidBridge.js\" ></script>");
             
           return  System.Text.Encoding.UTF8.GetBytes(content);
        }
        public override AdCreative Clone(AdGroup adGroup)
        {

            RichMediaCreative cloneObj = base.Clone<RichMediaCreative>();

            cloneObj.ClickMethod = this.ClickMethod;
            if (this.ClickTags!=null && this.ClickTags.Count>0)
            {
                cloneObj.ClickTags = new List<ClickTagTracker>();
                foreach (var clickTag in this.ClickTags)
                {
                    cloneObj.ClickTags.Add(clickTag.Clone());
                }
            }
            foreach (var adCreativeUnit in AdCreativeUnits)
            {
                var adCreativeUnitClone = adCreativeUnit.Copy(cloneObj);
                if (adCreativeUnitClone.SnapshotDocument != null)
                {
                    adCreativeUnitClone.SnapshotDocument.UpdateUsage();
                }

                cloneObj.AddCreativeUnit(adCreativeUnitClone);
            }
            if (this.GetRichMediaProtocol() != null)
            {
                cloneObj.SetRichMediaProtocol(this.GetRichMediaProtocol().ID, this.GetisMandRichMediaProtocol());
            }
            return cloneObj;
        }

        public override AdCreative Clone()
        {
            return Clone(this.Group);
        }



    }
}