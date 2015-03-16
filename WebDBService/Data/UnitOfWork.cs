
 
using WebDBService.Models;
using JiYun.Web.Data;

namespace WebDBService.Data
{
    public class UnitOfWork : Disposable, IUnitOfWork<JY_NDMSEntities>
    {
        private JY_NDMSEntities _dataContext;

        public JY_NDMSEntities Context()
        {
            return _dataContext ?? (_dataContext = new JY_NDMSEntities());
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
					private GenericRepository<AccidentalMaintenances, JY_NDMSEntities> accidentalmaintenancesRepository;
        public GenericRepository<AccidentalMaintenances, JY_NDMSEntities> AccidentalMaintenancesRepository()
        {
            return accidentalmaintenancesRepository ?? (accidentalmaintenancesRepository = new GenericRepository<AccidentalMaintenances, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<AssetInfos, JY_NDMSEntities> assetinfosRepository;
        public GenericRepository<AssetInfos, JY_NDMSEntities> AssetInfosRepository()
        {
            return assetinfosRepository ?? (assetinfosRepository = new GenericRepository<AssetInfos, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<AssetOptionalProperties, JY_NDMSEntities> assetoptionalpropertiesRepository;
        public GenericRepository<AssetOptionalProperties, JY_NDMSEntities> AssetOptionalPropertiesRepository()
        {
            return assetoptionalpropertiesRepository ?? (assetoptionalpropertiesRepository = new GenericRepository<AssetOptionalProperties, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<CabinetInfos, JY_NDMSEntities> cabinetinfosRepository;
        public GenericRepository<CabinetInfos, JY_NDMSEntities> CabinetInfosRepository()
        {
            return cabinetinfosRepository ?? (cabinetinfosRepository = new GenericRepository<CabinetInfos, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<CategoriesAndStatus, JY_NDMSEntities> categoriesandstatusRepository;
        public GenericRepository<CategoriesAndStatus, JY_NDMSEntities> CategoriesAndStatusRepository()
        {
            return categoriesandstatusRepository ?? (categoriesandstatusRepository = new GenericRepository<CategoriesAndStatus, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<EventInfos, JY_NDMSEntities> eventinfosRepository;
        public GenericRepository<EventInfos, JY_NDMSEntities> EventInfosRepository()
        {
            return eventinfosRepository ?? (eventinfosRepository = new GenericRepository<EventInfos, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<LaborInfos, JY_NDMSEntities> laborinfosRepository;
        public GenericRepository<LaborInfos, JY_NDMSEntities> LaborInfosRepository()
        {
            return laborinfosRepository ?? (laborinfosRepository = new GenericRepository<LaborInfos, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<MaintainPlans, JY_NDMSEntities> maintainplansRepository;
        public GenericRepository<MaintainPlans, JY_NDMSEntities> MaintainPlansRepository()
        {
            return maintainplansRepository ?? (maintainplansRepository = new GenericRepository<MaintainPlans, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<MaintainRecords, JY_NDMSEntities> maintainrecordsRepository;
        public GenericRepository<MaintainRecords, JY_NDMSEntities> MaintainRecordsRepository()
        {
            return maintainrecordsRepository ?? (maintainrecordsRepository = new GenericRepository<MaintainRecords, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<MaintainRequests, JY_NDMSEntities> maintainrequestsRepository;
        public GenericRepository<MaintainRequests, JY_NDMSEntities> MaintainRequestsRepository()
        {
            return maintainrequestsRepository ?? (maintainrequestsRepository = new GenericRepository<MaintainRequests, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<MaintenanceKnowledges, JY_NDMSEntities> maintenanceknowledgesRepository;
        public GenericRepository<MaintenanceKnowledges, JY_NDMSEntities> MaintenanceKnowledgesRepository()
        {
            return maintenanceknowledgesRepository ?? (maintenanceknowledgesRepository = new GenericRepository<MaintenanceKnowledges, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<MerchantInfos, JY_NDMSEntities> merchantinfosRepository;
        public GenericRepository<MerchantInfos, JY_NDMSEntities> MerchantInfosRepository()
        {
            return merchantinfosRepository ?? (merchantinfosRepository = new GenericRepository<MerchantInfos, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<MotorRoomInfos, JY_NDMSEntities> motorroominfosRepository;
        public GenericRepository<MotorRoomInfos, JY_NDMSEntities> MotorRoomInfosRepository()
        {
            return motorroominfosRepository ?? (motorroominfosRepository = new GenericRepository<MotorRoomInfos, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<OnDuties, JY_NDMSEntities> ondutiesRepository;
        public GenericRepository<OnDuties, JY_NDMSEntities> OnDutiesRepository()
        {
            return ondutiesRepository ?? (ondutiesRepository = new GenericRepository<OnDuties, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<SharedResources, JY_NDMSEntities> sharedresourcesRepository;
        public GenericRepository<SharedResources, JY_NDMSEntities> SharedResourcesRepository()
        {
            return sharedresourcesRepository ?? (sharedresourcesRepository = new GenericRepository<SharedResources, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<TaskDesignateInfos, JY_NDMSEntities> taskdesignateinfosRepository;
        public GenericRepository<TaskDesignateInfos, JY_NDMSEntities> TaskDesignateInfosRepository()
        {
            return taskdesignateinfosRepository ?? (taskdesignateinfosRepository = new GenericRepository<TaskDesignateInfos, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<UserCompanyInfos, JY_NDMSEntities> usercompanyinfosRepository;
        public GenericRepository<UserCompanyInfos, JY_NDMSEntities> UserCompanyInfosRepository()
        {
            return usercompanyinfosRepository ?? (usercompanyinfosRepository = new GenericRepository<UserCompanyInfos, JY_NDMSEntities>(Context()));
        }
					private GenericRepository<UserScheduleInfos, JY_NDMSEntities> userscheduleinfosRepository;
        public GenericRepository<UserScheduleInfos, JY_NDMSEntities> UserScheduleInfosRepository()
        {
            return userscheduleinfosRepository ?? (userscheduleinfosRepository = new GenericRepository<UserScheduleInfos, JY_NDMSEntities>(Context()));
        }
					#endregion
    }
}








