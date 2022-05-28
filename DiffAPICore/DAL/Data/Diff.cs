using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data
{
    public class Diff
    {
        public int Offset { get; set; }
        public int Length { get; set; }

        public Diff()
        {

        }
        // check the equality between objects
        public override bool Equals(object obj)
        {
            var diff= obj as Diff;
            bool same = false;
            if((this.Offset==diff.Offset)&&(diff.Length==this.Length))
                same=true;
            return same;
        }
    }
}
