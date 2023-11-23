using Verse;

namespace HPF_Moyo
{
    public class ConstraintPawnAgeLess : ConstraintPawnAge
    {
        public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
        {
            if (pawn == null || pawn.ageTracker == null)
            {
                return false;
            }
            if (!this.equal)
            {
                return pawn.ageTracker.AgeBiologicalYears < this.age;
            }
            return pawn.ageTracker.AgeBiologicalYears <= this.age;
        }

        public bool equal;
    }
}
