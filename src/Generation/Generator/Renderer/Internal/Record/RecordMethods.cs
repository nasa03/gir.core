﻿using Generator.Model;

namespace Generator.Renderer.Internal;

internal static class RecordMethods
{
    public static string Render(GirModel.Record record)
    {
        return $@"
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetInternalName(record.Namespace)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    {PlatformSupportAttribute.Render(record as GirModel.PlatformDependent)}
    public partial class {record.Name}
    {{
        {Functions.Render(record.TypeFunction)}
        {Functions.Render(record.Functions)}
        {Methods.Render(record.Methods)}
        {Constructors.Render(record.Constructors)}
    }}
}}";
    }
}
