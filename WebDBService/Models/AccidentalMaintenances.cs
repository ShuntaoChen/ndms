//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebDBService.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;
	using JiYun.Web.Models;
	using System.ComponentModel.DataAnnotations;
    
    public partial class AccidentalMaintenances:BaseModel
    {
        public Nullable<System.DateTime> MaintainTime { get; set; }
        public string EventCategory { get; set; }
        public string EventProperty { get; set; }
        public string UserInvolvedList { get; set; }
        public string MaintenanceContentDescription { get; set; }
        public Nullable<int> IsConfirmed { get; set; }
    
    	[ForeignKey("AssetInfos_ID")]
        public virtual AssetInfos AssetInfos { get; set; }
    	[ForeignKey("MerchantInfos_ID")]
        public virtual MerchantInfos MerchantInfos { get; set; }
    }
    
}
