using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppConfigTransforms.Tests
{
    public class TargetsTests
    {
        private readonly string _thisAssembly;
        private readonly string _binRoot;
        private readonly string _testRoot;
        private readonly IEnumerable<string> _binConfigs;
        private readonly IEnumerable<string> _testConfigs;

        public TargetsTests()
        {
            var thisAssemblyUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            _thisAssembly = Path.GetFileName(thisAssemblyUri.LocalPath);
            _binRoot = Path.GetDirectoryName(thisAssemblyUri.LocalPath);
            _testRoot = Path.Combine(_binRoot, "..\\..\\");
            _testRoot = Path.GetFullPath(_testRoot);
            _binConfigs = Directory.GetFiles(_binRoot, "*.config").Select(x => Path.GetFileName(x));
            _testConfigs = Directory.GetFiles(_testRoot, "App*.config").Select(x => Path.GetFileName(x));
        }

        [Fact]
        public void should_copy_and_rename_app_config_transforms_to_output()
        {
            Assert.Equal(_testConfigs.Count(), 3);
            foreach(var transform in _testConfigs)
            {
                var expectedName = transform.Replace("App", _thisAssembly);
                Assert.Contains(expectedName, _binConfigs, StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
