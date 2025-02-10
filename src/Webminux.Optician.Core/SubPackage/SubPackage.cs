using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webminux.Optician.SubPackage
{
    public class SubPackage: FullAuditedEntity
    {

        [ForeignKey(nameof(Webminux.Optician.Package.Pacakge))]
        public int PackageId { get; set; }
        public Webminux.Optician.Package.Pacakge Package { get; set; }
        public string Contains { get; set; }
        public virtual string ImagePublicKey { get; set; }
        public virtual string ImageUrl { get; set; }
      

        public static SubPackage Create(int packageId,string contains, string imagePublicKey,string imageUrl)
        {
            var data = new SubPackage
            {
                PackageId = packageId,
                Contains = contains,
                ImagePublicKey = imagePublicKey,
                ImageUrl = imageUrl,
         
            };

            return data;
        }
    }
}
