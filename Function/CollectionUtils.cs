using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Function {

    public static class CollectionUtils {

        public static IEnumerable<T> Reversed<T>(this IEnumerable<T> self) {
            var reversed = new List<T>(self);
            reversed.Reverse();
            return reversed.AsEnumerable();
        }
    }
}
