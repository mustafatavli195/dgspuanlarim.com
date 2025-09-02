using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dgspuanlarim.Models
{
    [Table("DgsPuanlarimDb")]
    public class DgsPuanlarimModel
    {
        [Key]
        public string program_kodu { get; set; }
        public string universite_adi { get; set; }
        public string? fakulte_adi { get; set; }
        public string bolum_adi { get; set; }
        public string puan_turu { get; set; }

        // 2024 verileri
        public int? kontenjan_2024 { get; set; }
        public int? yerlesen_2024 { get; set; }
        public decimal? en_kucuk_puan_2024 { get; set; }
        public decimal? en_buyuk_puan_2024 { get; set; }

        // 2023 verileri
        public int? kontenjan_2023 { get; set; }
        public int? yerlesen_2023 { get; set; }
        public decimal? en_kucuk_puan_2023 { get; set; }
        public decimal? en_buyuk_puan_2023 { get; set; }

        // 2022 verileri
        public int? kontenjan_2022 { get; set; }
        public int? yerlesen_2022 { get; set; }
        public decimal? en_kucuk_puan_2022 { get; set; }
        public decimal? en_buyuk_puan_2022 { get; set; }
    }
}
