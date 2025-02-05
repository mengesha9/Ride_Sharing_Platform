namespace Rideshare.Application.Features.PackageTypes.Dtos
{
    public class PackageTypeDto
    {
        public int Id { get; set; }
        public string type { get; set; }
        public int NumberOfDay { get; set; }
        public double Discount { get; set; }
    }
}