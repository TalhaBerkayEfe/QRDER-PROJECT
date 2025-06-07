using System;
using System.Collections.Generic;

namespace QRDER.Models.Data;

public partial class Siparisler
{
    public int SiparisId { get; set; }
    public string? MasaNo { get; set; }
    public string? SiparisDetay { get; set; }
    public decimal? ToplamFiyat { get; set; }
    public DateTime? SiparisTarihi { get; set; }
    public string? Durum { get; set; } // Beklemede, Onaylandı, Reddedildi
} 