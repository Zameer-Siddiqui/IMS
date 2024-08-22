using System;
using System.Collections.Generic;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImsContext _context;

        public HomeController(ImsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Generate_Invoice()
        {
            var customers = await _context.Customers.ToListAsync();
            ViewBag.Customers = new SelectList(customers, "Id", "Name");

            var products = await _context.Products.ToListAsync();
            ViewBag.Products = new SelectList(products, "Id", "Name");
            ViewBag.ProductsData = products; // Pass the products data to the view

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice(Invoice invoice, string invoiceItemsJson)
        {
            if (ModelState.IsValid)
            {
                // Deserialize JSON to List<InvoiceList>
                var invoiceItems = JsonConvert.DeserializeObject<List<InvoiceList>>(invoiceItemsJson);

                // Calculate the total discount from invoice items
                decimal totalDiscount = 0;
                foreach (var item in invoiceItems)
                {
                    totalDiscount += item.Discount ?? 0;
                }
                invoice.TotalDiscount = totalDiscount;

                // Add Invoice to the database
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync(); // Save changes to get the invoice ID

                // Add InvoiceItems to the database
                foreach (var item in invoiceItems)
                {
                    item.InvoiceId = invoice.Id; // Link with Invoice Id
                    _context.InvoiceLists.Add(item); // Add InvoiceList item with correct Name
                }

                await _context.SaveChangesAsync(); // Save changes after adding invoice items

                return RedirectToAction("Generate_Invoice");
            }

            // Reload the view with error messages
            ViewBag.Customers = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name");
            ViewBag.ProductsData = await _context.Products.Select(p => new { p.Id, p.Name, p.Price }).ToListAsync();
            return View("Generate_Invoice", invoice);
        }
    }
}
