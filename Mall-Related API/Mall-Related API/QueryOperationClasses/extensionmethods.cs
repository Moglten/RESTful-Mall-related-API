using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Mall_Related_API.Classes
{
    public static class Extensionmethods { 

        //Create Approprite exprission like (p=>p.propretyName = condition)
        //And call it by the purposed Funcution
        public static IQueryable<T> OrderByPropertyName<T>(this IQueryable<T> q, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var rs = Expression.Call(typeof(Queryable), method, types, q.Expression, Expression.Quote(exp));
            return q.Provider.CreateQuery<T>(rs);
        }


        //Create Approprite exprission like (p=>p.propretyName = condition)
        //And call it by the purposed Funcution
        public static IQueryable<T> FilterByPropertyName<T>(this IQueryable<T> q, string FilterField,string FilterAbout,string filterOpertator)
        {
            
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, FilterField);
            
            int value;
            Expression constant;
            if (int.TryParse(FilterAbout, out value))
            {
                constant = Expression.Constant(value);
            }
            else
            {
                constant = Expression.Constant(FilterAbout);
            }
            var operatorexp = GetOperatorExprision(prop, constant, filterOpertator);
            string method = "Where";
            var exp = Expression.Lambda(operatorexp, param);
            Type[] types = new Type[] { q.ElementType };
            var rs = Expression.Call(typeof(Queryable), method , types, q.Expression, Expression.Quote(exp));
            return q.Provider.CreateQuery<T>(rs);
        }


        public static bool Hasvalue(this string s)
        {
            if (s == null){return false;}
            return true;
      
        }


        public static Expression GetOperatorExprision(Expression prop, Expression constant, string filterOpertator)
        {
            Expression operatorexp = null;

            if (filterOpertator == "=")
            {
                operatorexp = Expression.Equal(prop, constant);
            }
            else if (filterOpertator == ">=")
            {
                operatorexp = Expression.GreaterThanOrEqual(prop, constant);
            }
            else if (filterOpertator == "<=")
            {
                operatorexp = Expression.LessThanOrEqual(prop, constant);
            }
            else if (filterOpertator == ">")
            {
                 operatorexp = Expression.GreaterThan(prop, constant);
            }
            else if (filterOpertator == "<")
            {
                operatorexp = Expression.LessThan(prop, constant);
            }

            return operatorexp;
        }

    }
}
