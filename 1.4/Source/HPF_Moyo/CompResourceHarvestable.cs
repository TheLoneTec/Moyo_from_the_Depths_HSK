using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace HPF_Moyo
{
	public class CompResourceHarvestable : ThingComp
	{
		public CompProperties_ResourceHarvestable Props
		{
			get
			{
				return (CompProperties_ResourceHarvestable)this.props;
			}
		}

		protected bool Active
		{
			get
			{
				if (this.parent.Faction == null)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				using (List<Constraint>.Enumerator enumerator = this.Props.constraints.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CheckActiveCondition(this, pawn, null))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1f;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.Props.saveUniqueKey, 0f, false);
		}

		public override void CompTick()
		{
			if (this.Active)
			{
				float progressPerTick = 1f / (this.Props.intervalDays * 60000f);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					progressPerTick *= PawnUtility.BodyResourceGrowthSpeed(pawn);

					if (Props.speedAffectedStats?.Any() ?? false)
					{
						foreach (var statMultiplier in Props.speedAffectedStats)
						{
							progressPerTick = Mathf.Max(0, progressPerTick + (parent.GetStatValue(statMultiplier.statDef) + statMultiplier.offset) * statMultiplier.multiplier);
						}
					}
				}

				this.fullness += progressPerTick;
				if (this.fullness > 1f)
				{
					this.fullness = 1f;
				}
			}
		}

		public void Gathered(Pawn doer, StatDef stat)
		{
			if (!this.Active)
			{
				Log.Error(doer + " gathered body resources while not Active: " + this.parent);
			}
			if (!Rand.Chance(doer.GetStatValue(stat, true)))
			{
				MoteMaker.ThrowText((doer.DrawPos + this.parent.DrawPos) / 2f, this.parent.Map, "TextMote_ProductWasted".Translate(), 3.65f);
			}
			else
			{
				float apparelMultipleNum = 1;
                float hediffMultipleNum = 1;

				int apparelPlusNum = 0;
                int hediffPlusNum = 0;

				ThingDef apparelMilkThingDef = null;
				ThingDef hediffMilkThingDef = null;

                Pawn pawn = (Pawn)parent;

				
				foreach (var apparelMultiple in Props.apparelSetting.apparelMultiples)
				{
					foreach (var apparelThingDef in apparelMultiple.apparelThingDefs)
					{
						bool skip = false;
                        foreach (var pawnApparel in pawn.apparel.WornApparel)
                        {
							if(pawnApparel.def == apparelThingDef)
							{
								skip = true;
                                apparelMultipleNum *= apparelMultiple.multiple;
								break;
                            }
                        }
						if(skip)
						{
							break;
						}
                    }
				}

                foreach (var hediffMultiple in Props.hediffSetting.hediffMultiples)
                {
                    foreach (var hediffDef in hediffMultiple.hediffDefs)
                    {
                        bool skip = false;
                        foreach (var pawnHediff in pawn.health.hediffSet.hediffs)
                        {
                            if (pawnHediff.def == hediffDef)
                            {
                                skip = true;
                                hediffMultipleNum *= hediffMultiple.multiple;
                                break;
                            }
                        }
                        if (skip)
                        {
                            break;
                        }
                    }
                }

                foreach (var apparelPlus in Props.apparelSetting.apparelPluses)
                {
                    foreach (var apparelThingDef in apparelPlus.apparelThingDefs)
                    {
                        bool skip = false;
                        foreach (var pawnApparel in pawn.apparel.WornApparel)
                        {
                            if (pawnApparel.def == apparelThingDef)
                            {
                                skip = true;
                                apparelPlusNum += apparelPlus.plus;
                                break;
                            }
                        }
                        if (skip)
                        {
                            break;
                        }
                    }
                }

                foreach (var hediffPlus in Props.hediffSetting.hediffPluses)
                {
                    foreach (var hediffDef in hediffPlus.hediffDefs)
                    {
                        bool skip = false;
                        foreach (var pawnHediff in pawn.health.hediffSet.hediffs)
                        {
                            if (pawnHediff.def == hediffDef)
                            {
                                skip = true;
                                hediffPlusNum += hediffPlus.plus;
                                break;
                            }
                        }
                        if (skip)
                        {
                            break;
                        }
                    }
                }

                foreach (var apparelMilk in Props.apparelSetting.apparelMilks)
                {
                    foreach (var apparelThingDef in apparelMilk.apparelThingDefs)
                    {
                        bool skip = false;
                        foreach (var pawnApparel in pawn.apparel.WornApparel)
                        {
                            if (pawnApparel.def == apparelThingDef)
                            {
                                skip = true;
                                apparelMilkThingDef = apparelMilk.thingDef;
                                break;
                            }
                        }
                        if (skip)
                        {
                            break;
                        }
                    }
                }

                foreach (var hediffMilk in Props.hediffSetting.hediffMilks)
                {
                    foreach (var hediffDef in hediffMilk.hediffDefs)
                    {
                        bool skip = false;
                        foreach (var pawnHediff in pawn.health.hediffSet.hediffs)
                        {
                            if (pawnHediff.def == hediffDef)
                            {
                                skip = true;
                                hediffMilkThingDef = hediffMilk.thingDef;
                                break;
                            }
                        }
                        if (skip)
                        {
                            break;
                        }
                    }
                }

                float baseAmount = (Props.amount + apparelPlusNum + hediffPlusNum) * apparelMultipleNum * hediffMultipleNum;

				if (Props.productAffectedStats?.Any() ?? false)
				{
					foreach (var statMultiplier in Props.productAffectedStats)
					{
						baseAmount = Mathf.Max(0, baseAmount + (parent.GetStatValue(statMultiplier.statDef) + statMultiplier.offset) * statMultiplier.multiplier);
					}
				}

				ThingDef thingDef;

                if(hediffMilkThingDef != null)
				{
                    thingDef = hediffMilkThingDef;
                }
                else if (apparelMilkThingDef !=null)
				{
					thingDef = apparelMilkThingDef;
                }
				else
				{
					thingDef = Props.thingDef;
                }

				int totalAmount = (int)Math.Round((double)GenMath.RoundRandom(baseAmount * this.fullness));
				while (totalAmount > 0)
				{
					int oneStackCount = Mathf.Clamp(totalAmount, 1, thingDef.stackLimit);
					totalAmount -= oneStackCount;

					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = oneStackCount;
					GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}

			this.fullness = 0f;
		}

		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			if (this.Props.inspectText == null)
			{
				return null;
			}
			return this.Props.inspectText.Translate() + ": " + this.fullness.ToStringPercent();
		}

		private float fullness;
	}
}
