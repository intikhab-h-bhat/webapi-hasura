using Hasura.GraphQl.Backend.Dtos;
using Hasura.GraphQl.Backend.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hasura.GraphQl.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;

        }



        [HttpGet]
        public async Task<ActionResult<List<Posts>>> GetAllPosts()
        {
            var allPosts = await _postService.GetAllPostsAsync();
            if (allPosts == null) return NotFound();

            return Ok(allPosts);
        }


        [HttpPost]
        public async Task<ActionResult<AddPost>> AddPost([FromBody] AddPost post)
        {

            var addPost = await _postService.AddPostAsync(post.Title, post.Content, post.User_Id);

            return addPost;

        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult<string>> DeletePost(int id)
        {
            var title = await _postService.DeletePostAsync(id);

            return title;
        }

    }
}
