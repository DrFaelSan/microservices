// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using DevShooping.IdentityServer.Model;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevShooping.IdentityServer.MainModule.Account;

/// <summary>
/// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
/// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
/// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
/// </summary>
[SecurityHeaders]
[AllowAnonymous]
public class RegisterController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clientStore;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IIdentityProviderStore _identityProviderStore;
    private readonly IEventService _events;

    public RegisterController(
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        IEventService events,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _interaction = interaction;
        _clientStore = clientStore;
        _schemeProvider = schemeProvider;
        _identityProviderStore = identityProviderStore;
        _events = events;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }


    [HttpGet]
    public async Task<IActionResult> Register(string returnUrl)
        => View(await BuildRegisterViewModelAsync(returnUrl));

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = true,
                FirstName = model.FirstName ?? "",
                LastName = model.LastName ?? ""
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(model.RoleName).GetAwaiter().GetResult())
                {
                    var userRole = new IdentityRole
                    {
                        Name = model.RoleName,
                        NormalizedName = model.RoleName,

                    };
                    await _roleManager.CreateAsync(userRole);
                }

                await _userManager.AddToRoleAsync(user, model.RoleName);

                await _userManager.AddClaimsAsync(user, new Claim[]{
                new(JwtClaimTypes.Name, model.Username),
                new(JwtClaimTypes.Email, model.Email),
                new(JwtClaimTypes.FamilyName, model.FirstName ?? ""),
                new(JwtClaimTypes.GivenName, model.LastName ?? ""),
                new(JwtClaimTypes.WebSite, $"http://{model.Username}.com"),
                new(JwtClaimTypes.Role,"User") });

                var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
                var loginresult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: true);
                if (loginresult.Succeeded)
                {
                    var checkuser = await _userManager.FindByNameAsync(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(checkuser.UserName, checkuser.Id, checkuser.UserName, clientId: context?.Client.ClientId));

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl ?? "");

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl ?? "");
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl ?? ""))
                        return Redirect(model.ReturnUrl ?? "");
                    else if (string.IsNullOrEmpty(model.ReturnUrl ?? ""))
                        return Redirect("~/");
                    else
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");

                }

            }
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    private async Task<RegisterViewModel> BuildRegisterViewModelAsync(string returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        List<string> roles = new()
        {
            "Admin",
            "Client"
        };

        ViewBag.message = roles;
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var vm = new RegisterViewModel
            {
                EnableLocalLogin = local,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint ?? "",
            };

            if (!local)
                vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context?.IdP ?? "" } };

            return vm;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null)
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            }).ToList();

        var allowLocal = true;
        if (context?.Client.ClientId != null)
        {
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;

                if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
            }
        }

        return new RegisterViewModel
        {
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
            ReturnUrl = returnUrl,
            Username = context?.LoginHint ?? "",
            ExternalProviders = providers.ToArray()
        };
    }
}