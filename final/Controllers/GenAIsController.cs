    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using final.Data;
using final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;


namespace final.Controllers
{   

    public class GenAIsController : Controller
    {
        private readonly ApplicationDbContext _context;
        GenAI genAI = new GenAI();
        

        public GenAIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GenAIs
        public async Task<IActionResult> Index()
        {
              return _context.GenAI != null ? 
                          View(await _context.GenAI.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.GenAI'  is null.");
        }

        // GET: GenAIs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GenAI == null)
            {
                return NotFound();
            }

            var genAI = await _context.GenAI
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genAI == null)
            {
                return NotFound();
            }

            return View(genAI);
        }

        // GET: GenAIs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GenAIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GenAIName,Summary,ImageFile,AnchorLink,Like")] GenAI genAI, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Get the file name and path
                    var fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(ImageFile.FileName)}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    // Save the file to wwwroot/images folder
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                        
                    }
                    

                    // Set the ImageFile property to the file name
                    genAI.ImageFileName = fileName;
                }

                _context.Add(genAI);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genAI);
        }




        // GET: GenAIs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GenAI == null)
            {
                return NotFound();
            }

            var genAI = await _context.GenAI.FindAsync(id);
            if (genAI == null)
            {
                return NotFound();
            }
            return View(genAI);
        }

        // POST: GenAIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GenAIName,Summary,ImageFilename,AnchorLink,Like")] GenAI genAI)
        {
            if (id != genAI.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {   
                try
                {
                    _context.Update(genAI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenAIExists(genAI.Id))
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
            return View(genAI);
        }

        // GET: GenAIs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GenAI == null)
            {
                return NotFound();
            }

            var genAI = await _context.GenAI
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genAI == null)
            {
                return NotFound();
            }

            return View(genAI);
        }

        // POST: GenAIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GenAI == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GenAI'  is null.");
            }
            var genAI = await _context.GenAI.FindAsync(id);
            if (genAI != null)
            {
                _context.GenAI.Remove(genAI);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenAIExists(int id)
        {
          return (_context.GenAI?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
