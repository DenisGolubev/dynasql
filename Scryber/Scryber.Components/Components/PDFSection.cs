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
using Scryber.Components.Support;

namespace Scryber.Components
{
    [PDFParsableComponent("Section")]
    [PDFRemoteParsableComponent("Section-Ref")]
    public class PDFSection : PDFPage
    {


        #region  public IPDFTemplate ContinuationHeader {get;set;}

        private IPDFTemplate _header;

        /// <summary>
        /// Gets or sets the template for the continuation footer of this Page 
        /// (the footer that will be shown on subsequent pages other than the first)
        /// </summary>
        [PDFTemplate()]
        [PDFElement("ContinuationHeader")]
        public IPDFTemplate ContinuationHeader
        {
            get { return _header; }
            set { _header = value; }
        }

        #endregion

        #region public IPDFTemplate ContinuationFooter {get;set;}

        private IPDFTemplate _footer;

        /// <summary>
        /// Gets or sets the template for the continuation footer of this Page 
        /// (the footer that will be shown on subsequent pages other than the first)
        /// </summary>
        [PDFTemplate()]
        [PDFElement("ContinuationFooter")]
        public IPDFTemplate ContinuationFooter
        {
            get { return _footer; }
            set { _footer = value; }
        }

        #endregion


        public PDFSection()
            : base(PDFObjectTypes.Section)
        {
        }


        public override int RegisterPageNumbering(int pageindex, PDFPageNumbering num)
        {
            return base.RegisterPageNumbering(pageindex, num);
        }



        public override IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context)
        {
            return new Support.SectionLayoutEngine(this, parent, context);
        }

               /// <summary>
        /// Overrides the base behaviour to add the overflow action of new page to this elements style.
        /// </summary>
        /// <returns></returns>
        protected override Scryber.Styles.PDFStyle GetBaseStyle()
        {
            Scryber.Styles.PDFStyle flat = base.GetBaseStyle();
            flat.Overflow.Action = OverflowAction.NewPage;

            return flat;
        }
    }
}
