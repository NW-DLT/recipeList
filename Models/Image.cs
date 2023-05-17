using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace recipeList.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public IFormFile FormFile { get; set; }
        public string Src { get; set; }

        public string getSrc(Image item)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");

            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //get file extension
            FileInfo fileInfo = new FileInfo(item.FormFile.FileName);
            string fileName = item.FormFile.FileName;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                this.FormFile.CopyTo(stream);
            }
            return fileName;
        }
    }
}
