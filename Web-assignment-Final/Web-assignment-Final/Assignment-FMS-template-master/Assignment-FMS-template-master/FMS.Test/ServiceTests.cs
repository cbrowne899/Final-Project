
using Xunit;
using FMS.Data.Models;
using FMS.Data.Services;

namespace FMS.Test
{

    public class ServiceTests
    {
        private readonly IFleetService svc;


        public ServiceTests()
        {
            // general arrangement
            svc = new FleetServiceDb();
          
            // ensure data source is empty before each test
            svc.Initialise();
        }

        // ========================== Fleet Tests =========================


        [Fact]//Test to get all the vehicles
        public void Vehicle_GetVehiclesWhenNone_ShouldReturnNone()
        {

            //act
            var vehicles = svc.GetVehicles();
            var count = vehicles.Count;

            //assert
            Assert.Equal(0, count);

        }


        [Fact]
        public void Vehicle_AddVehicle_WhenNone_ShouldSetAllProperties()
        {
            // act 
            
            var vehicletest= new Vehicle
            {
            Reg="SB16 ERG", 
            Make="Audi", 
            Model="Sport", 
            Year= 2015, 
            FuelType="Petrol", 
            BodyType= "Hatchback", 
            TransmissionType="Automatic", 
            Doors= 4,
            MotDue= System.DateTime.Today,
            //PhotoUrl="https://image.shutterstock.com/image-photo/hong-kong-china-april-30-600w-191257217.jpg;"
           
            };
            svc.AddVehicle(vehicletest.Reg, vehicletest.Make, vehicletest.Model, vehicletest.Year, vehicletest.FuelType, vehicletest.BodyType, vehicletest.TransmissionType, vehicletest.Doors, vehicletest.MotDue);
            
            // retrieve vehicle just added by using the Id returned by EF
            var v = svc.GetVehicle(vehicletest.VehicleId);

            // assert - that vehicle is not null
            Assert.NotNull(v);
            
            // now assert that the properties were set properly
            Assert.Equal(vehicletest.VehicleId, v.VehicleId);
            Assert.Equal(vehicletest.Make, v.Make);
            Assert.Equal(vehicletest.Model, v.Model);
            Assert.Equal(vehicletest.Year, v.Year);
            Assert.Equal(vehicletest.FuelType, v.FuelType);
            Assert.Equal(vehicletest.BodyType, v.BodyType);
            Assert.Equal(vehicletest.TransmissionType, v.TransmissionType);
            Assert.Equal(vehicletest.Doors, v.Doors);
            Assert.Equal(vehicletest.MotDue, v.MotDue);
        }
        
        [Fact] 
        public void Vehicle_AddVehicle_WhenDuplicateReg_ShouldReturnNull()
        {
            // act 
            var v1 = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
            // this is a duplicate as the registration is same as previous vehicle
            var v2 = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
            
            // assert
            Assert.NotNull(v1); // this vehicle should have been added correctly
            Assert.Null(v2); // this vehicle should NOT have been added        
        }

        [Fact]//test to update an exisiting vehicle
        public void Vehicle_UpdateWhenExists_ShouldSetAllProperties()
        {

            //arrane - create test vehicle
            var vehicletest= new Vehicle
            {
            Reg="SB16 ERG", 
            Make="Audi", 
            Model="Sport", 
            Year= 2015, 
            FuelType="Petrol", 
            BodyType= "Hatchback", 
            TransmissionType="Automatic", 
            Doors= 4,
            MotDue= System.DateTime.Today,
           //PhotoUrl= "https://image.shutterstock.com/image-photo/hong-kong-china-april-30-600w-191257217.jpg"

            };
            
            svc.AddVehicle(vehicletest.Reg, vehicletest.Make, vehicletest.Model, vehicletest.Year, vehicletest.FuelType, vehicletest.BodyType, vehicletest.TransmissionType, vehicletest.Doors, vehicletest.MotDue);

            //act - update test vehicle 

            vehicletest.Reg= "NM1 0YU";
            vehicletest.Make = "Mercedes";
            vehicletest.Model= "C-Class";
            vehicletest.Year = 2021;
            vehicletest.FuelType= "Diesel";
            vehicletest.BodyType="Sedan";
            vehicletest.TransmissionType= "Manual";
            vehicletest.Doors= 5;
            vehicletest.MotDue= System.DateTime.Now;
            //vehicletest.PhotoUrl= "https://image.shutterstock.com/image-photo/istanbul-march-09-2019-mercedesbenz-600w-1333364855.jpg";

            var updated = svc.UpdateVehicle(vehicletest);
            var vehicle = svc.GetVehicle(vehicletest.VehicleId);

            //assert- will return true if vehicle has been updated and Registration is equal


            Assert.Equal(vehicle.Reg, vehicletest.Reg);
        }

        [Fact]//test to add a vehicle which has a duplicate reg
        public void Vehicle_AddVehicleWhenDuplicateReg_ShouldReturnNull()
        {
             //arrange - create test vehicle
            var vehicletest= new Vehicle
            {
            Reg="SB16 ERG", 
            Make="Audi", 
            Model="Sport", 
            Year= 2015, 
            FuelType="Petrol", 
            BodyType= "Hatchback", 
            TransmissionType="Automatic", 
            Doors= 4, 
            MotDue= System.DateTime.Now,
            //PhotoUrl="https://image.shutterstock.com/image-photo/hong-kong-china-april-30-600w-191257217.jpg"
            };
            var test1= svc.AddVehicle(vehicletest.Reg, vehicletest.Make, vehicletest.Model, vehicletest.Year, vehicletest.FuelType, vehicletest.BodyType, vehicletest.TransmissionType, vehicletest.Doors, vehicletest.MotDue);

            //act - create and add duplicate vehicle
            var vehicletest2 = new Vehicle
             {
            Reg="SB16 ERG", 
            Make="Audi", 
            Model="Sport", 
            Year= 2015, 
            FuelType="Petrol", 
            BodyType= "Hatchback", 
            TransmissionType="Automatic", 
            Doors= 4,
            MotDue = System.DateTime.Now,
           //PhotoUrl ="https://image.shutterstock.com/image-photo/hong-kong-china-april-30-600w-191257217.jpg"

            };
            var test2 = svc.AddVehicle(vehicletest.Reg, vehicletest.Make, vehicletest.Model, vehicletest.Year, vehicletest.FuelType, vehicletest.BodyType, vehicletest.TransmissionType, vehicletest.Doors, vehicletest.MotDue);

            //assert
            Assert.NotNull(test1);
            Assert.Null(test2);
        }

        [Fact]
        public void Student_GetStudents_When2Exist_ShouldReturn2()
        {
            // arrange
            var v1 = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
            var v2= svc.AddVehicle("CDE 321", "Merc", "Benz", 2019, "Diesel", "Motorcar", "Automatic", 4, System.DateTime.Now);

            // act
            var vehicle = svc.GetVehicles();
            var count = vehicle.Count;

            // assert
            Assert.Equal(2, count);
        }

        [Fact] 
        public void Vehicle_GetVehicle_WhenNonExistent_ShouldReturnNull()
        {
            // act 
            var vehicle = svc.GetVehicle(1); // non existent vehicle

            // assert
            Assert.Null(vehicle);
        }

        [Fact] 
        public void Vehicle_GetVehicle_ThatExists_ShouldReturnVehicle()
        {
            // act 
            
            var v1 = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
            var v = svc.GetVehicle(v1.VehicleId);

            // assert
            Assert.NotNull(v);
            Assert.Equal(v1.VehicleId, v.VehicleId);
        }

        [Fact] 
        public void Vehicle_GetVehicle_ThatExistsWithMots_ShouldReturnVehicleWithMots()
        {
            // arrange 

            var v1 = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
            svc.CreateMot(v1.VehicleId, "Chloe", 15000, "Pass", "Passed All Tests");
            svc.CreateMot(v1.VehicleId, "James", 18000, "Fail", "No BreakLights");
            
            // act
            var vehicle = svc.GetVehicle(v1.VehicleId);

            // assert
            Assert.NotNull(v1);    
            Assert.Equal(2, vehicle.Mots.Count);
        }

        [Fact]
        public void Vehicle_DeleteVehicle_ThatExists_ShouldReturnTrue()
        {
            // act 
            var v = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
            var deleted = svc.DeleteVehicle(v.VehicleId);

            // try to retrieve deleted vehicle
            var v1 = svc.GetVehicle(v.VehicleId);

            // assert
            Assert.True(deleted); // delete vehicle should return true
            Assert.Null(v1);      // s1 should be null
        }

          [Fact]
        public void Vehicle_DeleteVehicle_ThatDoesntExist_ShouldReturnFalse()
        {
            // act 	
            var deleted = svc.DeleteVehicle(0);

            // assert
            Assert.False(deleted);
        } 



        // ---------------------- MOT Tests ------------------------

          [Fact] 
        public void Mot_CreateMot_ForExistingVehicle_ShouldBeCreated()
        {
            // arrange
            
           var v1 = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
         
            // act
            var m1= svc.CreateMot(v1.VehicleId, "Chloe", 15000, "Pass", "Passed All Tests");
           
            // assert
            Assert.NotNull(m1);
            Assert.Equal(v1.VehicleId, m1.VehicleId);
         
        }

        [Fact] // --- GetMot ById
        public void Mot_GetMotById_WhenExists_ShouldReturnMottAndVehicle()
        {
            // arrange
            var v1 = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
         
            var m1= svc.CreateMot(v1.VehicleId, "Chloe", 15000, "Pass", "Passed All Tests");
            // act
            var mot = svc.GetMotById(m1.VehicleId);

            // assert
            Assert.NotNull(mot);
            Assert.NotNull(mot.Vehicle);
            Assert.Equal(v1.Reg, mot.Vehicle.Reg); 
        }

        [Fact] // --- GetAll Tickets When two added should return two 
        public void Mot_GetAllMots_WhenTwoAdded_ShouldReturnTwo()
        {
            // arrange
            var m = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
            svc.CreateMot(m.VehicleId, "Chloe", 15000, "Pass", "Passed All Tests");
            svc.CreateMot(m.VehicleId, "Chloe", 15000, "Fail", "Faulty Breaks");

            // act
            var allmots = svc.GetAllMots();

            // assert
            Assert.Equal(2,allmots.Count);                        
        }

        [Fact] 
        public void Mot_DeleteMot_WhenExists_ShouldReturnTrue()
        {
            // arrange
            var v1 = svc.AddVehicle("ABC 123", "Audi", "A4", 2017, "Diesel", "Motorcar", "Manual", 4, System.DateTime.Now);
            var m1 = svc.CreateMot(v1.VehicleId, "Chloe", 15000, "Pass", "Passed All Tests");

            // act
            var deleted = svc.DeleteMot(m1.VehicleId);     // delete mot   
            
            // assert
            Assert.True(deleted);                    // mot should be deleted
        }   

        [Fact] 
        public void Mot_DeleteMot_WhenNonExistant_ShouldReturnFalse()
        {
            // arrange
           
            // act
            var deleted = svc.DeleteMot(1);     // delete non-existent mot   
            
            // assert
            Assert.False(deleted);                  // mot should not be deleted
        }  

        //  =================  User Tests ===========================
        
        [Fact] // --- Register Valid User test
        public void User_Register_WhenValid_ShouldReturnUser()
        {
            // arrange 
            var reg = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
            
            // act
            var user = svc.GetUserByEmail(reg.Email);
            
            // assert
            Assert.NotNull(reg);
            Assert.NotNull(user);
        } 

        [Fact] // --- Register Duplicate Test
        public void User_Register_WhenDuplicateEmail_ShouldReturnNull()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
            
            // act
            var s2 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);

            // assert
            Assert.NotNull(s1);
            Assert.Null(s2);
        } 

        [Fact] // --- Authenticate Invalid Test
        public void User_Authenticate_WhenInValidCredentials_ShouldReturnNull()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "guest");
            // assert
            Assert.Null(user);

        } 

        [Fact] // --- Authenticate Valid Test
        public void User_Authenticate_WhenValidCredentials_ShouldReturnUser()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "admin");
            
            // assert
            Assert.NotNull(user);
        } 

        

}
}