using RimWorld;

namespace HPF_Moyo
{
    public abstract class ConstraintPawnStat : Constraint
    {
        public StatDef stat;

        public float value;

        public bool equal;
    }
}
