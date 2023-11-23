using RimWorld;
using Verse;

namespace HPF_Moyo
{
    public class ConstraintPawnStatGreater : ConstraintPawnStat
    {
        public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
        {
            if (pawn == null || !this.equal)
            {
                return pawn.GetStatValue(this.stat, true) > this.value;
            }
            return pawn.GetStatValue(this.stat, true) >= this.value;
        }
    }
}
