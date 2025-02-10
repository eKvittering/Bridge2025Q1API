using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Webminux.Optician.Application.Customers.Dtos;

/// <summary>
/// This is a DTO class for Groups Listing.
/// </summary>
public class ProductItemDto : EntityDto, ICreationAudited
{
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public string Code { get; set; }
    public bool IsAvailable { get; set; } = false;

    public long RecieverEmployeeId { get; set; }
    public string ReceiverEmployeeName { get; set; }
    public DateTime ReceiverEmployeeDate { get; set; }

    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductNumber { get; set; }
    public long? CreatorUserId { get; set; }
    public DateTime CreationTime { get; set; }
    public bool IsMedicalDevice { get; set; }

    public int? InvoiceId { get; set; }
    public string InvoiceNo { get; set; }

    public int? InvoiceLineId { get; set; }
    public string InvoiceLineNo { get; set; }
    public int? CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public long? CustomerUserId { get; set; }
    public int? ActivityId { get; set; }
    public int? SupplierId { get; set; }
    public string SupplierName { get; set; }
    public string Description { get; set; }
}