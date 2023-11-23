using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace HPF_Moyo
{
	public class JobDriver_GatherPawnSelfResources : JobDriver
	{
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			HPFJobDef def = this.job.def as HPFJobDef;
			if (def == null)
			{
				Log.Error("def is not HPFJobDef. please use HPF.HPFJobDef instead of JobDef.");
				yield break;
			}
			Toil wait = new Toil();
			wait.WithProgressBar(TargetIndex.A, () => this.gatherProgress / ((HPFJobDef)this.job.def).totalWork, false, -0.5f);
			wait.FailOnDespawnedOrNull(TargetIndex.A);
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			wait.activeSkill = (() => def.activeSkill);
			wait.initAction = delegate ()
			{
				wait.actor.pather.StopDead();
			};
			wait.tickAction = delegate ()
			{
				Pawn actor = wait.actor;
				this.gatherProgress += actor.GetStatValue(def.activeStat, true);
				if (this.gatherProgress >= ((HPFJobDef)this.job.def).totalWork)
				{
					foreach (CompResourceHarvestable comp in from x in actor.GetComps<CompResourceHarvestable>()
															 where x.Props.harvestJobDef == this.job.def
															 select x)
					{
						comp.Gathered(actor, def.activeStat);
					}

					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			yield return wait;
			yield break;
		}

		private float gatherProgress;
	}
}
