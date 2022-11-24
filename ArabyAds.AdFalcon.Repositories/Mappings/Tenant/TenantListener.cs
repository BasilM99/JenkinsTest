using ArabyAds.AdFalcon.Domain.Repositories.Tenant;
using ArabyAds.Framework;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Tenant
{
    public  class TenantListener : IPreInsertEventListener
    {
		public bool OnPreInsert(PreInsertEvent @event)
		{
			var tenant = @event.Entity as ITenant<int>;
			if (tenant == null)
				return false;


			
			if(ApplicationContext.Instance.Tenant!=null)
			Set(@event.Persister, @event.State, "Tenant", new ArabyAds.Framework.Tenant { ID = ApplicationContext.Instance.Tenant.ID});
		

			

			return false;
		}

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
		{
			var index = Array.IndexOf(persister.PropertyNames, propertyName);
			if (index == -1)
				return;
			state[index] = value;
		}
	}
}

