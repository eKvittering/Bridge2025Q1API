using System.Collections.Generic;
using Abp.Application.Services.Dto;

/// <summary>
/// Dto to Update Site
/// </summary>
public class UpdateSiteDto : EntityDto
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
}