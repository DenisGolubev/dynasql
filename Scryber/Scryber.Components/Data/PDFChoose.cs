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

namespace Scryber.Data
{
    [PDFParsableComponent("Choose")]
    public class PDFChoose : PDFBindingTemplateComponent
    {

        /// <summary>
        /// Overridden - Not supported on the PDFChoose Component. Use the Where and Otherwise properties instead
        /// </summary>
        public override IPDFTemplate Template
        {
            get
            {
                return null;
            }
            set
            {
                throw RecordAndRaise.NotSupported(Errors.CannotUseBaseTemplateInChoose);
            }
        }

        private PDFChooseWhenList _whens;

        [PDFArray(typeof(PDFChooseWhen))]
        [PDFElement("")]
        public PDFChooseWhenList Whens
        {
            get
            {
                if (null == _whens)
                    _whens = new PDFChooseWhenList(this.InnerContent);
                return _whens;
            }
        }

        private PDFChooseOtherwise _otherwise;

        [PDFElement("Otherwise")]
        public PDFChooseOtherwise Otherwise
        {
            get { return _otherwise; }
            set 
            {
                if (null != _otherwise && _otherwise.Parent == this)
                    _otherwise.Parent = null;

                _otherwise = value;

                if (null != _otherwise && _otherwise.Parent != this)
                    _otherwise.Parent = this;
            }
        }

        public PDFChoose()
            : this(PDFObjectTypes.NoOp)
        {
        }

        public PDFChoose(PDFObjectType type)
            : base(type)
        {
        }


        protected override void DoDataBindTemplate(PDFDataContext context, IPDFContainerComponent container)
        {
            bool found = false;
            foreach (PDFChooseWhen where in this.Whens)
            {
                if(where.EvaluateTest(context))
                {
                    if (where.Template != null)
                    {
                        int oldindex = context.CurrentIndex;
                        int index = container.Content.IndexOf(this);
                        DoBindDataWithTemplate(container, index, where.Template, context);
                        context.CurrentIndex = oldindex;
                    }
                    found = true;
                    break;
                }
            }
            //If we have a template and should be binding on it
            if (!found && this.Otherwise != null && this.Otherwise.Template != null)
            {
                int oldindex = context.CurrentIndex;
                int index = container.Content.IndexOf(this);
                DoBindDataWithTemplate(container, index, this.Otherwise.Template, context);
                context.CurrentIndex = oldindex;
            }
        }
    }
}
