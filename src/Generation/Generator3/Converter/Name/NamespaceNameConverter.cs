﻿namespace Generator3.Converter
{
    public static class NamespaceNameConverter
    {
        public static string GetCanonicalName(this GirModel.Namespace @namespace)
            => $"{@namespace.Name}-{@namespace.Version}";

        public static string GetInternalName(this GirModel.Namespace @namespace)
            => $"{@namespace.Name}.Internal";
    }
}
