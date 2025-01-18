using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guarantib.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Guarantib.Controllers
{

    [Authorize]
    public class ProduitsController : Controller
    {
        private readonly GuarantibContext _context;

        public ProduitsController(GuarantibContext context)
        {
            _context = context;
        }

        
        // GET: Produits
        public async Task<IActionResult> Index()
        {
            var produits = await _context.produits.Include(p => p.Clients).Include(p => p.marques).Include(p => p.Professionnels).ToListAsync();

            return View(produits);
        }
        
        public async Task<IActionResult> GetisClose(int? id)
        {
            return View("index", await _context.produits.Where( p => p.IsClose == true ).Include(p => p.Clients)
           .Include(p => p.marques).Include(p => p.Professionnels).ToListAsync());
        }

        public async Task<IActionResult> GetBySerie(string search)
        {
            // Assurez-vous d'ajouter une validation ou une gestion d'erreur pour le paramètre de recherche

            var produits = await _context.produits
                .Where(p => p.NumSerie == search)
                .Include(p => p.Clients)
                .Include(p => p.marques)
                .Include(p => p.Professionnels)
                .ToListAsync();

            return View("Index", produits);
        }



        public async Task<IActionResult> CheckChange(int? id)
        {
            // Recherchez le produit avec l'id spécifié
            Produit p = await _context.produits.FindAsync(id);

            // Mettez à jour la propriété IsClose à true
            p.IsClose = !p.IsClose;

            // Enregistrez les modifications dans la base de données
            _context.Update(p);
            await _context.SaveChangesAsync();


            List<Produit> produits;

            if (p.IsClose == false)
            {
                // Récupérez la liste des produits non fermés
                produits = await _context.produits
                    .Where(p => p.IsClose == false)
                    .Include(p => p.Clients)
                    .Include(p => p.marques)
                    .Include(p => p.Professionnels)
                    .ToListAsync();
            }
            else
            {
                // Récupérez la liste des produits fermés
                produits = await _context.produits
                    .Where(p => p.IsClose == true)
                    .Include(p => p.Clients)
                    .Include(p => p.marques)
                    .Include(p => p.Professionnels)
                    .ToListAsync();
            }

            // Retournez la vue avec la liste des produits
            return View("index", produits);
        }

        public async Task<IActionResult> GetisNotClose(int? id)
        {
            return View("index", await _context.produits.Where(p => p.IsClose == false).Include(p => p.Clients)
           .Include(p => p.marques).Include(p => p.Professionnels).ToListAsync());
        }
        
        public async Task<IActionResult> GetProduitsByClient(int? id)
        {
            return View("index", await _context.produits.Where(c => c.Clients.Any(s=>s.Id.Equals(id))).Where(p => p.IsClose == false).Include(p => p.Clients)
           .Include(p => p.marques).Include(p => p.Professionnels).ToListAsync());
        }
       
        public async Task<IActionResult> GetProduitsByMarque(int? id)
        {
            return View("index", await _context.produits.Where(m => m.marques.Any(s => s.Id.Equals(id))).Where(p => p.IsClose == false).Include(p => p.Clients)
           .Include(p => p.marques).Include(p => p.Professionnels).ToListAsync());
        }

       


        // GET: Produits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.produits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        [HttpGet]
        
        // GET: Produits/Create
        public IActionResult Create()
        {
            ViewData["MarqueId"] = new SelectList(_context.marques, "Id", "Name");
            ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Name");
            return View();
        }

        // POST: Produits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Create([Bind("Id,Name,Brand,StartDate,EndDate,NumFile,NumAkuito,NumSerie,IsClose,Description,SelectedClientId,SelectedMarqueId")] Produit produit)
        {
            // Récupérer l'identifiant de l'utilisateur connecté
            string selectedProfessionnelId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Vérifier si l'identifiant de l'utilisateur est une chaîne valide et peut être converti en int
            if (int.TryParse(selectedProfessionnelId, out int userIdAsInt))
            {
                // Initialiser StartDate à la date actuelle
                produit.StartDate = DateTime.Now;
                // Associer l'identifiant de l'utilisateur au produit
                produit.SelectedProfessionnelId = userIdAsInt;

                // Récupérer la marque sélectionnée
                Marque selectedMarque = await _context.marques.FindAsync(produit.SelectedMarqueId);

                // Récupérer le client sélectionné
                Client selectedClient = await _context.clients.FindAsync(produit.SelectedClientId);

                // Vérifier si la marque et le client existent
                if (selectedMarque != null && selectedClient != null)
                {
                    // Ajouter la marque au produit
                    produit.marques = new List<Marque> { selectedMarque };

                    // Ajouter le client au produit
                    produit.Clients = new List<Client> { selectedClient };

                    // Récupérer le professionnel associé à l'utilisateur connecté
                    Professionnel professionnel = await _context.professionnels
                        .Include(p => p.Produits) // Assurez-vous que la collection Produits est chargée
                        .FirstOrDefaultAsync(p => p.Id == userIdAsInt);

                    // Vérifier si le professionnel existe
                    if (professionnel != null)
                    {
                        // Ajouter le produit à la collection Produits du Professionnel
                        professionnel.Produits ??= new List<Produit>();
                        professionnel.Produits.Add(produit);
                    }

                    // Ajouter le produit à la base de données
                    _context.Add(produit);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(GetisNotClose));
                }
            }

            // Gérer le cas où l'identifiant de l'utilisateur n'est pas valide ou la conversion échoue
            // Vous pouvez rediriger vers une page d'erreur ou effectuer une autre action appropriée
            return RedirectToAction("Error");
        }


        // GET: Produits/Edit/5
        // GET: Produits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.produits
                .Include(p => p.Clients) // Inclure la liste des clients associée au produit
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produit == null)
            {
                return NotFound();
            }

            ViewData["ClientId"] = new SelectList(_context.clients, "Id", "Name");
            ViewData["MarqueId"] = new SelectList(_context.marques, "Id", "Name");
            return View(produit);
        }

        // POST: Produits/Edit/5
        [HttpPost]
        
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Brand,StartDate,EndDate,NumFile,NumAkuito,NumSerie,IsClose,Description")] Produit produit, int selectedClientId, int selectedMarqueId)
        {
            if (id != produit.Id)
            {
                return NotFound();
            }

            // Récupérer le produit avec la liste des clients et des marques associées
            var existingProduit = await _context.produits
                .Include(p => p.Clients)
                .Include(p => p.marques)
                .FirstOrDefaultAsync(p => p.Id == id);

            // Mettre à jour les propriétés du produit
            existingProduit.Name = produit.Name;
            existingProduit.Brand = produit.Brand;
            existingProduit.StartDate = produit.StartDate;
            existingProduit.EndDate = produit.EndDate;
            existingProduit.NumFile = produit.NumFile;
            existingProduit.NumAkuito = produit.NumAkuito;
            existingProduit.NumSerie = produit.NumSerie;
            existingProduit.IsClose = produit.IsClose;
            existingProduit.Description = produit.Description;

            // Récupérer le client sélectionné par son ID
            Client selectedClient = await _context.clients.FindAsync(selectedClientId);

            // Récupérer la marque sélectionnée par son ID
            Marque selectedMarque = await _context.marques.FindAsync(selectedMarqueId);

            // Mettre à jour les listes des clients et des marques associées au produit
            existingProduit.Clients = new List<Client> { selectedClient };
            existingProduit.marques = new List<Marque> { selectedMarque };

            // Pas besoin d'appeler Update explicitement
            // _context.Update(existingProduit);

            // Enregistrer les modifications
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       
        // GET: Produits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.produits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produit = await _context.produits.FindAsync(id);
            if (produit != null)
            {
                _context.produits.Remove(produit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
            return _context.produits.Any(e => e.Id == id);
        }
    }
}
