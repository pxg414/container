﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Unity.ObjectBuilder.Strategies.BuildPlan.Resolution;
using Unity.Policy;

namespace Unity.ObjectBuilder.Strategies.BuildPlan.Method
{
    /// <summary>
    /// An implementation of <see cref="IMethodSelectorPolicy"/> that selects
    /// methods by looking for the given <typeparamref name="TMarkerAttribute"/>
    /// attribute on those methods.
    /// </summary>
    /// <typeparam name="TMarkerAttribute">Type of attribute used to mark methods
    /// to inject.</typeparam>
    public class MethodSelectorPolicy<TMarkerAttribute> : MethodSelectorPolicyBase<TMarkerAttribute>
        where TMarkerAttribute : Attribute
    {
        /// <summary>
        /// Create a <see cref="IDependencyResolverPolicy"/> instance for the given
        /// <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter">Parameter to create the resolver for.</param>
        /// <returns>The resolver object.</returns>
        protected override IDependencyResolverPolicy CreateResolver(ParameterInfo parameter)
        {
            return new FixedTypeResolverPolicy((parameter ?? throw new ArgumentNullException(nameof(parameter))).ParameterType);
        }
    }
}
