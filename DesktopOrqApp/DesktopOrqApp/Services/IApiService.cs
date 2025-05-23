using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace DesktopOrqApp.Services
{
    public interface IApiService
    {
        [Get("/workposition/GetAll")]
        Task<ApiResponse<List<WorkPosition>>> GetPositions();

        [Get("/UWPositions/Login/{email}/{password}")]
        Task<ApiResponse<DataPosition>> GetUserRoleData(string email, string password);

        [Get("/UWPositions/Get")]
        Task<ApiResponse<List<User_WorkPosition>>> GetUserWorkPositions();

        [Get("/workposition/GetById/{id}")]
        Task<ApiResponse<WorkPosition>> GetWorkPositionById(int id);

        [Post("/workposition/Add")]
        Task<ApiResponse<bool>> AddWorkPosition(WorkPosition wposition);

        [Post("/UWPositions/Add")]
        Task<ApiResponse<bool>> AddUserWorkPosition(User_WorkPosition wposition);

        [Put("/workposition/Update")]
        Task<ApiResponse<bool>> UpdateWorkPosition(WorkPosition position);

        [Delete("/workposition/Delete/{id}")]
        Task<ApiResponse<bool>> DeleteWorkPosition(int id);

        [Get("/workposition/GetMaxId")]
        Task<ApiResponse<int>> GetWorkPositionMaxId();

        [Get("/UWPositions/GetUWMaxId")]
        Task<ApiResponse<int>> GetUserWorkPositionMaxId();

    }
}
