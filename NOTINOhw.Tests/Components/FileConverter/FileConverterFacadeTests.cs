using Microsoft.AspNetCore.Http;
using NOTINOhw.Components.FileConverter;
using StorageSolution;

namespace NOTINOhw.Tests.Components.FileConverter
{
	public class FileConverterFacadeTests
	{
		private readonly FileConverterFacade _sut;
		private readonly string _savedFilesDirectoryPath = "SavedFacadeFiles";
		public FileConverterFacadeTests()
		{
			LocalStorage storage = new LocalStorage(_savedFilesDirectoryPath);
			_sut = new FileConverterFacade(new FileToJsonConverter(storage), new FileToXmlConverter(storage));
		}

		//Cleanup
		~FileConverterFacadeTests()
		{
			Directory.Delete(_savedFilesDirectoryPath, true);
		}

		[Fact]
		public void FileConverterFacade_ConvertJsonIFormFileToXml_ReturnsCorrectConvertedFilePath()
		{
			//Arrange
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
			string convertedFilePath = _sut.ConvertFile(file, FileConverterFacade.ConvertTo.xml);

			//Assert
			Assert.Equal(convertedFilePath, Path.GetFullPath(Path.Combine(_savedFilesDirectoryPath, xmlFileName)));
		}

		[Fact]
		public void FileConverterFacade_ConvertXmlIFormFileToJson_ReturnsCorrectConvertedFilePath()
		{
			//Arrange
			string xmlContent = @"<articleinfo>
  <title>My article</title>
</articleinfo>";
			string xmlFileName = "test.xml";
			string jsonFileName = "test.json";
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(xmlContent);
			writer.Flush();
			stream.Position = 0;

			IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", xmlFileName);

			//Act
			_sut.ConvertFile(file, FileConverterFacade.ConvertTo.json);

			//Act
			string convertedFilePath = _sut.ConvertFile(file, FileConverterFacade.ConvertTo.json);

			//Assert
			Assert.Equal(convertedFilePath, Path.GetFullPath(Path.Combine(_savedFilesDirectoryPath, jsonFileName)));
		}

		[Fact]
		public void FileConverterFacade_ConvertJsonIFormFileToJson_ThrowsException()
		{
			//Arrange
			string jsonContent = "{\"articleinfo\":{\"title\":\"My article\"}}";
			string jsonFileName = "test.json";
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(jsonContent);
			writer.Flush();
			stream.Position = 0;

			IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", jsonFileName);

			//Act & Assert
			Assert.Throws<Exception>(() => _sut.ConvertFile(file, FileConverterFacade.ConvertTo.json));
		}

		[Fact]
		public void FileConverterFacade_ConvertXmlIFormFileToXml_ThrowsException()
		{
			//Arrange
			string xmlContent = @"<articleinfo>
  <title>My article</title>
</articleinfo>";
			string xmlFileName = "test.xml";
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(xmlContent);
			writer.Flush();
			stream.Position = 0;

			IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", xmlFileName);

			//Act & Assert
			Assert.Throws<Exception>(() => _sut.ConvertFile(file, FileConverterFacade.ConvertTo.xml));
		}
	}
}
