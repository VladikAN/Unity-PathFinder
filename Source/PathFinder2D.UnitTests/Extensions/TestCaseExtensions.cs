using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PathFinder2D.UnitTests.Extensions
{
    public static class TestCaseExtensions
    {
        public static IEnumerable<TestCaseData> CombineArguments(this IEnumerable<TestCaseData> testCases, object[] args)
        {
            var result = new List<TestCaseData>();
            foreach (var arg in args)
            {
                foreach (var testCase in testCases)
                {
                    var arguments = testCase.Arguments.ToList();
                    arguments.Add(arg);

                    var newTestCase = new TestCaseData(args: arguments.ToArray());
                    newTestCase.SetName(string.Format("{0}: {1}", arg.GetType().Name, testCase.TestName));

                    result.Add(newTestCase);
                }
            }

            return result;
        }
    }
}