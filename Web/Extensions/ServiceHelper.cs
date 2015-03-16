using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public interface IServiceBase<T>
    {

    }
    public abstract class ServiceBase<T> : IServiceBase<T>
    {
        public event EventHandler BeginPrepareEvent;
        public abstract  void PrepareBasicConditions();
      
        public void GetAll()
        {
            PrepareBasicConditions();
            BeginPrepareEvent(this, new EventArgs());
        }
    }
    public class Service<T> : ServiceBase<T>
    {
        public override void PrepareBasicConditions()
        {
            throw new NotImplementedException();
        }
    }
    public abstract class ServiceFactory
    {
        //T CreateService<T>();
        IServiceBase<T> CreateService<T>() where T : class,IServiceBase<T>, new()
        {
            return new T();
        }

    }
}