namespace Mit.Models.Configurations
{
    public class MitConfiguration
    {
        public string? ApplicationName { get; set; }          
        public string? MyAppConnString { get; set; }          
        public string? DefaultConnection { get; set; }        
        public MapSettings? MapSettings { get; set; }        
        public string? ApiKey { get; set; }
        public ProjectSettings? ProjectSettings { get; set; }
    }
}
