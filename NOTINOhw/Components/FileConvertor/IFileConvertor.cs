namespace NOTINOhw.Components.FileConvertor
{
    public interface IFileConvertor
    {
        public string ConvertFile(IFormFile file);
        public string ConvertFile(string url);
        public string[] GetAcceptableTypes();
    }
}
