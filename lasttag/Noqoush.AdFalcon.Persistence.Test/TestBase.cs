using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.Cfg;
using MbUnit.Framework;
using NHibernate;
using Noqoush.Framework.Persistence;
using Noqoush.Framework.Persistence.NHibernate;

namespace Noqoush.AdFalcon.Persistence.Test
{
    public class TestBase
    {
        protected IUnitOfWorkFactory UoWFactory { get; set; }
        private ISessionFactory _sessionFactory;
        private ISession _session;
        public NHibernate.ISession CurrentSession
        {
            get
            {
                //var session =Noqoush.Framework.Persistence.UnitOfWork.Current.OrmSession<NHibernate.ISession>();
                //return session;
                return _session;
            }
        }
        public virtual void NHibernateConfigurationSetup()
        {
            Assembly[] assemblies = new Assembly[] { Assembly.Load("Noqoush.AdFalcon.Persistence"), Assembly.Load("Noqoush.Framework.DomainServices") };

            var cfg = new global::NHibernate.Cfg.Configuration();
            cfg.Configure(); // read config default style
            _sessionFactory = Fluently.Configure(cfg)
              .Mappings(m =>
              {
                  foreach (var assembly in assemblies)
                  {
                      m.HbmMappings.AddFromAssembly(assembly);
                      m.FluentMappings.AddFromAssembly(assembly);
                  }
              })
              .BuildSessionFactory();
            _session = _sessionFactory.OpenSession();
            _session.FlushMode = FlushMode.Auto;
        }
        public virtual void NHibernateTearDown()
        {
            _session.Close();
        }

        public virtual void UnitOFWorkSetup()
        {
            UoWFactory = new NHibernateUnitOfWorkFactory();
            Assembly[] assemblies = new Assembly[] { Assembly.Load("Noqoush.AdFalcon.Persistence"), Assembly.Load("Noqoush.Framework.DomainServices") };
            UoWFactory.Initialize(assemblies);

            UnitOfWork.SetUnitOfWorkFactory(UoWFactory);

            UnitOfWork.Create();
        }

        public virtual void UnitOFWorkTearDown()
        {
            UnitOfWork.DisposeCurrentUnitOfWork();
            UoWFactory.Dispose();
        }
    }
}
