using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace HPF_Moyo
{
	public class JobDriver_GatherPawnResources : JobDriver
	{
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			Job job = this.job;
			return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			HPFJobDef def = this.job.def as HPFJobDef;
			if (def == null)
			{
				Log.Error("def is not HPFJobDef. please use HPF.HPFJobDef instead of JobDef.");
				yield break;
			}
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);

			Toil wait = new Toil();
			wait.FailOnDespawnedOrNull(TargetIndex.A);
			wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			wait.initAction = delegate ()
			{
				Pawn actor = wait.actor;
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				actor.pather.StopDead();
				PawnUtility.ForceWait(pawn, 15000, null, true);
			};

			wait.tickAction = delegate ()
			{
				HPFJobDef hpfjobDef = (HPFJobDef)this.job.def;
				Pawn actor = wait.actor;
				actor.skills.Learn(def.activeSkill, def.xpPerTick, false);
				this.gatherProgress += actor.GetStatValue(def.activeStat, true);
				if (this.gatherProgress >= hpfjobDef.totalWork)
				{
					foreach (CompResourceHarvestable item in from x in (this.job.GetTarget(TargetIndex.A).Thing as ThingWithComps)?.GetComps<CompResourceHarvestable>()
															 where x.Props.harvestJobDef == job.def
															 select x)
					{
						item.Gathered(this.pawn, hpfjobDef.activeStat);
					}

					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			wait.AddFinishAction(delegate
			{
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				if (pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			});
			wait.AddEndCondition(delegate
			{
				LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
				foreach (CompResourceHarvestable item in from x in (target.Thing as ThingWithComps)?.GetComps<CompResourceHarvestable>()
														 where x.Props.harvestJobDef == this.job.def
														 select x)
				{
					if (!item.ActiveAndFull)
					{
						return JobCondition.Incompletable;
					}
				}

				return JobCondition.Ongoing;
			});

			wait.defaultCompleteMode = ToilCompleteMode.Never;
			wait.WithProgressBar(TargetIndex.A, () => this.gatherProgress / ((HPFJobDef)this.job.def).totalWork, false, -0.5f);
			wait.activeSkill = (() => def.activeSkill);
			yield return wait;
			yield break;
		}

		private float gatherProgress;
	}
}
