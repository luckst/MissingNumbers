namespace PuntosColombia.MissingNumbers.Infrastructure.Data.EntityFramework
{
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;

    public static class StoredProcedureExtension
    {
        public static int ExecuteSqlCommandSmart(this DatabaseFacade self, string storedProcedure, object parameters = null)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (string.IsNullOrEmpty(storedProcedure))
                throw new ArgumentException("storedProcedure");

            var arguments = PrepareArguments(storedProcedure, parameters);
            return self.ExecuteSqlCommand(arguments.Item1, arguments.Item2);
        }

        private static Tuple<string, object[]> PrepareArguments(string storedProcedure, object parameters)
        {
            var parameterNames = new List<string>();
            var parameterParameters = new List<object>();

            if (parameters != null)
            {
                foreach (PropertyInfo propertyInfo in parameters.GetType().GetProperties())
                {
                    string name = "@" + propertyInfo.Name;
                    object value = propertyInfo.GetValue(parameters, null);

                    parameterNames.Add(name);
                    parameterParameters.Add(new SqlParameter(name, value ?? DBNull.Value));
                }
            }

            if (parameterNames.Count > 0)
                storedProcedure += " " + string.Join(", ", parameterNames);

            return new Tuple<string, object[]>(storedProcedure, parameterParameters.ToArray());
        }
    }
}
