using System;
using System.Collections.Generic;
namespace Bean{
	public class MissionConfigData: ConfigBaseData, ICloneable{
		public MissionConfigData missionId;
		public MissionConfigData stepId;
		public MissionConfigData title;
		public MissionConfigData desc;
		public MissionConfigData type;
		public MissionConfigData filter;
		public MissionConfigData target;
		public MissionConfigData targetDescribe;
		public MissionConfigData targetNum;
		public MissionConfigData completeNum;
		public MissionConfigData award;
		public MissionConfigData awardNum;
		public MissionConfigData isPreCount;
		public MissionConfigData next;
		public MissionConfigData unlockMission;
		public MissionConfigData branch;
		public MissionConfigData branchBelong;
		public MissionConfigData isPreUnlock;
		public MissionConfigData(){
		}
		public MissionConfigData(MissionConfigData obj){
			missionId = obj.missionId;
			stepId = obj.stepId;
			title = obj.title;
			desc = obj.desc;
			type = obj.type;
			filter = obj.filter;
			target = obj.target;
			targetDescribe = obj.targetDescribe;
			targetNum = obj.targetNum;
			completeNum = obj.completeNum;
			award = obj.award;
			awardNum = obj.awardNum;
			isPreCount = obj.isPreCount;
			next = obj.next;
			unlockMission = obj.unlockMission;
			branch = obj.branch;
			branchBelong = obj.branchBelong;
			isPreUnlock = obj.isPreUnlock;
		}
		public object Clone()
		{
			return new MissionConfigData(this);
		}
	}
}