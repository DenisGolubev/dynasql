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

namespace Scryber.Resources
{
    public class PDFResourceCollection
    {
        

        private IPDFComponent _owner;

        public IPDFComponent Owner
        {
            get { return _owner; }
        }

        private List<PDFResource> _items;

        protected List<PDFResource> Items
        {
            get { return _items; }
        }

        public PDFResourceCollection(IPDFComponent owner) 
        {
            this._owner = owner;
            this._items = new List<PDFResource>();
        }

        public PDFResource GetResource(string type, string name)
        {
            foreach (PDFResource resx in this.Items)
            {
                if (resx.Equals(type, name))
                {
                    return resx;
                }

            }
            return null;
        }

        //public PDFImageXObject GetImage(string path)
        //{
        //    foreach (PDFResource resx in this.Items)
        //    {
        //        if (resx.Type == PDFObjectTypes.ImageXObject)
        //        {
        //            PDFImageXObject xobj = resx as PDFImageXObject;
        //            if (string.Equals(xobj.Source, path, StringComparison.OrdinalIgnoreCase))
        //                return xobj;
        //        }
        //    }
        //    return null;

        //}

        //public PDFFontResource GetFont(Scryber.Drawing.PDFFont font)
        //{
        //    foreach (PDFResource resx in this.Items)
        //    {
        //        if (resx.Type == PDFObjectTypes.FontResource)
        //        {
        //            PDFFontResource f = (PDFFontResource)resx;
        //            if (font != null && f.Equals(font))
        //                return f;
                    
        //        }
        //    }
        //    return null;
        //}

        public void Add(PDFResource resource)
        {
            this.Items.Add(resource);
        }
    }
}
