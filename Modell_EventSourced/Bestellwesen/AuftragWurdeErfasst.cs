﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modell.Bestellwesen
{
    public sealed class AuftragWurdeErfasst
    {
        public Guid Auftrag;
        public Guid Produkt;
        public Guid Kunde;
        public int Menge;

        public override string ToString()
        {
            return "Ein Auftrag über " + Menge + "x {ID:" + Produkt + "} für {ID:" + Kunde + "} wurde erfasst [" + Auftrag + "].";
        }
    }


}
