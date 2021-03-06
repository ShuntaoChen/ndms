//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Areas.Mes.Models
{
    using System;
    using System.Collections.Generic;
	using JiYun.Web.Models;
	using System.ComponentModel.DataAnnotations;
    
    public partial class X_Messages:BaseModel
    {
        public X_Messages()
        {
            this.X_MsgAtts = new HashSet<X_MsgAtts>();
            this.X_UserMessages = new HashSet<X_UserMessages>();
        }
    
        public string MessageTitle { get; set; }
        public string MessageContent { get; set; }
        public string MessageKeyWord { get; set; }
        public string MessageSend { get; set; }
        public string MessageReceive { get; set; }
        public string AttachmentID { get; set; }
        public string Remark { get; set; }
        public Nullable<int> CreaterUserID { get; set; }
        public string CreateUserReal { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ModifyUserID { get; set; }
        public string ModifyUserReal { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<int> X_Schedules_ID { get; set; }
    
        public virtual ICollection<X_MsgAtts> X_MsgAtts { get; set; }
        public virtual ICollection<X_UserMessages> X_UserMessages { get; set; }
    	[ForeignKey("X_Schedules_ID")]
        public virtual X_Schedules X_Schedules { get; set; }
    }
    
}
