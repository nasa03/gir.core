﻿using System;
using System.Collections.Generic;

namespace GirLoader.Output;

public partial class Method : GirModel.Method
{
    GirModel.ComplexType GirModel.Method.Parent => _parent ?? throw new Exception($"{Identifier}: Unknown parent");
    string GirModel.Callable.Name => Name;
    GirModel.ReturnType GirModel.Method.ReturnType => ReturnValue;
    string GirModel.Callable.CIdentifier => Identifier;
    GirModel.InstanceParameter GirModel.Method.InstanceParameter => ParameterList.InstanceParameter ?? throw new Exception("Instance parameter mis missing");
    GirModel.InstanceParameter? GirModel.Callable.InstanceParameter => ParameterList.InstanceParameter;
    IEnumerable<GirModel.Parameter> GirModel.Callable.Parameters => ParameterList.SingleParameters;
    bool GirModel.Method.Introspectable => Introspectable;
    GirModel.Property? GirModel.Method.GetProperty => GetProperty?.GetProperty();
    GirModel.Property? GirModel.Method.SetProperty => SetProperty?.GetProperty();
    string? GirModel.Method.Version => Version;
}
