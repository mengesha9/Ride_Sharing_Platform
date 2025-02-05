namespace Rideshare.Application.Features.Drivers.DTOs
{
  public class SearchDriversQueryDto
  {
    public string? SearchTerm { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
  }
}
