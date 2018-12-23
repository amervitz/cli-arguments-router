﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Clear
{
    public class HelpFormatter
    {
        public string GetCommands(string @namespace, IEnumerable<Type> types)
        {
            var typesWithoutNamespace = from t in types
                                        let ns = Regex.Replace(t.FullName, Regex.Escape(@namespace + "."), "", RegexOptions.IgnoreCase)
                                        select ns;

            var commands = from c in typesWithoutNamespace
                           let segments = c.Split('.')
                           select segments[0];

            var uniqueCommands = commands.Distinct();

            var output = new StringBuilder();

            foreach (var command in uniqueCommands)
            {
                output.AppendLine(command);
            }

            return output.ToString();
        }

        public string GetCommands(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            var output = new StringBuilder();

            foreach (var method in methods)
            {
                output.AppendLine(GetArguments(method));

                //var da = m.GetCustomAttribute<DisplayAttribute>();
                //if (da != null)
                //{
                //    Console.WriteLine($"\t\t{da.GetShortName()} - {da.GetName()} - {da.GetDescription()} - {da.GetPrompt()}");
                //}
            }

            return output.ToString();
        }

        public string GetArguments(MethodInfo method)
        {
            var output = new StringBuilder();
            output.Append(method.Name + " (");
            var ps = method.GetParameters();

            for (int i = 0; i < ps.Length; i++)
            {
                output.Append(ps[i].ParameterType.Name);
                output.Append(" ");
                output.Append(ps[i].Name);

                if (i < (ps.Length - 1))
                {
                    output.Append(", ");
                }
            }
            output.AppendLine(")");
            return output.ToString();
        }

        public string GetClasses(IEnumerable<Type> types)
        {
            var output = new StringBuilder();

            foreach (var type in types)
            {
                output.AppendLine(type.Name);
            }

            return output.ToString();
        }
    }
}
