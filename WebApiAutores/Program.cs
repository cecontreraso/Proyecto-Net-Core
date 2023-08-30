using WebApiAutores;

var builder = WebApplication.CreateBuilder(args);


var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

var servicioLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));// tenemos el servicio logger

startup.Configure(app, app.Environment, servicioLogger);


// Configure the HTTP request pipeline.


app.Run();
