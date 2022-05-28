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
using Microsoft.EntityFrameworkCore;

namespace APITest
{
    [Order(1)]
    public class DiffControllerTests
    {
        private DataRepository repository;
        public static DbContextOptions<TestDBContext> dbContextOptions { get; }
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDB1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // initialize dbcontextoptions
        static DiffControllerTests()
        {
            dbContextOptions = new DbContextOptionsBuilder<TestDBContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        // setup datarepository
        public DiffControllerTests()
        {
            var context = new TestDBContext(dbContextOptions);
            DataComparer dataComparer = new DataComparer();
            repository = new DataRepository(context,dataComparer);
        }

        // empty tables

        [Fact, Order(2)]
        public void EmptyTables()
        {
            
            var controller = new DiffController(repository);

            ActionResult actionResult = controller.Delete();
        
            actionResult.Should().BeOfType<OkResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);

        }

        // create leftdata in database

        [Fact, Order(3)]
        public void CreateLeftData_CreatedStatus_PassingLeftDataObjectToCreate()
        {
            var data = new DataModel();

            data.Data = "AAAAAA==";
          
            var controller = new DiffController(repository);
            
            ActionResult actionResult = controller.PostLeft(1, data);
            actionResult.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);

        }

        // get differences when only leftdata exists in database

        [Fact, Order(4)]
        public void GetDiffsLeftOnlyExists()
        {
            var controller = new DiffController(repository);

            ActionResult<DiffInfo> actionResult = controller.GetByID(1);
            var status = actionResult.Result as NotFoundResult;
            var statuscode = status.StatusCode;
            Assert.Equal(404, statuscode);
            actionResult.Should().BeOfType<ActionResult<DiffInfo>>();

        }

        // create rightdata in database

        [Fact, Order(5)]
        public void CreateRightData_CreatedStatus_PassingRightDataObjectToCreate()
        {
            var data = new DataModel();

            data.Data = "AAAAAA==";
           
            var controller = new DiffController(repository);
          
            ActionResult actionResult = controller.PostRight(1, data);
            actionResult.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);

        }

        // get differences when both leftdata and right data exist in database and are the same

        [Fact, Order(6)]
        public void GetDiffsLeftAndRightExist()
        {
            
            var controller = new DiffController(repository);

            ActionResult<DiffInfo> actionResult = controller.GetByID(1);
           
            var status = actionResult.Result as OkObjectResult;
            var content = status.Value as DiffInfo;
            var data = content.DiffResultType;
            var statuscode = status.StatusCode;
            Assert.Equal("Equals", data);
            Assert.Equal(200, statuscode);
            //actionResult.Should().BeOfType<ActionResult<DiffInfo>>().Which.Value.DiffResult.Count.Should().Be(0);


        }


        // update rightdata by id 1 with object

        [Fact, Order(7)]
        public void UpdateRightData_UpdatedStatus_PassingRightDataObjectToUpdate()
        {
            var data = new DataModel();

            data.Data = "AQABAQ==";
            var controller = new DiffController(repository);

            ActionResult actionResult = controller.PutRight(1, data);
            actionResult.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);

        }

        // getting difference between leftdata and rightdata that are not equal

        [Fact, Order(8)]
        public void GetDiffsLeftAndRightSameSizeDifferent()
        {
            //var info = new DiffInfo();
            //info.DiffResultType = "ContentDoNotMatch";
            //List<Diff> list= new List<Diff>();
            //var diff1=new Diff();
            //diff1.Offset= 1;
            //diff1.Length = 1;
            //list.Add(diff1);
            //var diff2=new Diff();
            //diff2.Offset = 3;
            //diff2.Length = 1;
            //list.Add(diff2);
            //Diff diff3=new Diff();
            //diff3.Offset = 5;
            //diff3.Length = 1;
            //list.Add(diff3);
            //info.DiffResult = list;

            //service.Setup(x => x.GetByID(1)).Returns(info);
            var controller = new DiffController(repository);

            ActionResult<DiffInfo> actionResult = controller.GetByID(1);
            var status = actionResult.Result as OkObjectResult;
            var statuscode = status.StatusCode;
            Assert.Equal(200, statuscode);
            var content = status.Value as DiffInfo;
            var data = content.DiffResultType;
            Assert.Equal("ContentDoNotMatch", data);
            int diffnum = content.DiffResult.Count;
            Assert.Equal(3, diffnum);
            //actionResult.Should().BeOfType<OkResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);


        }
    }
}
