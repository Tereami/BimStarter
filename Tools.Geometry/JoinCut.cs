using Autodesk.Revit.DB;

namespace Tools.Geometry
{
    public static class JoinCut
    {
        /// <summary>
        /// Вырезает экземпляр семейства c пустотным элементов из другого элемента в модели. Требуется открытая транзакция
        /// </summary>
        public static bool CutElement(Document doc, Element elemForCut, Element elemWithVoid)
        {
            //Проверяю, можно ли вырезать геометрию из данного элемента
            bool check1 = InstanceVoidCutUtils.CanBeCutWithVoid(elemForCut);

            //проверяю, есть ли в семействе полый элемент и разрешено ли вырезание
            bool check2 = InstanceVoidCutUtils.IsVoidInstanceCuttingElement(elemWithVoid);

            //проверяю, существует ли уже вырезание
            bool check3 = InstanceVoidCutUtils.InstanceVoidCutExists(elemForCut, elemWithVoid);

            //Если одно из условий не выполняется - возвращаю false
            if (!check1 || !check2 || check3)
            {
                return false;
            }

            try
            {
                InstanceVoidCutUtils.AddInstanceVoidCut(doc, elemForCut, elemWithVoid);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
