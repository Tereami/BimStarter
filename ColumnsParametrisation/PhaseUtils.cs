using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColumnsParametrisation
{
    internal class PhaseUtils
    {
        public static void LoadPhases(Document doc, out ElementId mainPhaseId, out ElementId onePhaseId)
        {
            mainPhaseId = FindPhase(doc, new[] { 0, 110001 }, new[] { "Новая конструкция", "New Construction" });

            onePhaseId = FindPhase(doc, new[] { 401574, 232823 }, new[] { "Подсчет на 1 конструкцию", "Calculate for 1 detail" });
        }

        private static ElementId FindPhase(Document doc, int[] Ids, string[] Names)
        {
            Phase phase = null;
            foreach (int id in Ids)
            {
                ElementId phaseId = GetElementId(id);
                phase = doc.GetElement(phaseId) as Phase;
                if (phase != null) break;
            }

            if (phase != null) return phase.Id;

            List<Phase> allPhases = new FilteredElementCollector(doc)
                .OfClass(typeof(Phase))
                .Cast<Phase>()
                .ToList();

            phase = allPhases.FirstOrDefault(i => Names.Contains(i.Name));

            if (phase == null) throw new Exception($"Failed to find Phase {Names[0]}! Please use a BimStarter Template!");
            return phase.Id;
        }


        private static ElementId GetElementId(int Id)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            ElementId elid = new ElementId(Id);

#else
            ElementId elid = new ElementId((long)Id);
#endif
            return elid;
        }
    }
}
