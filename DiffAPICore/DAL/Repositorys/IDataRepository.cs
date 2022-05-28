using System;
using System.Collections.Generic;
using System.Text;
using DAL.Data;
using DAL.Models;

namespace DAL.Repositorys
{
    public interface IDataRepository
    {
         DiffInfo GetByID(int ID);
        
         bool AddLeft(int ID, string val);
         bool UpdateLeft(int ID, string val);
         bool AddRight(int ID, string val);
         bool UpdateRight(int ID, string val);

         LeftData GetLeftByID(int ID);
         RightData GetRightByID(int ID);
         bool EmptyTables();
    }
}
