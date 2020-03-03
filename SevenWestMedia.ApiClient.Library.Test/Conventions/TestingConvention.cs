namespace SevenWestMedia.ApiClient.Library.Test.Conventions
{
    using Fixie;

    public class TestingConvention : Discovery
    {
        public TestingConvention()
        {
            // Setup the container;
            var services = Startup.Setup();

            // Use the container as a parameter source for the tests.
            var inputParameterSource = new InputParameterSource(services);

            Classes
                .Where(x => x.Name.EndsWith("Tests"));

            Methods
                .Where(x => x.Name != "TestSetup");

            Parameters
                .Add(inputParameterSource);
        }
    }
}

