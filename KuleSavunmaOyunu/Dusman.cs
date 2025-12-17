using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace KuleSavunmaOyunu
{
    public class Dusman
    {
        public int MaxCan { get; private set; }
        public int Can { get; private set; }
        public int Hiz { get; private set; }
        public int Odul { get; private set; }
        public int HasarGucu { get; private set; }

        public PictureBox Gorsel { get; private set; }
        public bool HedefeUlastiMi { get; private set; } = false;
        public bool OlduMu { get; private set; } = false;

        private List<Point> yolRotasi;
        private int suankiHedefIndex = 0;

        public Dusman(int can, int hiz, int odul, int hasar, Image resim, List<Point> rota)
        {
            this.MaxCan = can;
            this.Can = can;
            this.Hiz = hiz;
            this.Odul = odul;
            this.HasarGucu = hasar;
            this.yolRotasi = rota;

            Gorsel = new PictureBox();
            Gorsel.Image = resim;
            Gorsel.Size = new Size(40, 40);
            Gorsel.SizeMode = PictureBoxSizeMode.StretchImage;
            Gorsel.BackColor = Color.Transparent;

            if (yolRotasi != null && yolRotasi.Count > 0)
                Gorsel.Location = yolRotasi[0];

            // Can barı çizimi için
            Gorsel.Paint += Gorsel_Paint;
        }

        // --- TİTREMEYİ ÖNLEYEN KUSURSUZ HAREKET KODU ---
        public void HareketEt()
        {
            if (HedefeUlastiMi || yolRotasi == null || yolRotasi.Count == 0) return;

            Point hedef = yolRotasi[suankiHedefIndex];
            int x = Gorsel.Location.X;
            int y = Gorsel.Location.Y;

            // X EKSENİ (YATAY)
            if (x < hedef.X)
            {
                // Eğer kalan mesafe Hız'dan küçükse direkt hedefe yapış! (Titremeyi Çözen Satır)
                if (hedef.X - x <= Hiz) x = hedef.X;
                else x += Hiz;
            }
            else if (x > hedef.X)
            {
                if (x - hedef.X <= Hiz) x = hedef.X;
                else x -= Hiz;
            }

            // Y EKSENİ (DİKEY)
            if (y < hedef.Y)
            {
                if (hedef.Y - y <= Hiz) y = hedef.Y;
                else y += Hiz;
            }
            else if (y > hedef.Y)
            {
                if (y - hedef.Y <= Hiz) y = hedef.Y;
                else y -= Hiz;
            }

            // Konumu güncelle
            Gorsel.Location = new Point(x, y);

            // Hedefe tam olarak vardık mı?
            if (x == hedef.X && y == hedef.Y)
            {
                suankiHedefIndex++; // Sonraki köşeye geç

                if (suankiHedefIndex >= yolRotasi.Count)
                {
                    HedefeUlastiMi = true;
                }
            }
        }

        private void Gorsel_Paint(object sender, PaintEventArgs e)
        {
            // Can Barı Çizimi
            if (MaxCan <= 0) return;

            int barYukseklik = 5;
            int barGenislik = Gorsel.Width;

            // Kırmızı Zemin
            using (Brush kirmizi = new SolidBrush(Color.Red))
                e.Graphics.FillRectangle(kirmizi, 0, 0, barGenislik, barYukseklik);

            // Yeşil Can
            float oran = (float)Can / MaxCan;
            int yesilGenislik = (int)(barGenislik * oran);

            using (Brush yesil = new SolidBrush(Color.LimeGreen))
                e.Graphics.FillRectangle(yesil, 0, 0, yesilGenislik, barYukseklik);

            // Çerçeve
            using (Pen kalem = new Pen(Color.Black))
                e.Graphics.DrawRectangle(kalem, 0, 0, barGenislik - 1, barYukseklik);
        }

        public void HasarAl(int miktar)
        {
            Can -= miktar;
            Gorsel.Invalidate(); // Can barını güncelle
            if (Can <= 0)
            {
                Can = 0;
                OlduMu = true;
            }
        }
    }
}