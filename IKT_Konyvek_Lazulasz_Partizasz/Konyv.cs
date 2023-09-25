using System;

namespace IKT_Konyvek_Lazulasz_Partizasz{
    public class Konyv{
        int id;
        String konyv, szerzo, kiado;
        double ar;
        int raktaron;


        public Konyv(Int32 id, String konyv, String szerzo, String kiado, Double ar, Int32 raktaron){
            this.id = id;
            this.konyv = konyv;
            this.szerzo = szerzo;
            this.kiado = kiado;
            this.ar = ar;
            this.raktaron = raktaron;
        }


    }
}
