using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician;

/// <summary>
/// A service that can be used to manage comments.
/// </summary>
[AbpAuthorize]
public class CommentAppService : OpticianAppServiceBase, ICommentAppService
{
    private readonly ICommentManager _commentManager;

    /// <summary>
    /// Creates a new instance of <see cref="CommentAppService"/>.
    /// </summary>
    public CommentAppService(ICommentManager commentManager)
    {
        _commentManager = commentManager;
    }

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    public async Task CreateAsync(CreateCommentDto input)
    {
        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
        var userId = AbpSession.UserId.Value;
        var comment = Comment.Create(tenantId, userId, input.ActivityId, input.CommentText);
        await _commentManager.CreateAsync(comment);
    }

    /// <summary>
    /// Gets all comments against activity.
    /// </summary>
    public async Task<ListResultDto<CommentDto>> GetAllAsync(EntityDto input)
    {
        var comments = await _commentManager.GetAll()
        .Where(c => c.ActivityId == input.Id).OrderByDescending( c =>c.CreationTime)
        .Select(c => new CommentDto
        {
            Id = c.Id,
            CommentText = c.CommentText,
            UserId = c.UserId,
            UserName = c.User.FullName,
            ActivityId = c.ActivityId
        }).ToListAsync();

        return new ListResultDto<CommentDto>(comments);
    }
}
