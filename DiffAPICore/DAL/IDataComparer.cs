using System;
using System.Collections.Generic;
using System.Text;
using DAL.Data;

namespace DAL
{
    public interface IDataComparer
    {
        DiffInfo GetDifference(string a, string b);
    }
}
