using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DesktopOrqApp.Models;

namespace DesktopOrqApp.Helper
{
    public class MapperData
    {
        public MapperData() { }

        public static WorkPositionModel MapTo(WorkPosition workPosition)
        {
            WorkPositionModel wpModel = new WorkPositionModel();
            wpModel.Id = workPosition.Id;
            wpModel.Name = workPosition.Name;
            wpModel.Description = workPosition.Description;
            wpModel.IsSelected = false;

            return wpModel;

        }

        public static WorkPosition MapFrom(WorkPositionModel wpModel)
        {
            WorkPosition wp = new WorkPosition();
            wp.Id = wpModel.Id;
            wp.Name = wpModel.Name;
            wp.Description = wpModel.Description;

            return wp;
        }

        public static List<WorkPositionModel> MapToAll(List<WorkPosition> list)
        {
            var data = new List<WorkPositionModel>();
            foreach (var item in list)
            {
                var wpModel = new WorkPositionModel();
                wpModel = MapTo(item);
                data.Add(wpModel);
            }
            return data;
        }
    }
}

