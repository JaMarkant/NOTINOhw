namespace StorageSolution.Tests.FileStorage
{
	public class LocalStorageTests
	{
		private readonly LocalStorage _sut;
		private readonly string _saveDirectory;
		public LocalStorageTests()
		{
			_saveDirectory = "SavedFiles";
			_sut = new LocalStorage(_saveDirectory);
		}

		[Fact]
		public void Storage_SaveFile_NewDirectoryWasCreated()
		{

			//Arrange
			string testFileName = "testFile.txt";
			string testFileContents = "These are the contents of saved file";
			//Act
			_sut.SaveFile(testFileName, testFileContents);
			//Assert
			Assert.True(Directory.Exists(_saveDirectory));
		}
		[Fact]
		public void Storage_SaveFile_FileExistsAndHasCorrectContent()
		{
			//Arrange
			string testFileName = "testFile.txt";
			string testFileContents = "These are the contents of saved file";
			//Act
			_sut.SaveFile(testFileName, testFileContents);
			//Assert
			string testFileFullPath = Path.Combine(_saveDirectory, testFileName);
			Assert.True(File.Exists(testFileFullPath));
			Assert.True(File.ReadAllText(testFileFullPath) == testFileContents);

		}
	}
}