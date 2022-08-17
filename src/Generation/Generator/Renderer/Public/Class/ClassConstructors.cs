﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator.Model;

namespace Generator.Renderer.Public;

internal static class ClassConstructors
{
    public static string Render(GirModel.Class cls)
    {
        return $@"
using System;
using System.Linq;
using GObject;
using System.Runtime.InteropServices;
#nullable enable
namespace { Namespace.GetPublicName(cls.Namespace) }
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    public partial class { cls.Name }
    {{
        {cls.Constructors
            .Select(x => Render(x, cls))
            .Join(Environment.NewLine)}
    }}
}}";
    }

    private static string Render(GirModel.Constructor constructor, GirModel.Class cls)
    {
        try
        {
            var newKeyWord = Class.HidesConstructor(cls, constructor)
                ? "new "
                : string.Empty;

            return @$"
{VersionAttribute.Render(constructor.Version)}
public static {newKeyWord}{cls.Name} {Constructor.GetName(constructor)}({Parameters.Render(constructor.Parameters)})
{{
    {ParametersToNativeExpression.Render(constructor.Parameters, out var parameterNames)}
    {RenderCallStatement(cls, constructor, parameterNames)}
}}";
        }
        catch (Exception e)
        {
            var message = $"Did not generate constructor '{cls.Name}.{Constructor.GetName(constructor)}': {e.Message}";

            if (e is NotImplementedException)
                Log.Debug(message);
            else
                Log.Warning(message);

            return string.Empty;
        }
    }

    private static string RenderCallStatement(GirModel.Class cls, GirModel.Constructor constructor, IEnumerable<string> parameterNames)
    {
        var variableName = "handle";
        var call = new StringBuilder();
        call.Append($"var {variableName} = Internal.{cls.Name}.{Constructor.GetName(constructor)}(");
        call.Append(string.Join(", ", parameterNames));
        call.Append(");" + Environment.NewLine);

        var ownedRef = Transfer.IsOwnedRef(constructor.ReturnType.Transfer);

        var statement = cls.Fundamental
            ? $"new {cls.Name}({variableName})"
            : $"new {cls.Name}({variableName}, {ownedRef.ToString().ToLower()})";

        call.Append($"return {statement};");

        return call.ToString();
    }
}
