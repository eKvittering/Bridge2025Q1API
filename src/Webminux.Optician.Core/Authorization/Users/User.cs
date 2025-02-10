using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.Core;
using Webminux.Optician.Core.Customers;

namespace Webminux.Optician.Authorization.Users
{

    [Index(nameof(UserTypeId))]
    [Index(nameof(Name))]
    [Index(nameof(Surname))]
    [Index(nameof(EmailAddress))]
    [Index(nameof(UserName))]
    [Index(nameof(TenantId))]
    public class User : AbpUser<User>
    {
        [ForeignKey(nameof(UserType))]
        public virtual int UserTypeId { get; set; }
        public virtual UserType UserType { get; set; }

        public const string DefaultPassword = "Secure$888";

        [MaxLength(500)]
        public override string Name { get; set; }

        public override string Surname { get; set; }

        public virtual bool CanOpenCloseShop { get; set; }
        public virtual bool IsResponsibleForStocks { get; set; }
        public virtual bool IsReceptionAllowed { get; set; }
        public virtual bool CanAnswerPhoneCalls { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual string PictureUrl { get; set; }
        public virtual string PicturePublicId { get; set; }

        [InverseProperty("User")]
        public virtual Customer Customer { get; set; }
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress, int userType)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                UserTypeId = userType,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
