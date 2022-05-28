using DAL.Data;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DataComparer : IDataComparer
    {
        public DiffInfo Info { get; set; } = new DiffInfo();

        // get differences between 2 strings in DiffResultType and DiffResult that is list of differences with offset and length
        public DiffInfo GetDifference(string a, string b)
        {
            if (a.Length != b.Length)
            {
                Info.DiffResultType = "SizeDoNotMatch";
                return Info;
            }
            else if (a.Equals(b))
            {
                Info.DiffResultType = "Equals";
                return Info;
            }
            else
            {
                Info.DiffResultType = "ContentDoNotMatch";

                int counter = 0;
                int offset = 0;

                List<Diff> difflist = new List<Diff>();
                char[] arraya = a.ToCharArray();
                char[] arrayb = b.ToCharArray();
                int arraylength = arraya.Length;

                for (int i = 0; i < arraylength; i++)
                {
                    counter = 0;
                    Diff diff = new Diff();
                    while (arraya[i] != arrayb[i])
                    {
                        if (counter == 0)
                        {
                            offset = i;
                            diff.Offset = offset;
                        }
                        counter++;
                        i++;

                    }
                    if (counter > 0)
                    {
                        diff.Length = counter;
                        difflist.Add(diff);
                    }
                }
                Info.DiffResult = difflist;
                return Info;
            }
        }
           

    }
    }

