using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guarantib.Models;
using System.Security.Claims;
using Microsoft.CodeAnalysis;

namespace Guarantib.Controllers
{
    public class SuivisController : Controller
    {
        private readonly GuarantibContext _context;

        public SuivisController(GuarantibContext context)
        {
            _context = context;
        }

        // GET: Suivis
        public async Task<IActionResult> Index(int? selectedProduitId)
        {
            var suivis = await _context.suivis
                .Include(s => s.Produits)
                .Include(s => s.Professionnels)
                .ToListAsync();

            ViewData["SelectedProduitId"] = selectedProduitId;

            return View(suivis);
        }




        public async Task<IActionResult> GetSuivisByProduit(int? id)
        {
            var suivis = await _context.suivis
                .Where(s => s.Produits.Any(c => c.Id.Equals(id)))
                .Include(s => s.Produits)
                .Include(s => s.Professionnels)
                .ToListAsync();

            ViewData["SelectedProduitId"] = id;

            var viewModel = new SuivisViewModel
            {
                ListeSuivis = suivis,
                NouveauSuivi = new Suivis { SelectedProduitId = id.Value }
            };

            return View("GetSuivisByProduit", viewModel);
        }


        public IActionResult CreateSuivi(int? id)
        {
            if (id.HasValue)
            {
                return View(new Suivis { SelectedProduitId = id.Value });
            }

            // Gérer le cas où l'ID du produit est null
            return RedirectToAction("Error");
        }


        // POST: Suivis/GetSuivisByProduit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSuivi(SuivisViewModel viewModel)
        {
            var nouveauSuivi = viewModel.NouveauSuivi;

            if (!ModelState.IsValid)
            {
                
                // Initialiser DateSuivis à la date actuelle
                nouveauSuivi.DateSuivis = DateTime.Now;

                // Associer l'identifiant de l'utilisateur au Suivis
                string selectedProfessionnelId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (int.TryParse(selectedProfessionnelId, out int userId))
                {
                    nouveauSuivi.SelectedProfessionnelId = userId;
                }

                // Ajouter le Suivis au produit
                Produit produit = await _context.produits.FindAsync(nouveauSuivi.SelectedProduitId);
                if (produit != null)
                {
                    produit.suivis ??= new List<Suivis>();
                    produit.suivis.Add(nouveauSuivi);
                }

                // Ajouter le Suivis au professionnel
                Professionnel professionnel = await _context.professionnels.FindAsync(nouveauSuivi.SelectedProfessionnelId);
                if (professionnel != null)
                {
                    professionnel.suivis ??= new List<Suivis>();
                    professionnel.suivis.Add(nouveauSuivi);
                }

                // Ajouter le Suivis à la base de données
                _context.Add(nouveauSuivi);
                await _context.SaveChangesAsync();

                // Rediriger vers l'action GetSuivisByProduit pour afficher les suivis mis à jour
                return RedirectToAction(nameof(GetSuivisByProduit), new { id = nouveauSuivi.SelectedProduitId });
            }

            // Si la validation échoue, vous pouvez recharger la liste des suivis existants
            viewModel.ListeSuivis = await _context.suivis
                .Where(s => s.Produits.Any(c => c.Id.Equals(nouveauSuivi.SelectedProduitId)))
                .Include(s => s.Produits)
                .Include(s => s.Professionnels)
                .ToListAsync();

            return View("GetSuivisByProduit", viewModel);
        }




        // GET: Suivis/Create
        public IActionResult Create(int? id)
        {
            Console.WriteLine($"Create method called with id: {id}");

            if (id.HasValue)
            {
                return View( new Suivis { SelectedProduitId = id.Value });
            }

            // Gérer le cas où l'ID du produit est null
           return RedirectToAction("Error");
        }


        // POST: Suivis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Etape,DateSuivis,dialogue,SelectedProduitId")] Suivis suivis)
        {
            // Récupérer l'identifiant de l'utilisateur connecté
            string selectedProfessionnelId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Vérifier si l'identifiant de l'utilisateur est une chaîne valide et peut être converti en int
            if (int.TryParse(selectedProfessionnelId, out int userId))
            {
                // Initialiser DateSuivis à la date actuelle
                suivis.DateSuivis = DateTime.Now;

                // Associer l'identifiant de l'utilisateur au Suivis
                suivis.SelectedProfessionnelId = userId;

                
                // Ajouter le Suivis au produit
                Produit produit = await _context.produits.FindAsync(suivis.SelectedProduitId);
                if (produit != null)
                {
                    produit.suivis ??= new List<Suivis>();
                    produit.suivis.Add(suivis);
                }
                

                // Ajouter le Suivis au professionnel
                Professionnel professionnel = await _context.professionnels.FindAsync(suivis.SelectedProfessionnelId);
                if (professionnel != null)
                {
                    professionnel.suivis ??= new List<Suivis>();
                    professionnel.suivis.Add(suivis);
                }

                // Ajouter le Suivis à la base de données
                _context.Add(suivis);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Gérer le cas où l'identifiant de l'utilisateur n'est pas valide ou la conversion échoue
            return RedirectToAction("Error");
        }


        // GET: Suivis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suivis = await _context.suivis.FindAsync(id);
            if (suivis == null)
            {
                return NotFound();
            }
            return View(suivis);
        }

        // POST: Suivis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Etape,DateSuivis,dialogue")] Suivis suivis)
        {
            if (id != suivis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suivis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuivisExists(suivis.Id))
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
            return View(suivis);
        }

        // GET: Suivis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suivis = await _context.suivis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suivis == null)
            {
                return NotFound();
            }

            return View(suivis);
        }

        // POST: Suivis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suivis = await _context.suivis.FindAsync(id);
            if (suivis != null)
            {
                _context.suivis.Remove(suivis);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Suivis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suivis = await _context.suivis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suivis == null)
            {
                return NotFound();
            }

            return View(suivis);
        }

        private bool SuivisExists(int id)
        {
            return _context.suivis.Any(e => e.Id == id);
        }



    }
}
