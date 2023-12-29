using System;
using System.Collections.Generic;
namespace Bean{
	public class SkillConfigData: ConfigBaseData, ICloneable{
		public int mainId;
		public string name;
		public InputKey key;
		public int stepId;
		public double skillTime;
		public double outOfTime;
		public int holdingTime;
		public SkillTrigger trigger;
		public bool isCanForceStop;
		public int forceTimes;
		public int defaultForceTimes;
		public int forceResetTime;
		public string ani;
		public List<int> next;
		public bool attackEnable;
		public SkillConfigData(){
			next = new List<int>();
		}
		public SkillConfigData(SkillConfigData obj){
			mainId = obj.mainId;
			name = obj.name;
			key = obj.key;
			stepId = obj.stepId;
			skillTime = obj.skillTime;
			outOfTime = obj.outOfTime;
			holdingTime = obj.holdingTime;
			trigger = obj.trigger;
			isCanForceStop = obj.isCanForceStop;
			forceTimes = obj.forceTimes;
			defaultForceTimes = obj.defaultForceTimes;
			forceResetTime = obj.forceResetTime;
			ani = obj.ani;
			next = obj.next;
			attackEnable = obj.attackEnable;
		}
		public object Clone()
		{
			return new SkillConfigData(this);
		}
	}
}