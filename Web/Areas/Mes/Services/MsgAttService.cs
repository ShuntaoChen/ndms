using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiYun.Web.Services;
using Web.Areas.Mes.Data;
using Web.Areas.Mes.Models;

namespace Web.Areas.Mes.Services
{
    public class MsgAttService : BaseService
    {
        public UnitOfWork unitOfWork = new UnitOfWork();

        public void Create(X_MsgAtts message)
        {
            unitOfWork.X_MsgAttsRepository().Insert(message);
            unitOfWork.Save();
        }
        public void Update(X_MsgAtts message)
        {
            unitOfWork.X_MsgAttsRepository().Update(message);
            unitOfWork.Save();
        }

        //照搬原方法的Repository,可能有问题
        public void Delete(int id)
        {
            unitOfWork.X_UserMessagesRepository().Delete(id);
            unitOfWork.Save();
        }
        public X_MsgAtts GetByID(int id)
        {
            return unitOfWork.X_MsgAttsRepository().GetById(id);
        }
    }
}