using System.Linq;
using System.Web.Mvc;
using ChicagoInABox.Models;
using ChicagoInABox.Models.ViewModel;
namespace ChicagoInABox.Controllers
{
    public class AssemblingBoxController : Controller
    {
        ChicagoInABoxEntities db = new ChicagoInABoxEntities();
        //
        // GET: /AssemblingBox/
        public ActionResult Index()
        {
            // Set up our ViewModel

            // Return the view
            return View();
        }
    }
}