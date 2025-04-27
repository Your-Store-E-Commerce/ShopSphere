namespace ShopSphere.Web.Helper
{
	public class DocumentSetting
	{
		public static string UploadFiles(IFormFile file, string folderName)
		{
			if (file is null)
				return "";
			//1. Get Located Folder Path
			var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//files", folderName);

			//2.Get File Name and Make it Unique

			string fileName = $"{Guid.NewGuid()}-{file.FileName}"; ;

			//3.Get File Path[Folder Path + FileName]

			var FilePath = Path.Combine(FolderPath, fileName);

			//4.Save File As Streams

			using var FS = new FileStream(FilePath, FileMode.Create);

			file.CopyTo(FS);

			//5.Return File Name

			return fileName;
		}
		public static void DeleteFile(string fileName, string folderName)
		{
			//1.Get File Path

			var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//files", folderName, fileName);
			//2.Check if File Exist or not 
			if (File.Exists(FilePath))
			{//if Exist Remove it
				File.Delete(FilePath);
			}

		}
	}
}
