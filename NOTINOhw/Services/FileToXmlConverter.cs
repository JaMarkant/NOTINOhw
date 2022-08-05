using Newtonsoft.Json;
using NOTINOhw.Components;
using System.Xml;

namespace NOTINOhw.Services
{
    public class FileToXmlConverter
    {
        private readonly StorageInterface storage;
        private const string xmlFileExtension = ".xml";

        public FileToXmlConverter(StorageInterface storage)
        {
            this.storage = storage;
        }
        public string ConvertFile(IFormFile file)
        {
            string savedFilePath = "";
            using (var stram = file.OpenReadStream())
            {
                StreamReader sr = new StreamReader(stram);
                string text = sr.ReadToEnd();
                XmlDocument? doc = JsonConvert.DeserializeXmlNode(text);
                if (doc != null)
                    savedFilePath = storage.SaveFile(Path.GetFileNameWithoutExtension(file.FileName) + xmlFileExtension, doc.OuterXml);
                else
                    throw new Exception("Conversion went wrong");
            }
            return Path.GetFullPath(savedFilePath);
        }
    }
}
