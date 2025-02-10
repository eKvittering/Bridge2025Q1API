using System.Collections.Generic;

/// <summary>
/// Dto to Create Group
/// </summary>
public class CreateGroupDto
{
    /// <summary>
    /// Group Name
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// Get or Sets UserIds 
    /// </summary>
    public virtual ICollection<int> UserIds { get; set; }
}