﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Specification.Method.Injection;

namespace Method
{
    [TestClass]
    public class Injection : SpecificationTests
    {
        public override IUnityContainer GetContainer()
        {
            return new UnityContainer();
        }
    }
}