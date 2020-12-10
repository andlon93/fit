using FitnessTracker.Challenges;
using FitnessTracker.GraphQLTypes;
using FitnessTracker.Users;
using FitnessTracker.Workouts;
using FitnessTracker.Workouts.GraphTypes;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            GraphTypeRegistrator.Register(services);

            // Workouts
            services.AddSingleton<WorkoutRepository>();
            services.AddSingleton<WorkoutQueryService>();
            services.AddSingleton<WorkoutCommandService>();

            // Users
            services.AddSingleton<UserRepository>();
            services.AddSingleton<UserService>();

            // Challenges
            services.AddSingleton<ChallengeService>();

            services.AddSingleton<EndomondeZipFileReader>();
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
