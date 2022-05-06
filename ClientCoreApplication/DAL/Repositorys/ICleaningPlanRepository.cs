using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositorys
{
    public interface ICleaningPlanRepository
    {
        Task<IEnumerable<CleaningPlan>> GetCleaningPlansByCustomerId(int customerId);
        Task<CleaningPlan> GetCleaningPlanById(Guid id);
        Task<CleaningPlan> AddCleaningPlan(Customer customer);
        Task<CleaningPlan> UpdateCleaningPlan(Customer customer,Guid id);
        Task<CleaningPlan> DeleteCleaningPlan(Guid id);
    }
}
