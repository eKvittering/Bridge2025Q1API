using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Webminux.Optician;

/// <summary>
/// Dto to Update Group
/// </summary>
public class UpdateProductItemDto : EntityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public bool IsSold { get; set; } = false;

    public int RecieverEmployee { get; set; }
    public int ReceiverEmployeeDate { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}