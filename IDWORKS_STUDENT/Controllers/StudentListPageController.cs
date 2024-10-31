using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IDWORKS_STUDENT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IDWORKS_STUDENT.AppConfig;

namespace IDWORKS_STUDENT.Controllers
{
    public class StudentListPageController : Controller
    {
        public string ReturnUrl { get; set; }
        private readonly StudentContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public StudentListPageController(StudentContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        // GET: StudentListPage
        [HttpGet]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            UserSignAuth.WhichSignedOnPage = UserSignAuth.SignedOnPage.StudentPageList; 
            int pageSize = 10;
            var students = from s in _context.Colina_Student_Card_Accident
                           select s;
            return View(await Pagination.PaginatedList<Colina_Student_Card_Accident>.CreateAsync(students, pageNumber ?? 1, pageSize));
        }

        // GET: StudentListPage/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colina_Student_Card_Accident = await _context.Colina_Student_Card_Accident
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colina_Student_Card_Accident == null)
            {
                return NotFound();
            }

            return View(colina_Student_Card_Accident);
        }

        // GET: StudentListPage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentListPage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IDWFirstName,IDWLastName,IDWSchool,IDWPolicyNumber,IDWEffectiveDate,IDWEffectiveDateText,IDWCoverageProvider,IDWCoverage")] Colina_Student_Card_Accident colina_Student_Card_Accident)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colina_Student_Card_Accident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colina_Student_Card_Accident);
        }

        // GET: StudentListPage/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colina_Student_Card_Accident = await _context.Colina_Student_Card_Accident.FindAsync(id);
            if (colina_Student_Card_Accident == null)
            {
                return NotFound();
            }
            return View(colina_Student_Card_Accident);
        }

        // POST: StudentListPage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IDWFirstName,IDWLastName,IDWSchool,IDWPolicyNumber,IDWEffectiveDate,IDWEffectiveDateText,IDWCoverageProvider,IDWCoverage")] Colina_Student_Card_Accident colina_Student_Card_Accident)
        {
            if (id != colina_Student_Card_Accident.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colina_Student_Card_Accident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Colina_Student_Card_AccidentExists(colina_Student_Card_Accident.Id))
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
            return View(colina_Student_Card_Accident);
        }

        // GET: StudentListPage/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colina_Student_Card_Accident = await _context.Colina_Student_Card_Accident
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colina_Student_Card_Accident == null)
            {
                return NotFound();
            }

            return View(colina_Student_Card_Accident);
        }

        // POST: StudentListPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colina_Student_Card_Accident = await _context.Colina_Student_Card_Accident.FindAsync(id);
            _context.Colina_Student_Card_Accident.Remove(colina_Student_Card_Accident);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Colina_Student_Card_AccidentExists(int id)
        {
            return _context.Colina_Student_Card_Accident.Any(e => e.Id == id);
        }

        /*********************************************
         * Custom Actions for Filtering
         ****************************************************/
        #region CustomActionsFilter

        [HttpGet("searchPolicy")]
        public async Task<IActionResult> Index(string searchPolicy, DateTime searchDate, string searchSchool, string searchFirstName, string searchLastName, string currentFilter, int? pageNumber)
        {
            int pageSize = 10;
            ViewBag.CurrentFilter = searchPolicy;


            var students = from s in _context.Colina_Student_Card_Accident
                           select s;

            if (searchPolicy != null)
                pageNumber = 1;
            else
                searchPolicy = currentFilter != null && currentFilter.Split(',').Length > 0 ? currentFilter.Split(',')[0] : currentFilter;

            // Filter on Date if exists 
            if (searchDate != null && searchDate.Year >= 2000)
                students = students.Where(x => x.IDWEffectiveDate == searchDate);

            // Filter on Policy Number if exists 
            if (searchPolicy != null && searchPolicy.Length > 0)
                students = students.Where(x => x.IDWPolicyNumber.ToUpper().Contains(searchPolicy.ToUpper()));

            // Filter on School if exists 
            if (searchSchool != null && searchSchool.Length > 0)
                students = students.Where(x => x.IDWSchool.ToUpper().Contains(searchSchool.ToUpper()));

            // Filter on First Name if exists 
            if (searchFirstName != null && searchFirstName.Length > 0)
                students = students.Where(x => x.IDWFirstName.ToUpper().Contains(searchFirstName.ToUpper()));

            // Filter on Last Name if exists 
            if (searchLastName != null && searchLastName.Length > 0)
                students = students.Where(x => x.IDWLastName.ToUpper().Contains(searchLastName.ToUpper()));

            return View(await Pagination.PaginatedList<Colina_Student_Card_Accident>.CreateAsync(students, pageNumber ?? 1, pageSize)); ;
        }
        #endregion

    }
}
