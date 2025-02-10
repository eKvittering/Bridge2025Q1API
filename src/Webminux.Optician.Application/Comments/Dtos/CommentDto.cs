using Abp.Application.Services.Dto;

/// <summary>
/// A DTO class that can be used to transfer comment data.
/// </summary>
public class CommentDto : EntityDto
{
    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    public virtual string CommentText { get; set; }

    /// <summary>
    /// Gets or sets the User Id.
    /// </summary>
    public virtual long UserId { get; set; }

    /// <summary>
    /// Gets or sets the Name of User.
    /// </summary>
    public virtual string UserName { get; set; }

    /// <summary>
    /// Gets or sets the activity id.
    /// </summary>
    public virtual int ActivityId { get; set; }

}