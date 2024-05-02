using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Location Folder Path

            //string folderPath = "D:\\.Net C40\\Projects\\Demo MVC G04 Solution\\Demo.PL\\wwwroot\\files\\" + folderName;

            //string folderPath = Directory.GetCurrentDirectory()  + "wwwroot\\files\\" + folderName;

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);


            // 2. Get File Name Make It UNIQUE

            string fileName = $"{Guid.NewGuid()}{file.FileName}";


            // 3. Get File Path --> FolderPath + FileName

            string filePath = Path.Combine(folderPath, fileName);


            // 4. Save File as Stream : Data Per Time

            using var fileStream = new FileStream(filePath, FileMode.Create);


            file.CopyTo(fileStream);


            return fileName;


        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

    }
}
