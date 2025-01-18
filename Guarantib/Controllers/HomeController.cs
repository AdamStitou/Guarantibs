using Guarantib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


[Authorize]
public class HomeController : Controller
{
    
    private readonly GuarantibContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(GuarantibContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        int nonClosedProductsCount = _context.produits.Count(produit => produit.IsClose == false);
        ViewBag.NonClosedProductsCount = nonClosedProductsCount;

        DateTime sevenDaysFromNow = DateTime.Now.Date.AddDays(14);
        int approachingEndDateProductsCount = _context.produits.Count(produit => produit.EndDate.HasValue && produit.EndDate.Value.Date <= sevenDaysFromNow && produit.IsClose == false);
        ViewBag.ApproachingEndDateProductsCount = approachingEndDateProductsCount;

        List<Client> clients = _context.clients.ToList();

        Dictionary<int, int> clientProductCounts = new Dictionary<int, int>();

        foreach (var client in clients)
        {
            int clientNb = _context.produits
                .Where(p => p.Clients.Any(s => s.Id == client.Id) && p.IsClose == false)
                .Count();

            clientProductCounts[client.Id] = clientNb;
            
        }

        ViewBag.ClientProductCounts = clientProductCounts;

        return View(clients); // Passez la liste des clients à la vue
    }


    public IActionResult Menu()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Access");
    }

    public IActionResult EnCours()
    {
        return View();
    }

    public IActionResult Ajouter()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
