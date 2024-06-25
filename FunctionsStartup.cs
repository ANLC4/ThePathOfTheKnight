using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(ThePathofKnight.Startup))]

namespace ThePathofKnight;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {        
        builder.Services.AddSingleton<IThePathOfTheKnight>((s) => {
            return new ThePathOfTheKnightBFS();
        });        
    }
}