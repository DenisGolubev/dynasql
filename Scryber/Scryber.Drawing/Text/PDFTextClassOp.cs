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
using Scryber.Drawing;

namespace Scryber.Text
{
    public class PDFTextClassOp : PDFTextOp
    {
        private PDFTextOpType _type;

        public override PDFTextOpType OpType
        {
            get { return _type; }
        }

        private string _class;
        public string ClassName
        {
            get { return _class; }
        }

        

        public PDFTextClassOp(string name, bool start)
            : base()
        {
            this._class = name;
            if (start)
                _type = PDFTextOpType.ClassStart;
            else
                _type = PDFTextOpType.ClassEnd;
        }
    }
}
