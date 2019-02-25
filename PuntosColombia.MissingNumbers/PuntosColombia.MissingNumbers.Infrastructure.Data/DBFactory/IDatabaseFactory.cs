namespace PuntosColombia.MissingNumbers.Infrastructure.Data.DBFactory
{
    using Microsoft.EntityFrameworkCore;

    public interface IDatabaseFactory
    {
        DbContext GetDatabase();
    }
}
