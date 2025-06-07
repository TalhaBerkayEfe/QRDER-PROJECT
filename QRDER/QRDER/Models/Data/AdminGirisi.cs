using System;
using System.Collections.Generic;

namespace QRDER.Models.Data;

public partial class AdminGirisi
{
    public int Id { get; set; }

    public string? KullaniciAdi { get; set; }

    public string? Sifre { get; set; }
}
