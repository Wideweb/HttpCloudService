using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParallelLaibrary
{
    partial class CloudService: ISoapService
    {
        public bool isConnected()
        {
            Console.WriteLine("{0}: handshaking has been confirmed", DateTime.Now.TimeOfDay);
            return true;
        }

        public XElement DownloadFileList(int id)
        {
            Console.WriteLine("{0}: Client {1} is downloading fileList", DateTime.Now.TimeOfDay, id);
            return GetFileList(id);
        }

        public RemoteFileInfo DownloadFile(FileRequest request)
        {
            RemoteFileInfo response = new RemoteFileInfo();

            string path = Path.Combine(ServiceFolder, request.UserID.ToString(), request.FileName);
            if (!File.Exists(path))
            {
                Console.WriteLine("{0}: Client {1} is trying to DOWNLOAD the missing file",
                    DateTime.Now.TimeOfDay, request.UserID);
                return null;
            }

            response.UserID = request.UserID;
            response.FileName = request.FileName;
            response.FileByteStream = File.OpenRead(path);

            Console.WriteLine("{0}: Client {1} is downloading {3}",
                    DateTime.Now.TimeOfDay, request.UserID, request.FileName);
            return response;
        }

        public void UpDateFile(RemoteFileInfo request)
        {
            string path = Path.Combine(ServiceFolder, request.UserID.ToString(), request.FileName);

            Stream sourceStream = request.FileByteStream;
            using (Stream destinationStream = File.Open(path, FileMode.Create))
            {
                Console.WriteLine("{0}: Client {1} is updating {2}",
                    DateTime.Now.TimeOfDay, request.UserID, request.FileName);
                sourceStream.CopyTo(destinationStream);
            }
            sourceStream.Close();
            addFileToXML(request);
        }

        public void UploadFile(RemoteFileInfo request)
        {
            string path = Path.Combine(ServiceFolder, request.UserID.ToString(), request.FileName);
            CreateDirectoriesForFile(path);
            Stream sourceStream = request.FileByteStream;
            using (Stream destinationStream = File.Create(path))
            {
                Console.WriteLine("{0}: Client {1} is uploading {2}",
                    DateTime.Now.TimeOfDay, request.UserID, request.FileName);
                sourceStream.CopyTo(destinationStream);
            }
            sourceStream.Close();
            addFileToXML(request);
        }

        public void DeleteFile(FileRequest request)
        {
            string path = Path.Combine(ServiceFolder, request.UserID.ToString(), request.FileName);
            if (!File.Exists(path))
            {
                Console.WriteLine("{0}: Client {1} is trying to DELETE the missing file",
                    DateTime.Now.TimeOfDay, request.UserID);
                return;
            }

            File.Delete(path);
            RemoveFileFromXML(request);
            Console.WriteLine("{0}: Client {1}  deleted {2}",
                    DateTime.Now.TimeOfDay, request.UserID, request.FileName);

            DeleteEmptyFolders(Path.Combine(ServiceFolder, request.UserID.ToString()));
        }



        private XElement GetFileList(int id)
        {
            string path = Path.Combine(ServiceFolder, id.ToString() + ".xml");
            if (File.Exists(path))
            {
                return XElement.Load(path);
            }
            else
            {
                XElement serviceFiles = new XElement("ClientFiles");
                Stream st = File.Create(path);
                serviceFiles.Save(st);
                st.Close();
                return XElement.Load(path);
            }
        }

        private void addFileToXML(RemoteFileInfo fileInfo)
        {
            XElement serviceFiles = GetFileList(fileInfo.UserID);
            XElement xfile = new XElement("file");

            Console.WriteLine(fileInfo.LastWriteTime);
            xfile.Add(new XAttribute("Name", fileInfo.FileName));
            xfile.Add(new XAttribute("LastWriteTime", fileInfo.LastWriteTime));

            XElement serviceNode = serviceFiles.Elements()
                   .FirstOrDefault<XElement>(x => (x.Attribute("Name").Value == fileInfo.FileName));

            if (serviceNode == null)
            {
                serviceFiles.Add(xfile);
            }
            else
            {
                serviceNode.ReplaceWith(xfile);
            }

            string path = Path.Combine(ServiceFolder,
                fileInfo.UserID.ToString() + ".xml");

            using (Stream st = File.Create(path))
            {
                serviceFiles.Save(st);
            }
        }

        private void RemoveFileFromXML(FileRequest fileInfo)
        {
            XElement serviceFiles = GetFileList(fileInfo.UserID);
            XElement serviceNode = serviceFiles.Elements()
                   .FirstOrDefault<XElement>(x => (x.Attribute("Name").Value == fileInfo.FileName));

            if (serviceNode != null)
            {
                serviceNode.Remove();
            }

            string path = Path.Combine(ServiceFolder,
                fileInfo.UserID.ToString() + ".xml");

            using (Stream st = File.Create(path))
            {
                serviceFiles.Save(st);
            }
        }

        private void CreateDirectoriesForFile(string fileName)
        {
            string[] directories = fileName.Split('\\');
            string path = String.Empty;
            for (var i = 0; i < directories.Length - 1; i++)
            {
                path = Path.Combine(path, directories[i]);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }
        }

        private void DeleteEmptyFolders(string path)
        {
            if (!Directory.Exists(path))
                return;
            
            string[] dirs = Directory.GetDirectories(path);

            foreach (string dir in dirs)
            {
                DeleteEmptyFolders(dir);

                if (Directory.GetDirectories(dir).Length + Directory.GetFiles(dir).Length == 0)
                    Directory.Delete(dir);
            }
        }
    }
}
