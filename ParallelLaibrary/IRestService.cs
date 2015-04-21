using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.ServiceModel.Channels;
using System.Xml.Linq;

namespace ParallelLaibrary
{
    [ServiceContract]
    interface IRestService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/fileList/{id}")]
        XElement RestDownloadFileList(string id);

        [OperationContract]
        [WebInvoke(Method = "GET")]
        Stream RestDownload(string path);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "fileTree/{id}")]
        XElement RestDownloadFileTree(string id);
    }

    [DataContract]
    public class RestFileRequest
    {
        string userID;
        string fileName;

        [DataMember]
        public string UserID
        { get { return userID; } set { userID = value; } }

        [DataMember]
        public string FileName
        { get { return fileName; } set { fileName = value; } }

    }
}
