using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Core.Invoices;
using Webminux.Optician.Core.SubCustomers;
using Webminux.Optician.ProductItem;


[Index(nameof(TenantId))]
[Index(nameof(InvoiceNo))]
[Index(nameof(CustomerId))]
[Index(nameof(SubCustomerId))]
[Index(nameof(SerialNumber))]
[Index(nameof(IsDraft))]
[Index(nameof(AdminId))]
[Index(nameof(PaidAmount))]
public class Invoice : FullAuditedEntity, IMustHaveTenant
{
    public virtual int TenantId { get; set; }

    [Required]
    [StringLength(OpticianConsts.MaxTitleLength)]
    public virtual string InvoiceNo { get; set; }
    public virtual DateTime InvoiceDate { get; set; }
    public virtual DateTime DueDate { get; set; }

    [StringLength(OpticianConsts.MaxTitleLength)]
    public virtual string Currency { get; set; }
    public virtual decimal Amount { get; set; }

    [StringLength(OpticianConsts.MaxDescriptionLength)]
    public virtual string Comment { get; set; }

    [ForeignKey(nameof(Customer))]
    public virtual int? CustomerId { get; set; }
    public virtual Customer Customer { get; set; }

    [ForeignKey(nameof(SubCustomer))]
    public virtual int? SubCustomerId { get; set; }
    public virtual SubCustomer SubCustomer { get; set; }
    public virtual string SerialNumber { get; set; }
    public virtual bool IsSync { get; set; }

    public virtual bool IsDraft { get; set; }
    public virtual long AdminId { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainAmount { get; set; }

    public virtual ICollection<InvoiceLine> InvoiceLines { get; set; }
    public virtual ICollection<ProductItem> ProductItems { get; set; }
    protected Invoice()
    {
        InvoiceLines = new HashSet<InvoiceLine>();
        ProductItems = new HashSet<ProductItem>();
    }
    public static Invoice Create(int tenantId, string invoiceNO, DateTime invoiceDate, DateTime dueDate, string currency, decimal amount, string comment, int customerId, bool isSync = false, string serialNumber = "", bool isDraft = false)
    {
        Random rd = new Random();
        int rand_num = rd.Next(100, 1000);
        return new Invoice
        {
            TenantId = tenantId,
            InvoiceNo = invoiceNO,
            InvoiceDate = invoiceDate,
            DueDate = dueDate,
            Currency = currency,
            Amount = amount,
            Comment = comment,
            CustomerId = customerId,
            SerialNumber = (serialNumber == "") ? customerId.ToString() + invoiceNO.ToString() : serialNumber,
            IsSync = isSync,
            IsDraft = isDraft
        };
    }

    public static Invoice CreateForAdmin(int tenantId, string invoiceNO, DateTime invoiceDate, DateTime dueDate, string currency, decimal amount, string comment, long adminId, bool isSync = false, string serialNumber = "", bool isDraft = false)
    {
        Random rd = new Random();
        int rand_num = rd.Next(100, 1000);
        return new Invoice
        {
            TenantId = tenantId,
            InvoiceNo = invoiceNO,
            InvoiceDate = invoiceDate,
            DueDate = dueDate,
            Currency = currency,
            Amount = amount,
            Comment = comment,
            AdminId = adminId,
            SerialNumber = (serialNumber == "") ? adminId.ToString() + invoiceNO.ToString() : serialNumber,
            IsSync = isSync,
            IsDraft = isDraft
        };
    }
}