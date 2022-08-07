namespace NOTINOhw.Components.FileConvertor
{
    public class FileConvertorFacade
    {
        private readonly FileToJsonConvertor fileToJsonConvertor;
        private readonly FileToXmlConvertor fileToXmlConvertor;
        private List<string> CovertorsAcceptableTypes;

        public FileConvertorFacade(FileToJsonConvertor fileToJsonConvertor, FileToXmlConvertor fileToXmlConvertor)
        {
            this.fileToJsonConvertor = fileToJsonConvertor;
            this.fileToXmlConvertor = fileToXmlConvertor;
            CovertorsAcceptableTypes = new List<string>();
            CovertorsAcceptableTypes.AddRange(fileToJsonConvertor.GetAcceptableTypes());
            CovertorsAcceptableTypes.AddRange(fileToXmlConvertor.GetAcceptableTypes());
        }

        public string ConvertFile(IFormFile file, ConvertTo convertTo)
        {
            string pathExtension = Path.GetExtension(file.FileName);
            if (CovertorsAcceptableTypes.Contains(pathExtension))
            {

                string savedFile = string.Empty;
                switch (convertTo)
                {
                    case ConvertTo.json:
                        savedFile = fileToJsonConvertor.ConvertFile(file);
                        break;
                    case ConvertTo.xml:
                        savedFile = fileToXmlConvertor.ConvertFile(file);
                        break;
                }
                return savedFile;
            }
            else
            {
                throw new Exception("File format not supported");
            }
        }

        public string ConvertFileFromUrl(string url, ConvertTo convertTo)
        {
            string pathExtension = Path.GetExtension(url);
            if (CovertorsAcceptableTypes.Contains(pathExtension))
            {

                string savedFile = string.Empty;
                switch (convertTo)
                {
                    case ConvertTo.json:
                        savedFile = fileToJsonConvertor.ConvertFile(url);
                        break;
                    case ConvertTo.xml:
                        savedFile = fileToXmlConvertor.ConvertFile(url);
                        break;
                }
                return savedFile;
            }
            else
            {
                throw new Exception("File format not supported");
            }
        }

        public enum ConvertTo
        {
            xml,
            json
        }
    }
}
