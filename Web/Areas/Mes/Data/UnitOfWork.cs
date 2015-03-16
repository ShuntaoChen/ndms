
 
using Web.Areas.Mes.Models;
using JiYun.Web.Data;

namespace Web.Areas.Mes.Data
{
    public class UnitOfWork : Disposable, IUnitOfWork<Entities>
    {
        private Entities _dataContext;

        public Entities Context()
        {
            return _dataContext ?? (_dataContext = new Entities());
        }

        public void Save()
        {
            _dataContext.SaveChanges();
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }

        #region Repository
					private GenericRepository<X_Messages, Entities> x_messagesRepository;
        public GenericRepository<X_Messages, Entities> X_MessagesRepository()
        {
            return x_messagesRepository ?? (x_messagesRepository = new GenericRepository<X_Messages, Entities>(Context()));
        }
					private GenericRepository<X_MsgAtts, Entities> x_msgattsRepository;
        public GenericRepository<X_MsgAtts, Entities> X_MsgAttsRepository()
        {
            return x_msgattsRepository ?? (x_msgattsRepository = new GenericRepository<X_MsgAtts, Entities>(Context()));
        }
					private GenericRepository<X_Notice, Entities> x_noticeRepository;
        public GenericRepository<X_Notice, Entities> X_NoticeRepository()
        {
            return x_noticeRepository ?? (x_noticeRepository = new GenericRepository<X_Notice, Entities>(Context()));
        }
					private GenericRepository<X_NoticeDetail, Entities> x_noticedetailRepository;
        public GenericRepository<X_NoticeDetail, Entities> X_NoticeDetailRepository()
        {
            return x_noticedetailRepository ?? (x_noticedetailRepository = new GenericRepository<X_NoticeDetail, Entities>(Context()));
        }
					private GenericRepository<X_ScheduleDetails, Entities> x_scheduledetailsRepository;
        public GenericRepository<X_ScheduleDetails, Entities> X_ScheduleDetailsRepository()
        {
            return x_scheduledetailsRepository ?? (x_scheduledetailsRepository = new GenericRepository<X_ScheduleDetails, Entities>(Context()));
        }
					private GenericRepository<X_Schedules, Entities> x_schedulesRepository;
        public GenericRepository<X_Schedules, Entities> X_SchedulesRepository()
        {
            return x_schedulesRepository ?? (x_schedulesRepository = new GenericRepository<X_Schedules, Entities>(Context()));
        }
					private GenericRepository<X_SMS, Entities> x_smsRepository;
        public GenericRepository<X_SMS, Entities> X_SMSRepository()
        {
            return x_smsRepository ?? (x_smsRepository = new GenericRepository<X_SMS, Entities>(Context()));
        }
					private GenericRepository<X_UserMesCounts, Entities> x_usermescountsRepository;
        public GenericRepository<X_UserMesCounts, Entities> X_UserMesCountsRepository()
        {
            return x_usermescountsRepository ?? (x_usermescountsRepository = new GenericRepository<X_UserMesCounts, Entities>(Context()));
        }
					private GenericRepository<X_UserMessages, Entities> x_usermessagesRepository;
        public GenericRepository<X_UserMessages, Entities> X_UserMessagesRepository()
        {
            return x_usermessagesRepository ?? (x_usermessagesRepository = new GenericRepository<X_UserMessages, Entities>(Context()));
        }
					#endregion
    }
}








