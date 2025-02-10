
using System.ComponentModel.DataAnnotations;
///<summary>
/// Dto to create comment.
///</summary>
public class CreateCommentDto
{
    ///<summary>
    /// Gets or sets the comment text.
    ///</summary>
    [Required]
    public virtual string CommentText { get; set; }

    ///<summary>
    /// Gets or sets the activity id.
    ///</summary>
    public virtual int ActivityId { get; set; }

}