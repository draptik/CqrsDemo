﻿using System;
using Infrastruktur.Common;
using Infrastruktur.EventSourcing;
using Modell.Kunden;
using Modell.Warenwirtschaft;

namespace Modell.Bestellwesen
{

    public class Auftrag : AggregateRoot
    {
        private readonly AuftragProjektion _zustand;

        public static AggregateEvents AggregateEvents = new AggregateEvents()
            .AggregateIsAffectedBy<AuftragWurdeErfasst>(e => e.Auftrag)
            .AggregateIsAffectedBy<AuftragWurdeErfuellt>(e => e.Auftrag);

        public Auftrag(AuftragProjektion zustand, Action<Ereignis> eventsink):base(eventsink)
        {
            _zustand = zustand;
        }

        public Guid Produkt
        {
            get { return _zustand.Produkt; }
        }

        public Guid Id
        {
            get { return _zustand.Auftrag; }
        }

        public Guid Kunde
        {
            get { return _zustand.Kunde; }
        }


        public void Erfassen(Guid auftrag, Guid produkt, int menge, Kunde kunde)
        {
            if (_zustand.Erfasst) return;
            if (menge<1) throw new VorgangNichtAusgefuehrt("Die Bestellmenge muß > 0 sein");

            kunde.AuftragsannahmePruefen();
            WurdeErfasst(auftrag, produkt, menge, kunde.Id);
        }

        public void Ausfuehren(Lagerposten produkt)
        {
            if (!_zustand.Erfasst) throw new NichtGefunden("Auftrag");
            if (_zustand.Erfuellt) return;

            if (!produkt.BestandPruefen(_zustand.Menge)) throw new VorgangNichtAusgefuehrt("Die Bestellung überschreitet den aktuellen Lagerbestand.");

            produkt.Ausliefern(_zustand.Menge);

            WurdeAusgefuehrt(_zustand.Produkt, _zustand.Menge);
        }



        private void WurdeErfasst(Guid auftrag, Guid produkt, int menge, Guid kunde)
        {
            Publish(new AuftragWurdeErfasst { Auftrag = auftrag, Kunde = kunde, Produkt = produkt, Menge = menge });
        }

        private void WurdeAusgefuehrt(Guid produkt, int menge)
        {
            Publish(new AuftragWurdeErfuellt {Auftrag = Id, Kunde = Kunde, Produkt = produkt, Menge = menge });
        }



    }
}


