using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;

namespace Floxel.Generator
{

	[Generator]
	public class ViewModelGenerator : IIncrementalGenerator
	{
		private static (MethodDeclarationSyntax method, FieldDeclarationSyntax field) GetTargetForGeneration(GeneratorSyntaxContext context)
		{
			if (context.Node is MethodDeclarationSyntax methodDeclarationSyntax)
			{
				return (methodDeclarationSyntax, null);
			}
			else if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax)
			{
				return (null, fieldDeclarationSyntax);
			}
			return (null, null);
		}

		private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
		{
			switch (syntaxNode)
			{
				case MethodDeclarationSyntax methodDeclarationSyntax:
					return methodDeclarationSyntax.AttributeLists.Count > 0 &&
						   methodDeclarationSyntax.AttributeLists
							   .Any(al => al.Attributes
								   .Any(a => a.Name.ToString() == "RelayCommand"));
				case FieldDeclarationSyntax fieldDeclarationSyntax:
					return fieldDeclarationSyntax.AttributeLists.Count > 0 &&
						   fieldDeclarationSyntax.AttributeLists
							   .Any(al => al.Attributes
								   .Any(a => a.Name.ToString() == "UpdateProperty"));
				default:
					return false;
			}
		}

		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			var methodDeclarations = context.SyntaxProvider
				.CreateSyntaxProvider(
					predicate: (s, _) => IsSyntaxTargetForGeneration(s),
					transform: (ctx, _) => GetTargetForGeneration(ctx));

			var compilationAndEnums
				= context.CompilationProvider.Combine(methodDeclarations.Collect());

			context.RegisterSourceOutput(compilationAndEnums,
				(spc, source) => Execute(source.Left, source.Right, spc));
		}

		public void Execute(Compilation c, ImmutableArray<(MethodDeclarationSyntax method, FieldDeclarationSyntax field)> data, SourceProductionContext context)
		{
			var dataByClass = data.GroupBy(m => m.method?.Parent ?? m.field?.Parent);
			foreach (var dataGroup in dataByClass)
			{
				var classDeclaration = (ClassDeclarationSyntax)dataGroup.Key;
				var className = classDeclaration.Identifier.ToString();
				var namespaceName = "";

				// find root element (iterate through parents)
				var root = classDeclaration.Parent;
				while (root.Parent != null)
				{
					root = root.Parent;
				}

				// clone all usings from parent
				var usingList = root.ChildNodesAndTokens()
					.Where(n => n.IsNode && n.AsNode() is UsingDirectiveSyntax)
					.Select(n => n.AsNode() as UsingDirectiveSyntax)
					.Select(n => n.ToString())
					.ToList();

				var usings = string.Join("\n", usingList);


				var checkNext = false;
				foreach (var nodeOrToken in classDeclaration.Parent.ChildNodesAndTokens())
				{
					if (checkNext)
					{
						namespaceName = nodeOrToken.ToString();
						break;
					}
					if (nodeOrToken.IsToken && nodeOrToken.AsToken().IsKind(SyntaxKind.NamespaceKeyword))
					{
						checkNext = true;
					}
				}

				var src = $@"{usings}
namespace {namespaceName};

#nullable enable
public partial class {className} : BaseViewModel
{{

";

				foreach (var group in dataGroup)
				{
					if (group.field != null)
					{
						// check if has callback
						var attribute = group.field.AttributeLists
							.SelectMany(al => al.Attributes)
							.First(a => a.Name.ToString() == "UpdateProperty");
						var args = attribute.ArgumentList?.Arguments.ToArray();
						var result = args?.Length == 1 ? args[0].ToString() : "";
						if (result.Length > 1 && result[0] == '"' && result[result.Length - 1] == '"')
						{
							result = result.Substring(1, result.Length - 2);
						}
						result += ' ';

						var fieldType = group.field.Declaration.Type.ToString();

						foreach (var decl in group.field.Declaration.Variables)
						{
							var fieldName = decl.Identifier.ToString();
							var publicNameParts = fieldName.Split('_');
							publicNameParts = publicNameParts.Select(p =>
							{
								if (p.Length == 0) return p;
								return char.ToUpper(p[0]) + p.Substring(1);
							}).ToArray();
							var publicName = string.Join("", publicNameParts);

							src += $@"	public {fieldType} {publicName}
	{{
		get => {fieldName};
		set {{ SetField(ref {fieldName}, value); {result}}}
	}}

";
						}
					}
					else
					{
						var commandName = group.method.Identifier.ToString();
						var privateName = char.ToLower(commandName[0]) + commandName.Substring(1);

						src += $@"	private RelayCommand? _{privateName}Command;
	public RelayCommand {commandName}Command => _{privateName}Command ??= new RelayCommand({commandName});

";
					}

				}

				src += @"}
#nullable restore";
				context.AddSource($"{className}.g.cs", src);
			}
		}
	}
}
