using System;
using System.Collections.Generic;
namespace Bean{
	public class MissionConfigData: ConfigBaseData, ICloneable{
		public int missionId;
		public int stepId;
		public string title;
		public string desc;
		public MissionType type;
		public MissionFilter filter;
		public List<int> target;
		public List<string> targetDescribe;
		public Dictionary<int,int> targetNum;
		public Dictionary<int,int> completeNum;
		public List<int> award;
		public List<int> awardNum;
		public bool isPreCount;
		public MissionConfigData next;
		public List<int> unlockMission;
		public List<int> branch;
		public int branchBelong;
		public int isPreUnlock;
		public MissionConfigData(){
			target = new List<int>();
			targetDescribe = new List<string>();
			targetNum = new Dictionary<int,int>();
			completeNum = new Dictionary<int,int>();
			award = new List<int>();
			awardNum = new List<int>();
			unlockMission = new List<int>();
			branch = new List<int>();
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