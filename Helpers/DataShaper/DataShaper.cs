using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WorldEvents.API.Helpers.DataShaper
{
    public class DataShaper<T> : IDataShaper<T>
    {
        public PropertyInfo[] Proprties { get; set; }
        public DataShaper()
        {
            Proprties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }


        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredProperties);
        }

        public ExpandoObject ShapeData(T entity, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entity, requiredProperties);
        }
        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            var requiredProperties = new List<PropertyInfo>();

            if (!string.IsNullOrWhiteSpace(fieldsString))
            {
                var fields = fieldsString.Split(",",StringSplitOptions.RemoveEmptyEntries);

                foreach (var field in fields)
                {
                    var Property = Proprties.FirstOrDefault(pi => pi.Name.Equals(field.Trim(),StringComparison.InvariantCultureIgnoreCase));

                    if (Property == null)
                    {
                        continue;
                    }
                    requiredProperties.Add(Property);
                }
            }
            else
            {
                requiredProperties = Proprties.ToList();
            }
            return requiredProperties;
        }
        private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ExpandoObject();
            foreach (var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                shapedObject.TryAdd(property.Name, objectPropertyValue);
            }
            return shapedObject;
        }
        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ExpandoObject>();
            foreach (var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }
    }
}
