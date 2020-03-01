namespace SevenWestMedia.ApiClient.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var startup = new Startup();
            startup
                .ConfigureServices()
                .Configure();
        }
    }
}
