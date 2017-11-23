using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCommon.Error;

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
        /// The cardinality of a set of combinations
        /// </summary>
        public int SetCardinality { get; set; }

        /// <summary>
        /// A set of groups that was originally passed
        /// </summary>
        public List<List<T>> OriginalGroupSets { get; set; }

        /// <summary>
        /// The resultant sets
        /// </summary>
        public List<List<T>> ResultSets { get; set; }

        /// <summary>
        /// To provide a default constructor for processing in a list
        /// </summary>
        protected GroupCombinations() { }

        /// <summary>
        /// Constructor with the group sets whose combinations are to be created
        /// </summary>
        /// <param name="groupSets"></param>
        public GroupCombinations(List<List<T>> groupSets, int setCardinality = -1)
        {
            if (groupSets == null || groupSets.Count <= 0)
                throw new CombinatoricsException(CombinatoricsException.EErrorType.ARGUMENT_NULL_OR_EMPTY, "groupSets");

            // A referecne to the orginal T type objects
            OriginalGroupSets = groupSets;

            if (setCardinality == -1) SetCardinality = groupSets.Count;
            else SetCardinality = setCardinality;
        }

        public void GenerateNonRepetive()
        {
            ResultSets = Generate(0);
        }

        /// <summary>
        /// Generate Combinations
        /// </summary>
        private List<List<T>> Generate(int rowindex)
        {
            List<List<T>> resultSet = null;
            if (rowindex == OriginalGroupSets.Count - 1)
            {
                resultSet = new List<List<T>>();
                for(int i = 0; i < OriginalGroupSets[rowindex].Count; ++i)
                {
                    List<T> newList = new List<T>();
                    newList.Add(OriginalGroupSets[rowindex][i]);
                    resultSet.Add(newList);
                }
            }
            else
            {
                resultSet = Generate(rowindex + 1);
                resultSet = Combine(resultSet, OriginalGroupSets[rowindex]);
            }

            return resultSet;
        }

        private List<List<T>> Combine(List<List<T>> inputSet, List<T> list)
        {
            List<List<T>> resultSet = new List<List<T>>();

            for (int j = 0; j < list.Count; ++j)
            {
                for (int i = 0; i < inputSet.Count; ++i)
                {
                    List<T> newList = new List<T>();
                    newList.AddRange(inputSet[i]);
                    newList.Add(list[j]);
                    resultSet.Add(newList);
                }
            }

            return resultSet;
        }
    }
}
