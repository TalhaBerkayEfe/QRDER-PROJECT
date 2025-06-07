using System;
using System.Collections.Generic;

namespace QRDER.Models.Data;

public partial class AraYemek
{
    public int AraYemekId { get; set; }

    public string? AraYemek1 { get; set; }

    public decimal? AraYemekFiyat { get; set; }

    public string? AraYemekGorsel { get; set; }
}
