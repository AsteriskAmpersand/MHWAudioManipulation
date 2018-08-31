
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace Audio_Manip
{
    public static class HelperFunctions
    {
        public static G[] FunctionApply <T,G>(this T[] array, Func<T, G> function, bool inplace = false)
        {
            if (!typeof(T).IsAssignableFrom(typeof(G)))
            {
                throw new ArgumentException("Inplace requires compatible type in the return function");
            }
            G[] results = inplace?array.OfType<G>().ToArray():new G[array.Length];
            for (int i = 0; i < array.Length; i++){results[i] = function(array[i]);}
            return results;
        }

        private static bool IsTrueDirectory(this string s)
        {
            return !s.Contains('#');
        }

        public static string[] GetTrueDirectories(string root)
        {
            return Directory.GetDirectories("root").Where(s => s.IsTrueDirectory()).ToArray();
        }

        private static bool IsRelevantFile(this string s, Format formats)
        {
            return formats.Recognize(s);
        }
        
        public static string[] GetRelevantFiles(string root, Format formats)
        {
            return Directory.GetFiles(root,"*",SearchOption.TopDirectoryOnly).Where(s => s.IsRelevantFile(formats)).ToArray();
        }
        
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = 
                        Attribute.GetCustomAttribute(field, 
                            typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}