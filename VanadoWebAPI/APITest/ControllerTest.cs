using DAL.Entiteti;
using DAL;
using DAL.Servisi;
using VanadoWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace APITest
{
    public class ControllerTest
    {
        private static string conn = "Server=localhost;Database=VanadoDB;User Id=postgres;Password=user123;";
        private IDbServis dbservisa;
        private IStrojeviServis strojeviservis;
        private IKvaroviServis kvaroviServis;
        private StrojeviController controller1;
        private KvaroviController controller2;

        public ControllerTest()
        {
            dbservisa = new DbServis(conn);
            strojeviservis = new StrojeviServis(dbservisa);
            kvaroviServis = new KvaroviServis(dbservisa);
            controller1=new StrojeviController(strojeviservis);
            controller2=new KvaroviController(kvaroviServis);
        }
        [Fact]
        public void GetAllStrojeviTest()
        {
            var result = controller1.Get();
            //Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;

            Assert.IsType<List<StrojeviPrikaz>>(list.Value);

            var liststrojevi = list.Value as List<StrojeviPrikaz>;

            Assert.Equal(2, liststrojevi.Count);
        }

        [Fact]
        public void GetAllKvaroviTest()
        {
            var result = controller2.Get();
            //Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;

            Assert.IsType<List<Kvarovi>>(list.Value);

            var listkvarovi = list.Value as List<Kvarovi>;

            Assert.Equal(4, listkvarovi.Count);
        }

        [Fact]
        public void GetStrojByIdTest()
        {
            //Arrange
            var validid = 1;
            
            //Act
            
            var okResult = controller1.GetStroj(validid);
     
            var ok = okResult.Result as OkObjectResult;

            //Assert
          
            Assert.IsType<OkObjectResult>(ok);
 
            //We Expect to return a single stroj
            Assert.IsType<StrojeviPrikaz>(ok.Value);

          
        }
      

        [Fact]
        public void GetKvarByIdTest()
        {
            //Arrange
            var validid = 2;
           
            //Act         
            var okResult = controller2.GetKvar(validid);

            //Assert
           
            Assert.IsType<OkObjectResult>(okResult.Result);

            //Now we need to check the value of the result for the ok object result.
            var item = okResult.Result as OkObjectResult;

            //We Expect to return a single kvar
            Assert.IsType<Kvarovi>(item.Value);


        }
    }
}