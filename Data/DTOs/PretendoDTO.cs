namespace Pretendo.Backend.Data.DTOs
{
    /// <summary>
    /// Data Transfer Object for Pretendo
    /// </summary>
    public class PretendoDTO
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string ReturnObject { get; set; }
        public string Name { get; set; }
        public int StatusCode { get; set; }
    }
}
