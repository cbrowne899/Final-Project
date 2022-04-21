using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using FMS.Data.Models;
using FMS.Data.Repository;
using FMS.Data.Security;

namespace FMS.Data.Services
{
    public class FleetServiceDb : IFleetService
    {
        private readonly DataContext db;

        public FleetServiceDb()
        {
            db = new DataContext();
        }

        public void Initialise()
        {
            db.Initialise(); // recreate database
        }

        // ==================== Fleet Management ==================
       
        public IList<Vehicle> GetAllVehicles()
        {
            return db.Vehicles.ToList();
        }

        public Vehicle GetVehicleById(int id)
        {
            return db.Vehicles.FirstOrDefault(v=> v.Id == id);
        }

        Vehicle GetVehicleByReg(string reg)
        {
            return db.Vehicles.FirstOrDefault(v=> v.Reg ==reg);
        }


        Vehicle AddVehicle(Vehicle v1)
        {
            var exists = GetVehicleByReg(v1.Reg);
            if (exists !=null)
            {
                return null;
            }

            var vehicle1 = new Vehicle
            {
                Reg=v1.Reg,
                Make=v1.Make,
                Model=v1.Model,
                Year=v1.Year,
                FuelType=v1.FuelType,
                BodyType = v1.BodyType,
                TransmissionType=v1.TransmissionType,
                Doors=v1.Doors,
                MotDue =v1.MotDue
            };
            db.Vehicles.Add(vehicle1);
            db.SaveChanges();
            return vehicle1;
            
        }











        // ==================== User Authentication/Registration Management ==================
        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
        }

        public User Register(string name, string email, string password, Role role)
        {
            // check that the user does not already exist (unique user name)
            var exists = GetUserByEmail(email);
            if (exists != null)
            {
                return null;
            }

            // Custom Hasher used to encrypt the password before storing in database
            var user = new User 
            {
                Name = name,
                Email = email,
                Password = Hasher.CalculateHash(password),
                Role = role   
            };
   
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }

        public User GetUserByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

    }
}
