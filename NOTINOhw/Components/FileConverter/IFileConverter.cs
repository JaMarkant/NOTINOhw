namespace NOTINOhw.Components.FileConverter
{
    public interface IFileConverter
    {
        public string ConvertFile(IFormFile file);
        public string ConvertFile(string url);
        public string[] GetAcceptableTypes();
    }
}
