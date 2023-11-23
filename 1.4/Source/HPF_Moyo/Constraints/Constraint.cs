using Verse;

namespace HPF_Moyo
{
    public abstract class Constraint
    {
        public abstract bool CheckActiveCondition(ThingComp comp, Pawn pawn = null, ThingWithComps equipment = null);

        public bool passIfNotProperParent;
    }
}
