namespace CoolAppProject.Extentions
{
    public static class FileUpload
    {
        public static string CreateImage(this IFormFile file,string folder,string path)
        {
            string fullName = Guid.NewGuid().ToString() + file.FileName;
            string fullPath=Path.Combine(folder,path,fullName);
            using(FileStream fileStream=new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return fullName;
        }
    }
}
