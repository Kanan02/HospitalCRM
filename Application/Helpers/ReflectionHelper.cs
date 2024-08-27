using System.Reflection;

namespace Application.Helpers
{
    public static class ReflectionHelper
    {
        public static void SetPropValue<TVal>(this object entity, TVal propVal, string propName) =>
            GetProp(entity, propName)
            .SetValue(entity, propVal, null);

        public static object GetPropValue(this object entity, string propName) => GetProp(entity, propName)
            .GetValue(entity, null);

        private static PropertyInfo GetProp(this object entity, string propName) => entity.GetType()
            .GetProperty(propName);

        public static bool IsEqualValue(this object obj1, object obj2, string propName)
        => obj1.GetPropValue(propName).Equals(obj2.GetPropValue(propName));

        public static void SetListPropValue<TList>(this List<TList> list, object value, string propname) => list.ForEach(l => l.SetPropValue(value, propname));

        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }

        public static List<TEnum> GetEnumValues<TEnum>() => Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

    }
}
