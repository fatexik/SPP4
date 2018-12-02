using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TestGenerator
{
    public class TestClassGenerator
    {
        private SyntaxTreeInfo syntaxTreeInfo;

        public TestClassGenerator(SyntaxTreeInfo syntaxTreeInfo)
        {
            this.syntaxTreeInfo = syntaxTreeInfo;
        }

        public List<TestClassSignature> GetTestTemplates()
        {
            List<TestClassSignature> testTemplates = new List<TestClassSignature>();

            foreach (ClassInfo classInfo in syntaxTreeInfo.getClassInfos())
            {
                NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(QualifiedName(
                    IdentifierName(classInfo.getNamespace()), IdentifierName("Tests")));

                CompilationUnitSyntax compilationUnit = CompilationUnit()
                    .WithUsings(GetUsingDirectives(classInfo))
                    .WithMembers(SingletonList<MemberDeclarationSyntax>(namespaceDeclaration
                        .WithMembers(SingletonList<MemberDeclarationSyntax>(
                            ClassDeclaration(classInfo.getName() + "Tests")
                                .WithAttributeLists(
                                    SingletonList(
                                        AttributeList(
                                            SingletonSeparatedList(
                                                Attribute(
                                                    IdentifierName("TestClass"))))))
                                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                .WithMembers(GetClassMembers(classInfo))))));

                string fileName = classInfo.getName()+"Tests.cs";
                string fileData = compilationUnit.NormalizeWhitespace().ToFullString();

                testTemplates.Add(new TestClassSignature(fileName, fileData));
            }

            return testTemplates;
        }

        private SyntaxList<UsingDirectiveSyntax> GetUsingDirectives(ClassInfo classInfo)
        {
            List<UsingDirectiveSyntax> usingDirectives = new List<UsingDirectiveSyntax>
            {
                UsingDirective(IdentifierName("System")),
                UsingDirective(QualifiedName(
                    QualifiedName(
                        IdentifierName("System"),
                        IdentifierName("Collections")),
                    IdentifierName("Generic"))),
                UsingDirective(QualifiedName(
                    IdentifierName("System"),
                    IdentifierName("Linq"))),
                UsingDirective(
                    QualifiedName(
                        QualifiedName(
                            QualifiedName(
                                IdentifierName("Microsoft"),
                                IdentifierName("VisualStudio")),
                            IdentifierName("TestTools")),
                        IdentifierName("UnitTesting"))),
                UsingDirective(IdentifierName(classInfo.getNamespace()))
            };

            return List(usingDirectives);
        }

        private SyntaxList<MemberDeclarationSyntax> GetClassMembers(ClassInfo classInfo)
        {
            List<MemberDeclarationSyntax> classMembers = new List<MemberDeclarationSyntax>();

            foreach (MethodInfo methodInfo in classInfo.getMethodsList())
            {
                classMembers.Add(GetTestMethodDeclaration(methodInfo));
            }

            return List(classMembers);
        }

        private MethodDeclarationSyntax GetMethodDeclaration(string attributeName, string methodName,
            SyntaxList<StatementSyntax> blockMembers)
        {
            MethodDeclarationSyntax methodDeclaration = MethodDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.VoidKeyword)),
                    Identifier(methodName))
                .WithAttributeLists(
                    SingletonList(
                        AttributeList(
                            SingletonSeparatedList(
                                Attribute(
                                    IdentifierName(attributeName))))))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithBody(Block(blockMembers));

            return methodDeclaration;
        }

        private MethodDeclarationSyntax GetTestMethodDeclaration(MethodInfo methodInfo)
        {
            List<StatementSyntax> blockMembers = new List<StatementSyntax>();
            List<ArgumentSyntax> parameters = new List<ArgumentSyntax>();

            ArgumentListSyntax args = ArgumentList(SingletonSeparatedList(
                Argument(
                    LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        Literal("autogenerated")))));

            blockMembers.Add(ExpressionStatement(
                InvocationExpression(
                        GetMemberAccessExpression(
                            "Assert",
                            "Fail"))
                    .WithArgumentList(args)));

            return GetMethodDeclaration("TestMethod", $"{methodInfo.getName()}Test", List(blockMembers));
        }

        private MemberAccessExpressionSyntax GetMemberAccessExpression(string objectName, string memberName)
        {
            return MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName(objectName),
                IdentifierName(memberName));
        }
    }
}