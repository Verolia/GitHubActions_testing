using System.Diagnostics;
using NUnit.Framework;

[SetUpFixture]  // Applies to whole assembly when no namespace is declared
public class SetupTrace
{
    [OneTimeSetUp]
    public void StartTest()
    {
        // Writes Debug.WriteLine and Trace.WriteLine output to the console
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    [OneTimeTearDown]
    public void EndTest()
    {
        Trace.Flush();
    }
}