using Microsoft.Extensions.Configuration;

namespace StorageSolution
{
    public class LocalStorage : IStorage
    {
        public string SaveFolder { get; set; }
        public LocalStorage(IConfiguration configuration)
        {
            SaveFolder = configuration.GetValue<string>("LocalStorage:SaveFolder");
            if (!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }
        }
        public string SaveFile(string fileName, string contents)
        {
            string savedFilePath = Path.Combine(SaveFolder, fileName);
            using (StreamWriter writer = new StreamWriter(savedFilePath))
            {
                writer.Write(contents);
            }
            return savedFilePath;
        }
    }
}
