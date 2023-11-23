using Verse;

namespace HPF_Moyo
{
	public class ConstraintPawnHediffRestrict : ConstraintPawnHediff
	{
		public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
		{
			return pawn != null && pawn.health != null && !pawn.health.hediffSet.HasHediff(this.hediff, false);
		}
	}
}
