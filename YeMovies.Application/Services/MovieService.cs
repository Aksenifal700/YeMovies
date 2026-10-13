using FluentValidation;
using YeMovies.Application.Models;
using YeMovies.Application.Repositories;

namespace YeMovies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movieValidator;
    private readonly IRatingRepository _ratingRepository;
    private readonly IValidator<GetAllMoviesOptions> _movieOptionsValidator;

    public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator, IRatingRepository ratingRepository, IValidator<GetAllMoviesOptions> movieOptionsValidator)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
        _ratingRepository = ratingRepository;
        _movieOptionsValidator = movieOptionsValidator;
    }
    
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
        return await _movieRepository.CreateAsync(movie, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default)
    {
        return _movieRepository.GetByIdAsync(id, token, userid);
    }

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default)
    {
        return _movieRepository.GetBySlugAsync(slug, userid, token);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options,CancellationToken token = default, Guid? userid = default)
    {
        await _movieOptionsValidator.ValidateAndThrowAsync(options, token);
        
        return await _movieRepository.GetAllAsync(options, token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, Guid? userid = default, CancellationToken token = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
        var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id, token);
        if (!movieExists)
        {
            return null;
        }
        
        await _movieRepository.UpdateAsync(movie, token);

        if (!userid.HasValue)
        {
            var rating = await _ratingRepository.GetRatingAsync(movie.Id, token);
            movie.Rating = rating;
            return movie;
        }
        
        var ratings = await _ratingRepository.GetRatingAsync(movie.Id, userid.Value, token);
        movie.Rating = ratings.Rating;
        movie.UserRating = ratings.UserRating;
        return movie;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
    {
        return _movieRepository.DeleteAsync(id, token);
    }

    public Task<int> GetCountAsync(string? title, int? yearOfRelease, CancellationToken token = default)
    {
        return _movieRepository.GetCountAsync(title, yearOfRelease, token);
    }
}