﻿/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Scryber.Drawing;

namespace Scryber.Text
{
    class PDFXMLTextReader : PDFTextReader
    {

        private string _text;

        public string Text
        {
            get { return _text; }
        }

        public override int Length
        {
            get { return string.IsNullOrEmpty(_text) ? 0 : _text.Length; }
        }

        private bool _preserve;
        public bool PreserveWhiteSpace
        {
            get { return _preserve; }
        }


        private System.IO.TextReader _innertr;
        private System.Xml.XmlReader _innerreader;
        
        protected System.Xml.XmlReader InnerReader
        {
            get { return _innerreader; }
        }


        public PDFXMLTextReader(string text)
            : this(text,false)
        {
        }

        public PDFXMLTextReader(string text, bool preservewhitespace)
            : base()
        {
            this._preserve = preservewhitespace;
            this._text = text;
            if (!preservewhitespace)
            {
                this._text = this._text.Trim();
            }

            this.ResetTextMarkers();
        }

        protected override void ResetTextMarkers()
        {
            if (this._innerreader != null)
                this._innerreader.Close();

            if (this._innertr != null)
                this._innertr.Dispose();

            string content = "<Root>" + this._text + "</Root>";
            this._innertr = new System.IO.StringReader(content);

            this._innerreader = System.Xml.XmlReader.Create(this._innertr);
            this._innerreader.Read();//Move past the <Root> Component
        }

        private PDFTextOp _op = null;

        public override bool EOF
        {
            get { return this.InnerReader.EOF; }
        }

        public override bool Read()
        {
            PDFTextOp op = null;
            string style;

            while (this.InnerReader.Read())
            {
                if (this.InnerReader.NodeType == XmlNodeType.Element)
                {
                    
                    if (this.InnerReader.Name.Equals("br", StringComparison.CurrentCultureIgnoreCase))
                    {
                        op = new PDFTextNewLineOp();
                        break;
                    }
                    else if (this.IsFontStyleOp(this.InnerReader.Name, out style))
                    {
                        op = new PDFTextFontOp(style, true);
                        break;
                    }
                    else if (this.InnerReader.Name.Equals("span", StringComparison.CurrentCultureIgnoreCase))
                        throw new NotSupportedException("Span is not a currently supported Component");
                }
                else if (this.InnerReader.NodeType == XmlNodeType.Text)
                {
                    string text = this.StripWhiteSpace(this.InnerReader.Value.Trim(), op != null);
                    op = new PDFTextDrawOp(text);
                    break;
                }
                else if (this.InnerReader.NodeType == XmlNodeType.EndElement)
                {
                    if (this.IsFontStyleOp(this.InnerReader.Name, out style))
                    {
                        op = new PDFTextFontOp(style, false);
                        
                        break;
                    }
                    
                }
            }
            this._op = op;
            
            return op != null;
        }

        protected virtual bool IsFontStyleOp(string opname, out string style)
        {
            if (string.IsNullOrEmpty(opname) || opname.Length > 1)
            {
                style = string.Empty;
                return false;
            }
            char op = opname.ToLower()[0];

            switch (op)
            {
                case('b'):
                case('i'):
                    style = op.ToString();
                    return true;

                default:
                    style = string.Empty;
                    return false;

            }
        }

        protected string StripWhiteSpace(string text, bool isfirst)
        {
            bool startisspace = char.IsWhiteSpace(text, 0);
            bool endisspace = char.IsWhiteSpace(text, text.Length - 1);
            string[] lines = text.Split('\r', '\n', '\t');
            if (lines.Length == 1)
            {
                if (startisspace && !isfirst)
                {
                    if (endisspace)
                        return " " + lines[0].Trim() + " ";
                    else
                        return " " + lines[0].Trim();
                }
                else if (endisspace)
                    return lines[0].Trim() + " ";
                else
                    return lines[0].Trim();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < lines.Length; i++)
                {
                    string trimmed = lines[i].Trim();
                    if (i > 0 || startisspace)
                        sb.Append(" ");
                    sb.Append(trimmed);

                    if (i == lines.Length - 1 && endisspace)
                        sb.Append(" ");

                    
                }
                return sb.ToString();
            }
        }

        public override PDFTextOp Value
        {
            get { return this._op; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.InnerReader != null)
                    this.InnerReader.Close();
                if (this._innertr != null)
                    this._innertr.Dispose();
            }
            base.Dispose(disposing);
            
        }
    }
}
