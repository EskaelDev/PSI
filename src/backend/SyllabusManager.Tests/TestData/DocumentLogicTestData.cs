using System;
using System.Collections.Generic;

namespace SyllabusManager.Tests.TestData
{
    public static class DocumentLogicTestData
    {
        public static IEnumerable<object[]> GetVersions()
        {
            yield return new object[] { "2020_01_01_02", DateTime.UtcNow.ToString("yyyy_MM_dd_") + "01" };
            yield return new object[] { DateTime.UtcNow.ToString("yyyy_MM_dd_") + "02", DateTime.UtcNow.ToString("yyyy_MM_dd_") + "03" };
        }
    }
}
