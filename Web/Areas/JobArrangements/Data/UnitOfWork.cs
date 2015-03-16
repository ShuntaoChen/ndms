
 
using Web.Areas.JobArrangements.Models;
using JiYun.Web.Data;

namespace Web.Areas.JobArrangements.Data
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
					private GenericRepository<JobArrangement, Entities> jobarrangementRepository;
        public GenericRepository<JobArrangement, Entities> JobArrangementRepository()
        {
            return jobarrangementRepository ?? (jobarrangementRepository = new GenericRepository<JobArrangements, Entities>(Context()));
        }
					#endregion
    }
}








