using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;


namespace FMS.Data.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        
        // suitable vehicle properties/relationships

        public string Name {get; set; }
        //name of Person taking MOT

        public DateTime MotDate {get; set;} = DateTime.Now;
        //date of MOT
        public int mileage {get; set;}

        public string Status {get; set;} 
        
        public String Report {get; set;}

        //EF Dependant Relationship - Mot belongs to Vehicle
        //public int VehicleId {get; set;}

    

        




    }
}

    
