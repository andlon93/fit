using GraphQL;
using GraphQL.Language.AST;
using System.Linq;

namespace FitnessTracker.Users.GraphTypes
{
    public static class ResoleFieldContextExtensions
    {
        public static bool IsFieldSpecified(this IResolveFieldContext context, string fieldName)
            => context.SubFields.ContainsKey(fieldName) // Check if value is defined directly for fast lookup
               || context.SubFields.Any(s => s.Value.Name == fieldName) // Check value name
               || IsFragmentFieldSpecified(context.FieldAst.SelectionSet, context.Fragments, fieldName); // Check if field is specified as a fragment
        private static bool IsFragmentFieldSpecified(SelectionSet fields, Fragments fragments, string typeName)
        {
            var fragmentsInQuery = fields.Children.OfType<FragmentSpread>().Select(f => fragments.FindDefinition(f.Name));
            var fragmentFields = fragmentsInQuery.SelectMany(frag => frag.SelectionSet.Children.OfType<Field>());
            return fragmentFields.Any(field => field.Name == typeName);
        }
    }
}
