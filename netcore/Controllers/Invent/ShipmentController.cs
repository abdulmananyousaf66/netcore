﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

using netcore.Data;
using netcore.Models.Invent;

namespace netcore.Controllers.Invent
{


    [Authorize(Roles = "Shipment")]
    public class ShipmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shipment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Shipment.Include(s => s.branch).Include(s => s.customer).Include(s => s.salesOrder).Include(s => s.warehouse);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Shipment/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipment
                    .Include(s => s.branch)
                    .Include(s => s.customer)
                    .Include(s => s.salesOrder)
                    .Include(s => s.warehouse)
                        .SingleOrDefaultAsync(m => m.shipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }


        // GET: Shipment/Create
        public IActionResult Create()
        {
            ViewData["branchId"] = new SelectList(_context.Branch, "branchId", "branchName");
            ViewData["customerId"] = new SelectList(_context.Customer, "customerId", "customerName");
            ViewData["salesOrderId"] = new SelectList(_context.SalesOrder, "salesOrderId", "salesOrderNumber");
            ViewData["warehouseId"] = new SelectList(_context.Warehouse, "warehouseId", "warehouseName");
            Shipment shipment = new Shipment();
            return View(shipment);
        }




        // POST: Shipment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("shipmentId,salesOrderId,shipmentNumber,shipmentDate,customerId,customerPO,invoice,branchId,warehouseId,expeditionType,expeditionMode,HasChild,createdAt")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = shipment.shipmentId });
            }
            ViewData["branchId"] = new SelectList(_context.Branch, "branchId", "branchName", shipment.branchId);
            ViewData["customerId"] = new SelectList(_context.Customer, "customerId", "customerName", shipment.customerId);
            ViewData["salesOrderId"] = new SelectList(_context.SalesOrder, "salesOrderId", "salesOrderNumber", shipment.salesOrderId);
            ViewData["warehouseId"] = new SelectList(_context.Warehouse, "warehouseId", "warehouseName", shipment.warehouseId);
            return View(shipment);
        }

        // GET: Shipment/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipment.SingleOrDefaultAsync(m => m.shipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }
            ViewData["branchId"] = new SelectList(_context.Branch, "branchId", "branchName", shipment.branchId);
            ViewData["customerId"] = new SelectList(_context.Customer, "customerId", "customerName", shipment.customerId);
            ViewData["salesOrderId"] = new SelectList(_context.SalesOrder, "salesOrderId", "salesOrderNumber", shipment.salesOrderId);
            ViewData["warehouseId"] = new SelectList(_context.Warehouse, "warehouseId", "warehouseName", shipment.warehouseId);
            return View(shipment);
        }

        // POST: Shipment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("shipmentId,salesOrderId,shipmentNumber,shipmentDate,customerId,customerPO,invoice,branchId,warehouseId,expeditionType,expeditionMode,HasChild,createdAt")] Shipment shipment)
        {
            if (id != shipment.shipmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentExists(shipment.shipmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["branchId"] = new SelectList(_context.Branch, "branchId", "branchName", shipment.branchId);
            ViewData["customerId"] = new SelectList(_context.Customer, "customerId", "customerName", shipment.customerId);
            ViewData["salesOrderId"] = new SelectList(_context.SalesOrder, "salesOrderId", "salesOrderNumber", shipment.salesOrderId);
            ViewData["warehouseId"] = new SelectList(_context.Warehouse, "warehouseId", "warehouseName", shipment.warehouseId);
            return View(shipment);
        }

        // GET: Shipment/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipment
                    .Include(s => s.branch)
                    .Include(s => s.customer)
                    .Include(s => s.salesOrder)
                    .Include(s => s.warehouse)
                    .SingleOrDefaultAsync(m => m.shipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }




        // POST: Shipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var shipment = await _context.Shipment.SingleOrDefaultAsync(m => m.shipmentId == id);
            try
            {
                _context.Shipment.Remove(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(shipment);
            }
            
        }

        private bool ShipmentExists(string id)
        {
            return _context.Shipment.Any(e => e.shipmentId == id);
        }

    }
}





namespace netcore.MVC
{
    public static partial class Pages
    {
        public static class Shipment
        {
            public const string Controller = "Shipment";
            public const string Action = "Index";
            public const string Role = "Shipment";
            public const string Url = "/Shipment/Index";
            public const string Name = "Shipment";
        }
    }
}
namespace netcore.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "Shipment")]
        public bool ShipmentRole { get; set; } = false;
    }
}



