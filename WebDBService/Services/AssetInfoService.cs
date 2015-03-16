using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using JiYun.Web.Services;
using PagedList;
using WebDBService.Data;
using WebDBService.Models;

namespace WebDBService.Services
{
    /// <summary>
    /// 资产信息表
    /// </summary>
    public interface IAssetInfoService : IDependency
    {
        void Create(AssetInfos Model);
        void Update(AssetInfos Model);
        void Delte(int id);
        AssetInfos GetById(int id);
        IEnumerable<AssetInfos> GetAll();
        IPagedList<AssetInfos> GetList(NameValueCollection form);
    }

    public class AssetInfoService:IAssetInfoService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        /// <summary>
        /// 资产信息表 添加
        /// </summary>
        /// <param name="Model"></param>
        public void Create(AssetInfos Model)
        {
            unitOfWork.AssetInfosRepository().Insert(Model);
            unitOfWork.Save();
        }
        /// <summary>
        /// 修改资产信息表
        /// </summary>
        /// <param name="Model"></param>
        public void Update(AssetInfos Model)
        {
            unitOfWork.AssetInfosRepository().Update(Model);
            unitOfWork.Save();

        }
        /// <summary>
        /// 删除资产信息表
        /// </summary>
        /// <param name="id"></param>
        public void Delte(int id)
        {
            unitOfWork.AssetInfosRepository().Delete(id);
            unitOfWork.Save();
        }
        /// <summary>
        /// 获取资产信息表 实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AssetInfos GetById(int id)
        {
            return unitOfWork.AssetInfosRepository().GetById(id);
        }
        /// <summary>
        /// 资产信息表信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AssetInfos> GetAll()
        {
            return unitOfWork.AssetInfosRepository().GetAll();
        }
        /// <summary>
        /// 资产信息表分页
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public IPagedList<AssetInfos> GetList(NameValueCollection form)
        {
            Expression<Func<AssetInfos, bool>> filter = t => true;
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
                        filter = filter.And(t => t.AssetName.Contains(condition));
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
            Func<IQueryable<AssetInfos>, IOrderedQueryable<AssetInfos>> orderBy = i => i.OrderByDescending(item => item.ID);

            return unitOfWork.AssetInfosRepository().Get(page, filter, orderBy, pageSize); 
        }
    }
}
