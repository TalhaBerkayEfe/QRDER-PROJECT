using System;
using System.Collections.Generic;

namespace QRDER.Models.Data;

public partial class AnaYemek
{
    public int AnaYemekId { get; set; }

    public string? AnaYemek1 { get; set; }

    public decimal? AnaYemekFiyat { get; set; }

    public string? AnaYemekGorsel { get; set; }
}
