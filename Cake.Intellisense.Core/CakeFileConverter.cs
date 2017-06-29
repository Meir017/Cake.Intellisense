using Cake.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Linq;
using System.Text;

namespace Cake.Intellisense.Core
{
    public class CakeFileConverter
    {
        public void ConvertCsToCake()
        {
            string output = @"..\..\dist";
            string[] files = Directory.GetFiles(@"..\..\", "*.cake.cs");

            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }

            foreach (var info in files.Select(file => new FileInfo(file)))
            {
                string outputFilename = info.Name.Replace(".cs", string.Empty);
                string cakeFile = ToCakeFile(File.ReadAllText(info.FullName));
                File.WriteAllText(Path.Combine(output, outputFilename), cakeFile);
            }
        }

        private static string ToCakeFile(string content)
        {
            const int classMethodStatementIndentationSpaces = 12;
            var compilation = SyntaxFactory.ParseCompilationUnit(content);

            var builder = new StringBuilder();
            var script = GetExecuteMethodBody(compilation);

            foreach (string line in script)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    builder.AppendLine();
                    continue;
                }
                string trimmedLine = line.Substring(classMethodStatementIndentationSpaces);

                if (trimmedLine.StartsWith("Addin("))
                {
                    builder.AppendLine($"#addin {trimmedLine.Substring("Addin(".Length, trimmedLine.Length - "Addin(".Length - ");".Length)}");
                }
                else if (trimmedLine.StartsWith("Tool("))
                {
                    builder.AppendLine($"#tool {trimmedLine.Substring("Tool(".Length, trimmedLine.Length - "Tool(".Length - ");".Length)}");
                }
                else if (trimmedLine.StartsWith("Load("))
                {
                    builder.AppendLine($"#load {trimmedLine.Substring("Load(".Length, trimmedLine.Length - "Load(".Length - ");".Length)}");
                }
                else
                {
                    builder.AppendLine(trimmedLine);
                }
            }

            return builder.ToString();
        }
        private static string[] GetExecuteMethodBody(CompilationUnitSyntax compilation)
        {
            var @namespace = (NamespaceDeclarationSyntax)compilation.Members[0];
            var classes = @namespace.Members.OfType<ClassDeclarationSyntax>();

            var cakeFile = classes.FirstOrDefault(@class => @class.BaseList
                .Types.OfType<SimpleBaseTypeSyntax>()
                .Any(type => (type.Type as IdentifierNameSyntax).Identifier.ValueText == nameof(CakeFileIntellisense)));

            var execute = cakeFile.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(method => method.Identifier.ValueText == nameof(CakeFile.Execute));

            return execute.Body.Statements.ToFullString().SplitLines();
        }
    }
}
