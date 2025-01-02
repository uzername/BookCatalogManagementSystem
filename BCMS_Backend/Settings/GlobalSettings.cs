
namespace BCMS_Backend.Settings
{
    /// <summary>
    /// handle settings for web app
    /// https://stackoverflow.com/questions/58350949/how-to-write-connection-string-for-my-asp-net-core-mvc
    /// https://stackoverflow.com/questions/39231951/how-do-i-access-configuration-in-any-class-in-asp-net-core
    /// https://metanit.com/sharp/aspnet6/6.1.php
    /// </summary>
    public static class GlobalSettings
    {
        public static string Location { get; set; }
        public static IConfiguration Configuration;

        public static string? GetConnectionConnectionString()
        {
           return Configuration["ConnectionString:defaultConnection"];
        }
    }
}
