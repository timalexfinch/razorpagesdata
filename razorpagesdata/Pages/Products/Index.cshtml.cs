using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorpagesdata.Models;

namespace razorpagesdata.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly razorpagesdata.Models.AdventureWorksLT2012Context _context;

        public IndexModel(razorpagesdata.Models.AdventureWorksLT2012Context context)
        {
            _context = context;
        }

        public string CategorySort { get; set; }
        public string ModelSort { get; set; }
        public string NameSort { get; set; }
        //public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Product> Products { get; set; }
        // public IList<Product> Products { get;set; }

        public async Task OnGetAsync(string sortOrder,
                string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            CategorySort = String.IsNullOrEmpty(sortOrder) ? "Category_desc" : "";
            ModelSort = sortOrder == "Model" ? "Model_desc" : "Model";
            NameSort = sortOrder == "Name" ? "Name_desc" : "Name";

            IQueryable<Product> productIQ = _context.Product;

            switch (sortOrder)
            {
                case "Category_desc":
                    productIQ = productIQ.OrderByDescending(p => p.ProductCategory.Name);
                    break;
                case "Model":
                    productIQ = productIQ.OrderBy(p => p.ProductModel.Name);
                    break;
                case "Model_desc":
                    productIQ = productIQ.OrderByDescending(p => p.ProductModel.Name);
                    break;
                case "Name":
                    productIQ = productIQ.OrderBy(p => p.Name);
                    break;
                case "Name_desc":
                    productIQ = productIQ.OrderByDescending(p => p.Name);
                    break;
                default:
                    productIQ = productIQ.OrderBy(p => p.ProductCategory.Name);
                    break;
            }

            int pageSize = 5;
            Products = await PaginatedList<Product>.CreateAsync(
                productIQ.AsNoTracking()
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductModel)
                .Where(p => p.ThumbnailPhotoFileName != "no_image_available_small.gif"),
                pageIndex ?? 1, pageSize);


            // this is how it was with Sorting, but before Paging:
            //Products = await productIQ.AsNoTracking()
            //.Include(p => p.ProductCategory)
            //.Include(p => p.ProductModel)
            //.Where(p => p.ThumbnailPhotoFileName != "no_image_available_small.gif")
            //.ToListAsync();

            // This was the original, before we added Sorting:
            //Products = await _context.Product
            //    .Include(p => p.ProductCategory)
            //    .Include(p => p.ProductModel)
            //    .Where(p => p.ThumbnailPhotoFileName != "no_image_available_small.gif")
            //    .OrderBy(p => p.ProductCategory.Name)
            //    .ThenBy(p => p.ProductModel.Name)
            //    .ThenBy(p => p.Name)
            //    .ToListAsync();
        }
    }
}
