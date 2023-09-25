using System;

namespace IKT_Konyvek_Lazulasz_Partizasz{
    public class Konyv{
        
        int id;
        String konyv, szerzo, kiado;
        double ar;
        int raktaron;
        
        public Int32 Id{
            get{return id;}
            set{id = value;}
        }

        public String Konyv1{
            get{return konyv;}
            set{konyv = value;}
        }

        public String Szerzo{
            get{return szerzo;}
            set{szerzo = value;}
        }

        public String Kiado{
            get{return kiado;}
            set{kiado = value;}
        }

        public Double Ar{
            get{return ar;}
            set{ar = value;}
        }

        public Int32 Raktaron{
            get{return raktaron;}
            set{raktaron = value;}
        }

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
