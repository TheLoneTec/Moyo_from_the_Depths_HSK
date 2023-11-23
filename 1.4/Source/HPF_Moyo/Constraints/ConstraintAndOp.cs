using System.Collections.Generic;
using Verse;

namespace HPF_Moyo
{
    public class ConstraintAndOp : Constraint
    {
        public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
        {
            using (List<Constraint>.Enumerator enumerator = this.constraints.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.CheckActiveCondition(comp, pawn, equipment))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Constraint> constraints = new List<Constraint>();
    }
}
