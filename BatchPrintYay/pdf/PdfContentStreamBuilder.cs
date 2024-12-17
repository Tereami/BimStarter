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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;

namespace BatchPrintYay.pdf
{
    public class PdfContentStreamBuilder
    {

        #region Constants

        private const byte BYTE_BACKSPACE = 8;
        private const byte BYTE_HORIZONTAL_TAB = 9;
        private const byte BYTE_LINE_FEED = 10;
        private const byte BYTE_FORM_FEED = 12;
        private const byte BYTE_CARRIAGE_RETURN = 13;
        private const byte BYTE_SPACE = 32;
        private const byte BYTE_ROUND_OPENING_BRACKET = 40;
        private const byte BYTE_ROUND_CLOSING_BRACKET = 41;
        private const byte BYTE_ANGLE_OPENING_BRACKET = 60;
        private const byte BYTE_ANGLE_CLOSING_BRACKET = 62;
        private const byte BYTE_SQUARE_OPENING_BRACKET = 91;
        private const byte BYTE_SQUARE_CLOSING_BRACKET = 93;

        private const byte BYTE_REVERSE_SOLIDUS = 92;
        private const byte BYTE_ESCAPE_CHAR = 92;

        private const byte BYTE_CHAR_r = 114;
        private const byte BYTE_CHAR_n = 110;
        private const byte BYTE_CHAR_t = 116;
        private const byte BYTE_CHAR_b = 98;
        private const byte BYTE_CHAR_f = 102;

        private readonly byte[] BYTES_DOUBLE_ANGLE_OPENING_BRACKETS = { BYTE_ANGLE_OPENING_BRACKET, BYTE_ANGLE_OPENING_BRACKET };
        private readonly byte[] BYTES_DOUBLE_ANGLE_CLOSING_BRACKETS = { BYTE_ANGLE_CLOSING_BRACKET, BYTE_ANGLE_CLOSING_BRACKET };

        private readonly byte[] BYTES_LINE_BREAK_WINDOWS = { BYTE_CARRIAGE_RETURN, BYTE_LINE_FEED };
        private readonly byte[] BYTES_LINE_BREAK_UNIX = { BYTE_LINE_FEED };

        private readonly byte[] BYTES_REVERSE_SOLIDUS_AND_CHAR_r = { BYTE_REVERSE_SOLIDUS, BYTE_CHAR_r };
        private readonly byte[] BYTES_REVERSE_SOLIDUS_AND_CHAR_n = { BYTE_REVERSE_SOLIDUS, BYTE_CHAR_n };
        private readonly byte[] BYTES_REVERSE_SOLIDUS_AND_CHAR_t = { BYTE_REVERSE_SOLIDUS, BYTE_CHAR_t };
        private readonly byte[] BYTES_REVERSE_SOLIDUS_AND_CHAR_b = { BYTE_REVERSE_SOLIDUS, BYTE_CHAR_b };
        private readonly byte[] BYTES_REVERSE_SOLIDUS_AND_CHAR_f = { BYTE_REVERSE_SOLIDUS, BYTE_CHAR_f };

        #endregion

        #region Private variables

        private List<PdfObject> _content = new List<PdfObject>();

        #endregion

        #region Push

        public void Push(PdfObject pdfObject)
        {
            _content.Add(pdfObject);
        }

        #endregion

        #region Push

        public void Push(IEnumerable<PdfObject> pdfObject)
        {
            _content.AddRange(pdfObject);
        }

        #endregion

        #region Pop

        public List<PdfObject> Pop(int count)
        {
            List<PdfObject> retList = _content.GetRange(_content.Count - count, count);
            _content.RemoveRange(_content.Count - count, count);
            return retList;
        }

        #endregion

        #region GetBytes

        public byte[] GetBytes()
        {
            List<byte> rawContent = new List<byte>();

            for (int index = 0; index < _content.Count; index++)
            {
                this.CreateRawContent(rawContent, _content[index]);
            }

            return rawContent.ToArray();
        }

        #endregion

        #region CreateRawContent

        private void CreateRawContent(List<byte> rawContent, PdfObject pdfObject)
        {
            Type pdfObjectType = pdfObject.GetType();

            if (pdfObjectType == typeof(PdfName))
            {
                rawContent.AddRange(pdfObject.GetBytes());
            }
            else if (pdfObjectType == typeof(PdfLiteral))
            {
                string t = pdfObject.ToString();

                if (t == "EMC" || t == "BMC" || t == "BDC")
                {
                    rawContent.Add(BYTE_SPACE);
                }

                rawContent.AddRange(pdfObject.GetBytes());

                if (t == "BT")
                {
                    rawContent.AddRange(BYTES_LINE_BREAK_WINDOWS);
                }

                if (t == "EMC" || t == "BMC" || t == "BDC")
                {
                    rawContent.Add(BYTE_SPACE);
                }
            }
            else if (pdfObjectType == typeof(PdfNumber))
            {
                rawContent.AddRange(pdfObject.GetBytes());
            }
            else if (pdfObjectType == typeof(PdfString))
            {
                rawContent.Add(BYTE_ROUND_OPENING_BRACKET);

                byte[] objectBuffer = pdfObject.GetBytes();

                foreach (byte objectByte in objectBuffer)
                {
                    switch (objectByte)
                    {
                        case BYTE_CARRIAGE_RETURN:
                            {
                                rawContent.AddRange(BYTES_REVERSE_SOLIDUS_AND_CHAR_r);
                                break;
                            }
                        case BYTE_LINE_FEED:
                            {
                                rawContent.AddRange(BYTES_REVERSE_SOLIDUS_AND_CHAR_n);
                                break;
                            }
                        case BYTE_HORIZONTAL_TAB:
                            {
                                rawContent.AddRange(BYTES_REVERSE_SOLIDUS_AND_CHAR_t);
                                break;
                            }
                        case BYTE_BACKSPACE:
                            {
                                rawContent.AddRange(BYTES_REVERSE_SOLIDUS_AND_CHAR_b);
                                break;
                            }
                        case BYTE_FORM_FEED:
                            {
                                rawContent.AddRange(BYTES_REVERSE_SOLIDUS_AND_CHAR_f);
                                break;
                            }
                        case BYTE_ROUND_OPENING_BRACKET:
                        case BYTE_ROUND_CLOSING_BRACKET:
                        case BYTE_REVERSE_SOLIDUS:
                            {
                                rawContent.Add(BYTE_ESCAPE_CHAR);
                                rawContent.Add(objectByte);
                                break;
                            }
                        default:
                            {
                                rawContent.Add(objectByte);
                                break;
                            }
                    }
                }

                rawContent.Add(BYTE_ROUND_CLOSING_BRACKET);
            }
            else if (pdfObjectType == typeof(PdfDictionary))
            {
                rawContent.AddRange(BYTES_DOUBLE_ANGLE_OPENING_BRACKETS);

                PdfDictionary dict = pdfObject as PdfDictionary;

                foreach (PdfObject key in dict.Keys)
                {
                    CreateRawContent(rawContent, key);

                    PdfName keyName = key as PdfName;

                    this.CreateRawContent(rawContent, dict.Get(keyName));
                }

                rawContent.AddRange(BYTES_DOUBLE_ANGLE_CLOSING_BRACKETS);
            }
            else if (pdfObjectType == typeof(PdfArray))
            {
                rawContent.Add(BYTE_SQUARE_OPENING_BRACKET);

                PdfArray pa = pdfObject as PdfArray;

                foreach (PdfObject obj in pa.ArrayList)
                {
                    this.CreateRawContent(rawContent, obj);
                }

                rawContent.Add(BYTE_SQUARE_CLOSING_BRACKET);
            }
            else
            {
                rawContent.AddRange(pdfObject.GetBytes());
            }

            rawContent.Add(BYTE_SPACE);
        }

        #endregion

    }
}
