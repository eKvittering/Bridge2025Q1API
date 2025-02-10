using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.EyeTools
{
    public class EyeTool : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }


        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string ODRightSPH { get; set; }


        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string ODRightCYL { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string ODRightAXIS { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string ODRightVD { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string ODRightNEARADD { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string ODRightVN { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string OSLeftSPH { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string OSLeftCYL { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string OSLeftAXIS { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string OSLeftVD { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string OSLeftNEARADD { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string OSLeftVN { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string LensType { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string LensFor { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string LensSide { get; set; }

        [StringLength(OpticianConsts.MaxTitleLength)]
        public virtual string Remark { get; set; }


        [ForeignKey(nameof(Activity))]
        public virtual int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        protected EyeTool()
        {
        }

        public static EyeTool Create(int tenantId, string oDRightSPH, string oDRightCYL, string oDRightAXIS, string oDRightVD, string oDRightNEARADD, string oDRightVN, string oSLeftSPH, string oSLeftCYL, string oSLeftAXIS, string oSLeftVD, string oSLeftNEARADD, string oSLeftVN, string lensType, string lensFor, string lensSide, string remark,int activityId)
        {
            var eyeTool = new EyeTool
            {
                TenantId = tenantId,
                ODRightSPH = oDRightSPH,
                ODRightCYL = oDRightCYL,
                ODRightAXIS = oDRightAXIS,
                ODRightVD = oDRightVD,
                ODRightNEARADD = oDRightNEARADD,
                ODRightVN = oDRightVN,
                OSLeftSPH = oSLeftSPH,
                OSLeftCYL = oSLeftCYL,
                OSLeftAXIS = oSLeftAXIS,
                OSLeftVD = oSLeftVD,
                OSLeftNEARADD = oSLeftNEARADD,
                OSLeftVN = oSLeftVN,
                LensType = lensType,
                LensFor = lensFor,
                LensSide = lensSide,
                Remark = remark,
                ActivityId = activityId
            };
            return eyeTool;
        }
    }
}
