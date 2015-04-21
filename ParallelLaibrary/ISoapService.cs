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
    public interface ISoapService
    {
        [OperationContract]
        [WebInvoke(Method = "GET")]
        bool isConnected();

        [OperationContract]
        [WebInvoke(Method = "HEAD")]
        XElement DownloadFileList(int id);

        [OperationContract]
        [WebInvoke(Method = "GET")]
        RemoteFileInfo DownloadFile(FileRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT")]
        void UpDateFile(RemoteFileInfo request);

        [OperationContract]
        [WebInvoke(Method = "POST")]
        void UploadFile(RemoteFileInfo request);
 
        [OperationContract]
        [WebInvoke(Method = "DELETE")]
        void DeleteFile(FileRequest request);
    }


    [MessageContract]
    public class FileRequest
    {
        [MessageBodyMember] 
        public int UserID;

        [MessageBodyMember]
        public string FileName;
    }


    [MessageContract]
    public class RemoteFileInfo: IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public int UserID;

        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public DateTime LastWriteTime;

        [MessageBodyMember(Order = 1)]
        public Stream FileByteStream;

        public void Dispose()
        {
            if(FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
}
