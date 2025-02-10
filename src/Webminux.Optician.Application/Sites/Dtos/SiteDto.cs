using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

/// <summary>
/// This is a DTO class for Sites Listing.
/// </summary>
public class SiteDto : EntityDto, ICreationAudited
{

    public virtual int TenantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
    public bool IsMedicalType { get; set; }
    public string InvoiceCurrency { get; set; }
    public string Notes { get; set; }
    public long? CreatorUserId { get; set; }
    public DateTime CreationTime { get; set; }
}