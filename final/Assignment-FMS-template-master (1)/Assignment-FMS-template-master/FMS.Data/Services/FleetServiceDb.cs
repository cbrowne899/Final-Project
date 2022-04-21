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
       
        // implement IFleetService methods here

        public List<Vehicle> GetVehicles()
        {
            return db.Vehicles.ToList();
        }

        //retrieve vehicle by registration
        public Vehicle GetVehicle(string reg)
        {
            return db.Vehicles.FirstOrDefault(v => v.Reg == reg);
        }

        public Vehicle AddVehicle(string reg, string make, string model, int year, 
                                    string fuelType, string bodyType, string transmissionType, 
                                        int doors, DateTime mot )
        
        {
            var exists = GetVehicle(reg);

            if (exists.Equals(null))
            {
            return null;
            }

            var vehicle = new Vehicle 
            {
                Reg=reg,
                Make=make,
                Model=model,
                Year=year,
                FuelType=fuelType,
                BodyType = bodyType,
                TransmissionType=transmissionType,
                Doors=doors,
                MotDue =mot
                
            };
            db.Vehicles.Add(vehicle); 
            //add vehicle to list 
            db.SaveChanges(); // save changes to database
            return vehicle;
        }

        public Vehicle UpdateVehicle (Vehicle updated)
        {
            var vehicle = GetVehicle(updated.Reg);

            if (vehicle.Equals(null))
            {
                return null;
            }
            vehicle.Reg= updated.Reg;
            vehicle.Make=updated.Make;
            vehicle.Model=updated.Model;
            vehicle.Year=updated.Year;
            vehicle.FuelType=updated.FuelType;
            vehicle.BodyType=updated.BodyType;
            vehicle.Doors=updated.Doors;
            vehicle.MotDue=updated.MotDue;

            db.SaveChanges();
            return vehicle;
        

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
