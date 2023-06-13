
using System.Diagnostics.CodeAnalysis;
using TheFruityMixologist.Entities;

namespace TheFruityMixologist.Utilities.Comparer
{
    public class RecipeComparer : IEqualityComparer<Recipe>
    {
        public bool Equals(Recipe? x, Recipe? y)
        {
            if (Equals(x?.Id, y?.Id)) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] Recipe obj)
        {
            throw new NotImplementedException();
        }
    }
}
