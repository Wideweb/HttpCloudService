using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParallelLaibrary
{
    class FileStructSerializer
    {
        public static XElement FileStructToXMLTree(string path)
        {
            XElement clientFiles = new XElement("ClientFiles");
            AddDirectories(path, clientFiles);

            return clientFiles;
        }

        static void AddDirectories(string path, XElement parent)
        {
            if (!Directory.Exists(path))
                return;

            AddFiles(path, parent);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string dir in dirs)
            {
                var di = new DirectoryInfo(dir);
                XElement xdir = new XElement("directory");

                xdir.Add(new XAttribute("Name", di.Name));
                parent.Add(xdir);

                AddDirectories(dir, xdir);
            }
        }

        static void AddFiles(string path, XElement parent)
        {
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (!File.Exists(file)) continue;

                XElement xfile = new XElement("file");
                var fi = new FileInfo(file);

                xfile.Add(new XAttribute("Name", fi.Name));
                //xfile.Add(new XAttribute("LastWriteTime", fi.LastWriteTime));
                
                parent.Add(xfile);
            }
        }
    }
}
