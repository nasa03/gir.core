﻿namespace Generator3.Renderer.Public
{
    public static class Constant
    {
        public static string Render(this Model.Public.Constant constant)
            => $"public static { constant.TypeName } { constant.Name } = { constant.Value };";
    }
}