using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NOTINOhw.Components.FileConvertor;
using MailSender;

namespace NOTINOhw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly FileConvertorFacade fileConvertorFacade;
        private readonly ConvertedFilesMailSender mailSender;

        public ConversionController(FileConvertorFacade fileConvertorFacade, ConvertedFilesMailSender mailSender)
        {
            this.fileConvertorFacade = fileConvertorFacade;
            this.mailSender = mailSender;
        }

        [HttpPost("convert-file")]
        public async Task<IActionResult> ConvertFile(IFormFile file, FileConvertorFacade.ConvertTo convertTo, string? emailAddress)
        {

            if (file.Length > 0)
            {
                string savedFilePath;
                try
                {
                    savedFilePath = fileConvertorFacade.ConvertFile(file, convertTo);
                }
                catch (Exception e)
                {
                    return ValidationProblem(e.Message);
                }

                if (emailAddress != null)
                {
                    mailSender.sendConvertedFile(emailAddress, savedFilePath);
                }

                FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(savedFilePath, out string? contentType))
                {
                    contentType = "application/octet-stream";
                }
                byte[] bytes = await System.IO.File.ReadAllBytesAsync(savedFilePath);

                return File(bytes, contentType, Path.GetFileName(savedFilePath));
            }
            return BadRequest();
        }

        [HttpPost("convert-file-from-url")]
        public async Task<IActionResult> ConvertFileFromUrl(string fileUrl, FileConvertorFacade.ConvertTo convertTo, string? emailAddress)
        {
            string savedFilePath;
            try
            {
                savedFilePath = fileConvertorFacade.ConvertFileFromUrl(fileUrl, convertTo);
            }
            catch (Exception e)
            {
                return ValidationProblem(e.Message);
            }

            if (emailAddress != null)
            {
                mailSender.sendConvertedFile(emailAddress, savedFilePath);
            }

            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(savedFilePath, out string? contentType))
            {
                contentType = "application/octet-stream";
            }
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(savedFilePath);

            return File(bytes, contentType, Path.GetFileName(savedFilePath));
        }
    }
}
