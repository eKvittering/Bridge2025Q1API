using Abp.Application.Services.Dto;

/// <summary>
/// Input Dto to update invite response.
/// </summary>
public class UpdateInviteResponseInputDto : EntityDto
{
    /// <summary>
    /// Gets or sets the response id.
    /// </summary>
    public int Response { get; set; }
}