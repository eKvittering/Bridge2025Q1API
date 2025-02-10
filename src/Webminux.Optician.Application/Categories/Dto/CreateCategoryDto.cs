/// <summary>
/// Dto to Create Group
/// </summary>
public class CreateCategoryDto
{
    /// <summary>
    /// set category name
    /// </summary>
    public virtual string Name { get; set; }
 
    /// <summary>
    /// set parent id if child defaul  = 0
    /// </summary>
    public int ParentCategoryId { get; set; }

    /// <summary>
    /// default activi category 
    /// </summary>
    public bool IsDeactive { get; set; }


}