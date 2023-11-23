using RimWorld;
using System.Collections.Generic;
using Verse;

namespace HPF_Moyo
{
	public class StatModifier
	{
		public StatDef statDef;

		public float offset;
		public float multiplier;
	}

	public class CompProperties_ResourceHarvestable : CompProperties
	{
		public CompProperties_ResourceHarvestable()
		{
			this.compClass = typeof(CompResourceHarvestable);
		}

		public JobDef harvestJobDef;

		public ThingDef thingDef;

		public string saveUniqueKey = "";

		public float intervalDays = 15f;

		public int amount = 1;

		public string inspectText;

        public ApparelSetting apparelSetting = new ApparelSetting();

        public HediffSetting hediffSetting = new HediffSetting();

        public List<Constraint> constraints = new List<Constraint>();

		public List<StatModifier> speedAffectedStats = new List<StatModifier>();
		public List<StatModifier> productAffectedStats = new List<StatModifier>();
	}

	public class HediffSetting
    {
        public List<HediffMultiple> hediffMultiples = new List<HediffMultiple>();
        public List<Heidfflplus> hediffPluses = new List<Heidfflplus>();
        public List<HediffMilk> hediffMilks = new List<HediffMilk>();
    }
    public class ApparelSetting
    {
        public List<ApparelMultiple> apparelMultiples = new List<ApparelMultiple>();
        public List<Apparelplus> apparelPluses = new List<Apparelplus>();
        public List<ApparelMilk> apparelMilks = new List<ApparelMilk>();
    }
    public class Heidfflplus
    {
        public List<HediffDef> hediffDefs = new List<HediffDef>();
        public int plus = 1;
    }

	public class HediffMultiple
    {
        public List<HediffDef> hediffDefs = new List<HediffDef>();
        public float multiple = 1;
    }
    public class HediffMilk
    {
        public List<HediffDef> hediffDefs = new List<HediffDef>();
        public ThingDef thingDef;
    }

    public class ApparelMultiple
    {
        public List<ThingDef> apparelThingDefs = new List<ThingDef>();
        public float multiple = 1;
    }

	public class Apparelplus
    {
        public List<ThingDef> apparelThingDefs = new List<ThingDef>();
        public int plus = 1;
    }

    public class ApparelMilk
    {
        public List<ThingDef> apparelThingDefs = new List<ThingDef>();
        public ThingDef thingDef;
    }
}
