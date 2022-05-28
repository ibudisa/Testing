using System;
using System.Collections.Generic;
using System.Text;
using DAL.Data;
using DAL.Models;
using System.Linq;

namespace DAL.Repositorys
{
    public class DataRepository : IDataRepository
    {
        private readonly TestDBContext tDbContext;
        private readonly IDataComparer cmp;

        // create datarepository from dbcontext and datacomparer
        public DataRepository(TestDBContext tDbContext,IDataComparer cmp)
        {
            this.tDbContext = tDbContext;
            this.cmp = cmp;
        }
        // add leftdata to database
        public bool AddLeft(int ID, string val)
        {
            LeftData ldata = new LeftData();
            ldata.Id = ID;
            ldata.LeftValue = val;
            this.tDbContext.LeftData.Add(ldata);
            this.tDbContext.SaveChanges();
           return true;
        }

        // add rightdata to database
        public bool AddRight(int ID, string val)
        {
            RightData rdata = new RightData();
            rdata.Id = ID;
            rdata.RightValue = val;
            this.tDbContext.RightData.Add(rdata);
            this.tDbContext.SaveChanges();
            return true;
            
        }
        // delete data drom leftdata and leftdata
        public bool EmptyTables()
        {
            bool empty = false;
            if (tDbContext.LeftData.ToList().Count > 0)
            {
                foreach (var item in tDbContext.LeftData)
                {
                    tDbContext.LeftData.Remove(item);
                }
                tDbContext.SaveChanges();
            }
            if (tDbContext.RightData.ToList().Count > 0)
            {
                foreach (var item in tDbContext.RightData)
                {
                    tDbContext.RightData.Remove(item);
                }
                tDbContext.SaveChanges();
            }
            empty = true;
            return empty;
        }
        // get differences between leftdata and rightdata by id
        public DiffInfo GetByID(int ID)
        {
            var left = GetLeftByID(ID);
            var right = GetRightByID(ID);
            if (left == null || right == null)
            {
                return new DiffInfo();
            }
            var diffdata = this.cmp.GetDifference(left.LeftValue, right.RightValue);
            return diffdata;
        }
        // get leftdata by id
        public LeftData GetLeftByID(int ID)
        {
            LeftData leftData = this.tDbContext.LeftData.SingleOrDefault(l => l.Id == ID);
            return leftData;
        }

        // get rightdata by id
        public RightData GetRightByID(int ID)
        {
            RightData rightData = this.tDbContext.RightData.SingleOrDefault(l => l.Id == ID);
            return rightData;
        }

        // update leftdata by id and value
        public bool UpdateLeft(int ID, string val)
        {
            bool exists = false;
            var leftadata = GetLeftByID(ID);
            if (leftadata != null)
            {
                exists = true;
                leftadata.LeftValue = val;
                this.tDbContext.SaveChanges();
            }

            return exists;


        }
        // update rightdata by id and value
        public bool UpdateRight(int ID, string val)
        {
            bool exists = false;
            var rightadata = GetRightByID(ID);
            if (rightadata != null)
            {
                exists = true;
                rightadata.RightValue = val;
                this.tDbContext.SaveChanges();
            }

            return exists;
        }
    }
}
