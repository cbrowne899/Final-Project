using System;
using System.Collections.Generic;
	
using FMS.Data.Models;
	
namespace FMS.Data.Services
{
    // This interface describes the operations that a FleetService class should implement
    public interface IFleetService
    {
        void Initialise();
        
        // add suitable method definitions to implement assignment requirements            
        List <Vehicle> GetVehicles();
        Vehicle GetVehicle(string reg);
        Vehicle AddVehicle(string reg, string make, string model, int year, 
                                    string fuelType, string bodyType, string transmissionType, 
                                        int doors, DateTime mot);

        Vehicle UpdateVehicle (Vehicle updated);
        // ------------- User Management -------------------
        User Authenticate(string email, string password);
        User Register(string name, string email, string password, Role role);
        User GetUserByEmail(string email);
    
    }
    
}