using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BoDi;
using TechTalk.SpecFlow.Infrastructure;

#if SILVERLIGHT
using TechTalk.SpecFlow.Compatibility;
#endif

namespace TechTalk.SpecFlow
{
    public class ScenarioContext : SpecFlowContext
    {
        private static ScenarioContext current;
        public static ScenarioContext Current
        {
            get
            {
                if (current == null)
                {
                    Debug.WriteLine("Accessing NULL ScenarioContext");
                }
                return current;
            }
            internal set { current = value; }
        }

        public ScenarioInfo ScenarioInfo { get; private set; }

        public ScenarioBlock CurrentScenarioBlock { get; internal set; }

        internal TestStatus TestStatus { get; set; }
        public Exception TestError { get; internal set; }
        internal List<string> PendingSteps { get; private set; }
        internal List<string> MissingSteps { get; private set; }
        internal Stopwatch Stopwatch { get; private set; }

        internal ITestRunner TestRunner { get; private set; } //TODO: initialize

        private readonly IObjectContainer objectContainer;

        [Obsolete("eliminate this method when separating test runner from test execution engine")]
        internal void SetTestRunnerUnchecked(ITestRunner newTestRunner)
        {
            TestRunner = newTestRunner;
        }

        internal ScenarioContext(ScenarioInfo scenarioInfo, ITestRunner testRunner)
        {
            TestRunner = testRunner;

            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            CurrentScenarioBlock = ScenarioBlock.None;
            ScenarioInfo = scenarioInfo;
            TestStatus = TestStatus.OK;
            PendingSteps = new List<string>();
            MissingSteps = new List<string>();

            objectContainer = new ObjectContainer(((IContainedInstance)testRunner).Container);
        }

        public void Pending()
        {
            TestRunner.Pending();
        }

//        private Dictionary<Type, object> bindingInstances = new Dictionary<Type, object>();

        public object GetBindingInstance(Type bindingType)
        {
            return objectContainer.Resolve(bindingType);
/*

            object value;
            if (!bindingInstances.TryGetValue(bindingType, out value))
            {
                var ctors = bindingType.GetConstructors();
                if (bindingType.IsClass && ctors.Length == 0)
                    throw new MissingMethodException(String.Format("No public constructors found for type {0}", bindingType.FullName));

                var parameters = new List<object>();
                foreach (var param in ctors[0].GetParameters())
                {
                    parameters.Add(GetBindingInstance(param.ParameterType)); 
                }

                value = Activator.CreateInstance(bindingType, parameters.ToArray());
                bindingInstances.Add(bindingType, value);
            }

            return value;
*/
        }

        internal void SetBindingInstance(Type bindingType, object instance)
        {
            objectContainer.RegisterInstanceAs(instance, bindingType);
            //bindingInstances[bindingType] = instance;
        }

        protected override void Dispose()
        {
            base.Dispose();

            objectContainer.Dispose();
        }
    }
}