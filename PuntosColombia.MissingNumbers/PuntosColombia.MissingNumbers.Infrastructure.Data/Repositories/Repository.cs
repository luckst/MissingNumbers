namespace PuntosColombia.MissingNumbers.Infrastructure.Data.Repositories
{
    using PuntosColombia.MissingNumbers.Infrastructure.Data.EntityFramework;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.RepositoryPattern;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public DbContext context;
        public readonly DbSet<TEntity> dbSet;

        public Repository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entityList)
        {
            dbSet.AddRange(entityList);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(int id)
        {
            var entity = dbSet.Find(id);
            dbSet.Remove(entity);
        }

        public virtual void Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> objects = dbSet.Where<TEntity>(where).AsEnumerable();
            foreach (TEntity obj in objects)
                dbSet.Remove(obj);
        }

        public virtual TEntity GetById(params object[] id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<TEntity> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public int Commit()
        {
            /*var entities = from e in (new ChangeTracker(context).Entries())
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }*/

            return context.SaveChanges();
        }

        /*public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }*/

        public DataSet ExecuteStoreProcedure(string sqlQuery, List<DbParameter> parameters)
        {
            DataSet data = new DataSet();

            using (var oaCommand = context.Database.GetDbConnection().CreateCommand())
            {

                oaCommand.CommandTimeout = 600;

                oaCommand.CommandText = sqlQuery;
                oaCommand.CommandType = CommandType.StoredProcedure;

                foreach (var parameter in parameters)
                {
                    oaCommand.Parameters.Add(parameter);
                }

                try
                {
                    oaCommand.Connection.Open();

                    using (var oda = new SqlDataAdapter(oaCommand as SqlCommand))
                    {
                        oda.Fill(data);
                    }

                }
                catch (Exception)
                {
                    data = null;
                }
                finally
                {
                    oaCommand.Connection.Close();
                }
            }


            return data;
        }

        public void ExecuteSqlCommand(string procedureName, object parameteres)
        {
            context.Database.ExecuteSqlCommandSmart(procedureName, parameteres);

        }

        public IEnumerable<T> ExecuteStoreProcedure<T>(string procedureName, object parameters)
        {            
            var query = context.LoadStoredProc(procedureName);
            if (parameters != null)
            {
                foreach (PropertyInfo propertyInfo in parameters.GetType().GetProperties())
                {
                    query.WithSqlParam(propertyInfo.Name, propertyInfo.GetValue(parameters, null));
                }
            }

            var list = new List<T>();

            query.ExecuteStoredProc((handler) =>
            {
                list = handler.ReadToList<T>().ToList();
            });

            return list;
        }


    }
}
