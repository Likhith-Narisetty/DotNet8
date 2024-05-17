//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adding  services to the container //

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    //options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDataProtection();

// Configuring Jwt Token //

//var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
//var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = jwtIssuer,
//         ValidAudience = jwtIssuer,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//     };
// });


var app = builder.Build();

// Configuring the HTTP Request Pipeline. //

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configuring IIS Url Rewrite //

using (StreamReader iisUrlRewriteStreamReader = File.OpenText("Configurations/IISUrlRewrite.xml"))
{
    var options = new RewriteOptions().AddIISUrlRewrite(iisUrlRewriteStreamReader);
    app.UseRewriter(options);
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseSession();

app.UseCookiePolicy();

app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("Content-Security-Policy", "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://www.paynimo.com https://www.googletagmanager.com https://www.google-analytics.com https://connect.facebook.net https://www.facebook.com/ https://fonts.googleapis.com https://googleads.g.doubleclick.net https://snap.licdn.com https://cdnt.netcoresmartech.com https://stats.g.doubleclick.net https://dsp.rtbdemand.com https://cdn.linkedin.oribi.io https://osjs.netcoresmartech.com https://dsp-media.eskimi.com https://cdndc.netcoresmartech.com https://twa.netcoresmartech.com; connect-src 'self' https://www.paynimo.com https://www.googletagmanager.com https://www.google-analytics.com https://connect.facebook.net https://www.facebook.com/ https://fonts.googleapis.com https://googleads.g.doubleclick.net https://snap.licdn.com https://cdnt.netcoresmartech.com https://stats.g.doubleclick.net https://dsp.rtbdemand.com https://cdn.linkedin.oribi.io https://osjs.netcoresmartech.com https://dsp-media.eskimi.com https://cdndc.netcoresmartech.com https://twa.netcoresmartech.com;");
//    await next();
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
