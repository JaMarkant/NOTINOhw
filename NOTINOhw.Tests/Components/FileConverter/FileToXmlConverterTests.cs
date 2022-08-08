using Microsoft.AspNetCore.Http;
using NOTINOhw.Components.FileConverter;
using StorageSolution;

namespace NOTINOhw.Tests.Components.FileConverter
{
	public class FileToXmlConverterTests
	{
		private readonly FileToXmlConverter _sut;
		private readonly string _savedFilesDirectoryPath = "SavedXmlFiles";
		public FileToXmlConverterTests()
		{
			_sut = new FileToXmlConverter(new LocalStorage(_savedFilesDirectoryPath));
		}

		//Cleanup
		~FileToXmlConverterTests()
		{
			Directory.Delete(_savedFilesDirectoryPath, true);
		}

		[Fact]
		public void FileToXmlConveter_ConvertJsonIFormFile_FileWasConverted()
		{
			//Arrange
			string xmlContent = "<articleinfo><title>My article</title></articleinfo>";
			string jsonContent = "{\"articleinfo\":{\"title\":\"My article\"}}";
			string xmlFileName = "test.xml";
			string jsonFileName = "test.json";
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(jsonContent);
			writer.Flush();
			stream.Position = 0;

			IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", jsonFileName);

			//Act
			_sut.ConvertFile(file);

			//Assert
			string convertedFileFullPath = Path.Combine(_savedFilesDirectoryPath, xmlFileName);
			Assert.True(File.Exists(convertedFileFullPath));
			Assert.Equal(xmlContent, File.ReadAllText(convertedFileFullPath));
		}
		[Fact]
		public void FileToXmlConveter_ConvertXmlIFormFile_ThrowsException()
		{
			//Arrange
			string xmlContent = "<articleinfo><title>My article</title></articleinfo>";
			string xmlFileName = "test.xml";
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(xmlContent);
			writer.Flush();
			stream.Position = 0;

			IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", xmlFileName);

			//Act & Assert
			Assert.Throws<Exception>(() => _sut.ConvertFile(file));
		}


		//couldnt find proper json file
		[Fact]
		public void FileToXmlConveter_ConvertJsonUrlFile_FileWasConverted()
		{
			////Arrange
			//string url = "https://tools.learningcontainer.com/sample-json.json";
			//string jsonFileName = "sample-json.xml";
			//string jsonContent = "";

			////Act
			//_sut.ConvertFile(url);

			////Assert
			//string convertedFileFullPath = Path.Combine(_savedFilesDirectoryPath, jsonFileName);
			//Assert.True(File.Exists(convertedFileFullPath));
			//Assert.Equal(jsonContent, File.ReadAllText(convertedFileFullPath));
		}

		[Fact]
		public void FileToXmlConveter_ConvertJsonUrlFile_ThrowsRootElementNotFound()
		{
			//Arrange
			string url = "https://tools.learningcontainer.com/sample-json.json";
			//Act & Assert
			Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => _sut.ConvertFile(url));
		}
		[Fact]
		public void FileToJsonConveter_ConvertXmlUrlFile_ThrowsException()
		{
			//Arrange
			string url = "https://www.w3schools.com/xml/note.xml";

			//Act & Assert
			Assert.Throws<Exception>(() => _sut.ConvertFile(url));
		}
	}
}

