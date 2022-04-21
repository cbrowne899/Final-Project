using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FMS.Data.Models;
using FMS.Data.Services;

using FMS.Web.Models;

namespace FMS.Web.Controllers;

public class VehicleController : Controller
{
    // provide suitable controller actions

    private IFleetService svc;

    public VehicleController()
    {
        svc = new FleetServiceDb();
    }

    public IActionResult Index ()
    {
        var v1 = svc.GetAllVehicles();
        return View (v1);
    }

}