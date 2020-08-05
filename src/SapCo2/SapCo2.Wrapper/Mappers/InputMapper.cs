using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Fields;
using SapCo2.Wrapper.Fields.Abstract;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Mappers
{
    public static class InputMapper
    {
        private static readonly Lazy<MethodInfo> FieldApplyMethod = new Lazy<MethodInfo>(GetFieldApplyMethod);

        private static readonly ConcurrentDictionary<Type, Action<IRfcInterop, IntPtr, object>> ApplyActionsCache =
            new ConcurrentDictionary<Type, Action<IRfcInterop, IntPtr, object>>();

        public static void Apply(IRfcInterop interop, IntPtr dataHandle, object input)
        {
            if (input == null)
                return;

            Type inputType = input.GetType();
            Action<IRfcInterop, IntPtr, object> applyAction = ApplyActionsCache.GetOrAdd(inputType, BuildApplyAction);
            applyAction(interop, dataHandle, input);
        }

        private static MethodInfo GetFieldApplyMethod()
        {
            Expression<Action<IField>> expression = field => field.Apply(default, default);
            return ((MethodCallExpression)expression.Body).Method;
        }

        private static Action<IRfcInterop, IntPtr, object> BuildApplyAction(Type type)
        {
            ParameterExpression interopParameter = Expression.Parameter(typeof(IRfcInterop));
            ParameterExpression dataHandleParameter = Expression.Parameter(typeof(IntPtr));
            ParameterExpression inputParameter = Expression.Parameter(typeof(object));
            UnaryExpression castedInputParameter = Expression.Convert(inputParameter, type);

            Expression[] applyExpressionsForProperties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(propertyInfo => BuildApplyExpressionForProperty(
                    propertyInfo: propertyInfo,
                    interopParameter: interopParameter,
                    dataHandleParameter: dataHandleParameter,
                    inputParameter: castedInputParameter))
                .Where(x => x != null)
                .ToArray();

            var expression = Expression.Lambda<Action<IRfcInterop, IntPtr, object>>(
                Expression.Block(applyExpressionsForProperties),
                interopParameter,
                dataHandleParameter,
                inputParameter);

            return expression.Compile();
        }

        private static Expression BuildApplyExpressionForProperty(
            PropertyInfo propertyInfo,
            Expression interopParameter,
            Expression dataHandleParameter,
            Expression inputParameter)
        {
            RfcPropertyAttribute nameAttribute = propertyInfo.GetCustomAttribute<RfcPropertyAttribute>();
            ConstantExpression name = Expression.Constant(nameAttribute?.Name ?? propertyInfo.Name.ToUpper());

            // var value = propertyInfo.GetValue(input);
            Expression property = Expression.Property(inputParameter, propertyInfo);

            ConstructorInfo fieldConstructor = null;
            if (propertyInfo.PropertyType == typeof(string))
            {
                // new RfcStringField(name, (string)value);
                fieldConstructor = GetFieldConstructor(() => new StringField(default, default));
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                // new RfcIntField(name, (int)value);
                fieldConstructor = GetFieldConstructor(() => new IntField(default, default));
            }
            else if (propertyInfo.PropertyType == typeof(long))
            {
                // new RfcLongField(name, (long)value);
                fieldConstructor = GetFieldConstructor(() => new LongField(default, default));
            }
            else if (propertyInfo.PropertyType == typeof(double))
            {
                // new RfcDoubleField(name, (double)value);
                fieldConstructor = GetFieldConstructor(() => new DoubleField(default, default));
            }
            else if (propertyInfo.PropertyType == typeof(decimal))
            {
                // new RfcDecimalField(name, (decimal)value);
                fieldConstructor = GetFieldConstructor(() => new DecimalField(default, default));
            }
            else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
            {
                // new RfcDateField(name, (DateTime?)value);
                fieldConstructor = GetFieldConstructor(() => new DateField(default, default));
                property = Expression.Convert(property, typeof(DateTime?));
            }
            else if (propertyInfo.PropertyType == typeof(TimeSpan) || propertyInfo.PropertyType == typeof(TimeSpan?))
            {
                // new RfcTimeField(name, (TimeSpan?)value);
                fieldConstructor = GetFieldConstructor(() => new TimeField(default, default));
                property = Expression.Convert(property, typeof(TimeSpan?));
            }
            else if (propertyInfo.PropertyType.IsArray)
            {
                // new RfcTableField<TElementType>(name, (TElementType[])value);
                Type tableFieldType = typeof(TableField<>).MakeGenericType(propertyInfo.PropertyType.GetElementType());
                fieldConstructor = tableFieldType.GetConstructor(new[] { typeof(string), propertyInfo.PropertyType });
            }
            else if (!propertyInfo.PropertyType.IsPrimitive)
            {
                // new RfcStructureField<T>(name, (T)value);
                Type structureFieldType = typeof(StructureField<>).MakeGenericType(propertyInfo.PropertyType);
                fieldConstructor = structureFieldType.GetConstructor(new[] { typeof(string), propertyInfo.PropertyType });
            }

            NewExpression fieldNewExpression = Expression.New(
                constructor: fieldConstructor ?? throw new InvalidOperationException("No matching field constructor found"),
                name,
                property);

            // instance.Apply(interopParameter, dataHandleParameter);
            return Expression.Call(
                instance: fieldNewExpression,
                method: FieldApplyMethod.Value,
                arguments: new[] { interopParameter, dataHandleParameter });
        }

        private static ConstructorInfo GetFieldConstructor(Expression<Func<IField>> constructor)
            => ((NewExpression)constructor.Body).Constructor;
    }
}
