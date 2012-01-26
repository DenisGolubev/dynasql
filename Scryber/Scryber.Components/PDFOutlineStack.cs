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
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Native;

namespace Scryber
{
    internal class PDFOutlineStack : IArtefactCollection
    {

        #region ivars

        private Stack<PDFOutlineRef> _stack;
        private PDFOutlineRefCollection _roots;
        private string _name;

        #endregion

        /// <summary>
        /// Gets or sets the name of the collection
        /// </summary>
        string IArtefactCollection.CollectionName
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the collection of root outline items
        /// </summary>
        internal PDFOutlineRefCollection Roots
        {
            get { return _roots; }
        }

        /// <summary>
        /// Creates a new empty OutlineStack with the specified name
        /// </summary>
        /// <param name="name"></param>
        internal PDFOutlineStack(string name)
            : this(name, new PDFOutlineRefCollection())
        {
        }

        internal PDFOutlineStack(string name, PDFOutlineRefCollection roots)
        {
            this._name = name;
            this._roots = roots;
            this._stack = new Stack<PDFOutlineRef>();
        }

        internal void Clear()
        {
            this._roots.Clear();
            this._stack.Clear();
        }

        object IArtefactCollection.Register(IArtefactEntry entry)
        {
            if (entry is PDFOutlineRef)
                return this.Push((PDFOutlineRef)entry);
            else
                throw RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, entry, typeof(PDFOutlineRef).FullName);
        }

        internal object Push(PDFOutlineRef outlineref)
        {
            
            if (null == outlineref)
                throw RecordAndRaise.ArgumentNull("outlineref");
            if (null == outlineref.Outline)
                throw RecordAndRaise.ArgumentNull("outlineref.Outline");
            if (null == outlineref.Outline.BelongsTo)
                throw RecordAndRaise.ArgumentNull("outlineref.Outline.BelongsTo");

            if (_stack.Count == 0)
                _roots.Add(outlineref);
            else
                _stack.Peek().AddChild(outlineref);

            _stack.Push(outlineref);

            return outlineref.Outline.BelongsTo;
        }

        void IArtefactCollection.Close(object result)
        {
            this.Pop((IPDFComponent)result);
        }

        internal void Pop(IPDFComponent comp)
        {
            PDFOutlineRef last = _stack.Pop();
            if (last.Outline.BelongsTo != comp)
                throw RecordAndRaise.Operation(Errors.UnbalancedOutlineStack);
        }

        public PDFObjectRef RenderToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (this.Roots.Count > 0)
            {
                PDFObjectRef outlines = writer.BeginObject();
                writer.BeginDictionary();
                writer.WriteDictionaryNameEntry("Type", "Outlines");
                PDFObjectRef first, last;
                int count;

                this.RenderOutlineCollection(this.Roots, outlines, context, writer, out first, out last, out count);

                if (null != first)
                    writer.WriteDictionaryObjectRefEntry("First", first);
                if (null != last)
                    writer.WriteDictionaryObjectRefEntry("Last", last);
                if (count > 0)
                    writer.WriteDictionaryNumberEntry("Count", count);

                writer.EndDictionary();
                writer.EndObject();//outlines

                return outlines;
            }
            else
                return null;
        }

        private void RenderOutlineCollection(PDFOutlineRefCollection col, PDFObjectRef parent, PDFRenderContext context, PDFWriter writer, out PDFObjectRef first, out PDFObjectRef last, out int count)
        {
            PDFObjectRef prev = null;
            List<PDFObjectRef> previousitems = new List<PDFObjectRef>();
            first = null;
            last = null;
            count = 0;
            int i = 0;
            do
            {
                int innercount;
                PDFObjectRef one = RenderOutlineItem(col[i], parent, prev, context, writer, out innercount);
                if (i == 0)
                {
                    first = one;
                }
                if (i == col.Count - 1)
                {
                    last = one;
                }
                i++;
                prev = one;
                previousitems.Add(one);
                count += innercount;

            } while (i < col.Count);

            //close all the dictionaries and object in reverse order
            //adding the next entry first if we are not the last entry
            for (int p = previousitems.Count - 1; p >= 0; p--)
            {
                if (p < previousitems.Count - 1)
                {
                    writer.WriteDictionaryObjectRefEntry("Next", previousitems[p + 1]);
                }
                writer.EndDictionary();
                writer.EndObject();
            }


        }

        private PDFObjectRef RenderOutlineItem(PDFOutlineRef outlineref, PDFObjectRef parent, PDFObjectRef prev, PDFRenderContext context, PDFWriter writer, out int count)
        {
            PDFOutline outline = outlineref.Outline;
            PDFColor c = outlineref.GetColor();
            FontStyle fs = outlineref.GetFontStyle();
            bool isopen = outlineref.GetIsOpen();
            count = 1;//this one
            PDFObjectRef item = writer.BeginObject();

            writer.BeginDictionary();
            writer.WriteDictionaryObjectRefEntry("Parent", parent);
            writer.WriteDictionaryStringEntry("Title", outline.Title);
            writer.WriteDictionaryStringEntry("Dest", outline.DestinationName);
            if (null != c)
            {
                writer.BeginDictionaryEntry("C");
                writer.BeginArray();
                writer.WriteRealS(c.Red.Value, c.Green.Value, c.Blue.Value);
                writer.EndArray();
                writer.EndDictionaryEntry();
            }

            if (fs != FontStyle.Regular)
            {
                int f = 0;
                if ((fs & FontStyle.Bold) > 0)
                    f = 2;
                if ((fs & FontStyle.Italic) > 0)
                    f += 1;
                writer.WriteDictionaryNumberEntry("F", f);
            }

            if (null != prev)
                writer.WriteDictionaryObjectRefEntry("Prev", prev);

            if (outlineref.HasInnerItems)
            {
                int opencount;
                PDFObjectRef childfirst, childlast;
                this.RenderOutlineCollection(outlineref.InnerItems, item, context, writer, out childfirst, out childlast, out opencount);

                writer.WriteDictionaryObjectRefEntry("First", childfirst);
                writer.WriteDictionaryObjectRefEntry("Last", childlast);
                if (opencount > 0)
                {
                    if (isopen)
                    {
                        writer.WriteDictionaryNumberEntry("Count", opencount);
                        count += opencount;
                    }
                    else
                        writer.WriteDictionaryNumberEntry("Count", -opencount);
                }
            }

            //we don't close the dictionary here as we need the next entry written
            //It should be closed in the calling method

            return item;
        }


    }

    internal class PDFOutlineRefCollection : List<PDFOutlineRef>
    {
    }


    internal class PDFOutlineRef : IArtefactEntry


    {
        private PDFOutline _outline;
        private PDFOutlineStyle _style;
        private PDFOutlineRefCollection _inneritems;

        /// <summary>
        /// Gets the PDFOutline this reference refers to.
        /// </summary>
        internal PDFOutline Outline { get { return _outline; } }

        /// <summary>
        /// Gets the style associated with this outline
        /// </summary>
        internal PDFOutlineStyle Style { get { return _style; } }

        /// <summary>
        /// Gets the collection of inner items - can be null
        /// </summary>
        internal PDFOutlineRefCollection InnerItems { get { return _inneritems; } }

        /// <summary>
        /// Returns true if this outline reference has any inner items
        /// </summary>
        internal bool HasInnerItems { get { return _inneritems != null && _inneritems.Count > 0; } }


        internal PDFOutlineRef(PDFOutline outline, PDFOutlineStyle style)
        {
            if (null == outline)
                throw RecordAndRaise.ArgumentNull("outline");

            _outline = outline;
            _style = style;
        }

        internal void AddChild(PDFOutlineRef outlineref)
        {
            if (null == _inneritems)
                _inneritems = new PDFOutlineRefCollection();
            _inneritems.Add(outlineref);
        }

        internal PDFColor GetColor()
        {
            PDFColor c = this.Outline.Color;

            if (c == null || c == PDFColors.Transparent)
            {
                c = (null == this.Style) ? null : this.Style.Color;
                if (c == PDFColors.Transparent)
                    c = null;
            }

            return c;
        }

        internal FontStyle GetFontStyle()
        {
            FontStyle fs = FontStyle.Regular;
            if (this.Outline.HasBold)
            {
                if (this.Outline.FontBold)
                    fs = FontStyle.Bold;
            }
            else if (this.Style.FontBold)
                fs = FontStyle.Bold;

            if (this.Outline.HasItalic)
            {
                if (this.Outline.FontItalic)
                    fs |= FontStyle.Italic;
            }
            else if (this.Style.FontItalic)
                fs |= FontStyle.Italic;

            return fs;
        }

        public bool GetIsOpen()
        {
            bool open = false;
            if (this.Outline.HasOpen)
            {
                if (this.Outline.OutlineOpen)
                    open = true;
            }
            else
                open = this.Style.Open;

            return open;
        }

    }
}
