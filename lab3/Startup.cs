using Microsoft.EntityFrameworkCore;
using lab3.Models;
using lab3.CashedModels;

namespace lab3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BarbershopContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped<CashedClient>();
            services.AddScoped<CashedServiceKind>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseSession();
            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string strResponse =
                    "<html><head><title>Info</title>" +
                    "<meta charset='utf-8'></head>" +
                    "<body>" +
                        "<p><a href = '/'>На главную</a></p>" +
                        "<p>Информация:</p>" +
                        "Сервер: " + context.Request.Host +
                        "<BR> Путь: " + context.Request.PathBase +
                        "<BR> Протокол: " + context.Request.Protocol +
                    "</body></html>";
                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.Map("/searchInfo1", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string id = string.Empty;
                    if (context.Request.Cookies.ContainsKey("id"))
                    {
                        id = context.Request.Cookies["id"] ?? string.Empty;
                    }

                    string strResponse = "<html><head><title>SearchForm1</title>" +
                                        "<meta charset = 'utf-8'></head><body>" +
                                        "<p><a href = '/'>На главную</a></p>" +
                                        "<form action = / >" +
                                            "Поиск клиента по Id:<br><input type = 'text', name = 'id', value = " + id + ">" +
                                            "<br><br><input type = 'submit' value = 'Submit' >" +
                                        "</form>" +
                                        "</body></html>";

                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.MapWhen(context =>
            {
                return context.Request.Query.ContainsKey("id");
            }, HandleSearch1);
            app.Map("/searchInfo2", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string name = string.Empty;
                    if (context.Request.Cookies.ContainsKey("name"))
                    {
                        name = context.Request.Cookies["name"] ?? string.Empty;
                    }

                    string strResponse = "<html><head><title>SearchForm2</title>" +
                                        "<meta charset = 'utf-8'></head><body>" +
                                        "<p><a href = '/'>На главную</a></p>" +
                                        "<form action = / >" +
                                            "Поиск сервиса по Id:<br><input type = 'text', name = 'name', value = " + name + ">" +
                                            "<br><br><input type = 'submit' value = 'Submit' >" +
                                        "</form>" +
                                        "</body></html>";

                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.MapWhen(context =>
            {
                return context.Request.Query.ContainsKey("name");
            }, HandleSearch2);
            app.Map("/clients", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    CashedClient cashedClientService = context.RequestServices.GetService<CashedClient>();
                    IEnumerable<Client> clients = cashedClientService.GetClients("Clients20", 20);
                    string HtmlString = "<html><head><title>Клиенты</title>" +
                    "<meta charset='utf-8'></head>" +
                    "<body>" +
                        "<p><a href = '/'>На главную</a></p><br>" +
                        "<p>Список клиентов:</p>" +
                        "<table border=1>" +
                        "<tr>" +
                        "<th>Id</th>" +
                        "<th>Имя</th>" +
                        "<th>фамилия</th>" +
                        "<th>Адрес</th>" +
                        "<th>Телефон</th>" +
                        "</tr>";
                    foreach (var client in clients)
                    {
                        HtmlString += "<tr>" +
                        $"<td>{client.Id}</td>" +
                        $"<td>{client.Name}</td>" +
                        $"<td>{client.Surname}</td>" +
                        $"<td>{client.Address}</td>" +
                        $"<td>{client.Telephone}</td>" +
                        "</tr>";
                    }
                    HtmlString += "</table></body></html>";
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/serviceKind", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    CashedServiceKind cashedContractService = context.RequestServices.GetService<CashedServiceKind>();
                    IEnumerable<ServiceKind> kinds = cashedContractService.GetServiceKind(20);
                    string HtmlString = "<html><head><title>Main</title>" +
                    "<meta charset='utf-8'></head>" +
                    "<body>" +
                        "<p><a href = '/'>На главную</a></p><br>" +
                        "<p>Список типов сервиса:</p>" +
                        "<table border=1>" +
                        "<tr>" +
                        "<th>Id</th>" +
                        "<th>Название</th>" +
                        "<th>Описание</th>" +
                        "</tr>";
                    foreach (var kind in kinds)
                    {
                        HtmlString += "<tr>" +
                        $"<td>{kind.Id}</td>" +
                        $"<td>{kind.Name}</td>" +
                        $"<td>{kind.Description}</td>" +
                        "</tr>";
                    }
                    HtmlString += "</table></body></html>";
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Run((context) =>
            {
                CashedClient cashedClient = context.RequestServices.GetService<CashedClient>();
                CashedServiceKind cashedServiceKind = context.RequestServices.GetService<CashedServiceKind>();
                cashedClient.AddClients("Clients20", 20);
                string HtmlString = "<HTML><HEAD><TITLE>Типы сервиса</TITLE></head>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY>";
                HtmlString += "<BR><A href='/info'>Информация</A></BR>";
                HtmlString += "<BR><A href='/clients'>Клиенты</A></BR>";
                HtmlString += "<BR><A href='/serviceKind'>Типы сервиса</A></BR>";
                HtmlString += "<BR><A href='/searchInfo1'>Найти клиента</A></BR>";
                HtmlString += "<BR><A href='/searchInfo2'>Найти тип сервиса</A></BR>";
                HtmlString += "</BODY></HTML>";
                return context.Response.WriteAsync(HtmlString);
            });
        }
        public static void HandleSearch1(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                CashedClient cached = context.RequestServices.GetService<CashedClient>();
                if (context.Request.Cookies.ContainsKey("id"))
                {
                    context.Response.Cookies.Delete("id");
                }
                context.Response.Cookies.Append("id", context.Request.Query["id"]);
                await context.Response.WriteAsync(cached.GetTable(context.Request.Query["id"]));
            });
        }
        public static void HandleSearch2(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                CashedServiceKind cached =
                    context.RequestServices.GetService<CashedServiceKind>();
                if (context.Request.Cookies.ContainsKey("name"))
                {
                    context.Response.Cookies.Delete("name");
                }
                context.Response.Cookies.Append("name", context.Request.Query["name"]);
                await context.Response.WriteAsync(cached.GetTable(context.Request.Query["name"]));
            });
        }
    }
}