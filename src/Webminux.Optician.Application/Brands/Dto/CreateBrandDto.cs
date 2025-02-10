/// <summary>
/// Dto to Create Brand
/// </summary>
public class CreateBrandDto
{
    /// <summary>
    /// set Brand name
    /// </summary>
    public virtual string Name { get; set; }
 
    /// <summary>
    /// set parent id if child defaul  = 0
    /// </summary>
    public int ParentBrandId { get; set; }

    /// <summary>
    /// default activi Brand 
    /// </summary>
    public bool IsDeactive { get; set; }


}