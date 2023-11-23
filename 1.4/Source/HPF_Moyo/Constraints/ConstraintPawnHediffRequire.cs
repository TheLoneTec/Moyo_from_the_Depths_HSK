using System.Collections.Generic;
using Verse;

namespace HPF_Moyo
{
    public class ConstraintPawnHediffRequire : ConstraintPawnHediff
    {
        public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
        {
            return pawn != null && pawn.health != null && pawn.health.hediffSet.HasHediff(this.hediff, false);
        }
    }

    public class ConstraintPawnHediffListRequire : ConstraintPawnHediff
    {
        public List<HediffDef> hediffList = new List<HediffDef>();
        public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
        {
            bool hasHediff = false;
            foreach (var hediffDef in this.hediffList)
            {
                if (pawn != null && pawn.health != null && pawn.health.hediffSet.HasHediff(hediffDef, false))
                {
                    hasHediff = true;
                    break;
                }
            }
            return hasHediff;
        }
    }
}
