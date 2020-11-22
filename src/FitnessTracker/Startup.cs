using FitnessTracker.GraphQLTypes;
using FitnessTracker.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace FitnessTracker
{
    public class Startup
    {
        private const string path = "/app/Data/";
        private const string filename = "endomondo-2020-11-14.zip";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Unzip();
            services.AddControllers();

            GraphTypeRegistrator.Register(services);

            services.AddSingleton<UserRepository>();
            services.AddSingleton<UserService>();
        }

        private static void Unzip()
        {
            if (Directory.Exists(Path.Join(path, Path.GetFileNameWithoutExtension(filename))))
            {
                return;
            }

            System.IO.Compression.ZipFile.ExtractToDirectory(Path.Join(path, filename), path);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQLPlayground();

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
