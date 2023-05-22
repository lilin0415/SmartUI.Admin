using Koo.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YiSha.Model.Publishes
{
    public class PublishedInfoHelper
    {
      
        public static bool ParseJson(string json, out PublishedInfoHeader header, out PublishedInfo data)
        {

            if (string.IsNullOrEmpty(json))
            {
                header = null;
                data = null;
                return false;
            }

            header = PublishedInfoHeader.Read(json, out string dataJson);

            data = JsonHelper.ToObjectIgnoreTypeName<PublishedInfo>(dataJson);
            data.Header = header;

            return true;
        }

        public static string ToJson(PublishedInfo info)
        {
            var header = info.Header.ToData();

            var dataJson = JsonHelper.ToJson(info);
            return header + dataJson;
        }

        public static void SetParntId(PublishedDocument currentDoc, PublishedInfo info)
        {
            var filePath = currentDoc.RelativePath;
            if (FileHelper.GetFileExt(filePath).ToLower() == ".sproj")
            {
                currentDoc.ParentId = "0";
            }
            else if(FileHelper.GetFileNameWithoutExtension(filePath)=="主流程")
            {
                currentDoc.ParentId = "0";
            }
            else
            {

                var folderPath = FileHelper.GetParentDirectoryPath(filePath, true);
                var parentFlowName = FileHelper.GetFolderName(folderPath);
                var parentFlowFile = parentFlowName + ".sflow";

                var parentDoc = info.Documents.FirstOrDefault(x => FileHelper.Equals(x.RelativePath, parentFlowFile));
                if (parentDoc != null)
                {
                    currentDoc.ParentId = parentDoc.Id;
                }
                else
                {
                    currentDoc.ParentId = "0";
                }
            }
        }
    }
}
