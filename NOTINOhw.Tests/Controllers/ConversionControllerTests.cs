using MailSender;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOTINOhw.Components.FileConverter;
using NOTINOhw.Controllers;
using StorageSolution;

namespace NOTINOhw.Tests.Controllers
{
	public class ConversionControllerTests
	{
		private readonly ConversionController _sut;
		private readonly string _savedFilesDirectoryPath = "SavedControllerFiles";
		public ConversionControllerTests()
		{
			LocalStorage storage = new LocalStorage(_savedFilesDirectoryPath);
			FileConverterFacade converterFacade = new FileConverterFacade(
															new FileToJsonConverter(storage),
															new FileToXmlConverter(storage)
														);
			_sut = new ConversionController(converterFacade, new ConvertedFilesMailSender());
		}

		//Cleanup
		~ConversionControllerTests()
		{
			Directory.Delete(_savedFilesDirectoryPath, true);
		}

		[Fact]
		public async void ConversionController_ConvertJsonFile_FileResponse()
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

			//Act
			IActionResult result = await _sut.ConvertFile(file, FileConverterFacade.ConvertTo.xml, null);

			//Assert
			Assert.IsType<FileContentResult>(result);
		}

		[Fact]
		public async void ConversionController_ConvertWrongFormatFile_ValidationProblemResponse()
		{
			//Arrange
			string jsonContent = "{\"articleinfo\":{\"title\":\"My article\"}}";
			string jsonFileName = "test.jpg";
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(jsonContent);
			writer.Flush();
			stream.Position = 0;

			IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", jsonFileName);

			//Act
			IActionResult result = await _sut.ConvertFile(file, FileConverterFacade.ConvertTo.xml, null);
			ObjectResult objectResult = (ObjectResult)result;
			//Assert
			Assert.IsType<ValidationProblemDetails>(objectResult.Value);
		}


		[Fact]
		public async void ConversionController_ConvertUrlXmlFile_FileResponse()
		{
			//Arrange & Act
			IActionResult result = await _sut.ConvertFileFromUrl(
					"https://www.w3schools.com/xml/plant_catalog.xml",
					FileConverterFacade.ConvertTo.json,
					null
				);

			//Assert
			Assert.IsType<FileContentResult>(result);
		}

		[Fact]
		public async void ConversionController_ConvertWrongFormatUrlFile_ValidationProblemResponse()
		{
			//Arrange & Act
			IActionResult result = await _sut.ConvertFileFromUrl(
					"https://www.rd.com/wp-content/uploads/2021/01/GettyImages-1175550351.jpg",
					FileConverterFacade.ConvertTo.xml,
					null
				);
			ObjectResult objectResult = (ObjectResult)result;

			//Assert
			Assert.IsType<ValidationProblemDetails>(objectResult.Value);
		}
	}
}