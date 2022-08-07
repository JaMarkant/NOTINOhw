using Newtonsoft.Json;
using StorageSolution;
using System.Net;
using System.Xml;

namespace NOTINOhw.Components.FileConvertor
{
    public class FileToJsonConvertor : IFileConvertor
    {
        private readonly IStorage Storage;
        private const string JsonFileExtension = ".json";
        private readonly string[] AccepteableTypes = { ".xml" };

        public FileToJsonConvertor(IStorage storage)
        {
            this.Storage = storage;
        }
        public string ConvertFile(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) == JsonFileExtension)
                throw new Exception("Convert format and file format are the same");

            string savedFilePath = "";
            using (var stram = file.OpenReadStream())
            {
                StreamReader sr = new StreamReader(stram);
                string text = sr.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(text);
                string json = JsonConvert.SerializeXmlNode(doc);
                savedFilePath = Storage.SaveFile(Path.GetFileNameWithoutExtension(file.FileName) + JsonFileExtension, json);

            }
            return Path.GetFullPath(savedFilePath);
        }
        public string ConvertFile(string url)
        {
            if (Path.GetExtension(url) == JsonFileExtension)
                throw new Exception("Convert format and file format are the same");

            XmlDocument doc = new XmlDocument();
            doc.Load(url);
            string json = JsonConvert.SerializeXmlNode(doc);
            string savedFilePath = Storage.SaveFile(Path.GetFileNameWithoutExtension(url) + JsonFileExtension, json);

            return Path.GetFullPath(savedFilePath);
        }

        public string[] GetAcceptableTypes()
        {
            return AccepteableTypes;
        }
    }
}
