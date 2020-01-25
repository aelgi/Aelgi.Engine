// Learn more about F# at http://fsharp.org

open Aelgi.Engine.Server
open System
open Aelgi.Engine.Server.Handler
open Aelgi.Engine.DataAccess.Neo4j.Services
open Aelgi.Engine.Server.Handler
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
        Host.CreateDefaultBuilder(argv)
//            .ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureAppConfiguration)
            .ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureServices)
            .Build()

    let appSettings = builder.Services.GetService<AppSettings>()
    
    let dbAdapter = DBConnector.connect appSettings.Neo4j.URI appSettings.Neo4j.Username appSettings.Neo4j.Password
    DBConnector.testConnection dbAdapter () |> Async.RunSynchronously
    
    let handlers = {
        Delegator.pingDelegator = PingDelegator.handler
        Delegator.timeDelegator = TimeDelegator.handler
    }
    let delegator = Delegator.processMessage handlers

    Server.listen delegator 7707

    0 // return an integer exit code
