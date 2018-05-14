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


    [Authorize(Roles = "Receiving")]
    public class ReceivingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceivingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Receiving
        public async Task<IActionResult> Index()
        {
                    var applicationDbContext = _context.Receiving.Include(r => r.branch).Include(r => r.purchaseOrder).Include(r => r.vendor).Include(r => r.warehouse);
                    return View(await applicationDbContext.ToListAsync());
        }        

    // GET: Receiving/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var receiving = await _context.Receiving
                .Include(r => r.branch)
                .Include(r => r.purchaseOrder)
                .Include(r => r.vendor)
                .Include(r => r.warehouse)
                    .SingleOrDefaultAsync(m => m.receivingId == id);
        if (receiving == null)
        {
            return NotFound();
        }

        return View(receiving);
    }


    // GET: Receiving/Create
    public IActionResult Create()
    {
        ViewData["branchId"] = new SelectList(_context.Branch, "branchId", "branchId");
        ViewData["purchaseOrderId"] = new SelectList(_context.PurchaseOrder, "purchaseOrderId", "purchaseOrderId");
        ViewData["vendorId"] = new SelectList(_context.Vendor, "vendorId", "vendorId");
        ViewData["warehouseId"] = new SelectList(_context.Warehouse, "warehouseId", "warehouseId");
    return View();
    }




    // POST: Receiving/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("receivingId,purchaseOrderId,receivingNumber,receivingDate,vendorId,vendorDO,vendorInvoice,branchId,warehouseId,HasChild,createdAt")] Receiving receiving)
    {
        if (ModelState.IsValid)
        {
            _context.Add(receiving);
            await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
                ViewData["branchId"] = new SelectList(_context.Branch, "branchId", "branchId", receiving.branchId);
                ViewData["purchaseOrderId"] = new SelectList(_context.PurchaseOrder, "purchaseOrderId", "purchaseOrderId", receiving.purchaseOrderId);
                ViewData["vendorId"] = new SelectList(_context.Vendor, "vendorId", "vendorId", receiving.vendorId);
                ViewData["warehouseId"] = new SelectList(_context.Warehouse, "warehouseId", "warehouseId", receiving.warehouseId);
        return View(receiving);
    }

    // GET: Receiving/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var receiving = await _context.Receiving.SingleOrDefaultAsync(m => m.receivingId == id);
        if (receiving == null)
        {
            return NotFound();
        }
                ViewData["branchId"] = new SelectList(_context.Branch, "branchId", "branchId", receiving.branchId);
                ViewData["purchaseOrderId"] = new SelectList(_context.PurchaseOrder, "purchaseOrderId", "purchaseOrderId", receiving.purchaseOrderId);
                ViewData["vendorId"] = new SelectList(_context.Vendor, "vendorId", "vendorId", receiving.vendorId);
                ViewData["warehouseId"] = new SelectList(_context.Warehouse, "warehouseId", "warehouseId", receiving.warehouseId);
        return View(receiving);
    }

    // POST: Receiving/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("receivingId,purchaseOrderId,receivingNumber,receivingDate,vendorId,vendorDO,vendorInvoice,branchId,warehouseId,HasChild,createdAt")] Receiving receiving)
    {
        if (id != receiving.receivingId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(receiving);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceivingExists(receiving.receivingId))
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
                ViewData["branchId"] = new SelectList(_context.Branch, "branchId", "branchId", receiving.branchId);
                ViewData["purchaseOrderId"] = new SelectList(_context.PurchaseOrder, "purchaseOrderId", "purchaseOrderId", receiving.purchaseOrderId);
                ViewData["vendorId"] = new SelectList(_context.Vendor, "vendorId", "vendorId", receiving.vendorId);
                ViewData["warehouseId"] = new SelectList(_context.Warehouse, "warehouseId", "warehouseId", receiving.warehouseId);
        return View(receiving);
    }

    // GET: Receiving/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var receiving = await _context.Receiving
                .Include(r => r.branch)
                .Include(r => r.purchaseOrder)
                .Include(r => r.vendor)
                .Include(r => r.warehouse)
                .SingleOrDefaultAsync(m => m.receivingId == id);
        if (receiving == null)
        {
            return NotFound();
        }

        return View(receiving);
    }




    // POST: Receiving/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var receiving = await _context.Receiving.SingleOrDefaultAsync(m => m.receivingId == id);
            _context.Receiving.Remove(receiving);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReceivingExists(string id)
    {
        return _context.Receiving.Any(e => e.receivingId == id);
    }

  }
}





namespace netcore.MVC
{
  public static partial class Pages
  {
      public static class Receiving
      {
          public const string Controller = "Receiving";
          public const string Action = "Index";
          public const string Role = "Receiving";
          public const string Url = "/Receiving/Index";
          public const string Name = "Receiving";
      }
  }
}
namespace netcore.Models
{
  public partial class ApplicationUser
  {
      [Display(Name = "Receiving")]
      public bool ReceivingRole { get; set; } = false;
  }
}


