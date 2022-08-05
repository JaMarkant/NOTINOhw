using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOTINOhw.Services;

namespace NOTINOhw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
		private readonly FileToJsonConverter fileToJsonConverter;

		public ConversionController(FileToJsonConverter fileToJsonConverter)
		{
			this.fileToJsonConverter = fileToJsonConverter;
		}
        [HttpPost("convert-file")]
        public async Task<IActionResult> ConvertFile(IFormFile file, string convertFormat)
        {

            if (file.Length > 0)
            {
                FileTypes fileType;
                if (Enum.TryParse(convertFormat.ToLower(), out fileType))
                {
                    switch (fileType) {
                        case FileTypes.xml:
                            break;
                        case FileTypes.json:
                            break;
                    }
                }
            }
            return Ok(fileToJsonConverter.ConvertFile(file));
        }
    }

    public enum FileTypes
	{
        xml,
        json
	}
}
