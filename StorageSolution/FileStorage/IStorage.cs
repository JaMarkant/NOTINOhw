namespace StorageSolution
{
    public interface IStorage
    {
        public string SaveFile(string fileName, string contents);
    }
}
