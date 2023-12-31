/****************************** Module Header ******************************\
Module Name:  RecipeThumbnailProvider.h
Project:      CppShellExtThumbnailHandler
Copyright (c) Microsoft Corporation.

The code sample demonstrates the C++ implementation of a thumbnail handler 
for a new file type registered with the .recipe extension. 

A thumbnail image handler provides an image to represent the item. It lets 
you customize the thumbnail of files with a specific file extension. Windows 
Vista and newer operating systems make greater use of file-specific thumbnail 
images than earlier versions of Windows. Thumbnails of 32-bit resolution and 
as large as 256x256 pixels are often used. File format owners should be 
prepared to display their thumbnails at that size. 

The example thumbnail handler implements the IInitializeWithStream and 
IThumbnailProvider interfaces, and provides thumbnails for .recipe files. 
The .recipe file type is simply an XML file registered as a unique file name 
extension. It includes an element called "Picture", embedding an image file. 
The thumbnail handler extracts the embedded image and asks the Shell to 
display it as a thumbnail.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#pragma once

#include <windows.h>
#include <thumbcache.h>     // For IThumbnailProvider
#include <wincodec.h>       // Windows Imaging Codecs
#include "gdiplus.h"

#undef max

#pragma comment(lib, "windowscodecs.lib")
#pragma comment(lib, "gdiplus.lib")

//class RecipeThumbnailProvider : public IInitializeWithFile, public IThumbnailProvider
//class RecipeThumbnailProvider : public IThumbnailProvider, IObjectWithSite, IInitializeWithStream
class ComicThumbnailProvider :	public IThumbnailProvider, IObjectWithSite, IInitializeWithFile

{
public:
    // IUnknown

    STDMETHOD(QueryInterface)(REFIID, void**);
    STDMETHOD_(ULONG) AddRef();
    STDMETHOD_(ULONG) Release();

     STDMETHOD(Initialize)(LPCWSTR , DWORD);

    // IThumbnailProvider
    STDMETHOD(GetThumbnail)(UINT, HBITMAP*, WTS_ALPHATYPE*);
	//char easytolower(char in);
	    //  IObjectWithSite methods
    STDMETHOD(GetSite)(REFIID, void**);
    STDMETHOD(SetSite)(IUnknown*);

	HBITMAP LoadAnImage(char* data, int size);

    ComicThumbnailProvider();

protected:
    ~ComicThumbnailProvider();

private:
    // Reference count of component.
    long m_cRef;
	IUnknown* m_pSite;
	TCHAR m_szFile[1000];

	Gdiplus::Bitmap* ResizeClone(Gdiplus::Bitmap *bmp, INT width, INT height);

    // Provided during initialization.
    IStream *m_pStream;

	LPCWSTR *FilePath;
};