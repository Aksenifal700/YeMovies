using YeMovies.Application.Models;

namespace YeMovies.Application.Services;

public interface IMovieService
{
    Task<bool> CreateAsync(Movie movie, CancellationToken token = default);
    Task<Movie?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default);
    Task<Movie?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default, Guid? userid = default);
    Task<Movie?> UpdateAsync(Movie movie, Guid? userid = default, CancellationToken token = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
}