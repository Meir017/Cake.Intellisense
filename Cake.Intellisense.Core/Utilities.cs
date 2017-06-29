using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cake.Intellisense.Core
{
    public static class Utilities
    {
        public static bool IsCakeAliasMethod(MethodInfo method)
        {
            if (!method.IsStatic) return false;

            if (!method.IsDefined(typeof(ExtensionAttribute), false))
            {
                return false;
            }

            if (method.GetCustomAttribute<CakeMethodAliasAttribute>() == null && method.GetCustomAttribute<CakePropertyAliasAttribute>() == null)
            {
                return false;
            }

            var parameters = method.GetParameters();
            if (!parameters.Any()) return false;

            if (parameters[0].ParameterType != typeof(ICakeContext)) return false;

            return true;
        }

        public static IEnumerable<MethodInfo> GetCakeAliases(TypeInfo type)
        {
            return type.DeclaredMethods.Where(IsCakeAliasMethod);
        }

        public static string GetParameterRepresentation(ParameterInfo parameter)
        {
            string prefix = string.Empty;
            if (parameter.IsDefined(typeof(ParamArrayAttribute), false))
            {
                prefix = "params ";
            }
            else if (parameter.IsOut)
            {
                prefix = "out ";
            }
            else if (parameter.ParameterType.IsByRef)
            {
                prefix = "ref ";
            }

            return $"{prefix}{GetTypeRepresentation(parameter.ParameterType)}";
        }

        public static string GetTypeRepresentation(Type type)
        {
            string prefix = string.Empty;
            if (type.IsByRef)
            {
                type = type.GetElementType();
            }

            if (type.IsGenericType)
            {
                var genericArguments = string.Join(", ", type.GetGenericArguments().Select(GetTypeRepresentation));
                return $"{prefix}{type.Namespace}.{type.Name.Substring(0, type.Name.Length - 2)}<{genericArguments}>";
            }

            if(type.FullName == null)
            {
                return $"{prefix}{type.Name}";
            }

            return $"{prefix}{type.Namespace}.{type.Name}";
        }
    }
}
