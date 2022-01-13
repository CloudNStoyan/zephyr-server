using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Npgsql;
using Zephyr;
using Zephyr.DAL;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(x => x.AddServerHeader = false);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.Register((ctx, p) => new NpgsqlConnection(builder.Configuration.GetValue<string>("DatabaseConnectionString")))
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<Database>().InstancePerLifetimeScope();

    var serviceTypes = Assembly.GetExecutingAssembly()
        .DefinedTypes.Where(x => x.IsClass && x.Name.EndsWith("Service")).ToList();

    foreach (var serviceType in serviceTypes)
    {
        containerBuilder.RegisterType(serviceType).InstancePerLifetimeScope();
    }
});

// Add services to the container.
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

builder.Services.AddHttpContextAccessor();

#if DEBUG
builder.Services.AddHostedService(sp => new NpmWatchHostedService(
    enabled: sp.GetRequiredService<IWebHostEnvironment>().IsDevelopment(),
    logger: sp.GetRequiredService<ILogger<NpmWatchHostedService>>()));
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseAuthorization();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "areas",
        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
