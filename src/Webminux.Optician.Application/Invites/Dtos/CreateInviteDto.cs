
/// <summary>
/// Dto to Create Activity Invites
/// </summary>
public class CreateInviteDto
{
    /// <summary>
    /// Gets or sets the Group Id
    /// </summary>
    public virtual int GroupId { get; set; }

    /// <summary>
    /// Gets or sets the Activity Id
    /// </summary>
    public virtual int ActivityId { get; set; }

    /// <summary>
    /// Id of invited customer.
    /// </summary>
    public virtual int CustomerId { get; set; }
}