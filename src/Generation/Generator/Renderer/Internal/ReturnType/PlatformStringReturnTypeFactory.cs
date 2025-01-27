﻿using Generator.Model;

namespace Generator.Renderer.Internal;

internal static class PlatformStringReturnTypeFactory
{
    public static RenderableReturnType CreateForCallback(GirModel.ReturnType returnType)
    {
        // This must be IntPtr since SafeHandle's cannot be returned from managed to unmanaged.
        return new RenderableReturnType(Type.Pointer);
    }

    public static RenderableReturnType Create(GirModel.ReturnType returnType)
    {
        var nullableTypeName = returnType switch
        {
            { Nullable: true, Transfer: GirModel.Transfer.None } => PlatformString.GetInternalNullableUnownedHandleName(),
            { Nullable: false, Transfer: GirModel.Transfer.None } => PlatformString.GetInternalNonNullableUnownedHandleName(),
            { Nullable: true, Transfer: GirModel.Transfer.Full } => PlatformString.GetInternalNullableOwnedHandleName(),
            _ => PlatformString.GetInternalNonNullableOwnedHandleName(),
        };

        return new RenderableReturnType(nullableTypeName);
    }
}
