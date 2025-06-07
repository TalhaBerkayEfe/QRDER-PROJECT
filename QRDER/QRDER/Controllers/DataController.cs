using Microsoft.AspNetCore.Mvc;
using QRDER.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QRDER.Services;
using System.Text;
using System;
using System.Threading.Tasks;

namespace QRDER.Controllers
{
    public class DataController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DataController> _logger;
        private readonly BlobStorageService _blobService;

        public DataController(AppDbContext context, ILogger<DataController> logger, BlobStorageService blobService)
        {
            _context = context;
            _logger = logger;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Veritabanı bağlantısı deneniyor...");
                
                var anaYemekler = await _context.AnaYemeks.ToListAsync();
                var araYemekler = await _context.AraYemeks.ToListAsync();
                var icecekler = await _context.Iceceklers.ToListAsync();
                var tatlilar = await _context.Tatlilars.ToListAsync();
                var kategoriler = await _context.Kategorilers.ToListAsync();
                var qrVerileri = await _context.QrVerisis.ToListAsync();

                // Görsel URL'lerini güncelle ve logla
                foreach (var yemek in anaYemekler)
                {
                    if (yemek.AnaYemekGorsel != null)
                    {
                        _logger.LogInformation($"Ana Yemek Görsel İsmi: {yemek.AnaYemekGorsel}");
                        yemek.AnaYemekGorsel = _blobService.GetBlobUrl(yemek.AnaYemekGorsel);
                        _logger.LogInformation($"Ana Yemek Görsel URL: {yemek.AnaYemekGorsel}");
                    }
                }
                foreach (var yemek in araYemekler)
                {
                    if (yemek.AraYemekGorsel != null)
                    {
                        _logger.LogInformation($"Ara Yemek Görsel İsmi: {yemek.AraYemekGorsel}");
                        yemek.AraYemekGorsel = _blobService.GetBlobUrl(yemek.AraYemekGorsel);
                        _logger.LogInformation($"Ara Yemek Görsel URL: {yemek.AraYemekGorsel}");
                    }
                }
                foreach (var icecek in icecekler)
                {
                    if (icecek.IceceklerGorsel != null)
                    {
                        _logger.LogInformation($"İçecek Görsel İsmi: {icecek.IceceklerGorsel}");
                        icecek.IceceklerGorsel = _blobService.GetBlobUrl(icecek.IceceklerGorsel);
                        _logger.LogInformation($"İçecek Görsel URL: {icecek.IceceklerGorsel}");
                    }
                }
                foreach (var tatli in tatlilar)
                {
                    if (tatli.TatlilarGorsel != null)
                    {
                        _logger.LogInformation($"Tatlı Görsel İsmi: {tatli.TatlilarGorsel}");
                        tatli.TatlilarGorsel = _blobService.GetBlobUrl(tatli.TatlilarGorsel);
                        _logger.LogInformation($"Tatlı Görsel URL: {tatli.TatlilarGorsel}");
                    }
                }
                foreach (var kategori in kategoriler)
                {
                    if (kategori.KategoriGorsel != null)
                    {
                        _logger.LogInformation($"Kategori Görsel İsmi: {kategori.KategoriGorsel}");
                        kategori.KategoriGorsel = _blobService.GetBlobUrl(kategori.KategoriGorsel);
                        _logger.LogInformation($"Kategori Görsel URL: {kategori.KategoriGorsel}");
                    }
                }
                foreach (var qr in qrVerileri)
                {
                    if (qr.Qrresim != null)
                    {
                        _logger.LogInformation($"QR Görsel İsmi: {qr.Qrresim}");
                        qr.Qrresim = _blobService.GetBlobUrl(qr.Qrresim);
                        _logger.LogInformation($"QR Görsel URL: {qr.Qrresim}");
                    }
                }

                var viewModel = new
                {
                    AnaYemekler = anaYemekler,
                    AraYemekler = araYemekler,
                    Icecekler = icecekler,
                    Tatlilar = tatlilar,
                    Kategoriler = kategoriler,
                    QrVerileri = qrVerileri
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Veritabanı hatası: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder([FromBody] Siparisler siparis)
        {
            try
            {
                siparis.SiparisTarihi = DateTime.Now;
                siparis.Durum = "Beklemede";

                _context.Siparislers.Add(siparis);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
} 