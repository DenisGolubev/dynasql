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
using System.Reflection;

namespace Scryber.Generation
{
    internal class ParserArrayDefinition : ParserPropertyDefinition
    {
        private Type _contenttype;

        public Type ContentType
        {
            get { return _contenttype; }
        }

        internal ParserArrayDefinition(string name, Type contains, PropertyInfo pi)
            : base(name, pi, DeclaredParseType.ArrayElement)
        {
            this._contenttype = contains;
        }

        internal object CreateInstance()
        {
            return Activator.CreateInstance(this.ValueType);
        }

        private MethodInfo _add;

        /// <summary>
        /// Adds the inner object to the collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="inner"></param>
        internal void AddToCollection(object collection, object inner)
        {
            if (collection is System.Collections.IList)
            {
                ((System.Collections.IList)collection).Add(inner);
            }
            //else if (collection is PDFComponentWrappingList)
            //{
            //    PDFComponentList list = ((PDFComponentWrappingList)collection).InnerList;
            //    if (!(inner is PDFComponent))
            //        throw RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, inner.GetType(), typeof(PDFComponent));
            //    list.Add((PDFComponent)inner);
            //}
            else
            {
                if (null == _add)
                {
                    MethodInfo[] all = this.PropertyInfo.PropertyType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                    foreach (MethodInfo one in all)
                    {
                        if (one.Name == "Add")
                        {
                            ParameterInfo[] param = one.GetParameters();
                            if (param.Length == 1 && param[0].ParameterType == this.ContentType)
                            {
                                _add = one;
                                break;
                            }
                        }
                    }
                    if (null == _add)
                        throw new NullReferenceException(String.Format(Errors.NoAddMethodFoundOnCollection, this.ValueType, this.ContentType));
                }
                _add.Invoke(collection, new object[] { inner });
            }
        }
    }
}
