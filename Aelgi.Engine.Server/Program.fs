// Learn more about F# at http://fsharp.org

open Aelgi.Engine.Server
open System
open Aelgi.Engine.Server.Handler
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

let configureAppConfiguration (context: HostBuilderContext) (config: IConfigurationBuilder) =
    let environmentAppSettings =
        context.HostingEnvironment.EnvironmentName
        |> sprintf "appsettings.%s.json"
    
    config
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile(environmentAppSettings, true, true)
        .AddEnvironmentVariables() |> ignore
        
    ()
    
let configureServices (context: HostBuilderContext) (services: IServiceCollection) =
    let appSettings = AppSettings()
    context.Configuration.GetSection("App").Bind(appSettings)
    
    services.AddSingleton(appSettings) |> ignore
    
    ()

[<EntryPoint>]
let main argv =
    let builder =
        HostBuilder()
            .ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureAppConfiguration)
            .ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureServices)
            .Build()

    let appSettings = builder.Services.GetService<AppSettings>()         

    Server.listen 7707

    0 // return an integer exit code
