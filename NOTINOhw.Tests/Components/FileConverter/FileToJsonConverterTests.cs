using Microsoft.AspNetCore.Http;
using NOTINOhw.Components.FileConverter;
using StorageSolution;

namespace NOTINOhw.Tests.Components.FileConverter
{
	public class FileToJsonConverterTests
	{
		private readonly FileToJsonConverter _sut;
		private readonly string _savedFilesDirectoryPath = "SavedJsonFiles";
		public FileToJsonConverterTests()
		{
			_sut = new FileToJsonConverter(new LocalStorage(_savedFilesDirectoryPath));
		}

		//Cleanup
		~FileToJsonConverterTests()
		{
			Directory.Delete(_savedFilesDirectoryPath, true);
		}

		[Fact]
		public void FileToJsonConveter_ConvertXmlIFormFile_FileWasConverted()
		{
			//Arrange
			string xmlContent = @"<articleinfo>
  <title>My article</title>
</articleinfo>";
			string jsonContent = "{\"articleinfo\":{\"title\":\"My article\"}}";
			string xmlFileName = "test.xml";
			string jsonFileName = "test.json";
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(xmlContent);
			writer.Flush();
			stream.Position = 0;

			IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", xmlFileName);

			//Act
			_sut.ConvertFile(file);

			//Assert
			string convertedFileFullPath = Path.Combine(_savedFilesDirectoryPath, jsonFileName);
			Assert.True(File.Exists(convertedFileFullPath));
			Assert.Equal(jsonContent, File.ReadAllText(convertedFileFullPath));
		}
		[Fact]
		public void FileToJsonConveter_ConvertJsonIFormFile_ThrowsException()
		{
			//Arrange
			string content = "{\"articleinfo\":{\"title\":\"My article\"}}";
			string fileName = "test.json";
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(content);
			writer.Flush();
			stream.Position = 0;

			IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

			//Act & Assert
			Assert.Throws<Exception>(() => _sut.ConvertFile(file));
		}



		[Fact]
		public void FileToJsonConveter_ConvertXmlUrlFile_FileWasConverted()
		{
			//Arrange
			string url = "https://www.w3schools.com/xml/note.xml";
			string jsonFileName = "note.json";
			string jsonContent = "{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"UTF-8\"},\"note\":{\"to\":\"Tove\",\"from\":\"Jani\",\"heading\":\"Reminder\",\"body\":\"Don't forget me this weekend!\"}}";

			//Act
			_sut.ConvertFile(url);

			//Assert
			string convertedFileFullPath = Path.Combine(_savedFilesDirectoryPath, jsonFileName);
			Assert.True(File.Exists(convertedFileFullPath));
			Assert.Equal(jsonContent, File.ReadAllText(convertedFileFullPath));
		}
		[Fact]
		public void FileToJsonConveter_ConvertJsonUrlFile_ThrowsException()
		{
			//Arrange
			string url = "https://tools.learningcontainer.com/sample-json.json";

			//Act & Assert
			Assert.Throws<Exception>(() => _sut.ConvertFile(url));
		}
	}
}
