using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using JiYun.Web.Services;
using NPOI.HSSF.Record;
using WebDBService.Data;
using WebDBService.Models;

namespace WebDBService.Services
{
    /// <summary>
    //维护知识库
    /// </summary>
    public interface IMaintenanceKnowledgeService : IDependency
    {
        void Create(MaintenanceKnowledges Model);
        void Update(MaintenanceKnowledges Model);
        void Delete(int id);
        MaintenanceKnowledges GetById(int id);
        IEnumerable<MaintenanceKnowledges> GetAll();
        PagedList.IPagedList<MaintenanceKnowledges> GetList(NameValueCollection form);
    }
    /// <summary>
    /// 
    /// </summary>
    public class MaintenanceKnowledgeService: IMaintenanceKnowledgeService
    {
        private UnitOfWork unitOfWork= new UnitOfWork();
        /// <summary>
        /// 添加 知识库
        /// </summary>
        /// <param name="Model"></param>
        public void Create(MaintenanceKnowledges Model)
        {
            unitOfWork.MaintenanceKnowledgesRepository().Insert(Model);
            unitOfWork.Save();
        }
        /// <summary>
        /// 修改知识库
        /// </summary>
        /// <param name="Model"></param>
        public void Update(MaintenanceKnowledges Model)
        {
            unitOfWork.MaintenanceKnowledgesRepository().Update(Model);
            unitOfWork.Save();
        }
        /// <summary>
        /// 删除知识库
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            unitOfWork.MaintenanceKnowledgesRepository().Delete(id);
            unitOfWork.Save();
        }
        /// <summary>
        /// 根据ID获取知识库实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaintenanceKnowledges GetById(int id)
        {
          return unitOfWork.MaintenanceKnowledgesRepository().GetById(id);
        }
        /// <summary>
        /// 获取知识库
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MaintenanceKnowledges> GetAll()
        {
            return unitOfWork.MaintenanceKnowledgesRepository().GetAll();
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public PagedList.IPagedList<MaintenanceKnowledges> GetList(NameValueCollection form)
        {
            Expression<Func<MaintenanceKnowledges, bool>> filter = t => true;
            string orderField = string.Empty;
            string orderDir = string.Empty;
            int pageSize = 20;
            int page = 1;
            foreach (string key in form)
            {
                string condition = form[key];
                if (string.IsNullOrEmpty(condition))
                {
                    continue;
                }
                switch (key)
                {

                    case "Keyword":
                        filter = filter.And(t => t.KnowledgeTitle.Contains(condition));
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
            Func<IQueryable<MaintenanceKnowledges>, IOrderedQueryable<MaintenanceKnowledges>> orderBy = i => i.OrderByDescending(item => item.ID);

            return unitOfWork.MaintenanceKnowledgesRepository().Get(page, filter, orderBy, pageSize); 
        }
    }
}
