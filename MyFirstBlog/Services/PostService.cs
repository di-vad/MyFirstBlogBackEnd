namespace MyFirstBlog.Services;

using MyFirstBlog.Helpers;
using MyFirstBlog.Entities;
using System.Text.RegularExpressions;
using MyFirstBlog.Dtos;

public interface IPostService
{
    IEnumerable<PostDto> GetPosts();
    PostDto GetPost(string slug);
    PostDto CreatePost(string title, string description);
}

public class PostService : IPostService
{
    private readonly DataContext _context;

    public PostService(DataContext context)
    {
        _context = context;
    }

    public IEnumerable<PostDto> GetPosts() =>
        _context.Posts.Select(post => post.AsDto());

    public PostDto GetPost(string slug)
    {
        return getPost(slug).AsDto();
    }

    public PostDto CreatePost(string title, string description)
    {
        var slug = GenerateSlug(title);
        var newPost = new Post
        {
            Id = Guid.NewGuid(),
            Title = title,
            Slug = slug,
            Body = description,
            CreatedDate = DateTime.UtcNow
        };

        _context.Posts.Add(newPost);
        _context.SaveChanges();

        return newPost.AsDto();
    }

    private string GenerateSlug(string title)
    {
        return Regex.Replace(title.ToLower(), @"\s+", "-");
    }

    private Post getPost(string slug)
    {
        return _context.Posts.SingleOrDefault(p => p.Slug == slug);
    }
}

