using System;
using System.Collections.Generic;
namespace Bean{
	public class SkillConfigData: ConfigBaseData, ICloneable{
		public string name;
		public InputKey key;
		public double skillTime;
		public double outOfTime;
		public int holdingTime;
		public SkillTrigger trigger;
		public bool isCanForceStop;
		public int forceTimes;
		public int defaultForceTimes;
		public int forceResetTime;
		public string ani;
		public List<SkillConfigData> next;
		public SkillConfigData(){
			next = new List<SkillConfigData>();
		}
		public SkillConfigData(SkillConfigData obj){
			name = obj.name;
			key = obj.key;
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
		}
		public object Clone()
		{
			return new SkillConfigData(this);
		}
	}
}