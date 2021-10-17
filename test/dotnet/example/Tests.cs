using NUnit.Framework;
using static System.Diagnostics.FileVersionInfo;
using static System.Reflection.Assembly;

namespace Tests
{
    public class Version
    {
        string Expected =>
            TestContext.Parameters["version"] ?? "0.0.0.0";
        System.Reflection.Assembly Assembly =>
            GetExecutingAssembly();

        [Test]
        public void AssemblyVersion()
        {
            Assert.AreEqual(Expected, Assembly.GetName().Version.ToString());
        }

        [Test]
        public void FileVersion()
        {
            Assert.AreEqual(Expected, GetVersionInfo(Assembly.Location).FileVersion);
        }

        [Test]
        public void ProductVersion()
        {
            Assert.AreEqual(Expected, GetVersionInfo(Assembly.Location).ProductVersion);
        }
    }
}
