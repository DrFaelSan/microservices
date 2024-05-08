using DevShopping.IdentityServer.Configuration;
using DevShopping.IdentityServer.Extensions;
using DevShopping.IdentityServer.Initializer;
using DevShopping.IdentityServer.Model;
using DevShopping.IdentityServer.Model.Context;
using DevShopping.IdentityServer.Services;
using Duende.IdentityServer.Services;
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
builder.Services.AddScoped<IProfileService, ProfileService>();

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
