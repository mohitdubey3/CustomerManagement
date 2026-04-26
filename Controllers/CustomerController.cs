using Customer.App_Data;
using Customer.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace Customer.Controllers
{
    public class CustomerController : Controller
    {
        private readonly DemoEntities _context = new DemoEntities();

        public ActionResult Index()
        {
            var customers = _context.CustomerInfoes.OrderBy(c => c.Id).ToList();
            return View(customers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerInfoView request)
        {
            if (!ModelState.IsValid)
            {
                TempData["EMessage"] = "Please provide valid Name, Phone, and Email.";
                return RedirectToAction("Index");
            }

            var customer = new CustomerInfo
            {
                Name = request.Name.Trim(),
                Phone = request.Phone.Trim(),
                Email = request.Email.Trim()
            };

            _context.CustomerInfoes.Add(customer);
            await _context.SaveChangesAsync();
            TempData["SMessage"] = "Customer added successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, CustomerInfoView request)
        {
            if (!ModelState.IsValid)
            {
                TempData["EMessage"] = "Please provide valid Name, Phone, and Email.";
                return RedirectToAction("Index");
            }

            var customer = await _context.CustomerInfoes.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                TempData["EMessage"] = "Customer record not found.";
                return RedirectToAction("Index");
            }

            customer.Name = request.Name.Trim();
            customer.Phone = request.Phone.Trim();
            customer.Email = request.Email.Trim();

            await _context.SaveChangesAsync();
            TempData["SMessage"] = "Customer updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var customer = await _context.CustomerInfoes.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                TempData["EMessage"] = "Customer record not found.";
                return RedirectToAction("Index");
            }

            _context.CustomerInfoes.Remove(customer);
            await _context.SaveChangesAsync();
            TempData["SMessage"] = "Customer deleted successfully.";
            return RedirectToAction("Index");
        }

    }
}