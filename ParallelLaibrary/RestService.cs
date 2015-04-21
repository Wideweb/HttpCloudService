using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParallelLaibrary
{
    partial class CloudService: IRestService
    {
        public XElement RestDownloadFileList(string id)
        {
            return RestGetFileList(id);
        }

        public Stream RestDownload(string path)
        {
            string filePath = Path.Combine(ServiceFolder, path);
            if (!File.Exists(filePath))
            {
                Console.WriteLine("{0}: request to DOWNLOAD the missing file {1}",
                    DateTime.Now.TimeOfDay, filePath);
                return null;
            }

            Console.WriteLine("{0}: file {1} fileList was downloaded from service", DateTime.Now.TimeOfDay, path);
            return File.OpenRead(filePath);
        }

        public XElement RestDownloadFileTree(string id)
        {
            string dirPath = Path.Combine(ServiceFolder, id.ToString());
            if (Directory.Exists(dirPath))
            {
                Console.WriteLine("{0}: Client {1} fileTree was requested", DateTime.Now.TimeOfDay, id);
                return FileStructSerializer.FileStructToXMLTree(dirPath);
            }
            else
            {
                Console.WriteLine("{0}: request to DOWNLOAD the missing Client {1} FileTree",
                    DateTime.Now.TimeOfDay, id);
                return new XElement("Empty");
            }
        }

        private XElement RestGetFileList(string id)
        {
            string fileName = id + ".xml";
            string path = Path.Combine(ServiceFolder, fileName);

            if (File.Exists(path))
            {
                Console.WriteLine("{0}: Client {1} fileList was requested", DateTime.Now.TimeOfDay, id);
                return XElement.Load(path);
            }
            else
            {
                Console.WriteLine("{0}: request to DOWNLOAD the missing Client {1} FileList",
                    DateTime.Now.TimeOfDay, id);
                return new XElement("Empty");
            }
        }
    }
}
