using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Guarantib.Models;
using Microsoft.EntityFrameworkCore;

namespace Guarantib.Controllers
{
    public class AccessController : Controller
    {
        private readonly GuarantibContext _context;
        public AccessController(GuarantibContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.professionnels.ToListAsync());
        }

        public IActionResult Menu()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Name,job")] Professionnel professionnel)
        {
            _context.Add(professionnel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login","Access");
            
        }

        [HttpGet]
        
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if(claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Professionnel modelLogin)
        {
            var utilisateur = await _context.professionnels
             .FirstOrDefaultAsync(u => u.Name == modelLogin.Name);

            if (utilisateur != null && !string.IsNullOrEmpty(utilisateur.Name))
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
                    new Claim(ClaimTypes.Name, utilisateur.Name)


                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new()
                {
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties );
                return RedirectToAction("Index", "Home");


            }
            ViewData["ValidateMessage"] = "Utilisateur non trouvé";
            return View();
            
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professionnel = await _context.professionnels.FindAsync(id);
            if (professionnel == null)
            {
                return NotFound();
            }
            return View(professionnel);
        }

        // POST: professionnels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Lien")] Professionnel professionnel)
        {
            if (id != professionnel.Id)
            {
                return NotFound();
            }

            _context.Update(professionnel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        // GET: professionnels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professionnel = await _context.professionnels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professionnel == null)
            {
                return NotFound();
            }

            return View(professionnel);
        }

        // POST: professionnels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var professionnel = await _context.professionnels.FindAsync(id);
            if (professionnel != null)
            {
                _context.professionnels.Remove(professionnel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessionnelExists(int id)
        {
            return _context.professionnels.Any(e => e.Id == id);
        }
    }
}
