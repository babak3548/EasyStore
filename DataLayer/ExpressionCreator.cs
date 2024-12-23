using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace DataLayer
{
    public  class ExpressionCreator<T>  where T:class
    {
        /// <summary>
        /// تولید عبارت شرطی بر اساس اند و شامل بودن برای استرینق
        /// </summary>
        /// <param name="FilterParam"></param>
        /// <returns></returns>
        public  Expression<Func<T, bool>> AndContains(Dictionary<string, object> FilterParam)
        {
            BinaryExpression binaryExpression = Expression.And(Expression.Constant(true), Expression.Constant(true));
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MethodInfo contains = typeof(string).GetMethod("Contains");
            //MethodCallExpression  methodCallExpression;
            
            foreach (var param in FilterParam.Where(p => p.Value != null || p.Value != ""))
            {
                if (param.Key.Contains("LessThan"))
                {
                    binaryExpression = Expression.And(binaryExpression, Expression.GreaterThan(Expression.Property(parameter, param.Key.Replace("LessThan", "")), Expression.Constant((DateTime)Convert.ToDateTime(param.Value))));
                }
                else
                {
                    switch (typeof(T).GetProperty(param.Key).PropertyType.Name)
                    {
                        //Expression where = Expression.GreaterThanOrEqual(Expression.Property(parameter, param.Key),Expression.Convert(Expression.Constant(param.Value), typeof(T).GetProperty(param.Key).PropertyType));
                        case "String": {
                           
                                binaryExpression = Expression.And(binaryExpression, Expression.NotEqual(Expression.Property(parameter, param.Key), Expression.Constant(null, typeof(string))));
                                binaryExpression = Expression.AndAlso(binaryExpression, Expression.Call(Expression.Property(parameter, param.Key), contains, Expression.Constant(param.Value, typeof(string))));
                                break; 
                        }
                        case "Int32":   { binaryExpression = Expression.And(binaryExpression, Expression.Equal(Expression.Property(parameter, param.Key), Expression.Constant(Convert.ToInt32(param.Value)))); break; }
                        case "Decimal": { binaryExpression = Expression.And(binaryExpression, Expression.Equal(Expression.Property(parameter, param.Key), Expression.Constant(Convert.ToDecimal(param.Value)))); break; }
                        case "Nullable`1": { binaryExpression = Expression.And(binaryExpression, Expression.Equal(Expression.Property(parameter, param.Key), Expression.Constant(Convert.ToInt32(param.Value) , typeof(int?)))); break; }
                                                             
                        case "Date": {
                       binaryExpression = Expression.And(binaryExpression, Expression.GreaterThanOrEqual(Expression.Property(parameter, param.Key), Expression.Constant(Convert.ToDateTime(param.Value)))); break; }
                        default:
                            break;
                        //  Expression.Lambda<Func<T, bool>>( Expression.Equal(Expression.Property(parameter,param.Key), Expression.Constant(Convert.ToInt32(param.Value) , typeof(int?))),   param)
                    }
                }

            }
            return Expression.Lambda<Func<T, bool>>(binaryExpression, parameter); ;
        }


        /// <summary>
        /// تولید عبارت شرطی بر اساس اور و مساوی بودن برای استرینق
        /// </summary>
        /// <param name="FilterParam"></param>
        /// <returns></returns>
        public  Expression<Func<T, bool>> OrEqual(Dictionary<string, object> FilterParam)
        {
            BinaryExpression binaryExpression = Expression.Or(Expression.Constant(true), Expression.Constant(true));
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
             //MethodCallExpression  methodCallExpression;

            foreach (var param in FilterParam.Where(p => p.Value != null || p.Value != ""))
            {
                switch (typeof(T).GetProperty(param.Key).PropertyType.Name)
                {
                    case "String": { binaryExpression = Expression.Or(binaryExpression, Expression.Equal(Expression.Property(parameter, param.Key), Expression.Constant(param.Value))); break; }
                    case "Int32": { binaryExpression =  Expression.Or(binaryExpression, Expression.Equal(Expression.Property(parameter, param.Key), Expression.Constant(Convert.ToInt32(param.Value)))); break; }
                    default:
                        break;
                }

            }
            return Expression.Lambda<Func<T, bool>>(binaryExpression, parameter); 
        }

        public Expression<Func<T, bool>> EqualInt32(string ParamName, int Value)
        {
            if (ParamName == null || ParamName == "")
            {
                BinaryExpression binaryExpression = Expression.Or(Expression.Constant(true), Expression.Constant(true));
                ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                return Expression.Lambda<Func<T, bool>>(binaryExpression, parameter);
            }
            else
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                var binaryExpression = Expression.Equal(Expression.Property(parameter, ParamName), Expression.Constant(Value));
                return Expression.Lambda<Func<T, bool>>(binaryExpression, parameter);
            }
        }
    }
}
