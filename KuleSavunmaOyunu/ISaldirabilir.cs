using System.Collections.Generic; 

namespace KuleSavunmaOyunu
{
    public interface ISaldirabilir
    {
        // Bu bir kuraldır: "Beni kullanan herkes Saldir komutunu bilmek zorunda" der.
        void Saldir(List<Dusman> dusmanlar);
    }
}