using System.Collections.Generic;
using System.Linq;

namespace Function.Util {

    public static class CollectionUtils {

        public static IEnumerable<T> Reversed<T>(this IEnumerable<T> self) {
            var reversed = new List<T>(self);
            reversed.Reverse();
            return reversed.AsEnumerable();
        }
    }
}
