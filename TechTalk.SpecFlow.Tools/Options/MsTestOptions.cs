using CommandLine;

namespace TechTalk.SpecFlow.Tools.Options
{
    [Verb("MsTestExecutionReport", HelpText = "Formats an MsTest execution report to SpecFlow style.")]
    public class MsTestOptions : BaseReportOptions
    {
        [Option('t', "TestResult",
            Default = "TestResult.trx",
            HelpText = "Test Result file generated by MsTest. Defaults to TestResult.trx.")]
        public string TestResult { get; set; }
    }
}