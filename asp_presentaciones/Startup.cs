using lib_presentaciones.Implementaciones;
using lib_presentaciones.Interfaces;
using Newtonsoft.Json;
namespace asp_presentacion
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration? Configuration { set; get; }

        public void ConfigureServices(WebApplicationBuilder builder, IServiceCollection services)
        {
            

            // Presentaciones
            services.AddScoped<IDiscosPresentacion, DiscosPresentacion>();
            services.AddScoped<IClientesPresentacion, ClientesPresentacion>();

            services.AddScoped<IArtistasPresentacion, ArtistasPresentacion>();
            services.AddScoped<IOrdenesPresentacion, OrdenesPresentacion>();
            services.AddScoped<IPagosPresentacion, PagosPresentacion>();
            services.AddScoped<IOrdenesDiscosPresentacion, OrdenesDiscosPresentacion>();
            services.AddScoped<IMarcasPresentacion, MarcasPresentacion>();
            services.AddScoped<IFormatosPresentacion, FormatosPresentacion>();
            services.AddScoped<IUsuariosPresentacion, UsuariosPresentacion>();
            services.AddScoped<IRolesPresentacion, RolesPresentacion>();
            services.AddScoped<IAuditoriasPresentacion, AuditoriasPresentacion>();
           
            services.AddScoped<IPermisosPresentacion, PermisosPresentacion>();
            services.AddScoped<IRoles_PermisosPresentacion, Roles_PermisosPresentacion>();


            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddRazorPages();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.UseSession();
            app.Run();

        }
    }
}