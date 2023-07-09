using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hotel.PortalWWW.Controllers
{
    public class BaseController : Controller
    {
        protected readonly HotelContext _context;

        public BaseController(HotelContext context)
        {
            _context = context;


           


        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var layout = _context.Layout.FirstOrDefault();
            var logoURL = layout?.LogoUrl;
            ViewBag.LogoURL = logoURL;

            base.OnActionExecuting(filterContext);
        }
    }
}
