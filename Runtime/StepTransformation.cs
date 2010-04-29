using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow.Bindings;

namespace TechTalk.SpecFlow.Bindings
{
    //TODO: move to Bindigns folder
    public class StepTransformation : MethodBinding
    {
        public Regex Regex { get; private set; }

        public StepTransformation(string regexString, MethodInfo methodInfo)
            : base(methodInfo)
        {
            Regex regex = new Regex("^" + regexString + "$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            Regex = regex;
        }

        public string[] GetStepTransformationArguments(string stepSnippet)
        {
            var match = Regex.Match(stepSnippet);
            var argumentStrings = match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray();
            return argumentStrings;
        }

        public object Transform(string value)
        {
            var arguments = GetStepTransformationArguments(value);
            return BindingAction.DynamicInvoke(arguments);
        }
    }
}