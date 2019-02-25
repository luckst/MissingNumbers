namespace PuntosColombia.MissingNumbers.Infrastructure.Data.DBFactory
{
    using PuntosColombia.MissingNumbers.Infrastructure.Data.EntityFramework;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseFactory : IDatabaseFactory
    {
        private DBContext dataContext;
        public DbContext GetDatabase()
        {
            return dataContext ?? (dataContext = new DBContext());
        }

        public void Dispose()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
