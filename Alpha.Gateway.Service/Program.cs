namespace Alpha.Gateway;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureAppConfiguration( c => 
                    c.AddJsonFile("ocelot.json"));
            })
            .Build();

        host.Run();
    }
}
