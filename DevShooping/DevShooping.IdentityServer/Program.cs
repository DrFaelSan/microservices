using DevShooping.IdentityServer.Configuration;
using DevShooping.IdentityServer.Extensions;
using DevShooping.IdentityServer.Initializer;
using DevShooping.IdentityServer.Model;
using DevShooping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//EF Contexts
builder.Services.AddContexts(builder.Configuration);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SQLServerContext>()
    .AddDefaultTokenProviders();

var builderIdentity = builder.Services.AddIdentityServer(opts =>
                                        {
                                            opts.Events.RaiseErrorEvents = true;
                                            opts.Events.RaiseInformationEvents = true;
                                            opts.Events.RaiseFailureEvents = true;
                                            opts.Events.RaiseSuccessEvents = true;
                                            opts.EmitStaticAudienceClaim = true;
                                        })
                                        .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
                                        .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                                        .AddInMemoryClients(IdentityConfiguration.Clients)
                                        .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builderIdentity.AddDeveloperSigningCredential();

var app = builder.Build();

var dbInitializer = app.Services.CreateScope().ServiceProvider.GetService<IDbInitializer>();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

dbInitializer?.Initialize();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
