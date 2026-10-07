using Microsoft.Extensions.DependencyInjection;
using YeMovies.Application.Repositories;

namespace YeMovies.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }
}