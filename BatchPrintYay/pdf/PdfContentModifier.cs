#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion

using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;


namespace BatchPrintYay.pdf
{
    public class PdfContentModifier
    {

        #region Private variables

        private Dictionary<string, PdfContentOperatorHandler> _operators = new Dictionary<string, PdfContentOperatorHandler>();
        private Stack<PdfContentStreamBuilder> _contentStreamBuilderStack = new Stack<PdfContentStreamBuilder>();
        private PdfResourceDictionary _resourceDictionaryStack = new PdfResourceDictionary();

        #endregion

        #region Modify

        public void Modify(PdfReader reader, int pageNumber)
        {
            PdfDictionary pageDictionary = reader.GetPageN(pageNumber);
            PdfDictionary resourcesDictionary = pageDictionary.GetAsDict(PdfName.RESOURCES);

            byte[] contentBytes = reader.GetPageContent(pageNumber);

            contentBytes = this.Modify(contentBytes, resourcesDictionary);

            reader.SetPageContent(pageNumber, contentBytes);
        }

        #endregion

        #region Modify

        public byte[] Modify(byte[] contentBytes, PdfDictionary resourcesDictionary)
        {
            _contentStreamBuilderStack.Push(new PdfContentStreamBuilder());
            _resourceDictionaryStack.Push(resourcesDictionary);
            PRTokeniser tokeniser = new PRTokeniser(new RandomAccessFileOrArray(contentBytes));
            PdfContentParser ps = new PdfContentParser(tokeniser);

            List<PdfObject> operands = new List<PdfObject>();

            try
            {
                while (ps.Parse(operands).Count > 0)
                {
                    PdfLiteral oper = (PdfLiteral)operands[operands.Count - 1];

                    //System.Diagnostics.Trace.WriteLine("[Debug] Opr: " + oper.ToString());

                    PdfContentOperatorHandler operHandler = null;

                    if (_operators.TryGetValue(oper.ToString(), out operHandler))
                    {
                        operands = operHandler(oper, operands);
                    }

                    _contentStreamBuilderStack.Peek().Push(operands);
                }
            }
            catch
            {
                string errMsg = "PDF OPERANDS ERROR. REMOVE TRANSPARENT PNG IMAGES FROM YOUR TITLEBLOCK FAMILY";
                Autodesk.Revit.UI.TaskDialog.Show("Error", errMsg);
                throw new Exception(errMsg);
            }

            _resourceDictionaryStack.Pop();
            return _contentStreamBuilderStack.Pop().GetBytes();
        }

        #endregion

        #region RegisterOperator

        public void RegisterOperator(string oper, PdfContentOperatorHandler callback)
        {
            _operators.Add(oper, callback);
        }

        #endregion

        #region ResourceDictionary

        public PdfResourceDictionary ResourceDictionary
        {
            get { return _resourceDictionaryStack; }
        }

        #endregion

    }
}
