﻿using System.Collections.Generic;
using System.Linq;
using Generator.Model;

namespace Generator.Renderer.Internal;

internal static class GlobalFunctions
{
    public static string Render(IEnumerable<GirModel.Function> functions)
    {
        return $@"
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetInternalName(functions.First().Namespace)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    public partial class Functions
    {{
        {Functions.Render(functions)}
    }}
}}";
    }
}
