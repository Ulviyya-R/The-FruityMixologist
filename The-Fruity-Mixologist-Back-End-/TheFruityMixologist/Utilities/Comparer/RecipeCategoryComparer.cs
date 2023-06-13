
using System.Diagnostics.CodeAnalysis;
using TheFruityMixologist.Entities;

namespace TheFruityMixologist.Utilities.Comparer
{
    public class RecipeCategoryComparer : IEqualityComparer<RecipesCategory>
    {
        public bool Equals(RecipesCategory? x, RecipesCategory? y)
        {
            if (Equals(x?.CategoryId, y.CategoryId)) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] RecipesCategory obj)
        {
            throw new NotImplementedException();
        }
    }
}
