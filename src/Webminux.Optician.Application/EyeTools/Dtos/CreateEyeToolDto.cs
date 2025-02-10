using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.EyeTools.Dtos
{
    public class CreateEyeToolDto
    {
        /// <summary>
        /// Gets or sets ODRightSPH.
        /// </summary>
        public virtual string ODRightSPH { get; set; }
        
        /// <summary>
        /// Gets or sets ODRightCYL.
        /// </summary>
        public virtual string ODRightCYL { get; set; }
        
        /// <summary>
        /// Gets or sets ODRightAXIS.
        /// </summary>
        public virtual string ODRightAXIS { get; set; }
        
        /// <summary>
        /// Gets or sets ODRightVD.
        /// </summary>
        public virtual string ODRightVD { get; set; }
        
        /// <summary>
        /// Gets or sets ODRightNEARADD.
        /// </summary>
        public virtual string ODRightNEARADD { get; set; }
        
        /// <summary>
        /// Gets or sets ODRightVN.
        /// </summary>
        public virtual string ODRightVN { get; set; }
        
        /// <summary>
        /// Gets or sets OSLeftSPH.
        /// </summary>
        public virtual string OSLeftSPH { get; set; }
       
        /// <summary>
        /// Gets or sets OSLeftCYL.
        /// </summary>
        public virtual string OSLeftCYL { get; set; }
        
        /// <summary>
        /// Gets or sets OSLeftAXIS.
        /// </summary>
        public virtual string OSLeftAXIS { get; set; }
       
       /// <summary>
         /// Gets or sets OSLeftVD.
            /// </summary>
        public virtual string OSLeftVD { get; set; }
        
        /// <summary>
        /// Gets or sets OSLeftNEARADD.
        /// </summary>
        public virtual string OSLeftNEARADD { get; set; }
       
       /// <summary>
         /// Gets or sets OSLeftVN.
            /// </summary>
        public virtual string OSLeftVN { get; set; }
        
        /// <summary>
        /// Gets or sets LensType.
        /// </summary>
        public virtual string LensType { get; set; }
        
        ///<summary>
        /// Gets or sets LensFor.
        /// </summary>
        public virtual string LensFor { get; set; }
        
        /// <summary>
        /// Gets or sets LensSide.
        /// </summary>
        public virtual string LensSide { get; set; }
        
        /// <summary>
        /// Gets or sets Remark.
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// Gets or sets ActivityId.
        /// </summary>
        public virtual int ActivityId { get; set; }
        
    }
}
