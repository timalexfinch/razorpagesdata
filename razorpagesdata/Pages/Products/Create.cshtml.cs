﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using razorpagesdata.Models;

namespace razorpagesdata.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly razorpagesdata.Models.AdventureWorksLT2012Context _context;

        public CreateModel(razorpagesdata.Models.AdventureWorksLT2012Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategory, "ProductCategoryId", "Name");
        ViewData["ProductModelId"] = new SelectList(_context.ProductModel, "ProductModelId", "Name");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}