using FitnessTracker.Authentication;
using FitnessTracker.Challenges;
using FitnessTracker.GraphQLTypes;
using FitnessTracker.Users;
using FitnessTracker.Workouts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FitnessTracker
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS with named policy and middleware
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:19006")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                                  });
            });

            services.AddControllers();

            GraphTypeRegistrator.Register(services);

            services.AddHttpClient(AuthorizationConstants.GoogleAuthHttpClientName);

            // Workouts
            services.AddSingleton<WorkoutRepository>();
            services.AddSingleton<WorkoutQueryService>();
            services.AddSingleton<WorkoutCommandService>();

            // Users
            services.AddSingleton<UserRepository>();
            services.AddSingleton<UserQueryService>();
            services.AddSingleton<UserCommandService>();

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

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseMiddleware<GoogleJwtAuthenticationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
