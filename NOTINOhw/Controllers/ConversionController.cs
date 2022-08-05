using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NOTINOhw.Services;

namespace NOTINOhw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly FileToJsonConverter fileToJsonConverter;
        private readonly FileToXmlConverter fileToXmlConverter;

        private string[] AcceptableTypes { get; set; }

        public ConversionController(FileToJsonConverter fileToJsonConverter, FileToXmlConverter fileToXmlConverter)
        {
            this.fileToJsonConverter = fileToJsonConverter;
            this.fileToXmlConverter = fileToXmlConverter;
            AcceptableTypes = new string[] { ".json", ".xml"};
        }
        [HttpPost("convert-file")]
        public async Task<IActionResult> ConvertFile(IFormFile file)
        {

            if (file.Length > 0)
            {
                string pathExtension = Path.GetExtension(file.FileName);
                if (AcceptableTypes.Contains(pathExtension))
                {
                    string savedFile = "";
                    switch (pathExtension)
                    {
                        case ".xml":
                            savedFile = fileToJsonConverter.ConvertFile(file);
                            break;
                        case ".json":
                            savedFile = fileToXmlConverter.ConvertFile(file);
                            break;
                    }
                    var provider = new FileExtensionContentTypeProvider();
                    if (!provider.TryGetContentType(savedFile, out var contentType))
                    {
                        contentType = "application/octet-stream";
                    }
                    var bytes = await System.IO.File.ReadAllBytesAsync(savedFile);
                    return File(bytes, contentType, Path.GetFileName(savedFile));

                }
                else
                {
                    return ValidationProblem();
                }
            }
            return Ok(fileToJsonConverter.ConvertFile(file) + "    " + fileToXmlConverter.ConvertFile(file));
        }
    }
}
