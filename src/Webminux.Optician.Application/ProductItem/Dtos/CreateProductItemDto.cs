using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Webminux.Optician;
using Webminux.Optician.ProductItem.Dtos;

/// <summary>
/// Dto to Create Group
/// </summary>
public class CreateProductItemDto
{
    public string SerialNumber { get; set; }
    public int ProductId { get; set; }
    public int? ActivityId { get; set; }
    public int? InvoiceId { get; set; }
    public int? InvoiceLineId { get; set; }
    public List<SerialNumber> SerialNumbers { get; set; }
    public string Description { get; set; }

}