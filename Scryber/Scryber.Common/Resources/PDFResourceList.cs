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
using Scryber.Native;

namespace Scryber.Resources
{
    
    public class PDFResourceList
    {
        #region Obselete ProcSets that should continue to be rendered for Postcript printing

        private static PDFName[] ProcSets = new PDFName[] { 
                                                new PDFName("PDF"), 
                                                new PDFName("Text"), 
                                                new PDFName("ImageB"), 
                                                new PDFName("ImageC"), 
                                                new PDFName("ImageI") };

        #endregion

        #region Inner Classes

        private class InnerTypedList : System.Collections.ObjectModel.KeyedCollection<string, PDFResourceItemList>
        {
            protected override string GetKeyForItem(PDFResourceItemList item)
            {
                return item.Type;
            }

        }

        private class PDFResourceItemList : System.Collections.ObjectModel.KeyedCollection<PDFName, PDFResource>
        {
            private string _type;
            public string Type
            {
                get { return _type; }
            }

            public PDFResourceItemList(string type)
            {
                this._type = type;
            }
            protected override PDFName GetKeyForItem(PDFResource item)
            {
                return item.Name;
            }
        }

        #endregion

        private InnerTypedList types = new InnerTypedList();
        private IPDFResourceContainer _container;

        public IPDFResourceContainer Container
        {
            get { return _container; }
        }

        public PDFResourceList(IPDFResourceContainer container)
        {
            this._container = container;
        }

        public void EnsureInList(PDFResource Component)
        {
            
            PDFResourceItemList items;
            if (this.types.Contains(Component.ResourceType) == false)
            {
                items = new PDFResourceItemList(Component.ResourceType);
                this.types.Add(items);
            }
            else
                items = this.types[Component.ResourceType];

            if (items.Contains(Component.Name) == false)
                items.Add(Component);
        }


        public PDFObjectRef WriteResourceList(PDFContextBase context, PDFWriter writer)
        {
            PDFObjectRef oref = writer.BeginObject();

            writer.BeginDictionary();
                        
            writer.BeginDictionaryEntry("ProcSet");
            writer.WriteArrayNameEntries(ProcSets);
            writer.EndDictionaryEntry();

            foreach (PDFResourceItemList list in this.types)
            {
                writer.BeginDictionaryEntry(list.Type);
                writer.BeginDictionary();
                foreach (PDFResource rsrc in list)
                {

                    PDFObjectRef rref = rsrc.EnsureRendered(context, writer);
                    if (rref != null)
                    {
                        writer.BeginDictionaryEntry(rsrc.Name);
                        writer.WriteObjectRef(rref);
                        writer.EndDictionaryEntry();
                    }
                }
                writer.EndDictionary();
                writer.EndDictionaryEntry();
            }
            
            
            writer.EndDictionary();
            writer.EndObject();

            return oref;
        }

        
    }
}
