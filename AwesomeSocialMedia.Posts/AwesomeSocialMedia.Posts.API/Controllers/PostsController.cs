using AwesomeSocialMedia.Posts.Application.InputModels;
using AwesomeSocialMedia.Posts.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeSocialMedia.Posts.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _service;

        public PostsController(IPostService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid userId)
        {
            var result = await _service.GetAll(userId);

            return Ok(result);
        }

        //64D396B2-6974-4089-B5BE-BD662D339225
        [HttpPost("{userId}")]
        public async Task<IActionResult> Post(Guid userId, [FromBody] CreatePostInputModel model)
        {
            model.UserId = userId;

            var result = await _service.Create(model);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid userId, Guid id)
        {
            var result = await _service.Delete(id);

            return Ok(result);
        }
    }
}
