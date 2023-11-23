using Verse;

namespace HPF_Moyo
{
    public class CompEquipConstraint : ThingComp
    {
        public CompProperties_EquipConstraint Props
        {
            get
            {
                return (CompProperties_EquipConstraint)this.props;
            }
        }

        public bool CanPawnEquip(Pawn pawn)
        {
            foreach (var constraint in Props.constraints)
            {
                if (!constraint.CheckActiveCondition(this, pawn, parent))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
