using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCam.DAL;
using SafeCam.Models;
using SafeCam.ViewModel.Employee;

namespace SafeCam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController(AppDbContext _context, IWebHostEnvironment _env) : Controller
    {
        public IActionResult Index()
        {
            var data = _context.Employees.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Employees = _context.Employees.Where(x=>!x.IsDeleted).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateVM em)
        {
            if (em.CoverFile != null)
            {
                if (!em.CoverFile.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("File", "Image deyil");

                }
                if (em.CoverFile.Length > 3 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "Olcusu dogru deyil");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Employees.Where(x => !x.IsDeleted).ToList();
                return View();
            }

            string FileName = Path.GetRandomFileName() + Path.GetExtension(em.CoverFile.FileName);

            using (Stream s = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "Product", FileName)))
            {
                await em.CoverFile.CopyToAsync(s);
            }

            Employee pm = new Employee
            {
                Name = em.Name,
                LastName = em.LastName,
                Position = em.Position,
                Salary = em.Salary,
                CoverFile = FileName,
            };

            await _context.Employees.AddAsync(pm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Employees.FindAsync(id.Value);
            if (data == null) return NotFound();
            _context.Employees.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Employees.FindAsync(id.Value);
            if (data == null) return NotFound();
            data.IsDeleted = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Hide(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Employees.FindAsync(id.Value);
            if (data == null) return NotFound();
            data.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Employees = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
            if (!id.HasValue) return BadRequest();
            var employees = await _context.Employees.Where(x => x.Id == id.Value).Select(x => new EmployeeUpdateVM
            {
                Name = x.Name,
                LastName = x.LastName,
                Salary = x.Salary,
                Position = x.Position,
            }).FirstOrDefaultAsync();
            if (employees is null) return StatusCode(400, "BadRequest");
            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, EmployeeUpdateVM um)
        {
            if (!id.HasValue) return BadRequest();
            if (um.CoverFile != null)
            {
                if (!um.CoverFile.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("CoverFile", "Image deyil");
                }
                if (um.CoverFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("CoverFile", "max 2mb");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
                return View(um);
            }
            var data = await _context.Employees.Where(x => x.Id == id.Value).FirstOrDefaultAsync();
            if (data is null) return BadRequest();

            if (um.CoverFile != null)
            {
                string oldName = Path.Combine(_env.WebRootPath, "imgs", "Product", data.CoverFile);

                using (Stream str = System.IO.File.Create(oldName))
                {
                    await um.CoverFile!.CopyToAsync(str);
                }
            }
            data.Name = um.Name;
            data.LastName = um.LastName;
            data.Position = um.Position;
            data.Salary = um.Salary;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
