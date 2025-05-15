using Dapper.Contrib.Extensions;
using System.Reflection;
using UserService.UserService.Core.Entities;

namespace UserService.UserService.Application.Helpers
{
    public static class TableName<T> where T : BaseEnt
    {
        public static string Get
        {
            get
            {
                if (SqlMapperExtensions.TableNameMapper != null)
                {
                    return SqlMapperExtensions.TableNameMapper(typeof(T));
                }

                string text = "GetTableName";
                MethodInfo? method = typeof(SqlMapperExtensions).GetMethod(text, BindingFlags.Static | BindingFlags.NonPublic);
                if (method == null)
                {
                    throw new ArgumentOutOfRangeException($"Method '{text}' is not found in '{"SqlMapperExtensions"}' class.");
                }

                return method.Invoke(null, new object[1] { typeof(T) }) as string;
            }
        }
    }
}
