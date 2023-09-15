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
		private static MethodDeclarationSyntax GetTargetForGeneration(GeneratorSyntaxContext context)
		{
			var methodDeclarationSyntax = (MethodDeclarationSyntax)context.Node;
			return methodDeclarationSyntax;
		}

		private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
		{
			return syntaxNode is MethodDeclarationSyntax methodDeclarationSyntax &&
				methodDeclarationSyntax.AttributeLists.Count > 0 &&
				methodDeclarationSyntax.AttributeLists
					.Any(al => al.Attributes
						.Any(a => a.Name.ToString() == "RelayCommand"));
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
				(spc, source) => Execute(source.Item1, source.Item2, spc));
		}

		public void Execute(Compilation compilation, ImmutableArray<MethodDeclarationSyntax> methods, SourceProductionContext context)
		{
			var methodsByClass = methods.GroupBy(m => m.Parent);
			foreach (var methodGroup in methodsByClass)
			{
				var classDeclaration = (ClassDeclarationSyntax)methodGroup.Key;
				var className = classDeclaration.Identifier.ToString();
				var namespaceName = "";

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

				var src = $@"using FloxelLib.MVVM;
namespace {namespaceName};

#nullable enable
public partial class {className} : BaseViewModel
{{

";

				foreach (var method in methodGroup)
				{
					var commandName = method.Identifier.ToString();
					var privateName = char.ToLower(commandName[0]) + commandName.Substring(1);

					src += $@"	private RelayCommand? _{privateName}Command;
	public RelayCommand {commandName}Command => _{privateName}Command ??= new RelayCommand({commandName});

";
				}

				src += @"}
#nullable restore";
				context.AddSource($"{className}.g.cs", src);
			}
		}
	}
}
