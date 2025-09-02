using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dgspuanlarim.Models;
using dgspuanlarim.Data;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace dgspuanlarim.Controllers;

public class HomeController : Controller
{
    private readonly DgsDbContext _context;
    private readonly IConfiguration _configuration;
    private const int PageSize = 100;

    public HomeController(DgsDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }



    public async Task<IActionResult> Index(string search = "", string sort = "", string puanTuru = "", int page = 1)
    {
        var allPrograms = await _context.DgsPuanlarimContext.AsNoTracking().ToListAsync();

        // Arama filtresi
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            allPrograms = allPrograms
                .Where(p => p.program_kodu.ToLower().Contains(search) ||
                            p.universite_adi.ToLower().Contains(search) ||
                            p.bolum_adi.ToLower().Contains(search))
                .ToList();
        }

        // Puan türü filtresi
        if (!string.IsNullOrWhiteSpace(puanTuru))
        {
            allPrograms = allPrograms
                .Where(p => p.puan_turu?.Equals(puanTuru, System.StringComparison.OrdinalIgnoreCase) ?? false)
                .ToList();
        }

        // Sýralama
        allPrograms = sort switch
        {
            "en_kucuk_asc" => allPrograms.OrderBy(p => p.en_kucuk_puan_2024 ?? p.en_kucuk_puan_2023 ?? p.en_kucuk_puan_2022).ToList(),
            "en_kucuk_desc" => allPrograms.OrderByDescending(p => p.en_kucuk_puan_2024 ?? p.en_kucuk_puan_2023 ?? p.en_kucuk_puan_2022).ToList(),
            "en_buyuk_asc" => allPrograms.OrderBy(p => p.en_buyuk_puan_2024 ?? p.en_buyuk_puan_2023 ?? p.en_buyuk_puan_2022).ToList(),
            "en_buyuk_desc" => allPrograms.OrderByDescending(p => p.en_buyuk_puan_2024 ?? p.en_buyuk_puan_2023 ?? p.en_buyuk_puan_2022).ToList(),
            "kontenjan_asc" => allPrograms.OrderBy(p => p.kontenjan_2024 ?? p.kontenjan_2023 ?? p.kontenjan_2022).ToList(),
            "kontenjan_desc" => allPrograms.OrderByDescending(p => p.kontenjan_2024 ?? p.kontenjan_2023 ?? p.kontenjan_2022).ToList(),
            "yerlesen_asc" => allPrograms.OrderBy(p => p.yerlesen_2024 ?? p.yerlesen_2023 ?? p.yerlesen_2022).ToList(),
            "yerlesen_desc" => allPrograms.OrderByDescending(p => p.yerlesen_2024 ?? p.yerlesen_2023 ?? p.yerlesen_2022).ToList(),
            _ => allPrograms.OrderBy(p => p.universite_adi).ToList()
        };

        // Pagination
        var pagedPrograms = allPrograms.Skip((page - 1) * PageSize)
                                       .Take(PageSize)
                                       .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(allPrograms.Count / (double)PageSize);
        ViewBag.Search = search;
        ViewBag.Sort = sort;
        ViewBag.PuanTuru = puanTuru;

        ViewData["Search"] = search;
        ViewData["Sort"] = sort;
        ViewData["PuanTuru"] = puanTuru;

        return View(pagedPrograms); // artýk view'e direkt List<DgsSiralama> gönderiyoruz
    }
    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Contact()
    {
        return View(new ContactFormModel());
    }

    [HttpPost]
    public IActionResult Contact(ContactFormModel model)
    {

        if (ModelState.IsValid)
        {
            try
            {
                var fromAddress = new MailAddress("tavlioffical@gmail.com", "DGS Sitesi");
                var toAddress = new MailAddress("tavlioffical@gmail.com"); // kendine gönder

                string subject = $"Ýletiþim Formu: {model.Name}";
                string body = $"Gönderen: {model.Name} <{model.Email}>\n\nMesaj:\n{model.Message}";

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {

                    var smtpUser = _configuration["Smtp:User"];
                    var smtpPass = _configuration["Smtp:Password"];

                    smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true;
                    smtp.Send(fromAddress.Address, toAddress.Address, subject, body);
                }

                TempData["Success"] = "Mesajýnýz baþarýyla gönderildi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Mesaj gönderilirken bir hata oluþtu: " + ex.Message;
            }

            return RedirectToAction("Contact");
        }

        TempData["Error"] = "Lütfen formu eksiksiz doldurun.";
        return View(model);
    }

}