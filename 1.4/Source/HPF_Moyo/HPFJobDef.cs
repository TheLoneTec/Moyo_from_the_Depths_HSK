using RimWorld;
using Verse;

namespace HPF_Moyo
{
    public class HPFJobDef : JobDef
    {
        public bool isSelf;

        public float totalWork;

        public float xpPerTick;

        public SkillDef activeSkill;

        public StatDef activeStat;
    }
}
