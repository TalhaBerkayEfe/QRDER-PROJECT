using Microsoft.AspNetCore.Mvc;
using QRDER.Models.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QRDER.Services;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace QRDER.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly BlobStorageService _blobService;

        public AdminController(AppDbContext context, BlobStorageService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        public IActionResult Login()
        {
            ViewBag.Adminler = _context.AdminGirisis.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminGirisi model)
        {
            if (ModelState.IsValid)
            {
                var admin = await _context.AdminGirisis
                    .FirstOrDefaultAsync(a => a.KullaniciAdi == model.KullaniciAdi && a.Sifre == model.Sifre);

                if (admin != null)
                {
                    TempData["IsAdmin"] = true;
                    // Başarılı giriş
                    return RedirectToAction("Panel");
                }

                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AdminGirisi model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı adının benzersiz olduğunu kontrol et
                var existingUser = await _context.AdminGirisis
                    .FirstOrDefaultAsync(a => a.KullaniciAdi == model.KullaniciAdi);

                if (existingUser != null)
                {
                    ModelState.AddModelError("KullaniciAdi", "Bu kullanıcı adı zaten kullanılıyor!");
                    return View(model);
                }

                // Yeni admin kullanıcısını kaydet
                _context.AdminGirisis.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Panel()
        {
            // Basit oturum kontrolü (gelişmiş için authentication eklenmeli)
            if (TempData["IsAdmin"] == null || !(bool)TempData["IsAdmin"])
            {
                return RedirectToAction("Login");
            }
            TempData.Keep("IsAdmin");

            // Tüm ürünleri getir
            var anaYemekler = await _context.AnaYemeks.ToListAsync();
            var araYemekler = await _context.AraYemeks.ToListAsync();
            var icecekler = await _context.Iceceklers.ToListAsync();
            var tatlilar = await _context.Tatlilars.ToListAsync();
            var siparisler = await _context.Siparislers.OrderByDescending(s => s.SiparisTarihi).ToListAsync();

            ViewData["AnaYemekler"] = anaYemekler;
            ViewData["AraYemekler"] = araYemekler;
            ViewData["Icecekler"] = icecekler;
            ViewData["Tatlilar"] = tatlilar;
            ViewData["Siparisler"] = siparisler;

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            TempData["IsAdmin"] = false;
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AddAnaYemek()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAnaYemek(AnaYemek model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    await _blobService.UploadFileAsync(imageFile.OpenReadStream(), fileName);
                    model.AnaYemekGorsel = fileName;
                }

                _context.AnaYemeks.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Panel");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAnaYemek(int id)
        {
            var anaYemek = await _context.AnaYemeks.FindAsync(id);
            if (anaYemek == null)
            {
                return NotFound();
            }
            return View(anaYemek);
        }

        [HttpPost]
        public async Task<IActionResult> EditAnaYemek(AnaYemek model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Panel");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAnaYemek(int id)
        {
            var anaYemek = await _context.AnaYemeks.FindAsync(id);
            if (anaYemek == null)
            {
                return NotFound();
            }
            _context.AnaYemeks.Remove(anaYemek);
            await _context.SaveChangesAsync();
            return RedirectToAction("Panel");
        }

        [HttpGet]
        public IActionResult AddAraYemek()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAraYemek(AraYemek model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    await _blobService.UploadFileAsync(imageFile.OpenReadStream(), fileName);
                    model.AraYemekGorsel = fileName;
                }

                _context.AraYemeks.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Panel");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAraYemek(int id)
        {
            var araYemek = await _context.AraYemeks.FindAsync(id);
            if (araYemek == null)
            {
                return NotFound();
            }
            return View(araYemek);
        }

        [HttpPost]
        public async Task<IActionResult> EditAraYemek(AraYemek model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Panel");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAraYemek(int id)
        {
            var araYemek = await _context.AraYemeks.FindAsync(id);
            if (araYemek == null)
            {
                return NotFound();
            }
            _context.AraYemeks.Remove(araYemek);
            await _context.SaveChangesAsync();
            return RedirectToAction("Panel");
        }

        [HttpGet]
        public IActionResult AddIcecek()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddIcecek(Icecekler model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    await _blobService.UploadFileAsync(imageFile.OpenReadStream(), fileName);
                    model.IceceklerGorsel = fileName;
                }

                _context.Iceceklers.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Panel");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditIcecek(int id)
        {
            var icecek = await _context.Iceceklers.FindAsync(id);
            if (icecek == null)
            {
                return NotFound();
            }
            return View(icecek);
        }

        [HttpPost]
        public async Task<IActionResult> EditIcecek(Icecekler model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Panel");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteIcecek(int id)
        {
            var icecek = await _context.Iceceklers.FindAsync(id);
            if (icecek == null)
            {
                return NotFound();
            }
            _context.Iceceklers.Remove(icecek);
            await _context.SaveChangesAsync();
            return RedirectToAction("Panel");
        }

        [HttpGet]
        public IActionResult AddTatli()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTatli(Tatlilar model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    await _blobService.UploadFileAsync(imageFile.OpenReadStream(), fileName);
                    model.TatlilarGorsel = fileName;
                }

                _context.Tatlilars.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Panel");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditTatli(int id)
        {
            var tatli = await _context.Tatlilars.FindAsync(id);
            if (tatli == null)
            {
                return NotFound();
            }
            return View(tatli);
        }

        [HttpPost]
        public async Task<IActionResult> EditTatli(Tatlilar model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Panel");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTatli(int id)
        {
            var tatli = await _context.Tatlilars.FindAsync(id);
            if (tatli == null)
            {
                return NotFound();
            }
            _context.Tatlilars.Remove(tatli);
            await _context.SaveChangesAsync();
            return RedirectToAction("Panel");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderStatusUpdate model)
        {
            try
            {
                var siparis = await _context.Siparislers.FindAsync(model.SiparisId);
                if (siparis == null)
                {
                    return Json(new { success = false, message = "Sipariş bulunamadı." });
                }

                siparis.Durum = model.Durum;
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    public class OrderStatusUpdate
    {
        public int SiparisId { get; set; }
        public string Durum { get; set; }
    }
} 