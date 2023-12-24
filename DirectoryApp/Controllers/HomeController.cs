using DirectoryApp.Models;
using DirectoryApp.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using DirectoryApp.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DirectoryApp.Data;
using DirectoryApp.Repository;
using DirectoryApp.Models.Entities;
using System.Security.Claims;

namespace DirectoryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _myContext;
        private readonly ContactRepo _contactRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ContactRepo contactRepo, MyContext myContext, UserManager<ApplicationUser> userManager)
        {
            _contactRepo = contactRepo;
            _myContext = myContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        #region ContactCrud
        [HttpGet]
        public IActionResult GetContacts()
        {
            var query = from p in _myContext.Contacts
                        select new ContactViewModel
                        {
                            id = p.Id,
                            Name = p.Name,
                            Surname = p.SurName,
                            Phone = p.Phone,
                        };
            var DataSource = query.ToList();
            int count = DataSource.Cast<ContactViewModel>().Count();
            return Ok(DataSource);
        }
        [HttpPost]
        public async Task<IActionResult> AddRecord(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            var userId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            Contact contact = new Contact()
            {
                Name = model.Name.ToUpper(),
                SurName = model.Surname.ToUpper(),
                Phone = "+90"+model.Phone,
                CreatedUser = user ==null ? "" : user.ToString(),
                isDeleted = false

            };
            _contactRepo.Add(contact);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult UpdateRecord(ContactViewModel model)
        {
            var data = _contactRepo.GetById(model.id);
            data.Name = model.Name.ToUpper();
            data.SurName = model.Surname.ToUpper();
            data.Phone = "+90"+model.Phone;
            _contactRepo.Update(data);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteRecord(ContactViewModel model)
        {
            Contact contact = _contactRepo.GetById(model.id);
            _contactRepo.Remove(contact);
            return RedirectToAction(nameof(Index));
        }
        #endregion


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
