using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DAL.Repositorys
{
    public class CleaningPlanRepository : ICleaningPlanRepository
    {

        private readonly CustDbContext cDbContext;

        public CleaningPlanRepository(CustDbContext cDbContext)
        {
            this.cDbContext = cDbContext;
        }
        public async Task<CleaningPlan> AddCleaningPlan(Customer customer)
        {
            var plan = new CleaningPlan();
            plan.CustomerId = customer.CustomerId;
            plan.Title = customer.Title;
            plan.Description = customer.Description;
            plan.CreationDate = DateTime.Now;
            var result = await cDbContext.CleaningPlans.AddAsync(plan);
            await cDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<CleaningPlan> DeleteCleaningPlan(Guid id)
        {
            var result = await cDbContext.CleaningPlans
                 .FirstOrDefaultAsync(c => c.Id == id);
            if (result != null)
            {
                cDbContext.CleaningPlans.Remove(result);
                await cDbContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<CleaningPlan> GetCleaningPlanById(Guid id)
        {
           var result= await cDbContext.CleaningPlans
                .FirstOrDefaultAsync(c => c.Id == id);

            return result;
        }

        public async Task<IEnumerable<CleaningPlan>> GetCleaningPlansByCustomerId(int customerId)
        {
            
            var result =await cDbContext.CleaningPlans.Where(c=>c.CustomerId==customerId).ToListAsync();
                        

            return result;
        }
        public async Task<CleaningPlan> UpdateCleaningPlan(Customer customer,Guid id)
        {
            var result = await cDbContext.CleaningPlans.SingleOrDefaultAsync(c => c.Id == id);
            if(result!=null)
            {
                result.CustomerId = customer.CustomerId;
                result.Title = customer.Title;
                result.Description = customer.Description;
                await cDbContext.SaveChangesAsync();          
            }

            var data = await cDbContext.CleaningPlans.SingleOrDefaultAsync(c => c.Id == id);

            return data;
        }
    }
}
