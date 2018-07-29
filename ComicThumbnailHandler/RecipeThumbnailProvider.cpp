/******************************** Module Header ********************************\
Module Name:  RecipeThumbnailProvider.cpp
Project:      CppShellExtThumbnailHandler
Copyright (c) Microsoft Corporation.

The code sample demonstrates the C++ implementation of a thumbnail handler 
for a new file type registered with the .recipe extension. 

A thumbnail image handler provides an image to represent the item. It lets you 
customize the thumbnail of files with a specific file extension. Windows Vista 
and newer operating systems make greater use of file-specific thumbnail images 
than earlier versions of Windows. Thumbnails of 32-bit resolution and as large 
as 256x256 pixels are often used. File format owners should be prepared to 
display their thumbnails at that size. 

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
\*******************************************************************************/

#include "RecipeThumbnailProvider.h"
#include <Shlwapi.h>
#include <Wincrypt.h>   // For CryptStringToBinary.
#include <msxml6.h>

#include <strsafe.h>
#include <stdlib.h>
#include <stdio.h>
#include <vector>
#include <streambuf>
#include <iostream>
#include <istream>
#include <fstream> 
#include <string>
#include "fex/fex.h"
#include <algorithm>
#include "resource2.h"
#include <list>

#include <olectl.h> 
#include <ole2.h>

#pragma comment(lib, "Shlwapi.lib")
#pragma comment(lib, "Crypt32.lib")
#pragma comment(lib, "msxml6.lib")


extern HINSTANCE g_hInst;
extern long g_cDllRef;

enum { header_size = 16 };
struct archiveheader
{
	fex_pos_t Position;
	std::string filename;
};
struct myfile_t
{
	char header [header_size];  /* First 16 bytes of file */
	char* data;                 /* Pointer to remaining data */
	int data_size;              /* Size of remaining data */
};

struct membuf : std::streambuf
{
    membuf(char* begin, char* end) {
        this->setg(begin, begin, end);
    }
};

char easytolower(char in)
{
  if(in<='Z' && in>='A')
    return in-('Z'-'z');

  return in;
}

RecipeThumbnailProvider::RecipeThumbnailProvider() : m_cRef(1)
{
	m_pSite = NULL;
    InterlockedIncrement(&g_cDllRef);
}


RecipeThumbnailProvider::~RecipeThumbnailProvider()
{
	    if (m_pSite)
    {
        m_pSite->Release();
        m_pSite = NULL;
    }
    InterlockedDecrement(&g_cDllRef);
}


#pragma region IUnknown

// Query to the interface the component supported.
STDMETHODIMP RecipeThumbnailProvider::QueryInterface(REFIID riid, void **ppv)
{
    static const QITAB qit[] = 
    {
		//QITABENT(RecipeThumbnailProvider, IInitializeWithStream),
		QITABENT(RecipeThumbnailProvider, IInitializeWithFile), 
        QITABENT(RecipeThumbnailProvider, IThumbnailProvider),
		QITABENT(RecipeThumbnailProvider, IObjectWithSite),
        { 0 },
    };
    return QISearch(this, qit, riid, ppv);
}

// Increase the reference count for an interface on an object.
STDMETHODIMP_(ULONG) RecipeThumbnailProvider::AddRef()
{
    return InterlockedIncrement(&m_cRef);
}

// Decrease the reference count for an interface on an object.
STDMETHODIMP_(ULONG) RecipeThumbnailProvider::Release()
{
    ULONG cRef = InterlockedDecrement(&m_cRef);
    if (0 == cRef)
    {
        delete this;
    }

    return cRef;
}

#pragma endregion


#pragma region IInitializeWithStream

// Initializes the thumbnail handler with a stream.
STDMETHODIMP RecipeThumbnailProvider::Initialize(LPCWSTR pszFilePath, DWORD grfMode)
{

	wcscpy_s(m_szFile, pszFilePath); 

    return S_OK;
}


#pragma endregion


#pragma region IThumbnailProvider

HRESULT error( fex_err_t err )
{
    if ( err != NULL )
    {
        
		const char* str = fex_err_str( err );

		WCHAR    str3[255];
		MultiByteToWideChar( 0,0, str, 255, str3, 255);
		LPCWSTR cstr4 = str3;

		//MessageBox(NULL, cstr4, L"File Extractor Errorr", NULL);

		return -1;

        //exit( EXIT_FAILURE );
    }

	return S_OK;

}

// Gets a thumbnail image and alpha type. The GetThumbnail is called with the 
// largest desired size of the image, in pixels. Although the parameter is 
// called cx, this is used as the maximum size of both the x and y dimensions. 
// If the retrieved thumbnail is not square, then the longer axis is limited 
// by cx and the aspect ratio of the original image respected. On exit, 
// GetThumbnail provides a handle to the retrieved image. It also provides a 
// value that indicates the color format of the image and whether it has 
// valid alpha information.
STDMETHODIMP RecipeThumbnailProvider::GetThumbnail(UINT cx, HBITMAP *phbmp, 
    WTS_ALPHATYPE *pdwAlpha)
{

	/*

		ULONG_PTR m_gdiplusToken;
			Gdiplus::GdiplusStartupInput gdiplusStartupInput;
		Gdiplus::GdiplusStartup(&m_gdiplusToken, &gdiplusStartupInput, NULL);


		Gdiplus::Bitmap *bmp = Gdiplus::Bitmap::FromResource(GetModuleHandle(NULL), MAKEINTRESOURCE(IDB_BITMAP1));
		bmp = ResizeClone(bmp,256,256);
		HBITMAP* decode = new HBITMAP();
		Gdiplus::Color *color = new Gdiplus::Color(0,0,0);
		bmp->GetHBITMAP(*color,decode);

		Gdiplus::GdiplusShutdown(m_gdiplusToken);
		phbmp = decode;
		*/
		//return NOERROR;

			fex_t* fex;

			myfile_t file;
	const void* data;
	//int size;

	size_t   i;
	
	char *strChar = new char[1000];
	wcstombs_s(&i, strChar, (size_t)1000, m_szFile, (size_t)1000);

	//MessageBox(NULL, m_szFile, L"Message", NULL);

	HRESULT hr = error( fex_open( &fex, strChar ) );

	
	
	if(hr == S_OK)
	{

		//MessageBox(NULL, L"File Opened", L"Message", NULL);
	
	
	fex_pos_t currentposition = NULL;
	std::string currentname;


	/* old way
	//find first image

	while ( !fex_done( fex ) )
	{
		if ( fex_has_extension( fex_name( fex ), ".jpg" ) || fex_has_extension( fex_name( fex ), ".png") || fex_has_extension( fex_name( fex ), ".gif" ))
		{
			currentposition =  fex_tell_arc( fex );
			currentname = fex_name( fex );
			std::transform(currentname.begin(), currentname.end(), currentname.begin(), easytolower);
			break;
		}
		error( fex_next( fex ) );
	}

	error( fex_rewind( fex ) );

	if(currentposition != NULL)

	{
	//compare all images
	while ( !fex_done( fex ) )
	{
		if(fex_has_extension( fex_name( fex ), ".jpg" ) || fex_has_extension( fex_name( fex ), ".png" ) || fex_has_extension( fex_name( fex ), ".gif" ))
		{
			std::string newname = fex_name( fex );
			std::transform(currentname.begin(), currentname.end(), currentname.begin(), easytolower);
			std::transform(newname.begin(), newname.end(), newname.begin(), easytolower);

			int ir = strcmp(currentname.c_str(),newname.c_str());

			if(ir > 0)
			{
				currentposition =  fex_tell_arc( fex );
				currentname = fex_name( fex );
			}
		}

		error( fex_next( fex ) );
	}
	*/


	std::list<archiveheader> filenames;

	//find first image


	while ( !fex_done( fex ) )
	{


		if ( fex_has_extension( fex_name( fex ), ".jpg" ) || fex_has_extension( fex_name( fex ), ".png" )|| fex_has_extension( fex_name( fex ), ".jpeg" ))
		{
			archiveheader newheader;
			newheader.filename = fex_name( fex );
			std::transform(newheader.filename.begin(), newheader.filename.end(), newheader.filename.begin(), easytolower);
			newheader.Position = fex_tell_arc( fex );
			filenames.push_back(newheader);
			//currentposition =  fex_tell_arc( fex );
			//currentname = fex_name( fex );
			//std::transform(currentname.begin(), currentname.end(), currentname.begin(), easytolower);
			//break;
		}
		error( fex_next( fex ) );
	}

	error( fex_rewind( fex ) );

	if(filenames.size() == 0)
	{
		fex_close( fex );
				fex = NULL;
				return 1;
	}

	archiveheader *toextract;

	std::list<archiveheader>::iterator itr = filenames.begin();

	toextract = &(*itr);

	
	for(int i = 1; i< filenames.size(); i++)
	{
		itr++;
		if(toextract->filename.compare((*itr).filename) > 0)
				toextract = &(*itr);
	}


	//getfisrt
	
	fex_seek_arc(fex,toextract->Position);

			hr = error( fex_stat( fex ) );

			if(hr != S_OK)
			{
				fex_close( fex );
				fex = NULL;
				return 1;
			}

			hr = error( fex_data( fex, &data ) );
			if(hr != S_OK)
			{
				fex_close( fex );
			fex = NULL;

				return 1;
			}
				file.data_size = fex_size( fex );
		
				//file.data = (char *)malloc(file.data_size);
				//memcpy(file.data,data,file.data_size);

	
	
	
			*phbmp = LoadAnImage((char *)data,file.data_size);

			fex_close( fex );
			fex = NULL;
			//free( myfile.data );
			//pXMLDoc->Release();
	   // }
		return NOERROR;
	}

	fex_close( fex );
	fex = NULL;
	
	return 1;
}

#pragma endregion

    // Function LoadAnImage: accepts a file name and returns a HBITMAP.
    // On error, it returns 0.
    HBITMAP RecipeThumbnailProvider::LoadAnImage(char* data, int size)
    {

		IStream* s;



    
	HRESULT hResult = ::CreateStreamOnHGlobal( NULL, TRUE, &s );
	if(hResult == S_OK && s)
	{
	
		hResult = s->Write(data, ULONG(size), NULL);
	}

	//if(png)
	//{
		//MessageBox(NULL, L"is png", L"Message", NULL);

		ULONG_PTR m_gdiplusToken;
		Gdiplus::GdiplusStartupInput gdiplusStartupInput;
		Gdiplus::GdiplusStartup(&m_gdiplusToken, &gdiplusStartupInput, NULL);


		Gdiplus::Bitmap *bmp = Gdiplus::Bitmap::FromStream(s);
		bmp = ResizeClone(bmp,256,256);
		HBITMAP* decode = new HBITMAP();
		Gdiplus::Color *color = new Gdiplus::Color(0,0,0);
		bmp->GetHBITMAP(*color,decode);

		Gdiplus::GdiplusShutdown(m_gdiplusToken);
		return *decode;
	//}
     
		/* redundant
    OleLoadPicture(s,0,false,IID_IPicture,(void**)&p);
     
    if (!p)
    {
    s->Release();
    GlobalFree(hG);
    return NULL;
    }
    s->Release();
    GlobalFree(hG);
     
    HBITMAP hB = 0;
    p->get_Handle((unsigned int*)&hB);
     
    // Copy the image. Necessary, because upon p's release,
    // the handle is destroyed.
    HBITMAP hBB = (HBITMAP)CopyImage(hB,IMAGE_BITMAP,0,0,
    LR_COPYRETURNORG);
     
    p->Release();
    return hBB;
	*/

    }

	

STDMETHODIMP RecipeThumbnailProvider::GetSite(REFIID riid, 
                                         void** ppvSite)
{
    if (m_pSite)
    {
        return m_pSite->QueryInterface(riid, ppvSite);
    }
    return E_NOINTERFACE;

	
}


Gdiplus::Bitmap* RecipeThumbnailProvider::ResizeClone(Gdiplus::Bitmap *bmp, INT width, INT height)
{
    UINT o_height = bmp->GetHeight();
    UINT o_width = bmp->GetWidth();
    INT n_width = width;
    INT n_height = height;
    double ratio = ((double)o_width) / ((double)o_height);
    if (o_width > o_height) {
        // Resize down by width
        n_height = static_cast<UINT>(((double)n_width) / ratio);
    } else {
        n_width = static_cast<UINT>(n_height * ratio);
    }
    Gdiplus::Bitmap* newBitmap = new Gdiplus::Bitmap(n_width, n_height, bmp->GetPixelFormat());
    Gdiplus::Graphics graphics(newBitmap);
    graphics.DrawImage(bmp, 0, 0, n_width, n_height);
    return newBitmap;
}

STDMETHODIMP RecipeThumbnailProvider::SetSite(IUnknown* pUnkSite)
{
    if (m_pSite)
    {
        m_pSite->Release();
        m_pSite = NULL;
    }

    m_pSite = pUnkSite;
    if (m_pSite)
    {
        m_pSite->AddRef();
    }
    return S_OK;
}
