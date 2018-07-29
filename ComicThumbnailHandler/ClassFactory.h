/****************************** Module Header ******************************\
Module Name:  ClassFactory.h
Project:      CppShellExtThumbnailHandler
Copyright (c) Microsoft Corporation.

The file declares the class factory for the RecipeThumbnailProvider COM class. 

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#pragma once

#include <unknwn.h>     // For IClassFactory
#include <windows.h>


class ClassFactory : public IClassFactory
{
public:
    
    //  IUnknown methods
    STDMETHOD(QueryInterface)(REFIID, void**);
    STDMETHOD_(ULONG, AddRef)();
    STDMETHOD_(ULONG, Release)();

    //  IClassFactory methods
    STDMETHOD(CreateInstance)(IUnknown*, REFIID, void**);
    STDMETHOD(LockServer)(BOOL);

    ClassFactory();

protected:
    ~ClassFactory();

private:
    LONG m_cRef;
};