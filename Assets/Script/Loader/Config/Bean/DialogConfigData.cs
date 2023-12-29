using System;
using System.Collections.Generic;
namespace Bean{
	public class DialogConfigData: ConfigBaseData, ICloneable{
		public int mainId;
		public int stepId;
		public int branchId;
		public string target;
		public string content;
		public double autoSpeed;
		public List<string> selection;
		public List<int> next;
		public DialogConfigData(){
			selection = new List<string>();
			next = new List<int>();
		}
		public DialogConfigData(DialogConfigData obj){
			mainId = obj.mainId;
			stepId = obj.stepId;
			branchId = obj.branchId;
			target = obj.target;
			content = obj.content;
			autoSpeed = obj.autoSpeed;
			selection = obj.selection;
			next = obj.next;
		}
		public object Clone()
		{
			return new DialogConfigData(this);
		}
	}
}