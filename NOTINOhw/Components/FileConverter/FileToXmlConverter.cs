using Newtonsoft.Json;
using StorageSolution;
using System.Xml;

namespace NOTINOhw.Components.FileConverter
{
    public class FileToXmlConverter : IFileConverter
    {
        private readonly IStorage Storage;
        private const string XmlFileExtension = ".xml";
        private readonly string[] AccepteableTypes = { ".json" };

        public FileToXmlConverter(IStorage storage)
        {
            this.Storage = storage;
        }
        public string ConvertFile(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) == XmlFileExtension)
                throw new Exception("Convert format and file format are the same");

            string savedFilePath = "";
            using (var stram = file.OpenReadStream())
            {
                StreamReader sr = new StreamReader(stram);
                string text = sr.ReadToEnd();
                XmlDocument? doc = JsonConvert.DeserializeXmlNode(text);
                if (doc != null)
                    savedFilePath = Storage.SaveFile(Path.GetFileNameWithoutExtension(file.FileName) + XmlFileExtension, doc.OuterXml);
                else
                    throw new Exception("Conversion went wrong");
            }
            return Path.GetFullPath(savedFilePath);
        }

        public string ConvertFile(string url)
        {
            if (Path.GetExtension(url) == XmlFileExtension)
                throw new Exception("Convert format and file format are the same");

            string savedFilePath = "";
            using (HttpClient client = new HttpClient())
            {
                string text = client.GetStringAsync(url).Result;
                XmlDocument? doc = JsonConvert.DeserializeXmlNode(text);
                if (doc != null)
                    savedFilePath = Storage.SaveFile(Path.GetFileNameWithoutExtension(url) + XmlFileExtension, doc.OuterXml);
                else
                    throw new Exception("Conversion went wrong");
            }
            return Path.GetFullPath(savedFilePath);
        }

        public string[] GetAcceptableTypes()
        {
            return AccepteableTypes;
        }
    }
}
