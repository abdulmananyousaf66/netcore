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


    [Authorize(Roles = "PurchaseOrder")]
    public class PurchaseOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseOrder
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PurchaseOrder.Include(p => p.vendor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PurchaseOrder/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder
                    .Include(p => p.vendor)
                        .SingleOrDefaultAsync(m => m.purchaseOrderId == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }


        // GET: PurchaseOrder/Create
        public IActionResult Create()
        {
            PurchaseOrder po = new PurchaseOrder();
            ViewData["vendorId"] = new SelectList(_context.Vendor, "vendorId", "vendorName");
            return View(po);
        }




        // POST: PurchaseOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("purchaseOrderId,purchaseOrderNumber,top,poDate,deliveryDate,deliveryAddress,referenceNumberInternal,referenceNumberExternal,description,vendorId,picInternal,picVendor,purchaseOrderStatus,totalDiscountAmount,totalOrderAmount,purchaseReceiveNumber,HasChild,createdAt")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["vendorId"] = new SelectList(_context.Vendor, "vendorId", "vendorId", purchaseOrder.vendorId);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder.SingleOrDefaultAsync(m => m.purchaseOrderId == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }
            ViewData["vendorId"] = new SelectList(_context.Vendor, "vendorId", "vendorId", purchaseOrder.vendorId);
            return View(purchaseOrder);
        }

        // POST: PurchaseOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("purchaseOrderId,purchaseOrderNumber,top,poDate,deliveryDate,deliveryAddress,referenceNumberInternal,referenceNumberExternal,description,vendorId,picInternal,picVendor,purchaseOrderStatus,totalDiscountAmount,totalOrderAmount,purchaseReceiveNumber,HasChild,createdAt")] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.purchaseOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOrderExists(purchaseOrder.purchaseOrderId))
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
            ViewData["vendorId"] = new SelectList(_context.Vendor, "vendorId", "vendorId", purchaseOrder.vendorId);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder
                    .Include(p => p.vendor)
                    .SingleOrDefaultAsync(m => m.purchaseOrderId == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }




        // POST: PurchaseOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var purchaseOrder = await _context.PurchaseOrder.SingleOrDefaultAsync(m => m.purchaseOrderId == id);
            _context.PurchaseOrder.Remove(purchaseOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseOrderExists(string id)
        {
            return _context.PurchaseOrder.Any(e => e.purchaseOrderId == id);
        }

    }
}





namespace netcore.MVC
{
    public static partial class Pages
    {
        public static class PurchaseOrder
        {
            public const string Controller = "PurchaseOrder";
            public const string Action = "Index";
            public const string Role = "PurchaseOrder";
            public const string Url = "/PurchaseOrder/Index";
            public const string Name = "PurchaseOrder";
        }
    }
}
namespace netcore.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "PurchaseOrder")]
        public bool PurchaseOrderRole { get; set; } = false;
    }
}


