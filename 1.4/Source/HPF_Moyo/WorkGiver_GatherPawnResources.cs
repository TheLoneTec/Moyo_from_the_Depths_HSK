using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace HPF_Moyo
{
	public class WorkGiver_GatherPawnResources : WorkGiver_Scanner
	{
		// Token: 0x0600003F RID: 63 RVA: 0x0000283A File Offset: 0x00000A3A
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned)
			{
				if (pawn2.GetComp<CompResourceHarvestable>() != null)
				{
					yield return pawn2;
				}
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				HPFWorkGiverDef hpfworkGiverDef = this.def as HPFWorkGiverDef;
				if (hpfworkGiverDef == null)
				{
					return PathEndMode.None;
				}

				HPFJobDef jobDef = hpfworkGiverDef.harvestJobDef as HPFJobDef;
				if (jobDef == null)
				{
					return PathEndMode.None;
				}

				if (jobDef.isSelf)
				{
					return PathEndMode.None;
				}
				else
				{
					return PathEndMode.Touch;
				}
			}
		}

		public override bool HasJobOnThing(Pawn pawn, Thing thing, bool forced = false)
		{
			Pawn pawn2 = thing as Pawn;
			if (pawn2 == null)
			{
				return false;
			}

			HPFWorkGiverDef hpfworkGiverDef = this.def as HPFWorkGiverDef;
			if (hpfworkGiverDef == null)
			{
				return false;
			}

			HPFJobDef jobDef = hpfworkGiverDef.harvestJobDef as HPFJobDef;
			if (jobDef == null)
			{
				return false;
			}

			if (!jobDef.isSelf && pawn == thing)
			{
				return false;
			}

			if (jobDef.isSelf)
			{
				if (pawn == thing &&
					pawn.GetComps<CompResourceHarvestable>().Any((CompResourceHarvestable x) => x.Props.harvestJobDef == jobDef && x.ActiveAndFull))
				{
					return true;
				}
			}
			else
			{
				foreach (CompResourceHarvestable comp in from x in pawn2.GetComps<CompResourceHarvestable>()
														 where x.Props.harvestJobDef == jobDef && x.ActiveAndFull
														 select x)
				{
					_ = comp;
					if (PawnUtility.CanCasuallyInteractNow(pawn2, false) && pawn.CanReserve(pawn2))
					{
						return true;
					}
				}
			}

			return false;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002950 File Offset: 0x00000B50
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			HPFWorkGiverDef hpfworkGiverDef = this.def as HPFWorkGiverDef;
			if (hpfworkGiverDef == null)
			{
				return null;
			}
			return new Job(hpfworkGiverDef.harvestJobDef, t);
		}
	}
}
