using Microsoft.AspNetCore.Mvc;
using MyFirstBlog.Dtos;
using MyFirstBlog.Services;

[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public IEnumerable<PostDto> GetPosts() => _postService.GetPosts();

    [HttpGet("{slug}")]
    public ActionResult<PostDto> GetPost(string slug)
    {
        var post = _postService.GetPost(slug);
        if (post == null) return NotFound();
        return post;
    }

    [HttpPost]
    public ActionResult<object> CreatePost([FromBody] CreatePostRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { errors = new[] { "Title cannot be blank" } });
        }

        var createdPost = _postService.CreatePost(request.Title, request.Description);
        return Created(string.Empty, new { post = createdPost });
    }
}
