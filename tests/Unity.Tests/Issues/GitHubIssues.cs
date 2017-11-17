using System.Linq;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Attributes;
using Unity.Exceptions;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Tests.TestObjects;

namespace Unity.Tests.Issues
{
    [TestClass]
    public class GitHub
    {

        [TestMethod]
        public void unity_154_2()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<OtherEmailService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IService, OtherEmailService>();
            container.RegisterType<IOtherService, OtherEmailService>(new InjectionConstructor(container));

            Assert.AreNotSame(container.Resolve<IService>(),
                              container.Resolve<IOtherService>());

            Assert.AreSame(container.Resolve<IService>(),
                           container.Resolve<OtherEmailService>());
        }


        [TestMethod]
        public void unity_154_1()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<OtherEmailService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IService, OtherEmailService>();
            container.RegisterType<IOtherService, OtherEmailService>();

            Assert.AreSame(container.Resolve<IService>(), 
                           container.Resolve<IOtherService>());
        }


        [TestMethod]
        public void unity_153()
        {
            IUnityContainer rootContainer = new UnityContainer();
            rootContainer.RegisterType<ILogger, MockLogger>(new HierarchicalLifetimeManager());

            using (IUnityContainer childContainer = rootContainer.CreateChildContainer())
            {
                var a = childContainer.Resolve<ILogger>();
                var b = childContainer.Resolve<ILogger>();

                Assert.AreSame(a, b);
            }
        }


        [TestMethod]
        public void Issue_35()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ILogger, MockLogger>(new ContainerControlledLifetimeManager());
            ILogger logger = container.Resolve<ILogger>();

            Assert.IsNotNull(logger);
            Assert.AreSame(container.Resolve<ILogger>(), logger);

            container.RegisterType<MockLogger>(new TransientLifetimeManager());

            Assert.AreSame(container.Resolve<ILogger>(), logger);
        }

        [TestMethod]
        public void Issue_88()   // https://github.com/unitycontainer/unity/issues/88
        {
            using (var unityContainer = new UnityContainer())
            {
                unityContainer.RegisterInstance(true);
                unityContainer.RegisterInstance("true", true);
                unityContainer.RegisterInstance("false", false);

                var resolveAll = unityContainer.ResolveAll(typeof(bool));
                var arr = resolveAll.Select(o => o.ToString()).ToArray();
            }
        }

        [TestMethod]
        public void Issue_54()   // https://github.com/unitycontainer/unity/issues/54
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType(typeof(ITestClass), typeof(TestClass));
                container.RegisterInstance(new TestClass());
                var instance = container.Resolve<ITestClass>(); //0
                Assert.IsNotNull(instance);
            }

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType(typeof(ITestClass), typeof(TestClass));
                container.RegisterType<TestClass>(new ContainerControlledLifetimeManager());

                try
                {
                    var instance = container.Resolve<ITestClass>(); //2
                    Assert.IsNull(instance, "Should threw an exception");
                }
                catch (ResolutionFailedException e)
                {
                    Assert.IsInstanceOfType(e, typeof(ResolutionFailedException));
                }

            }
        }

        // Test types 
        public interface ITestClass
        { }

        public class TestClass : ITestClass
        {
            public TestClass()
            { }

            [InjectionConstructor]
            public TestClass(TestClass x) //1
            { }
        }
    }
}
