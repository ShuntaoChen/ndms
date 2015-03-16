using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JiYun.Web.Services;
using Web.Areas.Mes.Models;
using Web.Areas.Mes.Data;

namespace Web.Areas.Mes.Services
{
    public interface IUserMesCountService : IDependency
    {
        /// <summary>
        /// 检查当前用户是否有最新消息，检查后并把信息条数更新
        /// </summary>
        /// <returns></returns>
        X_UserMesCounts CheckNewMessageCount(int uid);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        void Update(X_UserMesCounts model);
    }
    public class UserMesCountService : IUserMesCountService
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public X_UserMesCounts CheckNewMessageCount(int uid)
        {
            return unitOfWork.X_UserMesCountsRepository().GetAll().Where(p => p.UserID == uid).FirstOrDefault();
        }

        public void Update(X_UserMesCounts model)
        {
            unitOfWork.X_UserMesCountsRepository().Update(model);
            unitOfWork.Save();
            
        }
    }
}