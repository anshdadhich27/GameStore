using GameStore.Models;
using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using GameStore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using WebMarkupMin.AspNetCore1;

namespace GameStore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            DbHelper.DbConnection = Configuration.GetConnectionString("DefaultConnection");
            DbHelper.EncryptionKey = Configuration.GetValue<string>("EncryptionKey");
            Constant.IgdbUserKey = Configuration.GetValue<string>("IgdbUserKey");
            Constant.TelrSecretKey = Configuration.GetValue<string>("TelrSecretKey");
            Constant.TelrAuthToken = Configuration.GetValue<string>("TelrAuthToken");
            Constant.TelrAPIKey = Configuration.GetValue<string>("TelrAPIKey");
            Constant.DomainNane = Configuration.GetValue<string>("DomainNane");
            Constant.DomainNaneTxt = Configuration.GetValue<string>("DomainNaneTxt");
            Constant.Phone_Number = Configuration.GetValue<string>("Phone_Number");
            Constant.Phone_NumberTxt = Configuration.GetValue<string>("Phone_NumberTxt");
            Constant.Currency = Configuration.GetValue<string>("Currency");
            Constant.TelrTestMode = Configuration.GetValue<int>("TelrTestMode");
            Constant.TelrMerchantID = Configuration.GetValue<int>("TelrMerchantID");
            Constant.StoreID = Configuration.GetValue<int>("StoreID");

            Constant.FetchrLocationId = Configuration.GetValue<string>("FetchrLocationId");
            Constant.FetchrTokenId = Configuration.GetValue<string>("FetchrTokenId");
            Constant.FetchrAuthToken = Configuration.GetValue<string>("FetchrAuthToken");
            Constant.FetchrUserName = Configuration.GetValue<string>("FetchrUserName");
            Constant.FetchrPassword = Configuration.GetValue<string>("FetchrPassword");


            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
            });
            
            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;


                // Cookie settings
                options.Cookies.ApplicationCookie.CookieName = "_GameStore";
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(2);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
                options.Cookies.ApplicationCookie.AutomaticChallenge = true;
                options.Cookies.ApplicationCookie.AuthenticationScheme = Constant.Auth_Scheme;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            
            services.AddSession();
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            }).AddMvcOptions(options => { options.Filters.Add(new MyCustomFilter()); });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddWebMarkupMin(options =>
            {
                options.AllowMinificationInDevelopmentEnvironment = true;
                options.AllowCompressionInDevelopmentEnvironment = true;
            }).AddHtmlMinification(options =>
            {
                options.MinificationSettings.RemoveRedundantAttributes = true;
                options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
            }).AddHttpCompression();

            services.AddMemoryCache();
            services.AddResponseCaching();
            

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAnyOrigin", builder => builder.AllowAnyOrigin());
            //});

            services.AddScoped<IUserClaimsPrincipalFactory<User>, AppClaimsPrincipalFactory>();

            services.AddTransient<IEmailSender, EmailHandler>();
            services.AddTransient<ISmsSender, SmsHandler>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IPlatformRepository, PlatformRepository>();
            services.AddTransient<ICollectionRepository, CollectionRepository>();
            services.AddTransient<IReviewRepository, ReviewRepository>();
            services.AddTransient<IGameEngineRepository, GameEngineRepository>();
            services.AddTransient<IGameModeRepository, GameModeRepository>();
            services.AddTransient<IKeywordRepository, KeywordRepository>();
            services.AddTransient<IGamePlatformRepository, GamePlatformRepository>();
            services.AddTransient<IGenereRepository, GenereRepository>();
            services.AddTransient<INewsLetterRepository, NewsLetterRepository>();
            services.AddTransient<IVideoRepository, VideoRepository>();
            services.AddTransient<IGamePlatformMapingRepository, GamePlatformMapingRepository>();
            services.AddTransient<ISearchRepository, SearchRepository>();
            services.AddTransient<IPageRepository, PageRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICommon_Name_UrlRepository, Common_Name_UrlRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IPaymentSettingRepository, PaymentSettingRepository>();
            services.AddTransient<IShopping_Cart_Repository, Shopping_Cart_Repository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IGuest_User_Repository, Guest_User_Repository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<ICareerRepository, CareerRepository>();
            services.AddTransient<IBannerRepository, BannerRepository>();
            services.AddTransient<IAdsRepository, AdsRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IWishListRepository, WishListRepository>();
            services.AddTransient<IFeatureLinkRepository, FeatureLinkRepository>();
            services.AddTransient<ICreditRepository, CreditRepository>();
            services.AddTransient<IVendorRepository, VendorRepository>();
            services.AddTransient<IDealRepository, DealRepository>();
            services.AddTransient<IOperatorRepository, OperatorRepository>();
            services.AddTransient<IHotDealsRepository, HotDealsRepository>();
            services.AddTransient<IGraphRepository, GraphRepository>();
            services.AddTransient<IOrder_Repository, Order_Repository>();

            services.Configure<AppSetings>(Configuration);
        }

        
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, ILoggerFactory loggerFactory, 
            IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            //app.UseCors("AllowAnyOrigin");
            
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(root),
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store"; // "public,max-age=31536000"; //
                    ctx.Context.Response.Headers[HeaderNames.Expires] = "-1"; // DateTime.UtcNow.AddYears(1).ToString("R"); //
                }
            });

            
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {                              
                AccessDeniedPath = new PathString("/account/login"),
                LoginPath = new PathString("/account/login"),
                AuthenticationScheme = Constant.Auth_Scheme,
                ExpireTimeSpan = TimeSpan.FromDays(2),
                ClaimsIssuer = Constant.DomainNane,
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieName = "_GameStore"
            });

            app.UseWhen(x => (x.Request.Path.Value.ToLower().StartsWith("/admin") ||
                            x.Request.Path.Value.ToLower().StartsWith("/master") ||
                            x.Request.Path.Value.ToLower().StartsWith("/vendor") ||
                            x.Request.Path.Value.ToLower().StartsWith("/deal") ||
                            x.Request.Path.Value.ToLower().StartsWith("/reports") ||
                            x.Request.Path.Value.ToLower().StartsWith("/operator") ||
                            x.Request.Path.Value.ToLower().StartsWith("/manage-products") ||
                            x.Request.Path.Value.ToLower().StartsWith("/manage-blogs") ||
                            x.Request.Path.Value.ToLower().StartsWith("/manage-review") ||                            
                            x.Request.Path.Value.ToLower().StartsWith("/dashboard")),
            builder =>
            {
                builder.UseCookieAuthentication(new CookieAuthenticationOptions()
                {
                    AuthenticationScheme = Constant.Admin_Auth_Scheme,                    
                    AccessDeniedPath = new PathString("/admin/login"),
                    LoginPath = new PathString("/admin/login"),
                    ExpireTimeSpan = TimeSpan.FromDays(5),
                    ClaimsIssuer = Constant.DomainNane,
                    CookieName = "_GameStore_Admin",
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true
                });
            });

            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["FacebookAppId"],
                AppSecret = Configuration["FacebookAppSecret"],
                SignInScheme = Constant.Auth_Scheme,
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = Configuration["GoogleClientId"],
                ClientSecret = Configuration["GoogleClientSecret"],
                SignInScheme = Constant.Auth_Scheme,
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            app.UseWebMarkupMin();
            app.UseResponseCaching();            
            app.UseSecurityMiddleware();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
