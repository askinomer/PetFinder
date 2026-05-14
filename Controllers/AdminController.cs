using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Services;

namespace PetFinder.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly IPetAdService _petAdService;
    private readonly IUserService _userService;

    public AdminController(IPetAdService petAdService, IUserService userService)
    {
        _petAdService = petAdService;
        _userService = userService;
    }

    // Sadece "admin" kullanıcısı erişebilir
    private IActionResult? CheckAdmin()
    {
        if (User.Identity?.Name != "admin")
            return Forbid();
        return null;
    }

    public async Task<IActionResult> Index()
    {
        var deny = CheckAdmin(); if (deny != null) return deny;

        var ads = await _petAdService.GetAllAsync();
        var users = await _userService.GetAllAsync();

        ViewBag.TotalAds = ads.Count;
        ViewBag.DogCount = ads.Count(a => a.Species == "Köpek");
        ViewBag.CatCount = ads.Count(a => a.Species == "Kedi");
        ViewBag.UserCount = users.Count;
        ViewBag.Users = users;
        return View(ads);
    }

    public async Task<IActionResult> Users()
    {
        var deny = CheckAdmin(); if (deny != null) return deny;
        var users = await _userService.GetAllAsync();
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deny = CheckAdmin(); if (deny != null) return deny;

        var user = await _userService.GetByIdAsync(id);
        if (user?.Username == "admin")
        {
            TempData["Success"] = "Admin kullanıcısı silinemez.";
            return RedirectToAction(nameof(Users));
        }

        await _userService.DeleteAsync(id);
        TempData["Success"] = "Kullanıcı silindi.";
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAd(int id)
    {
        var deny = CheckAdmin(); if (deny != null) return deny;
        await _petAdService.DeleteAsync(id);
        TempData["Success"] = "İlan silindi.";
        return RedirectToAction(nameof(Index));
    }
}
