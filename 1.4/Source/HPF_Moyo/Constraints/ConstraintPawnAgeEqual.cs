using Verse;

namespace HPF_Moyo
{
    public class ConstraintPawnAgeEqual : ConstraintPawnAge
    {
        public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
        {
            return pawn != null && pawn.ageTracker != null && pawn.ageTracker.AgeBiologicalYears == this.age;
        }
    }
}
