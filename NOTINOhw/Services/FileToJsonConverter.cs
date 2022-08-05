using Newtonsoft.Json;
using NOTINOhw.Components;
using System.Xml;

namespace NOTINOhw.Services
{
	public class FileToJsonConverter
	{
		private readonly StorageInterface storage;
		private const string jsonFileExtension = ".json";

		public FileToJsonConverter(StorageInterface storage)
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
                XmlDocument doc = new XmlDocument();
				doc.LoadXml(text);
				string json = JsonConvert.SerializeXmlNode(doc);
				savedFilePath = storage.SaveFile(Path.GetFileNameWithoutExtension(file.FileName) + jsonFileExtension, json);

			}
			return Path.GetFullPath(savedFilePath);
		}
	}
}
