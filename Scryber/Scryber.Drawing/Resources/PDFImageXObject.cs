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
using Scryber.Native;

namespace Scryber.Resources
{
    /// <summary>
    /// Defines an Image resource that can be registered with the 
    /// resouce container and rendered in the output stream.
    /// </summary>
    public class PDFImageXObject : PDFResource
    {
        private string _src;

        /// <summary>
        /// Gets or sets the source of this image resource.
        /// </summary>
        public string Source
        {
            get { return _src; }
            internal set { _src = value; _data = null; }
        }

        private PDFImageData _data;
        /// <summary>
        /// Gets the actual image data.
        /// </summary>
        public PDFImageData ImageData
        {
            get { return _data; }
            private set { _data = value; }
        }


        public override string ResourceType
        {
            get { return PDFResource.XObjectResourceType; }
        }

        public override string ResourceKey
        {
            get { return (this.ImageData == null) ? "" : this.ImageData.SourcePath; }
        }

        private PDFImageXObject() :this(PDFObjectTypes.ImageXObject)
        {
            
        }

        protected PDFImageXObject(PDFObjectType type)
            : base(type)
        {
        }



        public override bool Equals(string resourcetype, string name)
        {
            return string.Equals(this.ResourceType, resourcetype) && String.Equals(this.Source, name, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// Renderes this image xObject if it has not already been rendered. Otherwise returns the last ObjectRef.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override PDFObjectRef DoRenderToPDF(PDFContextBase context, PDFWriter writer)
        {
            if (this.ImageData == null)
            {
                throw new NullReferenceException("The Image resource '" + this.Name + "' does not have any ImageData assigned");
            }
            return this.ImageData.Render(this.Name, context, writer);
        }


        

        public PDFSize GetImageSize()
        {
            PDFSize size = PDFSize.Empty;

            if (this.Registered && String.IsNullOrEmpty(this.Source) == false)
            {
                if (this.ImageData == null)
                {
                    throw new NullReferenceException("The Image resource '" + this.Name + "' does not have any ImageData assigned");
                }
                size = this.ImageData.GetSize();
            }

            return size;
        }

        //
        // Load Image methods
        //


        public static PDFImageXObject Load(PDFImageData imgdata, string name)
        {
            PDFImageXObject x = new PDFImageXObject();
            x._src = imgdata.SourcePath;
            x._data = imgdata;
            x.Name = (Native.PDFName)name;

            return x;
        }
    }
}
