namespace YeMovies.Contracts.Requests;

public class CreateMovieRequest
{
    public string Title { get; set; }
    public int YearOfRelease { get; set; }
    public IEnumerable<string> Genre { get; set; }
}