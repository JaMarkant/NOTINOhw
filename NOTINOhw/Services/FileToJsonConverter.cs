namespace NOTINOhw.Services
{
	public class FileToJsonConverter : FileConversionInterface
	{
		public FileToJsonConverter() { }
		public string ConvertFile(IFormFile file)
		{
			var targetFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Document1.json");
			return Environment.CurrentDirectory;
		}
	}
}
