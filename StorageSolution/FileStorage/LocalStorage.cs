using Microsoft.Extensions.Configuration;

namespace StorageSolution
{
    public class LocalStorage : IStorage
    {
        private string SaveDirectory { get; set; }
        public LocalStorage(IConfiguration configuration)
        {
            SaveDirectory = configuration.GetValue<string>("LocalStorage:SaveFolder");
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }
        public LocalStorage(string saveFolderPath)
        {
            SaveDirectory = saveFolderPath;
            CheckDirectoryExistsAndCreate(SaveDirectory);
        }
        public string SaveFile(string fileName, string contents)
        {
            string savedFilePath = Path.Combine(SaveDirectory, fileName);
            using (StreamWriter writer = new StreamWriter(savedFilePath))
            {
                writer.Write(contents);
            }
            return savedFilePath;
        }
        private void CheckDirectoryExistsAndCreate(string saveDirectory)
		{
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }
    }
}
