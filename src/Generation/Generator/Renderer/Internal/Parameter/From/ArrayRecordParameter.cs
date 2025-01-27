﻿using Generator.Model;

namespace Generator.Renderer.Internal;

internal static class ArrayRecordParameter
{
    public static RenderableParameter Create(GirModel.Parameter parameter)
    {
        return new RenderableParameter(
            Attribute: GetAttribute(parameter),
            Direction: string.Empty,
            NullableTypeName: GetNullableTypeName(parameter),
            Name: Parameter.GetName(parameter)
        );
    }

    private static string GetNullableTypeName(GirModel.Parameter parameter)
    {
        var arrayType = parameter.AnyType.AsT1;

        return arrayType.Length is null
            ? Type.PointerArray
            : Record.GetFullyQualifiedInternalStructName((GirModel.Record) arrayType.AnyType.AsT0) + "[]";
    }

    private static string GetAttribute(GirModel.Parameter parameter)
    {
        return parameter.AnyType.AsT1.Length switch
        {
            { } length => MarshalAs.UnmanagedLpArray(sizeParamIndex: length),
            _ => string.Empty,
        };
    }
}
