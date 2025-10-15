namespace Mit.Models.Configurations
{
    public class MapSettings
    {
        public string? GoogleMapsApiKey { get; set; }
        public int DefaultZoomLevel { get; set; }
        public DefaultLocation? DefaultLocation { get; set; }
    }
}
