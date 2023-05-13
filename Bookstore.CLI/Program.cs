using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddSystemsManager( configSource =>
    {
        configSource.Path = "/BobsBookstore/";
        configSource.AwsOptions = config.GetAWSOptions();
    } )
    .Build();

Console.WriteLine($"Domain: {configuration["Authentication:Cognito:CognitoDomain"]}");
