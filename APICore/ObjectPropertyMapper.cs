// Created By abdulrahman elheyb
// 2021-11-28 9:34 PM

using System;
using System.Collections.Generic;

namespace APICore
{

    public enum TestTypes
    {
        One = 0,
        Two = 1,
        Three = 2
    }

    /// <summary>
    /// Represents 
    /// </summary>
    public static class ObjectPropertyMapper
    {
        /// <summary>
        /// This gets only object properties names when properties have values 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetObjectProperties(object obj)
        {
            var props = obj.GetType().GetProperties();
            var propsNames = new List<string>();
            foreach (var prop in props)
            {
                var propertyValue = prop.GetValue(obj);
                
                if(propertyValue is null)
                    continue;

                if (prop.PropertyType == typeof(string))
                    if (string.IsNullOrEmpty(propertyValue.ToString()))
                        continue; 
                
                if (prop.PropertyType == typeof(int))
                    if ((int)propertyValue < 1)
                        continue;
                
                if (prop.PropertyType == typeof(double))
                    if ((double)propertyValue is 0d)
                        continue;
                
                if (prop.PropertyType == typeof(float))
                    if ((float)propertyValue is 0f)
                        continue;
                
                if (prop.PropertyType.IsEnum)
                    if ((TestTypes)propertyValue == 0 )
                        continue;

                if (prop.PropertyType == typeof(DateTime))
                    if((DateTime)propertyValue == DateTime.MinValue)
                        continue;                
                
                propsNames.Add(prop.Name);
            }

            return propsNames;
        }
    }
}