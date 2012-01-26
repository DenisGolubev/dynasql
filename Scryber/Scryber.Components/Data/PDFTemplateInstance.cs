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
using Scryber.Drawing;

namespace Scryber.Data
{
    /// <summary>
    /// This class acts as a template container that is instantiated from a string entry.
    /// It is used in the databinding of template components and should not be used declaritively
    /// </summary>
    [PDFParsableComponent("TemplateInstance")]
    public class PDFTemplateInstance : PDFContainerComponent, IPDFNamingContainer, IPDFInvisibleContainer
    {
        public PDFTemplateInstance()
            : base(PDFObjectTypes.Template)
        {
        }


        [PDFArray(typeof(PDFComponent))]
        [PDFElement("Content")]
        public PDFComponentList Content
        {
            get
            {
                return base.InnerContent;
            }
            
        }


        public override void SetArrangement(PDFComponentArrangement arrange)
        {
            //We are a special case that there is no offset for the rendering of the inner components based upon the parent
            arrange.Bounds = new PDFRect(PDFPoint.Empty, arrange.Bounds.Size);
            base.SetArrangement(arrange);
        }
    }
}
