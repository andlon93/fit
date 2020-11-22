using FitnessTracker.Users.GraphTypes;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.GraphQLTypes
{
    public static class GraphTypeRegistrator
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            // DataLoader
            services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
            services.AddSingleton<DataLoaderDocumentListener>();

            // Schema
            services.AddSingleton<FitnessTrackerSchema>();

            // GraphTypes
            services.AddSingleton<WorkoutQuery>();
            services.AddSingleton<WorkoutType>();
            services.AddSingleton<PagingType>();
            services.AddSingleton<UserType>();
        }
    }
}
