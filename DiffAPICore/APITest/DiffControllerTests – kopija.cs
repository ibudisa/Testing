using System;
using Xunit;
using Moq;
using DiffAPICore;
using DiffAPICore.Models;
using DiffAPICore.Controllers;
using DAL.Repositorys;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Net;
using DAL;
using Xunit.Extensions.Ordering;
using Autofac.Extras.Moq;
using System.Collections.Generic;

namespace APITest
{
    [Order(1)]
    public class DiffControllerTests
    {
        private readonly Mock<IDataRepository> service;
        public DiffControllerTests()
        {
            service = new Mock<IDataRepository>();
        }

        [Fact, Order(2)]
        public void EmptyTables()
        {
            service.Setup(x => x.EmptyTables()).Returns(true);
            var repository = service.Object;

            var controller = new DiffController(repository);

            ActionResult actionResult = controller.Delete();
        
            actionResult.Should().BeOfType<OkResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);

        }
        [Fact, Order(3)]
        public void CreateLeftData_CreatedStatus_PassingLeftDataObjectToCreate()
        {
            var data = new DataModel();

            data.Data = "AAAAAA==";
            service.Setup(x => x.AddLeft(1, data.Data)).Returns(true);
            var controller = new DiffController(service.Object);
            
            ActionResult actionResult = controller.PostLeft(1, data);
            actionResult.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);

        }
        [Fact, Order(4)]
        public void GetDiffsLeftOnlyExists()
        {
            var data = new DiffInfo();
            service.Setup(x => x.GetByID(1)).Returns(data).Equals(null);
            var controller = new DiffController(service.Object);

            ActionResult<DiffInfo> actionResult = controller.GetByID(1);
            var status = actionResult.Result as StatusCodeResult;
            var statuscode = status.StatusCode;
            Assert.Equal(404, statuscode);
            actionResult.Should().BeOfType<ActionResult<DiffInfo>>();

        }
        [Fact, Order(5)]
        public void CreateRightData_CreatedStatus_PassingRightDataObjectToCreate()
        {
            var data = new DataModel();

            data.Data = "AAAAAA==";
            service.Setup(x => x.AddRight(1, data.Data)).Returns(true);
            var controller = new DiffController(service.Object);
          
            ActionResult actionResult = controller.PostRight(1, data);
            actionResult.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);

        }
        [Fact, Order(6)]
        public void GetDiffsLeftAndRightExist()
        {
            var info=new DiffInfo();
            info.DiffResultType = "Equals";
            service.Setup(x => x.GetByID(1)).Returns(info);
            var controller = new DiffController(service.Object);

            ActionResult<DiffInfo> actionResult = controller.GetByID(1);
            var content = actionResult.Value.DiffResultType;
            var status = actionResult.Result as StatusCodeResult;
            var statuscode = status.StatusCode;
            Assert.Equal("Equals", content);
            Assert.Equal(200, statuscode);
            actionResult.Should().BeOfType<ActionResult<DiffInfo>>().Which.Value.DiffResult.Count.Should().Be(0);


        }

        [Fact, Order(7)]
        public void UpdateRightData_UpdatedStatus_PassingRightDataObjectToUpdate()
        {
            var data = new DataModel();

            data.Data = "AQABAQ==";
            service.Setup(x => x.UpdateRight(1,data.Data)).Returns(true);
            var controller = new DiffController(service.Object);
            
            ActionResult actionResult = controller.PutRight(1, data);
            actionResult.Should().BeOfType<CreatedResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);

        }
        [Fact, Order(8)]
        public void GetDiffsLeftAndRightSameSizeDifferent()
        {
            var info = new DiffInfo();
            info.DiffResultType = "ContentDoNotMatch";
            List<Diff> list= new List<Diff>();
            var diff1=new Diff();
            diff1.Offset= 1;
            diff1.Length = 1;
            list.Add(diff1);
            var diff2=new Diff();
            diff2.Offset = 3;
            diff2.Length = 1;
            list.Add(diff2);
            Diff diff3=new Diff();
            diff3.Offset = 5;
            diff3.Length = 1;
            list.Add(diff3);
            info.DiffResult = list;

            service.Setup(x => x.GetByID(1)).Returns(info);
            var controller = new DiffController(service.Object);

            ActionResult<DiffInfo> actionResult = controller.GetByID(1);
            var status = actionResult.Result as StatusCodeResult;
            var statuscode = status.StatusCode;
            Assert.Equal(200, statuscode);
            var content = actionResult.Value.DiffResultType;
            Assert.Equal("ContentDoNotMatch", content);
            int diffnum = actionResult.Value.DiffResult.Count;
            Assert.Equal(3, diffnum);
            //actionResult.Should().BeOfType<OkResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);


        }
    }
}
