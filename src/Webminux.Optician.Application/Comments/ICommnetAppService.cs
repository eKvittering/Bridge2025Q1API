using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

/// <summary>
/// A service interface that can be used to manage comments.
/// </summary>
public interface ICommentAppService : IApplicationService
{
    /// <summary>
    /// Creates a new comment.
    /// </summary>
    Task CreateAsync(CreateCommentDto input);

    /// <summary>
    /// Gets all comments against activity.
    /// </summary>
    Task<ListResultDto<CommentDto>> GetAllAsync(EntityDto input);
}