using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Models;
using PetFinder.Services;

namespace PetFinder.Controllers;

public class PetAdController : Controller
{
    private readonly IPetAdService _petAdService;

    public PetAdController(IPetAdService petAdService) => _petAdService = petAdService;

    // Herkese açık liste + arama
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? species, string? city)
    {
        var ads = await _petAdService.GetAllAsync(species, city);
        ViewData["SelectedSpecies"] = species;
        ViewData["SelectedCity"] = city;
        return View(ads);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var ad = await _petAdService.GetByIdAsync(id);
        if (ad == null) return NotFound();
        return View(ad);
    }

    // ===== Yeni İlan =====
    [Authorize]
    [HttpGet]
    public IActionResult Create() => View();

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PetAd model, IFormFile? imageFile)
    {
        if (!ModelState.IsValid) return View(model);

        if (imageFile != null && imageFile.Length > 0)
        {
            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            model.ImageBytes = ms.ToArray();
        }

        await _petAdService.CreateAsync(model);
        TempData["Success"] = "İlan başarıyla oluşturuldu.";
        return RedirectToAction(nameof(Index));
    }

    // ===== Düzenle =====
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var ad = await _petAdService.GetByIdAsync(id);
        if (ad == null) return NotFound();
        return View(ad);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PetAd model, IFormFile? imageFile)
    {
        if (!ModelState.IsValid) return View(model);

        if (imageFile != null && imageFile.Length > 0)
        {
            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            model.ImageBytes = ms.ToArray();
        }

        var updated = await _petAdService.UpdateAsync(model);
        if (updated == null) return NotFound();

        TempData["Success"] = "İlan güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    // ===== Sil =====
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var ad = await _petAdService.GetByIdAsync(id);
        if (ad == null) return NotFound();
        return View(ad);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _petAdService.DeleteAsync(id);
        TempData["Success"] = "İlan silindi.";
        return RedirectToAction(nameof(Index));
    }
}
