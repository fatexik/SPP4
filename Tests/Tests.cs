
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;


namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private int readingTaskCount = 2;
        private int writingTaskCount = 2;
        private TestGeneratorClass.TestGeneratorClass testGeneratorClass;
        private string taskDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string outpDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private List<string> paths = new List<string>();
        private CompilationUnitSyntax root;
        [OneTimeSetUp]
        public void Initialize()
        {
            paths.Add(taskDirectory+"\\Class1.cs");
            testGeneratorClass = new TestGeneratorClass.TestGeneratorClass(outpDirectory);
            testGeneratorClass.generate(readingTaskCount, writingTaskCount, paths).Wait();
            string sourceCode = File.ReadAllText(outpDirectory + "\\Class1Tests.cs");
            root = CSharpSyntaxTree.ParseText(sourceCode).GetCompilationUnitRoot();
        }
        [Test]
        public void testUsing()
        {
            Assert.AreEqual("System", root.Usings[0].Name.ToString());
            Assert.AreEqual("System.Collections.Generic", root.Usings[1].Name.ToString());
            Assert.AreEqual("System.IO", root.Usings[2].Name.ToString());
            Assert.AreEqual("NUnit.Framework", root.Usings[3].Name.ToString());
            Assert.AreEqual("Tests", root.Usings[4].Name.ToString());
        }

        [Test]
        public void testNamespace()
        {
            IEnumerable<NamespaceDeclarationSyntax> namespaces = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

            Assert.AreEqual(1, namespaces.Count());
            Assert.AreEqual("Tests.Tests", namespaces.ElementAt<NamespaceDeclarationSyntax>(0).Name.ToString());
        }
        
        [Test]
        public void testMethodAttributes()
        {
            IEnumerable<MethodDeclarationSyntax> methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            for (int i = 0; i < methods.Count(); i++)
            {
                Assert.AreEqual("Test", methods.ElementAt(i).AttributeLists[0].Attributes[0].Name.ToString());
            }
        }
        
        [Test]
        public void testClassAtribute()
        {
            IEnumerable<ClassDeclarationSyntax> classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            Assert.AreEqual(1, classes.ElementAt<ClassDeclarationSyntax>(0).AttributeLists.Count);
            Assert.AreEqual("Test", classes.ElementAt<ClassDeclarationSyntax>(0).AttributeLists[0].Attributes[0].Name.ToString());
        }
    }
}