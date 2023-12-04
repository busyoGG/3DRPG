using System;
using System.Collections.Generic;
namespace Bean{
	public class DialogConfigData: ConfigBaseData, ICloneable{
		public int dialogId;
		public int stepId;
		public string target;
		public string content;
		public double autoSpeed;
		public List<string> selection;
		public List<DialogConfigData> next;
		public DialogConfigData(){
			next = new List<DialogConfigData>();
		}
		public DialogConfigData(DialogConfigData obj){
			dialogId = obj.dialogId;
			stepId = obj.stepId;
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