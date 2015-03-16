using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using JiYun.Web.Services;
using WebDBService.Data;
using WebDBService.Models;
using PagedList;

namespace WebDBService.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISharedResourceService : IDependency
    {
        void Create(SharedResources Model);
        void Update(SharedResources Model);
        void Delete(int id);
        IEnumerable<SharedResources> GetAll();
        SharedResources GetById(int id);
        PagedList.IPagedList<SharedResources> GetList(NameValueCollection form);
    }

    public class SharedResourceService :ISharedResourceService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public void Create(SharedResources Model)
        {
            unitOfWork.SharedResourcesRepository().Insert(Model);
            unitOfWork.Save();
        }

        public void Update(SharedResources Model)
        {
            unitOfWork.SharedResourcesRepository().Update(Model);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            unitOfWork.SharedResourcesRepository().Delete(id);
            unitOfWork.Save();
        }

        public IEnumerable<SharedResources> GetAll()
        {
            return unitOfWork.SharedResourcesRepository().GetAll();
        }

        public SharedResources GetById(int id)
        {
            return unitOfWork.SharedResourcesRepository().GetById(id);
        }

        public PagedList.IPagedList<SharedResources> GetList(NameValueCollection form)
        {
            Expression<Func<SharedResources, bool>> filter = t => true;
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
                        filter = filter.And(t => t.CategoryNo.ToString().Contains(condition));
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
            Func<IQueryable<SharedResources>, IOrderedQueryable<SharedResources>> orderBy = i => i.OrderByDescending(item => item.ID);

            return unitOfWork.SharedResourcesRepository().Get(page, filter, orderBy, pageSize); 
        }
    }
}
