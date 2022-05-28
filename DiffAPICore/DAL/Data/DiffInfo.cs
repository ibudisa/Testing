using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data
{
    public class DiffInfo
    {
        public string DiffResultType { get; set; } = String.Empty;
        public List<Diff> DiffResult { get; set; } = new List<Diff>();

        public DiffInfo()
        {

        }

        // check the equality between objects
        public override bool Equals(object obj)
        {
            DiffInfo data = obj as DiffInfo;
            bool status = false;
            status = this.DiffResultType.Equals(data.DiffResultType);
            bool liststatus=false;
            List<Diff> listdiff1=this.DiffResult;
            List<Diff> listdiff2 = data.DiffResult;
            int list1count = this.DiffResult.Count; 
            int list2count=data.DiffResult.Count;
            int count = 0;
            if(list1count==list2count)
            {
                var length = list1count;
                for(int i=0;i<list1count;i++)
                {
                    Diff diff1 = listdiff1[i];
                    Diff diff2 = listdiff2[i];
                    if (diff1.Equals(diff2))
                        count++;
                }
                if(count==length)
                    liststatus=true;
            }

            if (status && liststatus)
                return true;
            else
                return false;
        }
    }
}
