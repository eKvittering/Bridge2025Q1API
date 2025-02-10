using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.Authorization.Users;

namespace Webminux.Optician.Package
{
    public class Pacakge : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual DateTime Date { get; set; }
        public int EmployeeId { get; set; }

        public int PackageTypeId { get; set; }
        public virtual string ImagePublicKey { get; set; }
        public virtual string ImageUrl { get; set; }

        public virtual string OuterSenderFirstName { get; set; }
        public  virtual string OuterSenderLastName { get; set; }
        public  virtual string OuterSenderEmail { get; set; }
        public virtual string OuterSenderPhoneNumber { get; set; }


        [ForeignKey(nameof(User))]
        public long? SenderId { get; set; }
        public User User { get; set; }



        [ForeignKey(nameof(Activity))]
        public virtual int? ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public string? Description { get; set; }
        public string SenderType { get; set; }

        public static Pacakge Create(int activityId,int tenantId,long createdUserId, DateTime date ,int packageTypeId,string imagePublicKey,string imageUrl, long? senderId,string description = "", string senderFirstName = "", string SenderLastName = "", string senderEmail = "", string senderPhoneNumber = "") { 
        
            if(senderId == 0)
            {
                senderId = null;
            }
            var model = new Pacakge() { 
                CreationTime = DateTime.Now,
                CreatorUserId = createdUserId,
                TenantId = tenantId,
                EmployeeId = (int)createdUserId,
                Date = date,
                PackageTypeId = packageTypeId,
                ImagePublicKey = imagePublicKey,
                ImageUrl = imageUrl,
                SenderId = senderId,
                Description = description,
                OuterSenderFirstName = senderFirstName,
                OuterSenderLastName = SenderLastName,
                OuterSenderEmail = senderEmail,
                OuterSenderPhoneNumber = senderPhoneNumber,
                ActivityId = (activityId == 0 ? null : activityId)

            };
            return model;
        
        }

       

    
}
}