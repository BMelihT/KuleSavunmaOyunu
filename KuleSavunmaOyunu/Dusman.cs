using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KuleSavunmaOyunu
{
    public class Dusman
    {
        // Özellikler (Encapsulation)
        public int Can { get; private set; }
        public int MaxCan { get; private set; }
        public int Hiz { get; private set; } // Her tick'te kaç piksel gidecek
        public int Odul { get; private set; } // Ölünce vereceği altın
        public int HasarGucu { get; private set; } // Kaleye varırsa vereceği hasar

        public PictureBox Gorsel { get; private set; } // Ekranda görünen resim
        public bool OlduMu { get; private set; } = false;
        public bool HedefeUlastiMi { get; private set; } = false;

        // Yol takibi için gerekli değişkenler
        private List<Point> yolNoktalari;
        private int hedefNoktaIndeksi = 0;

        // YAPICI METOT (Constructor)
        public Dusman(int can, int hiz, int odul, int hasarGucu, Image resim, List<Point> yol)
        {
            Can = can;
            MaxCan = can;
            Hiz = hiz;
            Odul = odul;
            HasarGucu = hasarGucu;
            yolNoktalari = yol;

            // Görseli oluştur
            Gorsel = new PictureBox();
            Gorsel.Image = resim;
            Gorsel.SizeMode = PictureBoxSizeMode.StretchImage;
            Gorsel.Size = new Size(40, 40); // Kareden biraz küçük olsun
            Gorsel.BackColor = Color.Transparent;
            Gorsel.Tag = this; // Tıklama işlemleri için kendini sakla

            // Başlangıç konumuna koy
            if (yolNoktalari != null && yolNoktalari.Count > 0)
            {
                Gorsel.Location = yolNoktalari[0];
            }
        }

        // Hareket Mantığı: Bir sonraki noktaya doğru yürür
        public void HareketEt()
        {
            if (hedefNoktaIndeksi >= yolNoktalari.Count)
            {
                HedefeUlastiMi = true; // Yol bitti
                Gorsel.Dispose(); // Ekrandan sil
                return;
            }

            Point hedef = yolNoktalari[hedefNoktaIndeksi];
            int currentX = Gorsel.Location.X;
            int currentY = Gorsel.Location.Y;

            // X Ekseninde Hareket
            if (currentX < hedef.X) currentX += Math.Min(Hiz, hedef.X - currentX);
            else if (currentX > hedef.X) currentX -= Math.Min(Hiz, currentX - hedef.X);

            // Y Ekseninde Hareket
            if (currentY < hedef.Y) currentY += Math.Min(Hiz, hedef.Y - currentY);
            else if (currentY > hedef.Y) currentY -= Math.Min(Hiz, currentY - hedef.Y);

            Gorsel.Location = new Point(currentX, currentY);

            // Hedefe çok yaklaştıysa bir sonraki noktaya geç
            if (Math.Abs(currentX - hedef.X) <= Hiz && Math.Abs(currentY - hedef.Y) <= Hiz)
            {
                hedefNoktaIndeksi++;
            }
        }

        // Can Azaltma
        public void HasarAl(int miktar)
        {
            Can -= miktar;
            if (Can <= 0)
            {
                Can = 0;
                OlduMu = true;
                Gorsel.Dispose(); // Öldü, ekrandan kaldır
            }
        }
    }
}   