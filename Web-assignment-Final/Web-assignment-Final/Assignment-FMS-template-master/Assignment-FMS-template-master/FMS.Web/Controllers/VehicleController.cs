using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using FMS.Data.Models;
using FMS.Data.Services;

using FMS.Web.Models;

namespace FMS.Web.Controllers;
//[Authorize]
public class VehicleController : BaseController
{

    private IFleetService _svc;

    public VehicleController()
    {
        _svc = new FleetServiceDb();
    }

    public IActionResult Index(VehicleSearchViewModel v )
    {       
            // set the viewmodel Mot property by calling service method 
            // using the range and query values from the viewmodel 
             v.Vehicles= _svc.SearchVehicles(v.Query);
             
             return View (v);
    }

// GET /vehicle/details/{id}
    public IActionResult Details(int id)
    {
    // retrieve the vehicle with specifed id from the service
    var v = _svc.GetVehicle(id);
    
 // TBC check if v is null
    if (v == null)
    {
        Alert ("Vehicle was not found!", AlertType.warning);
        return RedirectToAction(nameof(Index));
    }
 // pass vehicle as parameter to the view
    return View(v);
 }
   //[Authorize(Roles = "admin")]
 // GET: /vehicle/create
     public IActionResult Create()
 {
    // display blank form to create a vehicle

    var v = new Vehicle();
    return View(v);
 }


    // POST /vehicle/create
   [HttpPost]
   [ValidateAntiForgeryToken]
   //[Authorize(Roles = "admin")]
    public IActionResult Create(Vehicle v)
      {
     // complete POST action to add vehicle

    if (ModelState.IsValid)
    {
     // TBC pass data to service to store
   
     _svc.AddVehicle(v.Reg, v.Make, v.Model, v.Year, v.FuelType, v.BodyType,v.TransmissionType, v.Doors, v.MotDue);
     Alert ("A new vehicle was added!", AlertType.success);
      }
  // redisplay the form for editing as there are validation errors
     return View(v);
 }


    //GET /vehicle/edit/{id}
     public IActionResult Edit(int id)
  {
  // load the vehicle using the service
     var v = _svc.GetVehicle(id);
  // TBC check if v is null and return NotFound()
     if (v == null)
  {   
     Alert("Vehicle was not found!", AlertType.warning);
      return NotFound();
  }
  // pass student to view for editing
     return View(v);
 }
    

 // POST /vehicle/edit/{id}
     [HttpPost]
     public IActionResult Edit(int id, Vehicle v)
  {
 // complete POST action to save vehicle changes
  if (ModelState.IsValid)
  {
  // TBC pass data to service to update
    _svc.UpdateVehicle(v);
    Alert("The Vehicle has been updated!", AlertType.info);
    return RedirectToAction(nameof(Index));
     }
 // redisplay the form for editing as validation errors
    return View(v);
  }
 
     //GET / vehicle/delete/{id}
     //[Authorize(Roles = "admin")]
    public IActionResult Delete(int id)
    {
 // load the vehicle using the service
      var v = _svc.GetVehicle(id);
 // check the returned vehicle is not null and if so return   NotFound 
    
    if (v == null)
    {
       Alert("Vehicle not found!", AlertType.warning);
        return NotFound();
    }
 // pass vehicle to view for deletion confirmation
        return View(v);
    }


 
 // POST /vehicle/delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
   //[Authorize(Roles = "admin")]
    public IActionResult DeleteConfirm(int id)
    {
 // TBC delete vehicle via service
    var v1 = _svc.GetVehicle(id);
    _svc.DeleteVehicle(id);
   
    Alert ($"Vehicle {v1.VehicleId} was deleted!", AlertType.danger);
 // redirect to the index view
    return RedirectToAction(nameof(Index));
    }



// ============== Vehicle MOT management ==============

//[Authorize(Roles = "admin, manager")]
public IActionResult CreateMot(int id)
        {
            var v = _svc.GetVehicle(id);
             if (v == null)
             {
               Alert("No vehicle was found -- MOT Not Created!", AlertType.warning);
                return NotFound();
             }

            // create a mot view model and set foreign key
            var mot = new MotCreateViewModel {VehicleId= id};

            

            // render blank form
            return View(mot);
        }

// POST /vehicle/create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "admin, manager")]
        public IActionResult CreateMot([Bind("VehicleId, Name, mileage, Status, Report")] MotCreateViewModel m)
        {
            if (ModelState.IsValid)
            {                
               
               var mot = _svc.CreateMot(m.VehicleId, m.Name, m.mileage, m.Status, m.Report);
               mot.MotDate= DateTime.Now.AddMonths(12);
               
               Alert ("New Mot History for Vehicle {m.VehicleId} has been added", AlertType.success);
               

               return RedirectToAction(nameof(Details), new { VehicleId = mot.VehicleId });
            }
            // redisplay the form for editing
            return View(m);
        }

        // [Authorize(Roles = "admin,manager")]
        public IActionResult MotDelete(int id)
        {
            // load the mot using the service
            var mot = _svc.GetMotById(id);
            // check the returned mot is not null and if so return NotFound()
            if (mot != null)
            {
                //Alert ("Vehicle {mot.VehicleId} has been deleted", AlertType.warning);
               return RedirectToAction(nameof(Index));
            }     
            
            // pass mot to view for deletion confirmation
            return View(mot);
        }

        // POST /vehicle/Motdeleteconfirm/{id}
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "admin,manager")]

        public IActionResult MotDeleteConfirm(int vehicleId)
        {
            // TBC delete mot via service
            var vehicle = _svc.GetVehicle(vehicleId);
            var mot = _svc.GetMotById(vehicle.VehicleId);

            _svc.DeleteMot(vehicleId);
            
            // TBC update to redirect to the vehicle details page
            return RedirectToAction(nameof(Details), mot);
        }











 }









