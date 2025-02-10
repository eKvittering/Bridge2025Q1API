using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webminux.Optician.Faults
{
    public class FaultFile : CreationAuditedEntity
    {
        [Required]
        [ForeignKey(nameof(Fault))]
        public int FaultId { get; set; }
        public virtual Fault Fault { get; set; }

        [Required]
        public virtual string ImagePublicKey { get; set; }

        [Required]
        public virtual string ImageUrl { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual int Size { get; set; }

        [Required]
        public virtual string Type { get; set; }

        protected FaultFile() { }

        public static FaultFile Create(int faultId, string imagePublicKey, string imageUrl, string name, int size, string type)
        {
            return new FaultFile
            {
                FaultId = faultId,
                ImagePublicKey = imagePublicKey,
                ImageUrl = imageUrl,
                Name = name,    
                Size = size,
                Type = type
            };
        }
    }
}
