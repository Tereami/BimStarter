using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace RebarTools
{
    public class MyBar
    {
        public Element RevitBar;

        public MyBar(Element bar)
        {
            RevitBar = bar;
        }

        public void SetSolidInView(View3D v, bool AsBody)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022
            if (RevitBar is Rebar)
            {
                Rebar bar = RevitBar as Rebar;
                bar.SetSolidInView(v, AsBody);
            }
            if (RevitBar is RebarInSystem)
            {
                RebarInSystem bar = RevitBar as RebarInSystem;
                bar.SetSolidInView(v, AsBody);
            }
#endif
        }

        public void SetUnobscuredInView(View v, bool IsUnobsqured)
        {
            if (RevitBar is Rebar)
            {
                Rebar bar = RevitBar as Rebar;
                bar.SetUnobscuredInView(v, IsUnobsqured);
            }
            if (RevitBar is RebarInSystem)
            {
                RebarInSystem bar = RevitBar as RebarInSystem;
                bar.SetUnobscuredInView(v, IsUnobsqured);
            }
        }

    }
}
