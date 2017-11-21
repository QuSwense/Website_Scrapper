using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCommon.Combinatorics
{
    /// <summary>
    /// Create a combination of values from a list of sets.
    /// E.g., if there are provided 3 sets of data as
    /// {A, B}, {C, D, E}, {F}
    /// Then the combination of data that this class produces:
    /// {A, C, F}, {A, D, F}, {A, E, F}, {B, C, F}, {B, D, F}, {B, E, F}
    /// The cardinality of the resultant sets is multiplication of count of each set, i.e.,
    /// f(A1) *  f(A2) * ...
    /// For the above example the cardinality is:
    /// 2 * 3 * 1 = 6
    /// </summary>
    /// <typeparam name="T">Represents each data in all the sets</typeparam>
    public class GroupCombinations<T>
    {
        /// <summary>
        /// To provide a default constructor for processing i na list
        /// </summary>
        protected GroupCombinations() { }
    }
}
