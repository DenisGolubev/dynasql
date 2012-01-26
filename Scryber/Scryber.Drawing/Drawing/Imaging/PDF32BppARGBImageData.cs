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
using System.Linq;
using System.Text;
using Scryber;
using Scryber.Native;
namespace Scryber.Drawing.Imaging
{
    internal class PDF32BppARGBImageData : PDFImageData
    {

        private byte[] _alpha;

        internal byte[] AlphaData
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        public PDF32BppARGBImageData(string uri, int width, int height)
            : base(uri, width, height)
        {
        }

        protected override void RenderImageInformation(PDFContextBase context, PDFWriter writer)
        {
            base.RenderImageInformation(context, writer);
            if (this.AlphaData != null)
            {
                PDFObjectRef alpha = this.RenderAlpaImageData(context, writer);
                writer.WriteDictionaryObjectRefEntry("SMask", alpha);
            }
        }

        private PDFObjectRef RenderAlpaImageData(PDFContextBase context, PDFWriter writer)
        {
            context.TraceLog.Add(TraceLevel.Debug, "IMAGE", "Rendering image alpha mask");
            PDFObjectRef mask = writer.BeginObject();
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "XObject");
            writer.WriteDictionaryNameEntry("Subtype", "Image");
            writer.WriteDictionaryNumberEntry("Width", this.PixelWidth);
            writer.WriteDictionaryNumberEntry("Height", this.PixelHeight);
            writer.WriteDictionaryNumberEntry("Length", this.AlphaData.LongLength);
            writer.WriteDictionaryNameEntry("ColorSpace", "DeviceGray");
            writer.WriteDictionaryNumberEntry("BitsPerComponent", 8);
            writer.EndDictionary();
            writer.BeginStream(mask);

            writer.WriteRaw(this.AlphaData, 0, this.AlphaData.Length);

            writer.EndStream();
            writer.EndObject();

            return mask;
        }
    }
}
