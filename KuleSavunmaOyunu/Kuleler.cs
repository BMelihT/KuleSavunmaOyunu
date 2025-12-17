using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KuleSavunmaOyunu
{
    // 1. ABSTRACT CLASS (Soyut Sınıf - 10 Puan)
    // Tek başına kullanılamaz, diğer kulelerin "Babası"dır.
    public abstract class Kule : ISaldirabilir
    {
        public int Fiyat { get; set; }
        public int Hasar { get; set; }
        public int Menzil { get; set; }
        public int AtisHizi { get; set; } // Milisaniye cinsinden bekleme süresi

        public Point Konum { get; set; }
        protected Timer SaldiriTimer; // Ateş etme sıklığını kontrol eder
        protected List<Dusman> SahadakiDusmanlar; // Kimi vuracağını bilmeli

        public Kule(int fiyat, int hasar, int menzil, int atisHizi, Point konum, List<Dusman> dusmanlar)
        {
            Fiyat = fiyat;
            Hasar = hasar;
            Menzil = menzil;
            AtisHizi = atisHizi;
            Konum = konum;
            SahadakiDusmanlar = dusmanlar;

            // Timer ayarları
            SaldiriTimer = new Timer();
            SaldiriTimer.Interval = AtisHizi;
            SaldiriTimer.Tick += (s, e) => Saldir(SahadakiDusmanlar);
            SaldiriTimer.Start();
        }

        // Abstract Metot: Alt sınıflar bunu doldurmak ZORUNDA (Polymorphism)
        public abstract void Saldir(List<Dusman> dusmanlar);

        // Yardımcı Metot: Mesafe Ölçer (Pisagor Teoremi)
        protected double MesafeHesapla(Point kuleYeri, Point dusmanYeri)
        {
            // Düşmanın merkezi ile kulenin merkezi arasındaki mesafe
            return Math.Sqrt(Math.Pow(kuleYeri.X - dusmanYeri.X, 2) + Math.Pow(kuleYeri.Y - dusmanYeri.Y, 2));
        }

        // Oyun bitince timer'ı durdurmak için
        public void Durdur()
        {
            SaldiriTimer.Stop();
        }
    }

    // 2. INHERITANCE (Miras Alma - 10 Puan)
    // --- OKÇU KULESİ ---
    public class OkcuKulesi : Kule
    {
        public OkcuKulesi(Point konum, List<Dusman> dusmanlar)
            : base(100, 15, 150, 800, konum, dusmanlar) { } // Fiyat: 100, Hasar: 15

        // POLYMORPHISM (10 Puan): Okçu en yakındaki TEK düşmana saldırır.
        public override void Saldir(List<Dusman> dusmanlar)
        {
            var hedef = dusmanlar
                .Where(d => !d.OlduMu && MesafeHesapla(this.Konum, d.Gorsel.Location) <= this.Menzil)
                .OrderBy(d => MesafeHesapla(this.Konum, d.Gorsel.Location)) // En yakını seç
                .FirstOrDefault();

            if (hedef != null)
            {
                hedef.HasarAl(this.Hasar);
            }
        }
    }

    // --- TOPÇU KULESİ ---
    public class TopcuKulesi : Kule
    {
        public TopcuKulesi(Point konum, List<Dusman> dusmanlar)
            : base(250, 40, 120, 2000, konum, dusmanlar) { } // Fiyat: 250, Hasar: 40

        // Topçu, vurduğu düşmanın çevresindeki herkese hasar verir (Alan Hasarı)
        public override void Saldir(List<Dusman> dusmanlar)
        {
            var anaHedef = dusmanlar
                .Where(d => !d.OlduMu && MesafeHesapla(this.Konum, d.Gorsel.Location) <= this.Menzil)
                .FirstOrDefault();

            if (anaHedef != null)
            {
                // Ana hedefi vur
                anaHedef.HasarAl(this.Hasar);

                // Patlama etkisi: Ana hedefe 60px yakın olan diğerlerini de vur
                foreach (var yanHedef in dusmanlar.ToList())
                {
                    if (yanHedef != anaHedef && !yanHedef.OlduMu &&
                        MesafeHesapla(anaHedef.Gorsel.Location, yanHedef.Gorsel.Location) < 60)
                    {
                        yanHedef.HasarAl(this.Hasar / 2); // Yan hedef yarı hasar alır
                    }
                }
            }
        }
    }

    //BÜYÜ KULESİ 
    public class BuyuKulesi : Kule
    {
        public BuyuKulesi(Point konum, List<Dusman> dusmanlar)
            : base(350, 25, 180, 1500, konum, dusmanlar) { }

        // Büyücü aynı anda 3 düşmana saldırır
        public override void Saldir(List<Dusman> dusmanlar)
        {
            var hedefler = dusmanlar
                .Where(d => !d.OlduMu && MesafeHesapla(this.Konum, d.Gorsel.Location) <= this.Menzil)
                .Take(3) // İlk 3 düşman
                .ToList();

            foreach (var h in hedefler)
            {
                h.HasarAl(this.Hasar);
            }
        }
    }

    // LAZER KULESİ
    public class LazerKulesi : Kule
    {
        public LazerKulesi(Point konum, List<Dusman> dusmanlar)
            : base(500, 5, 200, 100, konum, dusmanlar) { } // Çok seri ama az hasar

        // Lazer tek hedefe kilitlenip seri hasar verir 
        public override void Saldir(List<Dusman> dusmanlar)
        {
            var hedef = dusmanlar
                .Where(d => !d.OlduMu && MesafeHesapla(this.Konum, d.Gorsel.Location) <= this.Menzil)
                .FirstOrDefault();

            if (hedef != null)
            {
                hedef.HasarAl(this.Hasar);
            }
        }
    }
}