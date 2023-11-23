using Verse;

namespace HPF_Moyo
{
    public class ConstraintPawnGenderOnly : ConstraintPawnGender
    {
        public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
        {
            return pawn != null && pawn.gender == this.gender;
        }
    }
}
