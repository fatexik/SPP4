using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGenerator
{
    public class TreeBuilder
    {
        private string text;

        public TreeBuilder(string text)
        {
            this.text = text;
        }

        public SyntaxTreeInfo synteseTreeInfo()
        {
            SyntaxTree programSyntaxTree = CSharpSyntaxTree.ParseText(text);
            CompilationUnitSyntax root = programSyntaxTree.GetCompilationUnitRoot();
            return new SyntaxTreeInfo(getClasses(root));
        }

        private List<ClassInfo> getClasses(CompilationUnitSyntax root)
        {
            List<ClassInfo> classes = new List<ClassInfo>();

            foreach (ClassDeclarationSyntax classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                string namespaceName = ((NamespaceDeclarationSyntax) classDeclaration.Parent).Name.ToString();
                string className = classDeclaration.Identifier.ValueText;
                classes.Add(new ClassInfo(namespaceName, className, getMethods(classDeclaration)));
            }

            return classes;
        }

        private List<MethodInfo> getMethods(ClassDeclarationSyntax classDeclaration)
        {
            List<MethodInfo> methods = new List<MethodInfo>();

            foreach (MethodDeclarationSyntax methodDeclaration in
                classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>()
                    .Where((methodDeclaration) =>
                        methodDeclaration.Modifiers.Any((modifier) =>
                            modifier.IsKind(SyntaxKind.PublicKeyword))))
            {
                string methodName = methodDeclaration.Identifier.ValueText;
                methods.Add(new MethodInfo(methodName));
            }

            return methods;
        }
    }
}