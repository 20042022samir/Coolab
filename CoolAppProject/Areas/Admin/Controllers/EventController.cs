using CoolApp.Core.Entities;
using CoolAppProject.Contexts;
using CoolAppProject.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace CoolAppProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly CoolAppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public EventController(CoolAppDbContext context,IWebHostEnvironment enviroment)
        {
            _context = context;
            _environment = enviroment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Event> Events = await _context.Events.Where(x => !x.IsDeleted)
                .ToListAsync();
            return View(Events);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Event eventt)
        {
            if(eventt.file==null || eventt.Name==null || eventt.Description==null || eventt.StartedDate==null)
                return View(eventt);
            eventt.Image = eventt.file.CreateImage(_environment.WebRootPath, "Assets/img");
            await _context.Events.AddAsync(eventt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Event? eventt = await _context.Events.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (eventt == null)
                return NotFound()
;           eventt.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Event? eventt = await _context.Events.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (eventt == null)
                return NotFound();
            return View(eventt);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,Event eventtUpdate)
        {
            if (eventtUpdate.Name == null || eventtUpdate.Description == null || eventtUpdate.StartedDate == null)
                return View(eventtUpdate);
            Event? eventt = await _context.Events.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (eventt == null)
                return NotFound();
            eventt.Name=eventtUpdate.Name;eventt.Description=eventtUpdate.Description;
            eventt.StartedDate=eventtUpdate.StartedDate;
            eventt.Image = eventtUpdate.file != null ? eventtUpdate.file.CreateImage(_environment.WebRootPath, "Assets/img") : eventt.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        [HttpGet]
        public async Task<IActionResult> MakeForHomeScreen(int id)
        {
            IEnumerable<Event> events = await _context.Events.Where(x => !x.IsDeleted && x.ForMainPage)
                .ToListAsync();
            if (events.Count() == 4)
                return RedirectToAction("index","home");
            Event? eventt = await _context.Events.Where(x => !x.IsDeleted && x.Id == id)
               .FirstOrDefaultAsync();
            if (eventt == null)
                return NotFound();
            eventt.ForMainPage = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFromHome(int id)
        {
            IEnumerable<Event> events = await _context.Events.Where(x => !x.IsDeleted && x.ForMainPage)
               .ToListAsync();
            if (events.Count() == 2)
                return RedirectToAction("index", "home");
            Event? eventt = await _context.Events.Where(x => !x.IsDeleted && x.Id == id)
            .FirstOrDefaultAsync();
            if (eventt == null)
                return NotFound();
            eventt.ForMainPage = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        } 

        [HttpGet]
        public async Task<IActionResult> ShowContacts(int id)
        {
            Event? eventt = await _context.Events.Where(x => !x.IsDeleted && x.Id == id)
            .FirstOrDefaultAsync();
            if (eventt == null)
                return NotFound();
            IEnumerable<EventContact> contacts = await _context.EventContacts.Where(x => !x.IsDeleted && x.EvenntId == eventt.Id)
                .ToListAsync();
            return View(contacts);
        }


        [HttpGet]
        public async Task<IActionResult> CreateSpecialEvent(int id)
        {
            Event? specialEvent = await _context.Events
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if(specialEvent==null)
                return NotFound();
            specialEvent.SpecialEvent = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> ShowSingleContact(int id)
        {
            Event? eventt = await _context.Events.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.messages.Where(x => !x.IsDeleted))
                .FirstOrDefaultAsync();
            if(eventt==null)
                return NotFound();
            return View(eventt);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteEventContact(int id)
        {
            EventMessage? contact = await _context.EventMessages.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if(contact==null)
                return NotFound();
            contact.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowAllEventContacts));
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllEventContacts()
        {
            IEnumerable<EventMessage> messages = await _context.EventMessages
                .Where(x => !x.IsDeleted).Include(x => x.Event)
                .ToListAsync();
            return View(messages);
        }


        [HttpGet]
        public async Task<IActionResult> MakeSimpleEvent(int id)
        {
            IEnumerable<Event> events = await _context.Events
                .Where(x => !x.IsDeleted && x.SpecialEvent)
                .ToListAsync();
            if (events.Count() == 1)
                return RedirectToAction(nameof(Index));
            Event? eventt = await _context.Events.Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (eventt == null)
                return NotFound();
            eventt.SpecialEvent = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }



    
}
