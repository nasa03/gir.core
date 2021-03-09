﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository;
using Repository.Model;

namespace Generator
{
    internal static class ArgumentsExtension
    {
        public static string WriteManaged(this IEnumerable<Argument> arguments, Namespace currentNamespace)
        {
            var args = arguments.Where(x => x.ClosureIndex == null); // Exclude "userData" parameters

            return string.Join(", ", args.Select(x => x.WriteManaged(currentNamespace)));
        }
        
        public static string WriteNative(this IEnumerable<Argument> arguments, Namespace currentNamespace)
        {
            var args = new List<string>();
            foreach (var argument in arguments)
            {
                args.Add(argument.WriteNative(currentNamespace));
            }

            return string.Join(", ", args);
        }
        
        public static string WriteSignalArgsProperties(this IEnumerable<Argument> arguments, Namespace currentNamespace)
        {
            var builder = new StringBuilder();
            var converter = new CaseConverter(); //TODO Make this a service

            var index = 0;
            foreach (var argument in arguments)
            {
                index += 1;
                var type = argument.WriteManagedType(currentNamespace);
                var name = converter.ToPascalCase(argument.ManagedName);

                builder.AppendLine($"public {type} {name} => Args[{index}].Extract<{type}>();");
            }

            return builder.ToString();
        }
        
        public static string WriteCallbackMarshaller(this IEnumerable<Argument> arguments, ReturnValue returnValue, Namespace currentNamespace)
        {
            var builder = new StringBuilder();
            var args = new List<string>();

            foreach (Argument arg in arguments)
            {
                // Skip 'user_data' parameters (for callbacks, when closure index is not zero)
                if (arg.ClosureIndex.HasValue)
                    continue;

                builder.AppendLine(WriteMarshalArgumentToManaged(arg, currentNamespace));
                args.Add(arg.ManagedName + "Managed");
            }

            var funcArgs = string.Join(
                separator: ", ", 
                values: args
            );

            var funcCall = returnValue.IsVoid()
                ? $"managedCallback({funcArgs});" 
                : $"var managed_callback_result = managedCallback({funcArgs});";

            builder.Append(funcCall);

            return builder.ToString();
        }

        private static string WriteMarshalArgumentToManaged(Argument arg, Namespace currentNamespace)
        {
            // TODO: We need to support disguised structs (opaque types)
            var expression = (arg.SymbolReference.GetSymbol(), arg) switch
            {
                (Record r, { Array: null } a) => $"Marshal.PtrToStructure<{r.ManagedName}>({a.NativeName});",
                (Record r, { Array: {}} a) => $"{a.NativeName}.MarshalToStructure<{r.ManagedName}>();",
                (Class c, {Array: null} a) => $"Object.WrapHandle<{c.ManagedName}>({a.NativeName});",
                (Class c, {Array: {}}) => throw new NotImplementedException($"Cant create delegate for argument {arg.ManagedName}"),
                _ => $"({arg.SymbolReference.GetSymbol().ManagedName}){arg.NativeName};" // Other -> Try a brute-force cast
            };

            return $"{arg.WriteTypeAndName(Target.Managed, currentNamespace)}Managed = " + expression;
        }
    }
}
