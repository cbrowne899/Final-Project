using System;
using System.Collections.Generic;
	
using FMS.Data.Models;
	
namespace FMS.Data.Services
{
    // This interface describes the operations that a FleetService class should implement
    public interface IFleetService
    {
        void Initialise();

        // ------------- Vehicle Management -------------------
        
        IList<Vehicle> GetAllVehicles();
        Vehicle GetVehicleById(int id);
        Vehicle GetVehicleByReg(string reg);
        Vehicle AddVehicle(Vehicle v);
                  
    

        // ------------- User Management -------------------
        User Authenticate(string email, string password);
        User Register(string name, string email, string password, Role role);
        User GetUserByEmail(string email);
    
    }
    
}