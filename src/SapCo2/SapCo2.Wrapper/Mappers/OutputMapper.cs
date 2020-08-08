using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Fields;
using SapCo2.Wrapper.Fields.Abstract;

namespace SapCo2.Wrapper.Mappers
{
    public static class OutputMapper
    {
        private static readonly ConcurrentDictionary<Type, Func<IRfcInterop, IntPtr, object>> ExtractFuncsCache =
            new ConcurrentDictionary<Type, Func<IRfcInterop, IntPtr, object>>();

        public static TOutput Extract<TOutput>(IRfcInterop interop, IntPtr dataHandle)
        {
            Type outputType = typeof(TOutput);
            Func<IRfcInterop, IntPtr, object> extractFunc = ExtractFuncsCache.GetOrAdd(outputType, BuildExtractFunc);
            return (TOutput)extractFunc(interop, dataHandle);
        }

        private static Func<IRfcInterop, IntPtr, object> BuildExtractFunc(Type type)
        {
            ParameterExpression interop = Expression.Parameter(typeof(IRfcInterop));
            ParameterExpression dataHandle = Expression.Parameter(typeof(IntPtr));
            ParameterExpression result = Expression.Variable(type);

            IEnumerable<Expression> extractExpressionsForProperties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(propertyInfo => BuildExtractExpressionForProperty(
                    propertyInfo: propertyInfo,
                    interop: interop,
                    dataHandle: dataHandle,
                    result: result))
                .Where(x => x != null);

            Expression[] body = Array.Empty<Expression>()
                .Concat(new[] {Expression.Assign(result, Expression.New(type))})
                .Concat(extractExpressionsForProperties)
                .Concat(new[] {result})
                .ToArray();

            var expression = Expression.Lambda<Func<IRfcInterop, IntPtr, object>>(
                body: Expression.Block(
                    variables: new[] {result},
                    expressions: body),
                parameters: new[] {interop, dataHandle});

            return expression.Compile();
        }

        private static Expression BuildExtractExpressionForProperty(PropertyInfo propertyInfo, Expression interop,
            Expression dataHandle, Expression result)
        {
            if (Attribute.IsDefined(propertyInfo, typeof(RfcEntityIgnorePropertyAttribute)))
                return null;
            
            RfcEntityPropertyAttribute nameAttribute = propertyInfo.GetCustomAttribute<RfcEntityPropertyAttribute>();
            ConstantExpression name = Expression.Constant(nameAttribute?.Name ?? propertyInfo.Name.ToUpper());

            Expression property = Expression.Property(result, propertyInfo);

            bool convertToNonNullable = false;
            MethodInfo extractMethod = null;
            if (propertyInfo.PropertyType == typeof(string))
            {
                extractMethod = GetMethodInfo(() => StringField.Extract(default, default, default));
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                extractMethod = GetMethodInfo(() => IntField.Extract(default, default, default));
            }
            else if (propertyInfo.PropertyType == typeof(long))
            {
                extractMethod = GetMethodInfo(() => LongField.Extract(default, default, default));
            }
            else if (propertyInfo.PropertyType == typeof(double))
            {
                extractMethod = GetMethodInfo(() => DoubleField.Extract(default, default, default));
            }
            else if (propertyInfo.PropertyType == typeof(decimal))
            {
                extractMethod = GetMethodInfo(() => DecimalField.Extract(default, default, default));
            }
            else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
            {
                convertToNonNullable = propertyInfo.PropertyType == typeof(DateTime);
                extractMethod = GetMethodInfo(() => DateField.Extract(default, default, default));
            }
            else if (propertyInfo.PropertyType == typeof(TimeSpan) || propertyInfo.PropertyType == typeof(TimeSpan?))
            {
                convertToNonNullable = propertyInfo.PropertyType == typeof(TimeSpan);
                extractMethod = GetMethodInfo(() => TimeField.Extract(default, default, default));
            }
            else if (propertyInfo.PropertyType.IsArray)
            {
                Type elementType = propertyInfo.PropertyType.GetElementType();
                extractMethod = GetMethodInfo(() => TableField<object>.Extract<object>(default, default, default))
                    .GetGenericMethodDefinition()
                    .MakeGenericMethod(elementType);
            }
            else if (!propertyInfo.PropertyType.IsPrimitive)
            {
                extractMethod = GetMethodInfo(() => StructureField<object>.Extract<object>(default, default, default))
                    .GetGenericMethodDefinition()
                    .MakeGenericMethod(propertyInfo.PropertyType);
            }

            if (extractMethod == null)
                throw new InvalidOperationException(
                    $"No matching extract method found for type {propertyInfo.PropertyType.Name}");

            PropertyInfo fieldValueProperty = extractMethod.ReturnType.GetProperty(nameof(Field<object>.Value));

            MemberExpression fieldValue = Expression.Property(
                Expression.Call(
                    method: extractMethod,
                    arguments: new[] {interop, dataHandle, name}),
                // ReSharper disable once AssignNullToNotNullAttribute
                fieldValueProperty);

            return convertToNonNullable
                ? Expression.Assign(property, Expression.Coalesce(
                    left: fieldValue,
                    right: Expression.Default(propertyInfo.PropertyType)))
                : Expression.Assign(property, fieldValue);
        }

        private static MethodInfo GetMethodInfo(Expression<Action> extractMethod)
            => ((MethodCallExpression)extractMethod.Body).Method;
    }
}
