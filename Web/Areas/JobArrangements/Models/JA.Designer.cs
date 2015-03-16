﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace Web.Areas.JobArrangements.Models
{
    #region 上下文
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    public partial class Entities : ObjectContext
    {
        #region 构造函数
    
        /// <summary>
        /// 请使用应用程序配置文件的“Entities”部分中的连接字符串初始化新 Entities 对象。
        /// </summary>
        public Entities() : base("name=Entities", "Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 初始化新的 Entities 对象。
        /// </summary>
        public Entities(string connectionString) : base(connectionString, "Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 初始化新的 Entities 对象。
        /// </summary>
        public Entities(EntityConnection connection) : base(connection, "Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region 分部方法
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet 属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<JobArrangement> JobArrangements
        {
            get
            {
                if ((_JobArrangements == null))
                {
                    _JobArrangements = base.CreateObjectSet<JobArrangement>("JobArrangements");
                }
                return _JobArrangements;
            }
        }
        private ObjectSet<JobArrangement> _JobArrangements;

        #endregion
        #region AddTo 方法
    
        /// <summary>
        /// 用于向 JobArrangements EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToJobArrangements(JobArrangement jobArrangement)
        {
            base.AddObject("JobArrangements", jobArrangement);
        }

        #endregion
    }
    

    #endregion
    
    #region 实体
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Models", Name="JobArrangement")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class JobArrangement : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 JobArrangement 对象。
        /// </summary>
        /// <param name="id">Id 属性的初始值。</param>
        public static JobArrangement CreateJobArrangement(global::System.Int32 id)
        {
            JobArrangement jobArrangement = new JobArrangement();
            jobArrangement.Id = id;
            return jobArrangement;
        }

        #endregion
        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Subject
        {
            get
            {
                return _Subject;
            }
            set
            {
                OnSubjectChanging(value);
                ReportPropertyChanging("Subject");
                _Subject = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Subject");
                OnSubjectChanged();
            }
        }
        private global::System.String _Subject;
        partial void OnSubjectChanging(global::System.String value);
        partial void OnSubjectChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Category
        {
            get
            {
                return _Category;
            }
            set
            {
                OnCategoryChanging(value);
                ReportPropertyChanging("Category");
                _Category = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Category");
                OnCategoryChanged();
            }
        }
        private global::System.String _Category;
        partial void OnCategoryChanging(global::System.String value);
        partial void OnCategoryChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Content
        {
            get
            {
                return _Content;
            }
            set
            {
                OnContentChanging(value);
                ReportPropertyChanging("Content");
                _Content = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Content");
                OnContentChanged();
            }
        }
        private global::System.String _Content;
        partial void OnContentChanging(global::System.String value);
        partial void OnContentChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> CreatorNo
        {
            get
            {
                return _CreatorNo;
            }
            set
            {
                OnCreatorNoChanging(value);
                ReportPropertyChanging("CreatorNo");
                _CreatorNo = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("CreatorNo");
                OnCreatorNoChanged();
            }
        }
        private Nullable<global::System.Int32> _CreatorNo;
        partial void OnCreatorNoChanging(Nullable<global::System.Int32> value);
        partial void OnCreatorNoChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String DoerNoList
        {
            get
            {
                return _DoerNoList;
            }
            set
            {
                OnDoerNoListChanging(value);
                ReportPropertyChanging("DoerNoList");
                _DoerNoList = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("DoerNoList");
                OnDoerNoListChanged();
            }
        }
        private global::System.String _DoerNoList;
        partial void OnDoerNoListChanging(global::System.String value);
        partial void OnDoerNoListChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> SupervisorNo
        {
            get
            {
                return _SupervisorNo;
            }
            set
            {
                OnSupervisorNoChanging(value);
                ReportPropertyChanging("SupervisorNo");
                _SupervisorNo = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("SupervisorNo");
                OnSupervisorNoChanged();
            }
        }
        private Nullable<global::System.Int32> _SupervisorNo;
        partial void OnSupervisorNoChanging(Nullable<global::System.Int32> value);
        partial void OnSupervisorNoChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> AppointStatus
        {
            get
            {
                return _AppointStatus;
            }
            set
            {
                OnAppointStatusChanging(value);
                ReportPropertyChanging("AppointStatus");
                _AppointStatus = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("AppointStatus");
                OnAppointStatusChanged();
            }
        }
        private Nullable<global::System.Int32> _AppointStatus;
        partial void OnAppointStatusChanging(Nullable<global::System.Int32> value);
        partial void OnAppointStatusChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                OnRemarkChanging(value);
                ReportPropertyChanging("Remark");
                _Remark = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Remark");
                OnRemarkChanged();
            }
        }
        private global::System.String _Remark;
        partial void OnRemarkChanging(global::System.String value);
        partial void OnRemarkChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> Starttime
        {
            get
            {
                return _Starttime;
            }
            set
            {
                OnStarttimeChanging(value);
                ReportPropertyChanging("Starttime");
                _Starttime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Starttime");
                OnStarttimeChanged();
            }
        }
        private Nullable<global::System.DateTime> _Starttime;
        partial void OnStarttimeChanging(Nullable<global::System.DateTime> value);
        partial void OnStarttimeChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> Endtime
        {
            get
            {
                return _Endtime;
            }
            set
            {
                OnEndtimeChanging(value);
                ReportPropertyChanging("Endtime");
                _Endtime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Endtime");
                OnEndtimeChanged();
            }
        }
        private Nullable<global::System.DateTime> _Endtime;
        partial void OnEndtimeChanging(Nullable<global::System.DateTime> value);
        partial void OnEndtimeChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String LoopCategory
        {
            get
            {
                return _LoopCategory;
            }
            set
            {
                OnLoopCategoryChanging(value);
                ReportPropertyChanging("LoopCategory");
                _LoopCategory = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("LoopCategory");
                OnLoopCategoryChanged();
            }
        }
        private global::System.String _LoopCategory;
        partial void OnLoopCategoryChanging(global::System.String value);
        partial void OnLoopCategoryChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> CreatorId
        {
            get
            {
                return _CreatorId;
            }
            set
            {
                OnCreatorIdChanging(value);
                ReportPropertyChanging("CreatorId");
                _CreatorId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("CreatorId");
                OnCreatorIdChanged();
            }
        }
        private Nullable<global::System.Int32> _CreatorId;
        partial void OnCreatorIdChanging(Nullable<global::System.Int32> value);
        partial void OnCreatorIdChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> CreateOn
        {
            get
            {
                return _CreateOn;
            }
            set
            {
                OnCreateOnChanging(value);
                ReportPropertyChanging("CreateOn");
                _CreateOn = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("CreateOn");
                OnCreateOnChanged();
            }
        }
        private Nullable<global::System.DateTime> _CreateOn;
        partial void OnCreateOnChanging(Nullable<global::System.DateTime> value);
        partial void OnCreateOnChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> UpdaterId
        {
            get
            {
                return _UpdaterId;
            }
            set
            {
                OnUpdaterIdChanging(value);
                ReportPropertyChanging("UpdaterId");
                _UpdaterId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("UpdaterId");
                OnUpdaterIdChanged();
            }
        }
        private Nullable<global::System.Int32> _UpdaterId;
        partial void OnUpdaterIdChanging(Nullable<global::System.Int32> value);
        partial void OnUpdaterIdChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> LastUpdateOn
        {
            get
            {
                return _LastUpdateOn;
            }
            set
            {
                OnLastUpdateOnChanging(value);
                ReportPropertyChanging("LastUpdateOn");
                _LastUpdateOn = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("LastUpdateOn");
                OnLastUpdateOnChanged();
            }
        }
        private Nullable<global::System.DateTime> _LastUpdateOn;
        partial void OnLastUpdateOnChanging(Nullable<global::System.DateTime> value);
        partial void OnLastUpdateOnChanged();

        #endregion
    
    }

    #endregion
    
}