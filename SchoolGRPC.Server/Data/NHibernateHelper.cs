// SchoolGRPC.Server/Data/NHibernateHelper.cs
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using SchoolGRPC.Shared.Mappings; 

namespace SchoolGRPC.Server.Data
{
    public class NHibernateHelper
    {
        private static ISessionFactory? _sessionFactory;
        private static readonly object LockObject = new object();

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    lock (LockObject)
                    {
                        _sessionFactory ??= InitializeSessionFactory();
                    }
                }
                return _sessionFactory!;
            }
        }

        private static ISessionFactory InitializeSessionFactory()
        {
            string connectionString = "Server=NAGAYAMA_PC;Database=SchoolDB;User ID=kivyrrah;Password=1234;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;";

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(connectionString)
                    .ShowSql()
                    .FormatSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TeacherMap>()) 
                .BuildSessionFactory();
        }

        public static NHibernate.ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}