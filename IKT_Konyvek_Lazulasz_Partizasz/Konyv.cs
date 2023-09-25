using System;

namespace IKT_Konyvek_Lazulasz_Partizasz{
    public class Konyv{

        int id;
        String konyv, szerzo, kiado;
        int ar;
        int raktaron;

        public int getID(){return this.id;}
        
        public String Cim{
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

        public int Ar{
            get{return ar;}
            set{ar = value;}
        }

        public Int32 Raktaron{
            get{return raktaron;}
            set{raktaron = value;}
        }

        public Konyv(Int32 id, String konyv, String szerzo, String kiado, int ar, Int32 raktaron){
            this.id = id;
            this.konyv = konyv;
            this.szerzo = szerzo;
            this.kiado = kiado;
            this.ar = ar;
            this.raktaron = raktaron;
        }


    }
}
