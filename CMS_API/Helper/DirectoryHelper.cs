namespace CMS_API.Helper
{
    public static class DirectoryHelper
    {
        public static async Task<string> ProcessUploadFile(IFormFile postedFile, string fatherFolder, string fileNameStartWith)
        {
            try
            {
                if(postedFile == null || postedFile.Length == 0) return "Error: No file selected";

                var originalFileName = Path.GetFileName(postedFile.FileName);
                var fileName = fileNameStartWith + TimeHelper.ToUnixTimeStamp(DateTime.UtcNow) + Path.GetExtension(originalFileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fatherFolder);

                if(!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var path = Path.Combine(directory, fileName);

                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await postedFile.CopyToAsync(stream);
                }

                return path;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return $"Error:  + {ex.Message}";
            }
        }
    }
}
