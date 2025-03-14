﻿using System;
using System.Collections.Generic;

namespace Webminux.Optician.BackgroundJobs.Dto
{
    public class BillyContactDto
    {
        public string id { get; set; }
        public string type { get; set; }
        public string organizationId { get; set; }
        public DateTime createdTime { get; set; }
        public string name { get; set; }
        public object externalId { get; set; }
        public string countryId { get; set; }
        public string street { get; set; }
        public object cityId { get; set; }
        public string cityText { get; set; }
        public object stateId { get; set; }
        public string stateText { get; set; }
        public object zipcodeId { get; set; }
        public string zipcodeText { get; set; }
        public string contactNo { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public object currencyId { get; set; }
        public string registrationNo { get; set; }
        public string ean { get; set; }
        public object localeId { get; set; }
        public bool isCustomer { get; set; }
        public bool isSupplier { get; set; }
        public object paymentTermsMode { get; set; }
        public object paymentTermsDays { get; set; }
        public string accessCode { get; set; }
        public object emailAttachmentDeliveryMode { get; set; }
        public bool isArchived { get; set; }
        public bool isSalesTaxExempt { get; set; }
        public object defaultExpenseProductDescription { get; set; }
        public string defaultExpenseAccountId { get; set; }
        public string defaultTaxRateId { get; set; }
        public List<ContactPerson> contactPersons { get; set; }
    }
    public class ContactPerson
    {
        public string id { get; set; }
        public string contactId { get; set; }
        public bool isPrimary { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

}
