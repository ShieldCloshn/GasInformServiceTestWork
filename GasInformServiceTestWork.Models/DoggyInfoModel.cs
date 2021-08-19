namespace GasInformServiceTestWork.Models
{
    public class DoggyInfoModel
    {
        public DoggyInfoModel() 
        {
        }

        public DoggyInfoModel(string bread, string fileName)
        {
            Breed = bread;
            FileName = fileName;
        }

        public string Breed { get; set; }
        public string FileName { get; set; }
    }
}
