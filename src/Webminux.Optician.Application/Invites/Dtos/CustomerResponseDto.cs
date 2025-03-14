using Abp.Application.Services.Dto;

///<summery>
/// Dto for Customer Response.
///</summery>
public class CustomerResponseDto
{
    ///<summery>
    /// Gets or sets the Customer Name
    ///</summery>
    public long Id { get; set; }
    public string Name { get; set; }
    
    ///<summery>
    /// Gets or sets the customer response id.
    ///</summery>
    public int Response { get; set; }
}