using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using FMS.Data.Models;


namespace FMS.Web.Models
{   
    public class VehicleSearchViewModel
    {
        // result set
        public IList<Vehicle> Vehicles { get; set;} = new List<Vehicle>();

        // search options        
        public string Query { get; set; } = "";
    }
}
