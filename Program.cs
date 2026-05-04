using System;
using System.Collections.Generic;
using System.IO;

namespace TahsinAltın_Odev1
{
    public enum Bolum
    {
        Yazilim,
        Muhasebe,
        Grafik,
        Elektrik
    }

    public interface IKaydet
    {
        void DosyayaKaydet();
    }

    public abstract class Person
    {
        public string AdSoyad { get; set; }
        protected int Yas;
        public string TcNo { get; set; }

        protected Person()
        {
            AdSoyad = "Bilinmiyor";
            Yas = 0;
            TcNo = "Bilinmiyor";
        }

        protected Person(string adSoyad, int yas, string tcNo)
        {
            AdSoyad = adSoyad;
            Yas = yas;
            TcNo = tcNo;
        }

        public abstract void MaasHesapla();

        public virtual void BilgiYazdir()
        {
            Console.WriteLine($"Ad Soyad: {AdSoyad} | Yaş: {Yas} | TC: {TcNo}");
        }
    }

    public class Student : Person, IKaydet
    {
        private double ortalama;

        public string OgrenciNo { get; set; }
        public Bolum Bolum { get; set; }
        public double Ortalama
        {
            get { return ortalama; }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    ortalama = value;
                }
                else
                {
                    throw new ArgumentException("Ortalama 0-100 arasında olmalı!");
                }
            }
        }

        public Student() : base()
        {
            OgrenciNo = "Bilinmiyor";
            Bolum = Bolum.Yazilim;
            Ortalama = 0;
        }

        public Student(string adSoyad, int yas, string tcNo, string ogrenciNo, Bolum bolum, double ortalamaDegeri)
            : base(adSoyad, yas, tcNo)
        {
            OgrenciNo = ogrenciNo;
            Bolum = bolum;
            Ortalama = ortalamaDegeri;
        }

        public override void MaasHesapla()
        {
            Console.WriteLine("Öğrenciler için maaş hesaplama uygulanmaz.");
        }

        public override void BilgiYazdir()
        {
            Console.WriteLine($"Öğrenci: {AdSoyad} | No: {OgrenciNo} | Bölüm: {Bolum} | ORT: {Ortalama} | Yaş: {Yas}");
        }

        public void DosyayaKaydet()
        {
            File.AppendAllText("ogrenciler.txt", $"{OgrenciNo} - {AdSoyad} - {Bolum} - {Ortalama}{Environment.NewLine}");
        }
    }

    public class Teacher : Person, IKaydet
    {
        public string Brans { get; set; }
        public decimal Maas { get; set; }

        public Teacher() : base()
        {
            Brans = "Bilinmiyor";
            Maas = 0;
        }

        public Teacher(string adSoyad, int yas, string tcNo, string brans, decimal maas)
            : base(adSoyad, yas, tcNo)
        {
            Brans = brans;
            Maas = maas;
        }

        public override void MaasHesapla()
        {
            Console.WriteLine($"{AdSoyad} için maaş: {Maas:C}");
        }

        public override void BilgiYazdir()
        {
            Console.WriteLine($"Öğretmen: {AdSoyad} | Branş: {Brans} | Maaş: {Maas:C} | Yaş: {Yas}");
        }

        public void DosyayaKaydet()
        {
            File.AppendAllText("ogretmenler.txt", $"{AdSoyad} - {Brans} - {Maas}{Environment.NewLine}");
        }
    }

    internal class Program
    {
        private static readonly List<Person> Kisiler = new List<Person>();

        static void Main(string[] args)
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\nÖğrenci Kurs Kayıt Sistemi");
                Console.WriteLine("--------------------------");
                Console.WriteLine("[1] Öğrenci Ekle");
                Console.WriteLine("[2] Öğretmen Ekle");
                Console.WriteLine("[3] Listele");
                Console.WriteLine("[4] Dosyaya Kaydet");
                Console.WriteLine("[5] Dosyadan Oku (ogrenciler.txt)");
                Console.WriteLine("[6] Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine() ?? string.Empty;

                switch (secim)
                {
                    case "1":
                        OgrenciEkle();
                        break;
                    case "2":
                        OgretmenEkle();
                        break;
                    case "3":
                        Listele();
                        break;
                    case "4":
                        DosyayaKaydet();
                        break;
                    case "5":
                        DosyadanOku();
                        break;
                    case "6":
                        isRunning = false;
                        Console.WriteLine("Program sonlandırıldı.");
                        break;
                    default:
                        Console.WriteLine("Hatalı seçim yaptınız.");
                        break;
                }
            }
        }

        private static void OgrenciEkle()
        {
            try
            {
                Console.Write("- Ad Soyad: ");
                string adSoyad = Console.ReadLine() ?? string.Empty;

                Console.Write("- Yaş: ");
                int yas = int.Parse(Console.ReadLine() ?? string.Empty);

                Console.Write("- TC No: ");
                string tcNo = Console.ReadLine() ?? string.Empty;

                Console.Write("- Öğrenci No: ");
                string ogrNo = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("- Bölüm seçiniz: 0-Yazilim 1-Muhasebe 2-Grafik 3-Elektrik");
                int bolumSecim = int.Parse(Console.ReadLine() ?? string.Empty);
                Bolum bolum = (Bolum)bolumSecim;

                Console.Write("- Ortalama: ");
                double ortalama = double.Parse(Console.ReadLine() ?? string.Empty);

                Student ogrenci = new Student(adSoyad, yas, tcNo, ogrNo, bolum, ortalama);
                Kisiler.Add(ogrenci);
                Console.WriteLine("* Öğrenci eklendi. *");
            }
            catch (FormatException)
            {
                Console.WriteLine("Hata: Lütfen sayı girilmesi gereken alana sayı giriniz.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Beklenmedik hata: " + ex.Message);
            }
        }

        private static void OgretmenEkle()
        {
            try
            {
                Console.Write("- Ad Soyad: ");
                string adSoyad = Console.ReadLine() ?? string.Empty;

                Console.Write("- Yaş: ");
                int yas = int.Parse(Console.ReadLine() ?? string.Empty);

                Console.Write("- TC No: ");
                string tcNo = Console.ReadLine() ?? string.Empty;

                Console.Write("- Branş: ");
                string brans = Console.ReadLine() ?? string.Empty;

                Console.Write("- Maaş: ");
                decimal maas = decimal.Parse(Console.ReadLine() ?? string.Empty);

                Teacher ogretmen = new Teacher(adSoyad, yas, tcNo, brans, maas);
                Kisiler.Add(ogretmen);
                Console.WriteLine("* Öğretmen eklendi. *");
            }
            catch (FormatException)
            {
                Console.WriteLine("Hata: Lütfen sayı girilmesi gereken alana sayı giriniz.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Beklenmedik hata: " + ex.Message);
            }
        }

        private static void Listele()
        {
            if (Kisiler.Count == 0)
            {
                Console.WriteLine("Listelenecek kayıt yok.");
                return;
            }

            foreach (Person kisi in Kisiler)
            {
                kisi.BilgiYazdir();
            }
        }

        private static void DosyayaKaydet()
        {
            foreach (Person kisi in Kisiler)
            {
                if (kisi is IKaydet kaydedilebilir)
                {
                    kaydedilebilir.DosyayaKaydet();
                }
            }

            Console.WriteLine("Kayıtlar dosyalara yazıldı.");
        }

        private static void DosyadanOku()
        {
            try
            {
                if (!File.Exists("ogrenciler.txt"))
                {
                    throw new FileNotFoundException("ogrenciler.txt dosyası bulunamadı.");
                }

                string[] satirlar = File.ReadAllLines("ogrenciler.txt");
                Console.WriteLine("Dosya içeriği:");
                foreach (string satir in satirlar)
                {
                    Console.WriteLine(satir);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Beklenmedik hata: " + ex.Message);
            }
        }
    }
}
