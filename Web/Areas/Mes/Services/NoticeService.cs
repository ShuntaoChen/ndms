using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JiYun.Web.Services;
using Web.Areas.Mes.Data;
using Web.Areas.Mes.Models;
using PagedList;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace Web.Areas.Mes.Services
{
    /// <summary>
    /// 公告管理
    /// </summary>
    public interface INoticeService : IDependency
    {
        #region 公告管理
        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="model"></param>
        void Create(X_Notice model);
        /// <summary>
        /// 修改一个实体
        /// </summary>
        /// <param name="model"></param>
        void Update(X_Notice model);
        /// <summary>
        /// 根据ID删除一个实体
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// 根据ID删除多个实体
        /// </summary>
        /// <param name="ids"></param>
        void DeleteList(List<int> ids);
        /// <summary>
        /// 根据ID获得一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        X_Notice GetById(int id);
        /// <summary>
        /// 获得全部数据列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<X_Notice> GetAllList();
        /// <summary>
        /// 分页数据列表
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        IPagedList<X_Notice> GetList(NameValueCollection forms, int? userid = null); 
        #endregion

        #region 公告管理详情
        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="model"></param>
        void Create(X_NoticeDetail model);
        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <param name="model"></param>
        void Create(List<X_NoticeDetail> list);
        /// <summary>
        /// 修改一个实体
        /// </summary>
        /// <param name="model"></param>
        void Update(X_NoticeDetail model);
        /// <summary>
        /// 根据ID删除多个实体
        /// </summary>
        /// <param name="ids"></param>
        void DeleteListDetail(List<int> ids);
        /// <summary>
        /// 根据公告管理ID获得详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<X_NoticeDetail> GetByIdNoticeDetail(int id);


        #endregion

    }
    public class NoticeService : INoticeService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        #region 公告管理
        public void Create(X_Notice model)
        {
            unitOfWork.X_NoticeRepository().Insert(model);
            unitOfWork.Save();
        }

        public void Update(X_Notice model)
        {
            unitOfWork.X_NoticeRepository().Update(model);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            unitOfWork.X_NoticeRepository().Delete(id);
            unitOfWork.Save();
        }

        public void DeleteList(List<int> ids)
        {
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    unitOfWork.X_NoticeRepository().Delete(id);
                }
            }
            unitOfWork.Save();
        }

        public X_Notice GetById(int id)
        {
            return unitOfWork.X_NoticeRepository().GetById(id);
        }

        public IEnumerable<X_Notice> GetAllList()
        {
            return unitOfWork.X_NoticeRepository().GetAll();
        }

        /// <summary>
        /// 分页数据列表
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        public IPagedList<X_Notice> GetList(NameValueCollection forms,int? userid=null)
        {
            Expression<Func<X_Notice, bool>> filter = t => true;

            string orderField = string.Empty;
            string orderDir = string.Empty;
            int pageSize = 0;
            int page = 1;
            if (userid != null)
            {
                filter = filter.And(u => u.X_NoticeDetail.Any(t => t.UserID == userid&&(t.IsDelete==null||t.IsDelete==false)));
            }
            foreach (string key in forms)
            {
                string condition = forms[key];
                if (string.IsNullOrEmpty(condition))
                {
                    continue;
                }
                switch (key)
                {
                    case "Keytext":
                        filter = filter.And(t => t.Title.Contains(condition) || t.Content.Contains(condition));
                        break;
                    case "startDate":
                        DateTime startDate;
                        DateTime.TryParse(condition, out startDate);
                        filter = filter.And(t => t.CreateOn >= startDate);
                        break;
                    case "endDate":
                        var endDate = DateTime.Parse((condition + " 23:59"));
                        filter = filter.And(t => t.CreateOn <= endDate);
                        break;
                    
                    case "numPerPage":
                        int.TryParse(condition, out pageSize);
                        break;
                    case "pageNum":
                        int.TryParse(condition, out page);
                        break;

                    case "orderField":
                        orderField = condition;
                        break;
                    case "orderDir":
                        orderDir = condition;
                        break;
                }
            }

            Func<IQueryable<X_Notice>, IOrderedQueryable<X_Notice>> order;
            switch (orderField)
            {
                default:
                    order = t => t.OrderByDescending(s => s.ID);
                    break;
            }

            return unitOfWork.X_NoticeRepository().Get(page, filter, order, pageSize);
        } 
        #endregion

        #region 公告管理详情
        public void Create(X_NoticeDetail model)
        {
            unitOfWork.X_NoticeDetailRepository().Insert(model);
            unitOfWork.Save();
        }
        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <param name="model"></param>
        public void Create(List<X_NoticeDetail> list)
        {
            if (list.Any())
            {
                foreach (var model in list)
                {
                    unitOfWork.X_NoticeDetailRepository().Insert(model);
                }
                unitOfWork.Save();
            }
        }
        public void Update(X_NoticeDetail model)
        {
            unitOfWork.X_NoticeDetailRepository().Update(model);
            unitOfWork.Save();
        }
        /// <summary>
        /// 根据ID删除多个实体
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteListDetail(List<int> ids)
        {
            if (ids != null && ids.Any())
            {
                foreach (var id in ids)
                {
                    try
                    {
                        unitOfWork.X_NoticeDetailRepository().Delete(id);
                    }
                    catch
                    {
                    }
                }
            }
            unitOfWork.Save();
        }
        /// <summary>
        /// 根据公告管理ID获得详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<X_NoticeDetail> GetByIdNoticeDetail(int id)
        {
            return unitOfWork.X_NoticeDetailRepository().GetAll(u => u.X_Notice_ID == id);
        }

        #endregion


    }
}