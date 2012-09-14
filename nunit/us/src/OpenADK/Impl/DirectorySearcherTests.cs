using System;
using System.Collections;
using System.IO;
using OpenADK.Util;
using NUnit.Framework;

namespace Library.Nunit.US.Impl
{
    /// <summary>
    /// Summary description for DirectorySearcherTests.
    /// </summary>
    [TestFixture]
    public class DirectorySearcherTests
    {
        [Test]
        public void EnumerateAllFiles()
        {
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            GetAllDllFilesFilter filter = new GetAllDllFilesFilter(); //Poorly named - Only does dlls starting with Library

            AssertFileListing(
                new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory),
                new GetAllDllFilesFilter(),
                4);
        }

        [Test]
        public void EnumerateOnlyTestDll()
        {
            AssertFileListing(
                new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory),
                new GetOnlyThisAssemblyFilter(),
                1);
        }

        [Test]
        public void EnumerateNoFiles()
        {
            AssertFileListing(
                new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory),
                new GetNoFilesFilter(),
                0);
        }

        private void AssertFileListing(DirectoryInfo info, IFileNameFilter filter, int expectedItems)
        {
            IEnumerator enumerator = DirectorySearcher.GetFileEnumerator(info, filter);
            Assert.AreEqual(expectedItems, CountItemsInEnumeration(enumerator), "Wrong number of files returned");

            FileInfo[] infos = DirectorySearcher.GetFiles(info, filter);
            Assert.AreEqual(expectedItems, infos.Length, "Wrong number of files returned");
        }

        private int CountItemsInEnumeration(IEnumerator enumerator)
        {
            int i = 0;
            while (enumerator.MoveNext())
            {
                i++;
            }
            return i;
        }

        private class GetAllDllFilesFilter : IFileNameFilter
        {
            #region IFileNameFilter Members

            public bool Accept(FileInfo info, string name)
            {
                return true;
            }

            public string SearchPattern
            {
                get { return "Library.*.dll"; }
            }

            #endregion
        }

        private class GetNoFilesFilter : IFileNameFilter
        {
            #region IFileNameFilter Members

            public bool Accept(FileInfo info, string name)
            {
                return false;
            }

            public string SearchPattern
            {
                get { return "*.*"; }
            }

            #endregion
        }


        private class GetOnlyThisAssemblyFilter : IFileNameFilter
        {
            public GetOnlyThisAssemblyFilter()
            {
                mFileName = GetType().Assembly.GetName().Name;
            }

            #region IFileNameFilter Members

            public bool Accept(FileInfo info, string name)
            {
                return name.StartsWith(mFileName);
            }

            public string SearchPattern
            {
                get { return "*.dll"; }
            }

            #endregion

            private string mFileName;
        }
    }
}