using Noqoush.Framework.BigData.CardinalityEstimation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.ReportsGP.MinHashEstimator
{
    public class CombinedEstimator
    {

        public IList<MinHashSet> MinHashSets { get; set; }

        public IList<Noqoush.Framework.BigData.CardinalityEstimation.CardinalityEstimator> Cardinalities { get; set; }
        public CombinedEstimator()
        {
            this.MinHashSets = new List<MinHashSet>();
            this.Cardinalities = new List<Noqoush.Framework.BigData.CardinalityEstimation.CardinalityEstimator>();

        }
        public CombinedEstimator(IList<Noqoush.Framework.BigData.CardinalityEstimation.CardinalityEstimator> Card, IList<MinHashSet> MinHash)
        {
            this.MinHashSets = MinHash;
            this.Cardinalities = Card;

        }

        public void AddToMinHash(MinHashSet minSet)
        {

            this.MinHashSets.Add(minSet);
        }

        public void AddToCardinlity(Noqoush.Framework.BigData.CardinalityEstimation.CardinalityEstimator cardEst)
        {
            this.Cardinalities.Add(cardEst);

        }
        public long GetTotalCombined(Noqoush.Framework.BigData.CardinalityEstimation.CardinalityEstimator card , MinHashSet hashSet, long requests=0)
        {
            if (this.Cardinalities!=null)
            {
                if (this.Cardinalities.Count > 0)
                {

                    this.Cardinalities.Add(card);
                    this.MinHashSets.Add(hashSet);
                    var result= CardinalityIntersector.IntersectAll(this.Cardinalities.ToArray(), this.MinHashSets.ToArray());
                    this.Cardinalities.RemoveAt(this.Cardinalities.Count-1);
                    this.MinHashSets.RemoveAt(this.MinHashSets.Count - 1);

                    return result;

                }

                else
                {
                    if (requests == 0)
                        return (long)card.Count();
                    else
                        return requests;
                }
            }

            return 0;
        }
    }
}
