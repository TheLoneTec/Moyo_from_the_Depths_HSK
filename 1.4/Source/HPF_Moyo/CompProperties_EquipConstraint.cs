using System.Collections.Generic;
using Verse;

namespace HPF_Moyo
{
    public class CompProperties_EquipConstraint : CompProperties
    {
        public CompProperties_EquipConstraint()
        {
            this.compClass = typeof(CompEquipConstraint);
        }

        public override void PostLoadSpecial(ThingDef parent)
        {
            CompProperties_EquipConstraint.cachedThingDefs.Add(parent, this);
        }

        public static Dictionary<ThingDef, CompProperties_EquipConstraint> cachedThingDefs = new Dictionary<ThingDef, CompProperties_EquipConstraint>();
        public List<Constraint> constraints = new List<Constraint>();
    }
}
