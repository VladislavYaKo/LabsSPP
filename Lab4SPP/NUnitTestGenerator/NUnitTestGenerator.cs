using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace NUnitTestGenerator
{
    public class NUnitTestGenerator
    {
        private SyntaxNode GenerateCompilationUnit(ClassDeclarationSyntax classDeclaration)
        {
            if (!(classDeclaration.Parent is NamespaceDeclarationSyntax))
            {
                return null;
            }

            string namespaceName = (classDeclaration.Parent as NamespaceDeclarationSyntax).Name.ToString();
            return CompilationUnit()
                    .WithUsings(
                        List<UsingDirectiveSyntax>(
                            new UsingDirectiveSyntax[] {
                                UsingDirective(
                                    IdentifierName("System")),
                                UsingDirective(
                                    QualifiedName(
                                        IdentifierName("System"),
                                        IdentifierName("Generic"))),
                                UsingDirective(
                                    QualifiedName(
                                        IdentifierName("System"),
                                        IdentifierName("Linq"))),
                                UsingDirective(
                                    QualifiedName(
                                        IdentifierName("System"),
                                        IdentifierName("Text"))),
                                UsingDirective(
                                    QualifiedName(
                                        IdentifierName("NUnit"),
                                        IdentifierName("Framework"))),
                                    UsingDirective(
                                        IdentifierName(namespaceName))
                            }))
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            NamespaceDeclaration(
                                QualifiedName(
                                    IdentifierName(namespaceName),
                                    IdentifierName("Tests")))))
                    .NormalizeWhitespace();
        }

        private SyntaxNode GenerateClassNode(SyntaxNode root, ClassDeclarationSyntax classDeclaration)
        {
            string className = classDeclaration.Identifier.Text;
            NamespaceDeclarationSyntax oldNamespaceDeclaration = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            NamespaceDeclarationSyntax newNamespaceDeclaration = oldNamespaceDeclaration.AddMembers(
                ClassDeclaration(className)
                    .WithAttributeLists(
                        SingletonList<AttributeListSyntax>(
                            AttributeList(
                                SingletonSeparatedList<AttributeSyntax>(
                                    Attribute(
                                        IdentifierName("TestFixture"))))))
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword)))
                    );

            return root.ReplaceNode(oldNamespaceDeclaration, newNamespaceDeclaration);
        }

        private SyntaxNode GenerateTestMethods(SyntaxNode root, IEnumerable<MethodDeclarationSyntax> methods)
        {
            Dictionary<string, int> OveloadedMethods = new Dictionary<string, int>();
            ClassDeclarationSyntax oldClassDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            ClassDeclarationSyntax newClassDeclaration = oldClassDeclaration;
            foreach (MethodDeclarationSyntax method in methods)
            {
                string substr = "";
                if (methods.Any(Method => (method != Method) && (method.Identifier.ValueText == Method.Identifier.ValueText)))
                {
                    if (OveloadedMethods.ContainsKey(method.Identifier.ValueText))
                    {
                        OveloadedMethods[method.Identifier.ValueText] += 1;
                    }
                    else
                    {
                        OveloadedMethods[method.Identifier.ValueText] = 1;
                    }
                    substr = OveloadedMethods[method.Identifier.ValueText].ToString();
                }
                newClassDeclaration = newClassDeclaration.AddMembers(GenerateTestMethod(method, substr));
            }

            return root.ReplaceNode(oldClassDeclaration, newClassDeclaration);
        }

        private MemberDeclarationSyntax GenerateTestMethod(MethodDeclarationSyntax method, string overridenMethodNumber)
        {
            string methodIdentifier = method.Identifier.Text + overridenMethodNumber + "MethodTest";
            return MethodDeclaration(
                        PredefinedType(
                            method.ReturnType.GetFirstToken()),
                        Identifier(methodIdentifier))
                    .WithAttributeLists(
                        SingletonList<AttributeListSyntax>(
                            AttributeList(
                                SingletonSeparatedList<AttributeSyntax>(
                                    Attribute(
                                        IdentifierName("Test"))))))
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword)))
                    .WithBody(
                        Block(
                            SingletonList<StatementSyntax>(
                                ExpressionStatement(
                                    InvocationExpression(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("Assert"),
                                            IdentifierName("Fail")))
                                    .WithArgumentList(
                                        ArgumentList(
                                            SingletonSeparatedList<ArgumentSyntax>(
                                                Argument(
                                                    LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        Literal("autogenerated"))))))))));
        }

        public List<SyntaxNode> GenerateCompilationUnitFromSourceCode(string sourceCode)
        {
            try
            {
                CompilationUnitSyntax sourceRoot = CSharpSyntaxTree.ParseText(sourceCode).GetCompilationUnitRoot();
                if (sourceRoot.Members.Count == 0)
                {
                    return null;
                }

                List<ClassDeclarationSyntax> classDeclarations = sourceRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
                List<SyntaxNode> syntaxNodes = new List<SyntaxNode>();
                foreach (var classDeclaration in classDeclarations)
                {
                    SyntaxNode result = GenerateCompilationUnit(classDeclaration);
                    result = GenerateClassNode(result, classDeclaration);
                    result = GenerateTestMethods(result, classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.Modifiers.Any(modifier => modifier.ToString() == "public")));
                    syntaxNodes.Add(result.NormalizeWhitespace());
                }
                return syntaxNodes;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<SyntaxNode> Generate(string source)
        {
            return GenerateCompilationUnitFromSourceCode(source);
        }
    }
}
